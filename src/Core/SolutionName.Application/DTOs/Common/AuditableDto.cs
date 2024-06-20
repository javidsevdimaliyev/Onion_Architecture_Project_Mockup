using System.Runtime.Serialization;

namespace SolutionName.Application.DTOs.Common
{
    public class AuditableDto<TKey> : BaseDto<TKey>
    {
        [DataMember] public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DataMember(EmitDefaultValue = false)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? CreatedUserId { get; set; }

        [DataMember(EmitDefaultValue = false)]
         [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? UpdateDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
         [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? UpdatedUserId { get; set; }


        [DataMember(EmitDefaultValue = false)]
         [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? OrderIndex { get; set; } = 0;
    }
}
