using System;
using System.Collections.Generic;
using System.Text;
using com.weispa.Web.api;
using com.weispa.Web.Utils;
using tencentyun;

namespace com.weispa.Web.Controllers
{
    public class TimApiController: BaseController
    {
        private int sdkappid = 1400276323;
        private string key = "22519898d6c4c67ad62b9dd44b11ba31929d205dfccd8391c37296c9308ecbcc";
        private string identifier = "administrator";

        private string apiTmp = "https://console.tim.qq.com/v4/{0}/{1}?sdkappid={2}&identifier={3}&usersig={4}&random={5}&contenttype=json";

        public string import(List<ImportAccountReq> reqs)
        {
            StringBuilder sb = new StringBuilder();
            string sign = genSign();
            foreach (var req in reqs)
            {
                string url = string.Format(apiTmp,"im_open_login_svc","account_import",sdkappid, identifier, sign, GetTimeStamp());
                sb.AppendLine(string.Format("{0}:{1}", req.Identifier, WeispaUtil.HttpPost(url, req)));
            }
            Console.Write(sb);
            return sb.ToString();
        }

        public string del(List<string> userIDs)
        {
//            StringBuilder sb = new StringBuilder();
//            string sign = genSign();
//            foreach (var userId in userIDs)
//            {
//                string url = string.Format(apiTmp,"im_open_login_svc", "account_delete", sdkappid, identifier, sign, GetTimeStamp());
//                sb.AppendLine(string.Format("{0}:{1}", userId, WeispaUtil.HttpPost(url,
//                    new
//                    {
//                        DeleteItem = new object[]
//                        {
//                            new {UserID=userId}
//                        }
//                    }
//                )));
//            }
    
            return "{\"ActionStatus\":\"OK\",\"ErrorCode\":0,\"ErrorInfo\":\"\"}";
        }

        public string update(List<ImportAccountReq> reqs)
        {
            StringBuilder sb = new StringBuilder();
            string sign = genSign();
            foreach (var req in reqs)
            {
                string url = string.Format(apiTmp,"profile","portrait_set",sdkappid, identifier, sign, GetTimeStamp());
                sb.AppendLine(string.Format("{0}:{1}", req.Identifier, WeispaUtil.HttpPost(url,
                    new
                    {
                        From_Account = req.Identifier,
                        ProfileItem = new object[]
                        {
                            new {Tag = "Tag_Profile_IM_Nick", Value = req.Nick},
                            new {Tag = "Tag_Profile_IM_Image", Value = req.FaceUrl},
                            new {Tag = "Tag_Profile_IM_Gender", Value = req.Gender}
                        }
                    }
                )));
                
            }
            return sb.ToString();
        }

        private string genSign()
        {
            TLSSigAPIv2 api = new TLSSigAPIv2(sdkappid, key);
            string sig = api.GenSig(identifier);
            return sig;
        }
        
        private string GetTimeStamp() 
        { 
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0); 
            return Convert.ToInt64(ts.TotalSeconds).ToString(); 
        } 
    }
}