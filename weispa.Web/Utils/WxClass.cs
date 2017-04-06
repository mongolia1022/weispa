using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace com.weispa.Web.Util
{
    /// <summary>
    /// 微信请求类
    /// </summary>
    public class RequestXML
    {
        private string toUserName = "";
        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号
        /// </summary>
        public string ToUserName
        {
            get { return toUserName; }
            set { toUserName = value; }
        }

        private string fromUserName = "";
        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName
        {
            get { return fromUserName; }
            set { fromUserName = value; }
        }

        private string createTime = "";
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        private string msgType = "";
        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image
        /// </summary>
        public string MsgType
        {
            get { return msgType; }
            set { msgType = value; }
        }

        private string content = "";
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// 消息Id，64位整型[主要用于排重]
        /// </summary>
        public long MsgId { get; set; }

        private string location_X = "";
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Location_X
        {
            get { return location_X; }
            set { location_X = value; }
        }

        private string location_Y = "";
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y
        {
            get { return location_Y; }
            set { location_Y = value; }
        }

        private string scale = "";
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private string label = "";
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        private string picUrl = "";
        /// <summary>
        /// 图片链接，开发者可以用HTTP GET获取
        /// </summary>
        public string PicUrl
        {
            get { return picUrl; }
            set { picUrl = value; }
        }
        private string eventType = "";
        /// <summary>
        /// 事件类型
        /// 带参二维码：未关注（event：subscribe :qrscene_scenid）
        /// </summary>
        public string EventType
        {
            get { return eventType; }
            set { eventType = value; }
        }
        private string eventKey = "";
        /// <summary>
        /// 事件key值
        /// </summary>
        public string EventKey
        {
            get { return eventKey; }
            set { eventKey = value; }
        }
    }

    /// <summary>
    /// 微信异步调用数据模板
    /// </summary>
    public class WeChatInvokerStateData
    {
        /// <summary>
        /// 微信用户标识
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 帖子Id
        /// </summary>
        public int TId { get; set; }

        /// <summary>
        /// 延迟执行时间
        /// </summary>
        public TimeSpan? DelayActionTime { get; set; }
    }
    /// <summary>
    /// 微信用户信息
    /// </summary>
    public class WechatUserInfo
    {
        public string OpenId { get; set; }
        public string Uinionid { get; set; }

        public string Nickname { get; set; }
    }
    public class BaseMsg
    {
        public long MsgId { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class WeiXinAccessTokenModel
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 授权用户唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string scope { get; set; }
    }

    #region jssdk实体类
    public class SignPackage
    {
        /*
        wx.config({
         debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
         appId: '', // 必填，公众号的唯一标识
         timestamp: , // 必填，生成签名的时间戳
         nonceStr: '', // 必填，生成签名的随机串
         signature: '',// 必填，签名，见附录1
         jsApiList: [] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
        }); 
         */
        public bool debug { get; set; }
        public string appId { get; set; } // 必填，公众号的唯一标识
        public long timestamp { get; set; } // 必填，生成签名的时间戳
        public string nonceStr { get; set; } // 必填，生成签名的随机串
        public string signature { get; set; }// 必填，签名，见附录1
        public string[] jsApiList { get; set; } // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
    }
    [Flags]
    public enum JsApiEnum : ulong
    {
        //分享到朋友圈
        onMenuShareTimeline = 0x1,
        onMenuShareAppMessage = 0x2,//分享给朋友
        onMenuShareQQ = 0x4,
        onMenuShareWeibo = 0x8,
        startRecord = 0x10,
        stopRecord = 0x20,
        onVoiceRecordEnd = 0x40,
        playVoice = 0x80,
        pauseVoice = 100,
        stopVoice = 0x200,
        onVoicePlayEnd = 0x400,
        uploadVoice = 0x800,
        downloadVoice = 0x1000,
        chooseImage = 0x2000,
        previewImage = 0x4000,
        uploadImage = 0x8000,
        downloadImage = 0x10000,
        translateVoice = 0x20000,
        getNetworkType = 0x40000,
        openLocation = 0x80000,
        getLocation = 0x100000,
        hideOptionMenu = 0x200000,
        showOptionMenu = 0x400000,
        hideMenuItems = 0x800000,
        showMenuItems = 0x1000000,
        hideAllNonBaseMenuItem = 0x2000000,
        showAllNonBaseMenuItem = 0x4000000,
        closeWindow = 0x8000000,
        scanQRCode = 0x10000000,
        chooseWXPay = 0x20000000,
        openProductSpecificView = 0x40000000,
        addCard = 0x80000000,
        chooseCard = 0x100000000,
        openCard = 0x200000000
    }
    public class TimeStamp
    {
        public static long Now()
        {
            return (long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
        public static DateTime ToDateTime(long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
        }
        public static string ToDateTimeString(long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp).ToString();
        }
        public static string ToDateTimeString(long timestamp, string format)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp).ToString(format);
        }

    }
    /// <summary>
    /// 签名类
    /// </summary>
    public class Signature
    {
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="data">参与签名的参数字典</param>
        /// <returns>签名</returns>
        public string Sign(Dictionary<string, string> data)
        {
            var dataList = data.ToList();

            dataList.Sort(ParameterKeyComparison);
            var queryString = dataList.Aggregate(string.Empty, (query, item) => string.Concat(query, "&", item.Key, "=", item.Value)).TrimStart('&');

            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                var hashed = sha1.ComputeHash(Encoding.Default.GetBytes(queryString));
                return HexStringFromBytes(hashed);
            }
        }
        static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        static int ParameterKeyComparison(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            return x.Key.CompareTo(y.Key);
        }
    }
    #endregion
}