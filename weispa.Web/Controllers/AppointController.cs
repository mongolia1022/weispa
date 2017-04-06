using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using com.ccfw.Dal.Base;
using com.ccfw.Utility;
using com.weispa.Web.Dal;
using com.weispa.Web.Filter;
using com.weispa.Web.Models;
using com.weispa.Web.ViewModels;
using com.weispa.Web.Utils;

namespace com.weispa.Web.Controllers
{
    [WxLogin]
    public class AppointController : BaseController
    {
        /// <summary>
        /// 预定护理页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(DateTime? time)
        {
            var now = DateTime.Now;
            var selectDate = (time ?? now).Date;
            var model = new AppointVm { DateList = new List<DateTime>(), SelectDate = selectDate,MemberInfo =MemberInfo};
            var role = (MemberRole)MemberInfo.Role;

            var today = now.Date;
            var lastDate = today.AddDays(6);

            //日期列表
            for (var d = today; d <= lastDate; d = d.AddDays(1))
            {
                model.DateList.Add(d);
            }

            //获取1天内预约列表
            var orders = OrderDal.GetList(string.Format("StartTime>={0} and StartTime<={1} and (Status={2} or Status={3})",
                selectDate.ToUnixStamp(), selectDate.AddDays(1).ToUnixStamp(), (int)OrderStatus.已确认, (int)OrderStatus.已锁定))
                .OrderBy(_=>_.StartTime).ThenBy(_=>_.Room).ToList();

            model.AppointList=new List<AppointVm.AppointItem>();

            var opentime = new DateTime(selectDate.Year, selectDate.Month, selectDate.Day, 10, 0, 0);//选择日期营业开始时间
            var closetime = new DateTime(selectDate.Year, selectDate.Month, selectDate.Day, 23, 30, 0);//选择日期关门时间

            while (opentime < closetime)
            {
                var appoint=new AppointVm.AppointItem
                {
                    StartTime = opentime,
                    EndTime  = opentime.AddMinutes(WeispaConfig.ServiceMinutes),
                    Rooms = new List<AppointVm.Room>()
                };

                var startTime = appoint.StartTime.ToUnixStamp();
                var endTime = appoint.EndTime.ToUnixStamp();

                for (int roomType = 0; roomType <=1 ; roomType++)
                {
                    var room = new AppointVm.Room(){ToStatusList = new List<int>()};
                    if (now >= appoint.StartTime)
                    {
                        room.Status = 2;
                    }
                    else
                    {
                        var order = orders.FirstOrDefault(_ =>_.Room == roomType &&WeispaUtil.CkeckOccurred(_.StartTime, _.EndTime, startTime, endTime));
                        if (order == null)
                        {
                            room.Status = 0;
                            room.ToStatusList.Add(0);

                            if (role == MemberRole.管理员)
                                room.ToStatusList.Add(1);
                        }
                        else if (order.Status == (int)OrderStatus.已锁定)
                        {
                            room.Status = 2;
                            room.OrderId = order.OId;

                            if (role == MemberRole.管理员)
                                room.ToStatusList.Add(2);
                        }
                        else
                        {
                            room.Status = 1;

                            if (role == MemberRole.管理员)
                                room.ToStatusList.Add(1);
                        }
                    }

                    room.Name = ((RoomType)roomType).ToString();
                    room.Type = roomType;

                    appoint.Rooms.Add(room);
                }
                opentime = appoint.EndTime.AddMinutes(WeispaConfig.RestMinutes);
                model.AppointList.Add(appoint);
            }

            return View(model);
        }

        /// <summary>
        /// 我的预定页
        /// </summary>
        /// <returns></returns>
        public ActionResult MyAppoint(int? type)
        {
            var model = new MyAppointVm { MyOrders = new List<MyOrderVm>(), Type = type ?? 0 };
            var today = DateTime.Now.Date;
            var role = (MemberRole)MemberInfo.Role;
            var where = role == MemberRole.顾客 ? string.Format("MemberId={0} and", MemberInfo.Uid) : "";
            var orders = OrderDal.GetList(string.Format("{0} StartTime>={1} and StartTime<{2} and {3}" ,
                where, today.ToUnixStamp(), today.AddDays(WeispaConfig.LimitDays).ToUnixStamp(), model.Type == 1 ? "Status>1 and Status<5" : "Status<2"));
            
            foreach (var order in orders)
            {
                var optList  = new List<int>();
                var orderVm = new MyOrderVm { Order = order, ToStatusList = optList };
                var status = (OrderStatus)order.Status;
                //根据权限添加操作选项

                if (role == MemberRole.管理员)
                {
                    if (status == OrderStatus.未确认)
                    {
                        optList.Add((int) OrderStatus.已确认);
                    }
                    else if (status == OrderStatus.已确认)
                    {
                        optList.Add((int)OrderStatus.已完成);
                        optList.Add((int)OrderStatus.未完成);
                    }
                }
                if (status == OrderStatus.未确认 || status == OrderStatus.已确认)
                {
                    optList.Add((int)OrderStatus.已取消);
                }
                model.MyOrders.Add(orderVm);
            }

            return View(model);
        }

        /// <summary>
        /// 预定表单
        /// </summary>
        /// <param name="start"></param>
        /// <param name="room"></param>
        /// <returns></returns>
        public ActionResult AppointForm(AppointFormVm model)
        {
            return View(model);
        }

        /// <summary>
        /// 预定提交处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AppointPost(AppointRq rq)
        {
            var now = DateTime.Now;
            var today = now.Date;
            var limitDate = today.AddDays(WeispaConfig.LimitDays);

            if (string.IsNullOrEmpty(rq.CustomName))
            {
                return ReturnJson(false, "请填写客户名称");
            }
            if (string.IsNullOrEmpty(rq.CustomPhone))
            {
                return ReturnJson(false, "请填写客户电话");
            }
            if (rq.Room > 1 || rq.Room < 0)
            {
                return ReturnJson(false, "请选择房间");
            }

            if (rq.Start > limitDate)
            {
                return ReturnJson(false, string.Format("只能预订{0}天内", WeispaConfig.LimitDays));
            }
            if (rq.Start < now)
            {
                return ReturnJson(false, "已过时间不可预订");
            }

            //未确认,已确认的订单数
            var weekCount = OrderDal.GetCount(string.Format("MemberId={0} and StartTime>={1} and StartTime<{2} and Status<2", MemberInfo.Uid, now.ToUnixStamp(), limitDate.ToUnixStamp()));
            if (weekCount >= WeispaConfig.LimitOrderCount && MemberInfo.Role!=(int)MemberRole.管理员)
            {
                return ReturnJson(false, string.Format("{0}天内只能预订{1}次", WeispaConfig.LimitDays, WeispaConfig.LimitOrderCount));
            }

            var model = new Order
            {
                MemberId = MemberInfo.Uid,
                CustomName = rq.CustomName,
                CustomPhone = rq.CustomPhone,
                Room = rq.Room,
                Status = 0,
                CreateOn = now.ToUnixStamp(),
                StartTime = rq.Start.ToUnixStamp(),
                EndTime = rq.Start.AddMinutes(WeispaConfig.ServiceMinutes).ToUnixStamp(),
                Remark=rq.Remark
            };

            //查找有冲突的订单
            var isExist = OrderDal.Exists(string.Format("{0} and Room={1} and (Status={2} or Status={3})", 
                WeispaUtil.GetOccurredSql(model.StartTime,model.EndTime),
                model.Room, (int)OrderStatus.已确认,(int)OrderStatus.已锁定));
            
            if (isExist)
            {
                return ReturnJson(false, "该房间已被其他人预订");
            }

            OrderDal.Add(model);

            return ReturnJson(true, "已成功提交预订，待系统确认");
        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="toStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeStatus(int orderId, int toStatus)
        {
            var order = OrderDal.GetModel(orderId);
            if (order == null)
            {
                return ReturnJson(false, "订单不存在");
            }

            var oriStatus = (OrderStatus) order.Status;
            //权限检查
            var role = (MemberRole) MemberInfo.Role;
            var toStatusEnum = (OrderStatus) toStatus;
            switch (toStatusEnum)
            {
                case OrderStatus.未确认:
                    return ReturnJson(false, "操作无效");
                    break;
                case OrderStatus.已确认:
                    if (role != MemberRole.管理员)
                        return ReturnJson(false, "您没有权限");

                    if (oriStatus != OrderStatus.未确认)
                        return ReturnJson(false, "这不是未确认的订单");

                    //查找有冲突的订单
                    var isExist = OrderDal.Exists(string.Format("{0} and Room={1} and Status={2}",
                        WeispaUtil.GetOccurredSql(order.StartTime, order.EndTime),
                        order.Room, (int)OrderStatus.已确认));
                    if (isExist)
                        return ReturnJson(false, "已经有人预定该房间");
                    break;
                case OrderStatus.已完成:
                    if (role != MemberRole.管理员)
                        return ReturnJson(false, "您没有权限");

                    if (oriStatus != OrderStatus.已确认)
                        return ReturnJson(false, "这不是已确认的订单");

                    break;
                case OrderStatus.未完成:
                    if (role != MemberRole.管理员)
                    {
                        return ReturnJson(false, "您没有权限");
                    }

                    if (oriStatus != OrderStatus.已确认)
                        return ReturnJson(false, "这不是已确认的订单");
                    break;
                case OrderStatus.已取消:
                    if (oriStatus != OrderStatus.未确认 && oriStatus != OrderStatus.已确认)
                        return ReturnJson(false, "不可取消");
                    break;

                default:
                    return ReturnJson(false, "操作无效");
            }

            order.Status = toStatus;
            var b = OrderDal.Update(order, "Status");
            return b ? 
                ReturnJson(true, new {status = toStatusEnum.ToString(), opts = StringHelper.GetArrayStr(OptList(role, toStatusEnum))}) : 
                ReturnJson(false, "操作失败");
        }

        /// <summary>
        /// 提交成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Success()
        {
            return View();
        }

        /// <summary>
        /// 管理员专用预定处理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminAppoint(AdminAppointVm.AdminAppointRq rq)
        {
            if (MemberInfo.Role != (int) MemberRole.管理员)
                return Content("没有权限");

            var model = new AdminAppointVm
            {
                Orders = new List<MyOrderVm>(),
                Rq = rq = rq ?? new AdminAppointVm.AdminAppointRq()
            };

            var where = new StringBuilder("1=1");
            var parms = new List<DbParam>();
            var today = DateTime.Now;

            if (!rq.Start.HasValue)
            {
                rq.Start = today;
            }
            where.AppendFormat(" and StartTime>{0}", rq.Start.Value.ToUnixStamp());

            if (!rq.End.HasValue)
            {
                rq.End = today.AddDays(WeispaConfig.LimitDays);
            }
            where.AppendFormat(" and StartTime<{0}", rq.End.Value.ToUnixStamp());

            if (rq.RoomType.HasValue)
            {
                where.AppendFormat(" and Room={0}", (int) rq.RoomType);
            }
            if (rq.Status.HasValue)
            {
                where.AppendFormat(" and Status={0}", (int) rq.Status);
            }
            if (!string.IsNullOrEmpty(rq.Name))
            {
                where.Append(" and CustomName like @Name");
                parms.Add(new DbParam
                {
                    ParamDbType = DbType.String,
                    ParamName = "@Name",
                    ParamValue = string.Format("%{0}%", rq.Name)
                });
            }
            if (!string.IsNullOrEmpty(rq.Phone))
            {
                where.Append(" and CustomPhone like @Phone");
                parms.Add(new DbParam
                {
                    ParamDbType = DbType.String,
                    ParamName = "@Phone",
                    ParamValue = string.Format("%{0}%", rq.Phone)
                });
            }

            if (rq.Pn < 1)
                rq.Pn = 1;

            var orders = OrderDal.GetList(where.ToString(), 10, rq.Pn, false, "*", "OId", parms);

            foreach (var order in orders)
            {
                var optList = new List<int>();
                var orderVm = new MyOrderVm {Order = order, ToStatusList = optList};
                var status = (OrderStatus) order.Status;
                //根据权限添加操作选项
                if (status == OrderStatus.未确认)
                {
                    optList.Add((int) OrderStatus.已确认);
                }
                else if (status == OrderStatus.已确认)
                {
                    optList.Add((int) OrderStatus.已完成);
                    optList.Add((int) OrderStatus.未完成);
                }

                if (status == OrderStatus.未确认 || status == OrderStatus.已确认)
                {
                    optList.Add((int) OrderStatus.已取消);
                }
                model.Orders.Add(orderVm);
            }

            model.TotalCount = OrderDal.GetCount(where.ToString(), parms);
            return View(model);
        }

        public ActionResult AdminAppointListAjax(AdminAppointVm.AdminAppointRq rq)
        {
            if (MemberInfo.Role != (int)MemberRole.管理员)
                return Content("没有权限");

            var model = new AdminAppointVm
            {
                Orders = new List<MyOrderVm>(),
                Rq = rq = rq ?? new AdminAppointVm.AdminAppointRq()
            };

            var where = new StringBuilder("1=1");
            var parms = new List<DbParam>();
            var today = DateTime.Now;

            if (!rq.Start.HasValue)
            {
                rq.Start = new DateTime(2016, 11, 1);
            }
            where.AppendFormat(" and StartTime>{0}", rq.Start.Value.ToUnixStamp());

            if (!rq.End.HasValue)
            {
                rq.End = today.AddDays(WeispaConfig.LimitDays);
            }
            where.AppendFormat(" and StartTime<{0}", rq.End.Value.ToUnixStamp());

            if (rq.RoomType.HasValue)
            {
                where.AppendFormat(" and Room={0}", (int)rq.RoomType);
            }
            if (rq.Status.HasValue)
            {
                where.AppendFormat(" and Status={0}", (int)rq.Status);
            }
            if (!string.IsNullOrEmpty(rq.Name))
            {
                where.Append(" and CustomName like @Name");
                parms.Add(new DbParam { ParamDbType = DbType.String, ParamName = "@Name", ParamValue = string.Format("%{0}%", rq.Name) });
            }
            if (!string.IsNullOrEmpty(rq.Phone))
            {
                where.Append(" and CustomPhone like @Phone");
                parms.Add(new DbParam { ParamDbType = DbType.String, ParamName = "@Phone", ParamValue = string.Format("%{0}%", rq.Phone) });
            }

            if (rq.Pn < 1)
                rq.Pn = 1;

            var orders = OrderDal.GetList(where.ToString(), 10, rq.Pn, false, "*", "OId", parms);

            foreach (var order in orders)
            {
                var optList = new List<int>();
                var orderVm = new MyOrderVm { Order = order, ToStatusList = optList };
                var status = (OrderStatus)order.Status;
                //根据权限添加操作选项
                if (status == OrderStatus.未确认)
                {
                    optList.Add((int)OrderStatus.已确认);
                }
                else if (status == OrderStatus.已确认)
                {
                    optList.Add((int)OrderStatus.已完成);
                    optList.Add((int)OrderStatus.未完成);
                }

                if (status == OrderStatus.未确认 || status == OrderStatus.已确认)
                {
                    optList.Add((int)OrderStatus.已取消);
                }
                model.Orders.Add(orderVm);
            }

            return View("_AdminAppointListPartial", model.Orders);
        }

        /// <summary>
        /// 锁定房间
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
         [HttpPost]
        public ActionResult LockRoom(AppointRq rq)
        {
            if (MemberInfo.Role != (int) MemberRole.管理员)
            {
                return Content("没有权限");
            }

            var model = new Order
            {
                MemberId = MemberInfo.Uid,
                CustomName = "",
                CustomPhone = "",
                Room = rq.Room,
                Status = (int)OrderStatus.已锁定,
                CreateOn = DateTime.Now.ToUnixStamp(),
                StartTime = rq.Start.ToUnixStamp(),
                EndTime = rq.Start.AddMinutes(WeispaConfig.ServiceMinutes).ToUnixStamp(),
                Remark = rq.Remark
            };

            var isExist = OrderDal.Exists(string.Format("{0} and Room={1} and (Status={2} or Status={3})",
                WeispaUtil.GetOccurredSql(model.StartTime, model.EndTime),
                model.Room, (int) OrderStatus.已确认, (int) OrderStatus.已锁定));

            if(isExist)
            {
                return ReturnJson(false,"改房间已被预订，请先取消预订才可锁定");
            }

            var orderId = OrderDal.Add(model);

            return ReturnJson(true, new { status = 2, opts = 2, orderId });
        }

        [HttpPost]
        public ActionResult UnLockRoom(int orderId)
        {
            if (MemberInfo.Role != (int)MemberRole.管理员)
            {
                return Content("没有权限");
            }

            var exist = OrderDal.Exists(string.Format("OId={0} and Status={1}",orderId,(int)OrderStatus.已锁定));
            if (!exist)
                return ReturnJson(false, "该房间没有被锁定");

            var b = OrderDal.Delete(orderId)>0;
            return b ?
                ReturnJson(true, new { status = 0, opts ="0,1" }) :
                ReturnJson(false, "操作失败");
        }

        private List<int> OptList(MemberRole role,OrderStatus status)
        {
            //根据权限添加操作选项
            var optList = new List<int>();
            if (role == MemberRole.管理员)
            {
                if (status == OrderStatus.未确认)
                {
                    optList.Add((int)OrderStatus.已确认);
                }
                else if (status == OrderStatus.已确认)
                {
                    optList.Add((int)OrderStatus.已完成);
                    optList.Add((int)OrderStatus.未完成);
                }
            }
            if (status == OrderStatus.未确认 || status == OrderStatus.已确认)
            {
                optList.Add((int)OrderStatus.已取消);
            }
            return optList;
        }

        #region prop

        private OrderDal _orderDal;

        private OrderDal OrderDal
        {
            get { return _orderDal ?? (_orderDal = new OrderDal()); }
        }
        #endregion
    }
}
