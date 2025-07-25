using System.Runtime.Serialization;

namespace Gender.SoapApiServices.DuyVK.SoapModelExtensions
{
    [DataContract]
    public class SearchRequest
    {
        [DataMember(Order = 1)]
        public int? CurrentPage { get; set; } = 1;

        [DataMember(Order = 2)]
        public int? PageSize { get; set; } = 5;
    }

    [DataContract]
    public class MenstrualCycleReminderSearchRequest : SearchRequest
    {
        [DataMember(Order = 3)]
        public string? Title { get; set; }

        [DataMember(Order = 4)]
        public string? ColorCode { get; set; }

        [DataMember(Order = 5)]
        public double? ImportanceScore { get; set; }
    }
}
