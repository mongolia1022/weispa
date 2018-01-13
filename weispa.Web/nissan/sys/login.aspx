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

    protected int season = 0;
    void Page_Load(object sender, EventArgs e)
    {
        season = ConvertHelper.StrToInt(Request["season"]);
    }
</script>

<!DOCTYPE html>

<html>

	<head>
		<meta charset="utf-8">
		<meta http-equiv="X-UA-Compatible" content="IE=edge">
		<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
		<title>登录</title>
		<link rel="stylesheet" href="plugins/layui/css/layui.css" media="all" />
		<link rel="stylesheet" href="css/login.css" />
        <script type="text/javascript" src="../dongfengnissan/js/jquery-1.9.1.min.js"></script>
	</head>

	<body class="beg-login-bg">
		<div class="beg-login-box">
			<header>
				<h1>后台登录</h1>
			</header>
			<div class="beg-login-main">
					<div class="layui-form-item">
						<label class="beg-login-icon">
                        <i class="layui-icon">&#xe612;</i>
                    </label>
						<input id="field_name" type="text" name="userName" lay-verify="userName" autocomplete="off" placeholder="这里输入登录名" class="layui-input">
					</div>
					<div class="layui-form-item">
						<label class="beg-login-icon">
                        <i class="layui-icon">&#xe642;</i>
                    </label>
                        <input id="field_pw" type="password" name="password" lay-verify="password" autocomplete="off" placeholder="这里输入密码" class="layui-input">
					</div>
					<div class="layui-form-item">
						<div class="beg-pull-left beg-login-remember">
							<label>记住帐号？</label>
							<input type="checkbox" name="rememberMe" value="true" lay-skin="switch" checked title="记住帐号">
						</div>
						<div class="beg-pull-right">
							<button class="layui-btn layui-btn-primary" lay-filter="login" id="loginBtn">
                            <i class="layui-icon">&#xe650;</i> 登录
                        </button>
						</div>
						<div class="beg-clear"></div>
					</div>
			</div>
			<footer>
				<p></p>
			</footer>
		</div>
		<script type="text/javascript" src="plugins/layui/layui.js"></script>
	<script>
	    //layui.use(['layer', 'form'], function() {
	    //    var layer = layui.layer,
	    //        $ = layui.jquery,
	    //        form = layui.form();

	    //    form.on('submit(login)', function(data) {

	    //        location.href = 'index.html';
	    //        return false;
	    //    });
	    //});
	</script>

	<script>
        $('#loginBtn').click(function() {
            var req = {
                name: $('#field_name').val(),
                pw: $('#field_pw').val(),
                season:<%=season%>,
            };
	        $.ajax({
	            url: '/nissan/sys/loginpost.aspx?r=' + Math.random(),
	            data: {
	                data: JSON.stringify(req)
	            },
	            dataType: 'json',
	            type: "post",
	            success: function (d) {
	                if (d.success == 0) {
	                    location.href = "admin.aspx?season=<%=season%>";
	                } else {
	                    alert('账号密码不正确');
	                }
	            },
	            error: function (e, d) {
	                alert("登录失败");
	            }
	        });
	    });
    </script>
	</body>

</html>