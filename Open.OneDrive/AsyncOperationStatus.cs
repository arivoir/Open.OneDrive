using System.Runtime.Serialization;

namespace Open.OneDrive
{
    [DataContract]
    public class AsyncOperationStatus
    {
        [DataMember(Name = "operation", IsRequired = true)]
        public string Operation { get; set; }
        [DataMember(Name = "percentageComplete", IsRequired = true)]
        public double PercentageComplete { get; set; }
        [DataMember(Name = "status", IsRequired = true)]
        public Status Status { get; set; }
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
        [EnumMember(Value = "updating")]
        Updating,
        [EnumMember(Value = "failed")]
        Failed,
        [EnumMember(Value = "deletePending")]
        DeletePending,
        [EnumMember(Value = "deleteFailed")]
        DeleteFailed,
        [EnumMember(Value = "waiting")]
        Waiting,
    }
}
