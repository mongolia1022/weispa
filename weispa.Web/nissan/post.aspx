<%@ Page Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="com.ccfw.Dal.Base" %>
<%@ Import Namespace="com.ccfw.Model.Base" %>
<%@ Import Namespace="com.ccfw.Utility" %>
<%@ Import Namespace="com.weispa.Web.ServiceReference1" %>
<%@ Import Namespace="com.weispa.Web.Util" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<script language="C#" runat="server">
    void Page_Load(object   sender,   EventArgs   e)
    {
        Response.Clear();
        var data = Request["data"];
        var dal = new BaseDAL<NissanCustom>();
        var model = JsonConvert.DeserializeObject<NissanCustom>(data);
        model.createOn=DateTime.Now;

        var oldCount = dal.GetCount(string.Format("phone='{0}'", model.phone));
        if (oldCount > 0)
        {
            returnJson(false, "该手机号已经参与了");
            return;
        }

        if (string.IsNullOrEmpty(model.name))
        {
             returnJson(false, "请输入姓名");
            return;
        }

        if (string.IsNullOrEmpty(model.car))
        {
            returnJson(false, "请选择车型");
            return;
        }

        if (string.IsNullOrEmpty(model.phone))
        {
            returnJson(false, "请输入手机");
            return;
        }

        if (string.IsNullOrEmpty(model.province))
        {
            returnJson(false, "请选择省");
            return;
        }

        if (string.IsNullOrEmpty(model.city))
        {
            returnJson(false, "请选择市");
            return;
        }

        if (string.IsNullOrEmpty(model.store))
        {
            returnJson(false, "请选择店");
            return;
        }

        var dbmodel = new NissanCustom()
        {
            name = model.name,
            car = model.car.Split('_')[0],
            phone = model.phone,
            province = model.province.Split('_')[0],
            city = model.city.Split('_')[0],
            store = model.store.Split('_')[0],
            createOn = model.createOn,
            season = model.season
        };
        
        model.id=dal.Add(dbmodel);
        
        apiService(model);
        
        Response.Write(JsonConvert.SerializeObject(new { success = 0, info = "报名成功" }));
        Response.End();
    }

    private bool returnJson(bool success,string info)
    {
         Response.Write(JsonConvert.SerializeObject(new {success = success ? 0 : -1, info = info}));
         Response.End();
         return success;
    }

    public class NissanCustom:BaseModel
    {
        public NissanCustom()
        {
            PrimaryKey = "id";
            IsAutoId = true;
            ConnName = "nissan";
        }
        
        public int id { get; set; }

        public string name { get; set; }

        public string car { get; set; }

        public string phone { get; set; }

        public string province { get; set; }

        public string city { get; set; }
        
        public string store { get; set; }

        public DateTime createOn { get; set; }

        public int season { get; set; }
    }

    private void apiService(NissanCustom model)
    {
        LogHelper.AddLog("apiservice开始");
        LogHelper.AddLog(JsonConvert.SerializeObject(model));
        var citydata = model.city.Split('_');
        if (citydata.Length < 3)
            return;
        
        //if(!model.name.Contains("测试"))
        //    return;

        try
        {
            string param = JsonConvert.SerializeObject(new
            {
                AuthenticatdKey = model.city.Split('_')[2],//"A0000-000-000-00-00000"测试
                RequestObject = new ArrayList()
                {
                    {
                        new
                        {
                            MEDIA_LEAD_ID = model.id.ToString(),
                            FK_DEALER_ID = model.store.Split('_')[1], //专营店编号
                            CUSTOMER_NAME = model.name,
                            MOBILE = model.phone,
                            PROVINCE = model.province.Split('_')[1], //省编号
                            CITY = model.city.Split('_')[1], //市编号
                            SERIES = model.car.Split('_')[1], //车型编号,
                            MODEL = "",
                            ORDER_TIME = model.createOn.ToString("yyyy-MM-dd HH:mm:ss"), //报名时间
                            COMMENTS = "", //商家备注
                            OPERATE_TYPE = "0", //新增
                            OPERATE_TIME = model.createOn.ToString("yyyy-MM-dd HH:mm:ss"),
                            STATUS = "0",
                            SMART_CODE =  model.city.Split('_')[2]//城市对应smartcode值 "A0000-000-000-00-00000"测试
                        }
                    }
                }
            });
            string re = new BaseInfoServiceClient().SyncSaleClues(param);
            LogHelper.AddLog(param);
            LogHelper.AddLog(re);
        }
        catch (Exception ex)
        {
            LogHelper.AddLog(ex.ToString());
            LogHelper.AddLog(ex.StackTrace);
            throw;
        }
    }
</script>
