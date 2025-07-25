using System.Runtime.Serialization;

namespace Gender.SoapApiServices.DuyVK.SoapModelExtensions
{
    [DataContract]
    public class PaginationResultResponse<T>
    {
        [DataMember]
        public T Items { get; set; }

        [DataMember]
        public int TotalPages { get; set; }

        [DataMember]
        public int TotalItems { get; set; }

        [DataMember]
        public int CurrentPage { get; set; }

        [DataMember]
        public int PageSize { get; set; }
    }
}
