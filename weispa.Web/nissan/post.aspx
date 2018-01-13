<%@ Page Language="C#" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="com.ccfw.Dal.Base" %>
<%@ Import Namespace="com.ccfw.Model.Base" %>
<%@ Import Namespace="com.ccfw.Utility" %>
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
        
        //var oldCount = dal.GetCount(string.Format("phone='{0}'",model.phone));
        //if (oldCount > 0)
        //{
        //    Response.Write(JsonConvert.SerializeObject(new {success=false,info="该手机号已经参与了"}));
        //    Response.End();
        //    return;
        //}

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
        
        dal.Add(model);
        
        RequestArgs arg=new RequestArgs();

        
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

    private void apiService()
    {
        RequestArgs mArgs = new RequestArgs
        {
            Encode = "utf-8",
            Method = "POST",
            TimeOut = 3000,
            Url = "http://202.96.191.228:8080/MediaInterface/BaseInfoService.svc",
            postData = JsonConvert.SerializeObject(new {})
        };
        try
        {
            var returnData = CWebRequest.GetPost(mArgs);
            LogHelper.AddLog(returnData);
        }
        catch (Exception ex)
        {
            LogHelper.AddLog(ex.ToString());
        }
    }
</script>
