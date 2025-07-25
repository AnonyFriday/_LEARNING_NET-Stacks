using System.Runtime.Serialization;

namespace Gender.SoapApiServices.DuyVK.SoapModelExtensions
{
    [DataContract]
    public sealed record LoginRequest
    {
        [DataMember(Order = 0)]
        public string UserName { get; init; }

        [DataMember(Order = 1)]
        public string Password { get; init; }
    }
}
