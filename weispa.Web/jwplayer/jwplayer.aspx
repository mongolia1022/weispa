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
<meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, user-scalable=no" />
<meta name="renderer" content="webkit">
<meta http-equiv="X-UA-Compatible" content="IE=Edge">
<title>Hi~ o(*￣▽￣*)ブ，您有一个暖冬礼物待接收</title>
<meta content="嘘！别让太多人知道" name="Keywords">
<meta content="嘘！别让太多人知道" name="Description">
<meta name="format-detection" content="telephone=no">

<link rel="stylesheet" type="text/css" href="jwplayer/style.css" />
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript">
        wx.config(<%=configstr%>);
        wx.ready(function () {
            var share_link = 'http://www.lwgjjd.com/jwplayer/jwplayer.aspx';
            var title = 'Hi~ o(*￣▽￣*)ブ，您有一个暖冬礼物待接收';
            var imgurl = 'http://www.lwgjjd.com/jwplayer/images/bg2.jpg';
            wx.onMenuShareAppMessage({
                title: title,
                desc: '嘘！别让太多人知道',
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
<style>
.jwlogo{display:none;}
.jwplayer{position:fixed; top:0; left:0; z-index:99999; filter:alpha(opacity=1);-moz-opacity: 1;opacity: 1;}
.video_bg{width:100%; position:fixed; top:0; left:0; background:url(images/bg2.jpg) center top no-repeat; background-size:100%; z-index:9999999999;}
.video_end{width:100%; position:fixed; top:0; left:0; background:url(images/qr2.gif) #bce2f9 center top no-repeat; background-size:100%; z-index:9999999999; display:none;}
.video_end a{width:100%; height:inherit; display:block;}
</style>

<!--JQ基础文件-->
<script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
</head>
<body>

<div id="mediaplayer"></div> 
<div class="video_bg"></div>
<div class="video_end"><a href="https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzA5MTkyNTQzNQ==&scene=124#wechat_redirect"></a></div>

<script src="jwplayer/jwplayer.js"></script>
<script src="jwplayer/jwpsrv.js"></script>
<script type="text/javascript">
    var w_h = $(window).height();
    var w_w = $(window).width();
    $('.video_bg').height(w_h);
    $('.video_end').height(w_h);
    $('.video_end a').height(w_h);
    jwplayer('mediaplayer').setup({
        'flashplayer': 'jwplayer/jwplayer.flash.swf',
        'image': '',
        'id': 'playerID',
        'width': w_w,
        'height': w_h,
        'file': 'http://www.lwgjjd.com.img.800cdn.com/jwplayer/movie/video.mp4',
        'autostart': 'false',
        'repeat': 'false',
    });

    $('.video_bg').click(function () {
        $(this).hide();
        /*jwplayer().setFullscreen(true);*/
        jwplayer('mediaplayer').play();

    });

    $(function () {
        jwplayer('mediaplayer').onComplete(
            function () {
                $('.video_end').show();
            }
        )

    });

</script>
</body>
</html>