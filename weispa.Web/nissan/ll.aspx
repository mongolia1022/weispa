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
        LogHelper.AddLog(Request["season"]);
    }
</script>
