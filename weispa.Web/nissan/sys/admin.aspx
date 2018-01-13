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
    private int season = 0;

    void Page_Load(object sender, EventArgs e)
    {
        season = ConvertHelper.StrToInt(Request["season"]);
        var cookieInfo=CookieHelper.GetCookie("nissan"+season);
        if (string.IsNullOrEmpty(cookieInfo))
        {
            Response.Redirect("login.html?season=" + season);
            return;
        }

        if (!IsPostBack)
            FillData();
    }

    protected List<NissanCustom> list = null;

    protected void btn1_Click(object sender, EventArgs e)
    {
        export();
    }

    protected void Pager_pageChanged(object sender, EventArgs e)
    {
        FillData();
    }

    private void FillData()
    {
        list = new BaseDAL<NissanCustom>().GetList("season=" + season, pager.PageSize, pager.CurrentPageIndex, true, "*", "id");
    }

    private void export()
    {
        string filename = string.Format("活动{0}数据.csv", season);
        Response.ContentType = "application/CSV";
        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
        Response.Clear();

        var stringWriter = new StringWriter();
        var csvWirter = new CsvWriter(stringWriter);
        csvWirter.Configuration.Encoding = Encoding.GetEncoding("gbk");
        csvWirter.WriteRecord(new
        {
            id = "id",
            name = "客户姓名",
            phone = "电话",
            car = "车型",
            province = "省",
            city = "市",
            store = "专卖店",
            createOn = "报名时间"
        });

        var list = new BaseDAL<NissanCustom>().GetList("season=" + season);
        foreach (var item in list)
        {
            csvWirter.WriteRecord(new
            {
                id = item.id,
                name = item.name,
                phone = item.phone,
                car = item.car,
                province = item.province,
                city = item.city,
                store = item.store,
                createOn = item.createOn.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        Response.Write(stringWriter.ToString());
        stringWriter.Dispose();
        Response.End();
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


</script>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>东风尼桑报名信息</title>
		<link rel="stylesheet" href="plugins/layui/css/layui.css" media="all" />
		<link rel="stylesheet" href="css/global.css" media="all">
		<link rel="stylesheet" href="plugins/font-awesome/css/font-awesome.min.css">
		<link rel="stylesheet" href="css/table.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="admin-main">
			<blockquote class="layui-elem-quote">
<asp:LinkButton runat="server" class="layui-btn layui-btn-small"  OnClick="btn1_Click"><i class="layui-icon">&#xe615;</i> 导出数据</asp:LinkButton>
			</blockquote>
            
            <fieldset class="layui-elem-field">
				<legend>活动<%=season %>报名信息</legend>
				<div class="layui-field-box">
					<table class="site-table table-hover">
						<thead>
							<tr>
								<th><input type="checkbox" id="selected-all"></th>
								<th>id</th>
								<th>姓名</th>
								<th>手机</th>
								<th>车型</th>
								<th>省</th>
								<th>市</th>
								<th>专卖店</th>
                                <th>报名时间</th>
							</tr>
						</thead>
						<tbody>
						    <% foreach (var item in list)
						       {%>
						           <tr>
								<td><%=item.id %></td>
								<td><%=item.name %></td>
								<td><%=item.phone %></td>
								<td><%=item.car %></td>
								<td><%=item.province %></td>
								<td><%=item.province %></td>
                                <td><%=item.city %></td>
                                <td><%=item.store %></td>
                                <td><%=item.createOn.ToString("yyyy-MM-dd HH:mm:ss") %></td>
							</tr>
						       <%} %>
						</tbody>
					</table>
				</div>
			</fieldset>
            
            <div class="admin-table-page">
				<div id="page" class="page">
				    <webdiyer:AspNetPager ID="pager" CssClass="paginator" CurrentPageButtonClass="cpb"
                                                Width="99%" PageSize="20" runat="server" AlwaysShow="true" FirstPageText="<<"
                                                LastPageText=">>" NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Left"
                                                ShowInputBox="Never" OnPageChanged="Pager_pageChanged" CustomInfoTextAlign="Left"
                                                CurrentPageButtonPosition="Beginning" CustomInfoHTML="第 %CurrentPageIndex% 页，共 %PageCount%页,共%RecordCount%条"
                                                ShowPageIndexBox="Always" PageIndexBoxType="DropDownList" TextBeforePageIndexBox="转到第"
                                                TextAfterPageIndexBox="页">
                                            </webdiyer:AspNetPager>
				</div>
			</div>
            </div>
    </form>
</body>
</html>