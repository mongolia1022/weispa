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
<title>视频播放</title>
<meta content="" name="Keywords">
<meta content="" name="Description">
<meta name="format-detection" content="telephone=no">
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

<link rel="stylesheet" type="text/css" href="jwplayer/style.css" />
<style>
.jwlogo{display:none;}
.jwplayer{position:fixed; top:0; left:0; z-index:99999; filter:alpha(opacity=1);-moz-opacity: 1;opacity: 1;}
.video_bg{width:100%; position:fixed; top:0; left:0; background:url(images/bg.jpg) center top no-repeat; background-size:100%; z-index:9999999999;}
.video_end{width:100%; position:fixed; top:0; left:0; background:url(images/qr.gif) #bce2f9 center top no-repeat; background-size:100%; z-index:9999999999; display:none;}
.video_end a{width:100%; height:inherit; display:block;}
</style>

<!--JQ基础文件-->
<script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
</head>
<body>



<div class="video_end" style="display:block;"><a href="http://mkf.countrygarden.com.cn/crm/page/ebei/ownerWeixin/index.html?projectType=53&code=071UyQhd06v9yu13aJjd0qPbid0UyQhd&state=code&from=groupmessage&isappinstalled=0"></a></div>


<script type="text/javascript">
    var w_h = $(window).height();
    var w_w = $(window).width();
    $('.video_end').height(w_h);
    $('.video_end a').height(w_h);
</script>

</body>
</html>