using System.Runtime.Serialization;

namespace Open.OneDrive
{
    [DataContract]
    public class AsyncJobStatus
    {
        [DataMember(Name = "operation")]
        public string Operation { get; set; }

        [DataMember(Name = "percentageComplete", IsRequired = true)]
        public double PercentageComplete { get; set; }

        [DataMember(Name = "status", IsRequired = true)]
        public Status Status { get; set; }

        [DataMember(Name = "statusDescription")]
        public string StatusDescription { get; set; }

        [DataMember(Name = "resourceId")]
        public string ResourceId { get; set; }
    }

    [DataContract(IsReference = false)]
    public enum Status
    {
        [EnumMember(Value = "notStarted")]
        NotStarted,
        [EnumMember(Value = "inProgress")]
        InProgress,
        [EnumMember(Value = "completed")]
        Completed,
        [EnumMember(Value = "failed")]
        Failed,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "waiting")]
        Waiting,
        [EnumMember(Value = "cancelPending")]
        CancelPending,
    }
}
