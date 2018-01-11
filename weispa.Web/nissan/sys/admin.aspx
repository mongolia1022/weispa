<%@ Page Language="C#" ResponseEncoding="gbk"%>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="com.ccfw.Dal.Base" %>
<%@ Import Namespace="com.ccfw.Model.Base" %>
<%@ Import Namespace="com.weispa.Web.Util" %>
<%@ Import Namespace="CsvHelper" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<script language="C#" runat="server">
    void Page_Load(object   sender,   EventArgs   e)
    {
        
    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        export(1);
    }
    protected void btn2_Click(object sender, EventArgs e)
    {
        export(2);
    }
    protected void btn3_Click(object sender, EventArgs e)
    {
        export(3);
    }
    

    private void export(int season)
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
<asp:LinkButton runat="server" class="layui-btn layui-btn-small"  OnClick="btn1_Click"><i class="layui-icon">&#xe615;</i> 导出活动1</asp:LinkButton>
<asp:LinkButton runat="server" class="layui-btn layui-btn-small"  OnClick="btn2_Click"><i class="layui-icon">&#xe615;</i> 导出活动2</asp:LinkButton>
<asp:LinkButton runat="server" class="layui-btn layui-btn-small"  OnClick="btn3_Click"><i class="layui-icon">&#xe615;</i> 导出活动3</asp:LinkButton>
			</blockquote>
            </div>
    </form>
</body>
</html>