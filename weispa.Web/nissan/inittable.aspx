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
    BaseDAL<nissanfield_value> vndal = new BaseDAL<nissanfield_value>();

    void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        var list = new BaseDAL<NissanCustom>().GetList("id<147");
        foreach (var item in list)
        {
            if (item.city == "福州市"||
                item.city == "潮州市"||
                item.city == "惠州市" ||
                item.city == "揭阳市" ||
                item.city == "梅州市" ||
                item.city == "汕头市" ||
                item.city == "汕尾市" ||
                item.city == "深圳市" ||
                item.city == "海口市"||
                item.city == "长沙市")
            {
                checkAndAdd(item.car);
                checkAndAdd(item.city);
                checkAndAdd(item.province);
                checkAndAdd(item.store);
                checkAndAdd(item.city + "_" + item.season);
            }
        }

        Response.Write(JsonConvert.SerializeObject(new {success = 0, info = "报名成功"}));
        Response.End();
    }

    private void checkAndAdd(string name)
    {
        if (!vndal.Exists(string.Format("filed_name='{0}'", name)))
        {
            vndal.Add(new nissanfield_value() {filed_name = name});
        }
    }

    private bool returnJson(bool success, string info)
    {
        Response.Write(JsonConvert.SerializeObject(new {success = success ? 0 : -1, info = info}));
        Response.End();
        return success;
    }

    public class nissanfield_value : BaseModel
    {
        public nissanfield_value()
        {
            PrimaryKey = "filed_name";
            IsAutoId = false;
            ConnName = "nissan";
        }

        public string filed_name { get; set; }
        public string filed_value { get; set; }
    }

    public class NissanCustom : BaseModel
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
                AuthenticatdKey = model.city.Split('_')[2], //"A0000-000-000-00-00000"测试
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
                            SMART_CODE = model.city.Split('_')[2] //城市对应smartcode值 "A0000-000-000-00-00000"测试
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
