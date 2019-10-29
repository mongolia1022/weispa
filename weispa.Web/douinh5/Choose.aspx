﻿<%@ Page Language="C#" %>
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
<meta name="format-detection" content="telephone=no">
<meta name="format-detection" content="email=no">

<title>抖in广州</title>
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
<link rel="stylesheet" type="text/css" href="css/style.css" />
<link rel="stylesheet" type="text/css" href="css/css.css" />
<link rel="stylesheet" type="text/css" href="css/iconfont.css" />
<link rel="stylesheet" type="text/css" href="css/animate.min.css" />

<!--JQ基础文件-->
<script type="text/javascript" src="js/jquery-3.3.1.min.js"></script>

<!-- Link Swiper's CSS -->
<link rel="stylesheet" href="dist/css/swiper.min.css">
<style>
.swiper-container {
	width: 100%;
}
.swiper-slide {
	background-position: center;
	background-size: cover;
	width: 38vh;
	border:2px solid #fff;
	border-radius:2vw;
	overflow:hidden;

}
.swiper-container-horizontal>.swiper-pagination-bullets, .swiper-pagination-custom, .swiper-pagination-fraction {
	bottom: 7%;
	left: 0;
	width: 100%
}
</style>


</head>

<body class="choose_bg">

<!--背景音乐-->
<audio style="display:none; height: 0" id="bg-music" preload="auto" src="music/ai1.mp3" ></audio>
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

<!--选择相框-->
<div class="choose_moon wow fadeIn" data-wow-delay="0s"><img src="images/choose_moon.png" width="100%" /></div>
<div class="choose_title1 wow fadeInDown" data-wow-delay="1s"><img src="images/choose_title1.png" width="100%" /></div>
<div class="choose_title2 wow fadeInLeft" data-wow-delay="1.5s"><img src="images/choose_title2.png" width="100%" /></div>

<div class="choose_swiper wow fadeInRight" data-wow-delay="2s">
    <!-- Swiper -->
    <div class="swiper-container">
        <div class="swiper-wrapper">
            <div class="swiper-slide" frame-name="frame1">
            	<img src="images/choose_cp1.jpg" width="100%" />
          	</div>
            <div class="swiper-slide" frame-name="frame2">
            	<img src="images/choose_cp2.jpg" width="100%" />
          	</div>
            <div class="swiper-slide" frame-name="frame3">
            	<img src="images/choose_cp3.jpg" width="100%" />
          	</div>
        </div>
        <!-- Add Pagination
        <div class="swiper-pagination"></div>
         -->
    </div>
    
    <!-- Swiper JS -->
    <script src="dist/js/swiper.min.js"></script>
    
    <!-- Initialize Swiper -->
    <script>
    var swiper = new Swiper('.swiper-container', {
        pagination: '.swiper-pagination',
        effect: 'coverflow',
        grabCursor: true,
        centeredSlides: true,
        slidesPerView: 'auto',
        coverflow: {
            rotate: 50,
            stretch: 0,
            depth: 100,
            modifier: 1,
            slideShadows : true
        }
    });
    </script>
</div>

<div class="choose_btn wow fadeInUp" data-wow-delay="2.5s" data-wow-offset="-100"><a id="chooseBtn"><img src="images/choose_btn.png" width="100%" /></a></div>


<!--wow.js-->
<script type="text/javascript" src="js/wow.js"></script>
<script>
(function() {
  var Util,
    __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; };

  Util = (function() {
    function Util() {}

    Util.prototype.extend = function(custom, defaults) {
      var key, value;
      for (key in custom) {
        value = custom[key];
        if (value != null) {
          defaults[key] = value;
        }
      }
      return defaults;
    };

    Util.prototype.isMobile = function(agent) {
      return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(agent);
    };

    return Util;

  })();

  this.WOW = (function() {
    WOW.prototype.defaults = {
      boxClass: 'wow',
      animateClass: 'animated',
      offset: 0,
      mobile: true
    };

    function WOW(options) {
      if (options == null) {
        options = {};
      }
      this.scrollCallback = __bind(this.scrollCallback, this);
      this.scrollHandler = __bind(this.scrollHandler, this);
      this.start = __bind(this.start, this);
      this.scrolled = true;
      this.config = this.util().extend(options, this.defaults);
    }

    WOW.prototype.init = function() {
      var _ref;
      this.element = window.document.documentElement;
      if ((_ref = document.readyState) === "interactive" || _ref === "complete") {
        return this.start();
      } else {
        return document.addEventListener('DOMContentLoaded', this.start);
      }
    };

    WOW.prototype.start = function() {
      var box, _i, _len, _ref;
      this.boxes = this.element.getElementsByClassName(this.config.boxClass);
      if (this.boxes.length) {
        if (this.disabled()) {
          return this.resetStyle();
        } else {
          _ref = this.boxes;
          for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            box = _ref[_i];
            this.applyStyle(box, true);
          }
          window.addEventListener('scroll', this.scrollHandler, false);
          window.addEventListener('resize', this.scrollHandler, false);
          return this.interval = setInterval(this.scrollCallback, 50);
        }
      }
    };

    WOW.prototype.stop = function() {
      window.removeEventListener('scroll', this.scrollHandler, false);
      window.removeEventListener('resize', this.scrollHandler, false);
      if (this.interval != null) {
        return clearInterval(this.interval);
      }
    };

    WOW.prototype.show = function(box) {
      this.applyStyle(box);
      return box.className = "" + box.className + " " + this.config.animateClass;
    };

    WOW.prototype.applyStyle = function(box, hidden) {
      var delay, duration, iteration;
      duration = box.getAttribute('data-wow-duration');
      delay = box.getAttribute('data-wow-delay');
      iteration = box.getAttribute('data-wow-iteration');
      return box.setAttribute('style', this.customStyle(hidden, duration, delay, iteration));
    };

    WOW.prototype.resetStyle = function() {
      var box, _i, _len, _ref, _results;
      _ref = this.boxes;
      _results = [];
      for (_i = 0, _len = _ref.length; _i < _len; _i++) {
        box = _ref[_i];
        _results.push(box.setAttribute('style', 'visibility: visible;'));
      }
      return _results;
    };

    WOW.prototype.customStyle = function(hidden, duration, delay, iteration) {
      var style;
      style = hidden ? "visibility: hidden; -webkit-animation-name: none; -moz-animation-name: none; animation-name: none;" : "visibility: visible;";
      if (duration) {
        style += "-webkit-animation-duration: " + duration + "; -moz-animation-duration: " + duration + "; animation-duration: " + duration + ";";
      }
      if (delay) {
        style += "-webkit-animation-delay: " + delay + "; -moz-animation-delay: " + delay + "; animation-delay: " + delay + ";";
      }
      if (iteration) {
        style += "-webkit-animation-iteration-count: " + iteration + "; -moz-animation-iteration-count: " + iteration + "; animation-iteration-count: " + iteration + ";";
      }
      return style;
    };

    WOW.prototype.scrollHandler = function() {
      return this.scrolled = true;
    };

    WOW.prototype.scrollCallback = function() {
      var box;
      if (this.scrolled) {
        this.scrolled = false;
        this.boxes = (function() {
          var _i, _len, _ref, _results;
          _ref = this.boxes;
          _results = [];
          for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            box = _ref[_i];
            if (!(box)) {
              continue;
            }
            if (this.isVisible(box)) {
              this.show(box);
              continue;
            }
            _results.push(box);
          }
          return _results;
        }).call(this);
        if (!this.boxes.length) {
          return this.stop();
        }
      }
    };

    WOW.prototype.offsetTop = function(element) {
      var top;
      top = element.offsetTop;
      while (element = element.offsetParent) {
        top += element.offsetTop;
      }
      return top;
    };

    WOW.prototype.isVisible = function(box) {
      var bottom, offset, top, viewBottom, viewTop;
      offset = box.getAttribute('data-wow-offset') || this.config.offset;
      viewTop = window.pageYOffset;
      viewBottom = viewTop + this.element.clientHeight - offset;
      top = this.offsetTop(box);
      bottom = top + box.clientHeight;
      return top <= viewBottom && bottom >= viewTop;
    };

    WOW.prototype.util = function() {
      return this._util || (this._util = new Util());
    };

    WOW.prototype.disabled = function() {
      return !this.config.mobile && this.util().isMobile(navigator.userAgent);
    };

    return WOW;

  })();

}).call(this);


wow = new WOW(
  {
    animateClass: 'animated',
    offset: 100
  }
);
wow.init();

$(function(){
    var aTag = $("#chooseBtn");
    aTag.click(function(){
        var activeDiv = $(".swiper-slide-active");
        if (activeDiv.length == 0) {
            return;
        }

        location.href="Combine.aspx?frame=" + activeDiv.attr('frame-name');
    });
});
</script>

</body>
</html>

