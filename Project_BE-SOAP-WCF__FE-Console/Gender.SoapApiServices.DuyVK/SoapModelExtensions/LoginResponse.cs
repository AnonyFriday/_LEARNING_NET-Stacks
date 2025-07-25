using System.Runtime.Serialization;

namespace Gender.SoapApiServices.DuyVK.SoapModelExtensions
{
    [DataContract]
    public class LoginResponse
    {
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public int ExpiresIn { get; set; }
    }
}