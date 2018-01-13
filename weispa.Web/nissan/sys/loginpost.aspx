<%@ Page Language="C#" ResponseEncoding="gbk"%>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="com.ccfw.Dal.Base" %>
<%@ Import Namespace="com.ccfw.Model.Base" %>
<%@ Import Namespace="com.weispa.Web.Util" %>
<%@ Import Namespace="CsvHelper" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="com.ccfw.Utility" %>
<script language="C#" runat="server">
    void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        var data = JsonConvert.DeserializeObject <LoginInfo>(Request["data"]);
        bool right = false;
        if (data.season == 1)
        {
           right = data.name == "admin1" && data.pw == "pwd1";
        }
        else if (data.season == 2)
        {
            right = data.name == "admin2" && data.pw == "pwd2";
        }
        else if (data.season == 3)
        {
            right = data.name == "admin3" && data.pw == "pwd3";
        }

        if(right)
            CookieHelper.SetCookie("nissan" + data.season, "1");

        returnJson(right, right?"登录成功":"登录失败");
    }

    private bool returnJson(bool success, string info)
    {
        Response.Write(JsonConvert.SerializeObject(new { success = success ? 0 : -1, info = info }));
        return success;
    } 

    class LoginInfo
    {
        public int season { get; set; }
        public string name { get; set; }
        public string pw { get; set; }
    }
</script>