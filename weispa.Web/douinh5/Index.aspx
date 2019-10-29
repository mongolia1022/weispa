<%@ Page Language="C#" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="com.weispa.Web.Util" %>
<script   language="C#"   runat="server">
    private string configstr;
    void   Page_Load(object   sender,   EventArgs   e)   
    {
        WxHelper wxhelper = new WxHelper();
        SignPackage config = wxhelper.GetSignPackage(JsApiEnum.onMenuShareAppMessage | JsApiEnum.onMenuShareTimeline, false);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        configstr = serializer.Serialize(config);
    }   
  </script>
<!doctype html>
<html>
<head>
<meta charset="utf-8">

<meta name="format-detection" content="telephone=no">
<meta name="format-detection" content="email=no">
<meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1">
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
        <script type="text/javascript">
            wx.config(<%=configstr%>);
            wx.ready(function () {
                var share_link = 'http://www.lwgjjd.com/city/index.aspx';
                var title = '古镇新颜，印象新桥';
                var imgurl = 'http://www.lwgjjd.com/city/images/yxq.png';
                wx.onMenuShareAppMessage({
                    title: title,
                    desc: '深圳市宝安区新桥街道办事处',
                    link: share_link,
                    imgUrl: imgurl
                });
    
                wx.onMenuShareTimeline({
                    title: title,
                    link: share_link,
                    imgUrl: imgurl,
                });
            });
            wx.error(function (res) {
                //alert("接口验证失败，详细信息：\n" + JSON.stringify(res));
            });
        </script>
<script src="js/flexible.js"></script>
<link rel="stylesheet" type="text/css" href="css/flexible.css" />

<title>抖in广州</title>
<link rel="stylesheet" type="text/css" href="css/style.css" />
<link rel="stylesheet" type="text/css" href="css/css.css" />
<link rel="stylesheet" type="text/css" href="css/iconfont.css" />
<link rel="stylesheet" type="text/css" href="css/animate.min.css" />

<!--JQ基础文件-->
<script type="text/javascript" src="js/jquery-3.3.1.min.js"></script>

<!--wow.js-->
<script type="text/javascript" src="js/wow.js"></script>
<script>
var wow = new WOW({  
    boxClass: 'wow',  
    animateClass: 'animated',  
    offset: 0,  
    mobile: true,  
    live: true  
});  
wow.init();

$(function(){
    $.ajax({
        url:"/server/openCount",
        dataType:"json",
        async:true,
        type:"GET",
        success:function(req){
        },
        error:function(){
        }
    });
});
</script>


</head>

<body>

<!--背景音乐-->
<audio style="display:none; height: 0" id="bg-music" preload="auto" src="music/bgm.mp3" loop></audio>
<div id="music_btn"></div>
<script>
document.addEventListener('DOMContentLoaded', function (){
    function audioAutoPlay() {
        var audio = document.getElementById('bg-music');
        audio.play();
        document.addEventListener("WeixinJSBridgeReady", function () {
            audio.play();
        }, false);
    }
    audioAutoPlay();
    
});

$('body').find("audio").attr('id', 'bg-music')
var myVid = document.getElementById("bg-music");
$('#music_btn').click(function() {
//here "sound-icon" is a anchor class. 
var sta = myVid.muted;
if (sta == true) {
myVid.muted = false;
$('#music_btn').css('background-position','center top');
} else {
myVid.muted = true;
$('#music_btn').css('background-position','center bottom');
}
})
</script> 

<!--长图-->
<div class="pic clear">
    <img src="images/pic1.jpg" />
</div>

<div class="entrance_btn ">
    <a href="Choose.aspx"><img src="images/entrance_btn.png" /></a>
</div>

<div class="idx_finger animated fadeInUp"><img src="images/idx_finger.png" /></div>

<!--手势提示-->
<script>
$(function(){
	setTimeout(function(){
		$('.idx_finger').hide();
		$('.entrance_btn').fadeIn();
		$('.entrance_btn').addClass('animated pulse');
	}, 2800);
	$(window).scroll(function(event){
		$('.idx_finger').hide();
		$('.entrance_btn').fadeIn();
		$('.entrance_btn').addClass('animated pulse');
    });
});
</script>


</body>
</html>

