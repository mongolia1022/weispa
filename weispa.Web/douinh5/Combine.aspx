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

<meta name="format-detection" content="telephone=no">
<meta name="format-detection" content="email=no">
<meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1">

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


</head>

<body>

<!--背景音乐-->
<audio style="display:none; height: 0" id="bg-music" preload="auto" src="music/ai2.mp3" ></audio>
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
$(function(){
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
});
</script> 

<!--合成前-->
<div class="combine">
	<div class="combine_photo clear wow fadeInDown" data-wow-delay="0.1s">
    	<img id="frameImg" class="frame" src="images/frame1.png" />
    	<img class="photo" src="images/photo.jpg" />
    </div>
    <div class="combine_btn wow fadeInUp" data-wow-delay="0.6s">
    	<a class="upload_pic" href="###"><img src="images/combine_btn0.png" /></a>
    	<a class="submit_photo" href="###"><img class="submit_photo" src="images/combine_btn2.png" /></a>
    </div>
    <!--展示内容-->
	<script>
    $(function(){
        setTimeout(function(){
            $('.combine_photo,.combine_btn').show();
        }, 100);
    });
    </script>
</div>

<!--图片裁剪-->
<div class="clipbg displaynone">
    <div id="clipArea"></div>
        <div class="loading displaynone">正在载入图片...</div>
        <div class="footer">
        <dl>
            <dd style="background: #fe1041; color: #ffffff;border: none;">打开相册<input type="file" id="file" accept="image/*" ></dd>
            <dd id="clipBtn">完成裁剪</dd>
        </dl>
        <div class="back">取消</div>
    </div>
</div>

<!--合成后-->
<div class="combine_finished">
	<div class="combine_photo">
    	<img id="combineImg" src="images/photo_finished.jpg" />
    </div>
    <div class="combine_finished_btn">
    	<a href="Index.aspx"><img src="images/combine_finished_btn.png" /></a>
    </div>
    <div class="combine_av">
    	<img src="images/combine_av.png" />
    </div>
    <div class="combine_finger animated pulse"><img src="images/combine_finger.png" /></div>
</div>

<canvas id="canvas" width="1080" height="1920" style='display:none'></canvas>

<!--弹出合成后界面-->
<script>
$(function(){
	$('.submit_photo').click(function(){
		$('.combine_finished').fadeIn();
		setTimeout(function(){
			$('.combine_finger').fadeOut();
		}, 5000);
	});
});
</script>

<!--图片裁剪-->
<link rel="stylesheet" type="text/css" href="css/aui.css"/>
<link rel="stylesheet" href="css/intial.css" />
<script type="text/javascript" src="js/camera.js/hammer.min.js" ></script>
<script type="text/javascript" src="js/camera.js/lrz.all.bundle.js" ></script>
<script type="text/javascript" src="js/camera.js/iscroll-zoom-min.js" ></script>
<script type="text/javascript" src="js/camera.js/PhotoClip.js" ></script>
<script>
$(function(){
	$(".upload_pic").click(function(){
		$(".clipbg").fadeIn();
	})
	var clipArea = new  PhotoClip("#clipArea", {
			//size: [300, 300],//裁剪框大小
			size: [300, 533],//裁剪框大小
			outputSize:[0,0],//打开图片大小，[0,0]表示原图大小
			file: "#file",
			ok: "#clipBtn",
			loadStart: function() { //图片开始加载的回调函数。this 指向当前 PhotoClip 的实例对象，并将正在加载的 file 对象作为参数传入。（如果是使用非 file 的方式加载图片，则该参数为图片的 url）
				$(".loading").removeClass("displaynone");

			},
			loadComplete: function() {//图片加载完成的回调函数。this 指向当前 PhotoClip 的实例对象，并将图片的 <img> 对象作为参数传入。
				$(".loading").addClass("displaynone");

			},
			done: function(dataURL) { //裁剪完成的回调函数。this 指向当前 PhotoClip 的实例对象，会将裁剪出的图像数据DataURL作为参数传入。			
				console.log(dataURL);//dataURL裁剪后图片地址base64格式提交给后台处理
				$(".clipbg").fadeOut()
				$('.photo').attr('src',dataURL); 
				$('.upload_pic img').attr('src','images/combine_btn1.png');
                newImage($('#frameImg').attr('src'),dataURL)
            }
		});
		$(".back").click(function(){
			$(".clipbg").fadeOut()
		})
		});
</script>


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

function GetQueryString(name)
{
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return  unescape(r[2]); return null;
}

$(function(){
    var frameName=GetQueryString('frame');
    console.log("images/"+frameName+".jpg");
    $('#frameImg').attr('src',"images/"+frameName+".png");

    $(function(){
        $.ajax({
            url:"/server/useCount?frameName="+frameName,
            dataType:"json",
            async:true,
            type:"GET",
            success:function(req){
            },
            error:function(){
            }
        });
    });
});

function newImage(bg,photo) {
    // 生成图片
    var imageBox = document.getElementById("combineImg")
    var canvas = document.getElementById("canvas")
    var cxt = canvas.getContext("2d")
    var img = new Image()
    img.src = photo
    img.setAttribute("crossOrigin",'anonymous');
    // 图片加载完成，才可处理
    img.onload = () => {
        // 画图(这里画布与图片等宽高)
        cxt.drawImage(img, 0,0,1080,1920)

        // 画第二张图
        draw(bg)
    }

    function draw(bg) {
        var img2 = new Image()
        img2.src = bg
        img2.setAttribute("crossOrigin",'anonymous');
        img2.onload = () => {
            cxt.drawImage(img2, 0,0,1080,1920)
            imageBox.src = canvas.toDataURL("image/jpg")
            $('.clipbg').remove();
        }
    }
}
</script>


</body>
</html>


