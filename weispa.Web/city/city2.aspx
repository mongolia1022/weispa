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
<meta content="width=device-width, initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" name="viewport" />
<title>蝶变：新桥儿女多壮志，敢叫日月换新天</title>
<link rel="stylesheet" type="text/css" href="css/style.css" />
<link rel="stylesheet" type="text/css" href="css/css.css" />
<link rel="stylesheet" type="text/css" href="iconfont/iconfont.css" />
<link rel="stylesheet" type="text/css" href="css/animate.min.css" />
<script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript">
        wx.config(<%=configstr%>);
        wx.ready(function () {
            var share_link = 'http://www.lwgjjd.com/city/index.aspx';
            var title = '蝶变：新桥儿女多壮志，敢叫日月换新天';
            var imgurl = 'http://www.lwgjjd.com/city/images/yxq.png';
            wx.onMenuShareAppMessage({
                title: title,
                desc: '蝶变：新桥儿女多壮志，敢叫日月换新天',
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
<!--JQ基础文件-->
<script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>

<!--JQ MOBILE
<script type="text/javascript" src="js/jquery.mobile-1.4.5.min.js"></script>-->

<!--IE6 PNG透明-->
<!--[if lte IE 6]>
<script src="js/DD_belatedPNG_0.0.8a.js" type="text/javascript"></script>
    <script type="text/javascript">
        DD_belatedPNG.fix('div, ul, img, li, input , a');
    </script>
<![endif]-->

<!-- Link Swiper's CSS -->
<link rel="stylesheet" href="dist/css/swiper.min.css">

<script>
    $(document).ready(function () {
        var w_h = $(window).height();
        var w_w = $(window).width();
        $('.swp_01').height(w_h);
        $('.swp_01').width(w_w);
        $('.swp_02').height(w_h);
        $('.swp_02').width(w_w);
        $('.swp_03').height(w_h);
        $('.swp_03').width(w_w);
        $('.swp_04').height(w_h);
        $('.swp_04').width(w_w);
        $('.swp_05').height(w_h);
        $('.swp_05').width(w_w);
        $('.swp_06').height(w_h);
        $('.swp_06').width(w_w);
        $('.swp_07').height(w_h);
        $('.swp_07').width(w_w);
        $('.swp_08').height(w_h);
        $('.swp_08').width(w_w);
        $('.swp_09').height(w_h);
        $('.swp_09').width(w_w);
        $('.swp_10').height(w_h);
        $('.swp_10').width(w_w);
    });
</script>

</head>

<body>

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

<!-- Swiper -->
<div class="swiper-container">
    <div class="swiper-wrapper">
        <div class="swiper-slide">
        	<div class="go_back"><a href="city.aspx"></a></div>
        
        	<div class="swp_07 animated" style="background:url(images/201801/7.jpg) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_01 animated bounce	" style="background:url(images/201801/4.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_02 animated swp_in1 block pulse" style="background:url(images/201801/2.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_04 animated" style="background:url(images/201801/1.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_05 animated" style="background:url(images/201801/5.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_06 animated" style="background:url(images/201801/6.png) left top no-repeat; background-size:100% 100%;"></div>
            
        </div>
        <div class="swiper-slide">
        
        	<div class="swp_04 animated" style="background:url(images/201802/4.jpg) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_01 animated" style="background:url(images/201802/1.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_02 animated swp_in2" style="background:url(images/201802/2.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_03 animated" style="background:url(images/201802/3.png) left top no-repeat; background-size:100% 100%;"></div>
            
        </div>
        <div class="swiper-slide">
        	
            <div class="swp_04 animated" style="background:url(images/201803/4.jpg) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_01 animated" style="background:url(images/201803/1.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_02 animated swp_in3" style="background:url(images/201803/2.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_03 animated" style="background:url(images/201803/3.png) left top no-repeat; background-size:100% 100%;"></div>
            
        </div>
        <div class="swiper-slide">
        	
            <div class="swp_04 animated" style="background:url(images/201804/4.jpg) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_01 animated" style="background:url(images/201804/1.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_02 animated swp_in4" style="background:url(images/201804/2.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_03 animated" style="background:url(images/201804/3.png) left top no-repeat; background-size:100% 100%;"></div>
            
        </div>
        <div class="swiper-slide">
        	
            <div class="swp_04 animated" style="background:url(images/201805/4.jpg) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_01 animated" style="background:url(images/201805/1.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_02 animated swp_in5" style="background:url(images/201805/2.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_03 animated" style="background:url(images/201805/3.png) left top no-repeat; background-size:100% 100%;"></div>
            
        </div>
        <div class="swiper-slide">
        	
            <div class="swp_04 animated" style="background:url(images/201806/4.jpg) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_01 animated" style="background:url(images/201806/1.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_02 animated swp_in6" style="background:url(images/201806/2.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_03 animated" style="background:url(images/201806/3.png) left top no-repeat; background-size:100% 100%;"></div>
            
        </div>
        <div class="swiper-slide">
        	
            <div class="swp_04 animated" style="background:url(images/201807/4.jpg) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_01 animated" style="background:url(images/201807/1.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_02 animated swp_in7" style="background:url(images/201807/2.png) left top no-repeat; background-size:100% 100%;"></div>
        	<div class="swp_03 animated" style="background:url(images/201807/3.png) left top no-repeat; background-size:100% 100%;"></div>
            
        </div>
        
    </div>
    
    <!-- Add Pagination -->
    <div class="swiper-pagination "></div>
    <!-- Add Arrows -->
    <div class="swiper-button-next"></div>
    <div class="swiper-button-prev"></div>
    
</div>
<script>
    $(function () {

        $(".swiper-button-next").addClass('animated fadeInLeft move');
    });

</script>


<!-- Swiper JS -->
<script src="dist/js/swiper.min.js"></script>

<!-- Initialize Swiper -->
<script>
    var swiper = new Swiper('.swiper-container', {
        pagination: '.swiper-pagination',
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
        paginationClickable: true,
        onSlideChangeEnd: function (swiper) {
            var spidx = swiper.activeIndex;
            $(".swiper-slide").eq(spidx).children('.swp_01').addClass('block animated bounce');
            $(".swiper-slide").eq(spidx).children('.swp_in1').addClass('block animated pulse');
            $(".swiper-slide").eq(spidx).children('.swp_in2').addClass('block animated flipInX');
            $(".swiper-slide").eq(spidx).children('.swp_in3').addClass('block animated rotateInDownLeft');
            $(".swiper-slide").eq(spidx).children('.swp_in4').addClass('block animated slideInLeft');
            $(".swiper-slide").eq(spidx).children('.swp_in5').addClass('block animated bounceInUp');
            $(".swiper-slide").eq(spidx).children('.swp_in6').addClass('block animated flipInY');
            $(".swiper-slide").eq(spidx).children('.swp_in7').addClass('block animated swing');

        },

        onSlideChangeStart: function (swp) {
            $(".swiper-button-next").removeClass('animated fadeInLeft move');
            $(".swp_01").removeClass('block animated bounce');
            $(".swp_in1").removeClass('block animated pulse');
            $(".swp_in2").removeClass('block animated flipInX');
            $(".swp_in3").removeClass('block animated rotateInDownLeft');
            $(".swp_in4").removeClass('block animated slideInLeft');
            $(".swp_in5").removeClass('block animated bounceInUp');
            $(".swp_in6").removeClass('block animated flipInY');
            $(".swp_in7").removeClass('block animated swing');


        },



    });

</script>





</body>
</html>
