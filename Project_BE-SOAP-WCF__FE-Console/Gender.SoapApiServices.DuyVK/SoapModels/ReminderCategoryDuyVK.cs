using System.Runtime.Serialization;

namespace Gender.SoapApiServices.DuyVK.SoapModels
{
    [DataContract]
    public partial class ReminderCategoryDuyVK
    {
        [DataMember(Order = 1)]
        public int ReminderCategoryDuyVKid { get; set; }

        [DataMember(Order = 2)]
        public string Code { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        public string Description { get; set; }

        [DataMember(Order = 5)]
        public bool? IsActive { get; set; }

        [DataMember(Order = 6)]
        public int? PriorityLevel { get; set; }

        [DataMember(Order = 7)]
        public int? DefaultOffset { get; set; }

        [DataMember(Order = 8)]
        public string ColorCode { get; set; }

        [DataMember(Order = 9)]
        public DateTime? CreatedAt { get; set; }

        [DataMember(Order = 10)]
        public DateTime? UpdatedAt { get; set; }

        [IgnoreDataMember]
        public List<MenstrualCycleReminderDuyVK> MenstrualCycleReminderDuyVKs { get; set; } = new List<MenstrualCycleReminderDuyVK>();
    }
}
