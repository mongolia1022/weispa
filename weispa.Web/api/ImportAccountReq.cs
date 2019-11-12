namespace com.weispa.Web.api
{
    public class ImportAccountReq
    {
        private string _faceUrl="";
        public string Identifier { get; set; }
        
        public string Nick { get; set; }

        public string FaceUrl
        {
            get => _faceUrl;
            set => _faceUrl = value;
        }

        public string Gender { get; set; }
    }
}