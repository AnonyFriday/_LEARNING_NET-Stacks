using System.Runtime.Serialization;

namespace Gender.SoapApiServices.DuyVK.SoapModels
{
    [DataContract]
    public partial class SystemUserAccount
    {
        [DataMember]
        public int UserAccountId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string EmployeeCode { get; set; }

        [DataMember]
        public int RoleId { get; set; }

        [DataMember]
        public string RequestCode { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public string ApplicationCode { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public DateTime? ModifiedDate { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}
