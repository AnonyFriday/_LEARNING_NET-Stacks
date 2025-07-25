using System.Runtime.Serialization;

namespace Gender.SoapApiServices.DuyVK.SoapModels
{
    [DataContract]
    public partial class MenstrualCycleReminderDuyVK
    {
        [DataMember(Order = 1)]
        public int MenstrualCycleReminderDuyVKid { get; set; }

        [DataMember(Order = 2)]
        public int ReminderCategoryDuyVKid { get; set; }

        [DataMember(Order = 3)]
        public string Title { get; set; }

        [DataMember(Order = 4)]
        public string Note { get; set; }

        [DataMember(Order = 5)]
        public DateTime ReminderDate { get; set; }

        [DataMember(Order = 6)]
        public DateTime? SentAt { get; set; }

        [DataMember(Order = 7)]
        public bool? IsSent { get; set; }

        [DataMember(Order = 8)]
        public int? RepeatInterval { get; set; }

        [DataMember(Order = 9)]
        public double? ImportanceScore { get; set; }

        [DataMember(Order = 10)]
        public DateTime? CreatedAt { get; set; }

        [DataMember(Order = 11)]
        public DateTime? UpdatedAt { get; set; }

        [DataMember(Order = 12)]
        public ReminderCategoryDuyVK ReminderCategoryDuyVK { get; set; }
    }
}
