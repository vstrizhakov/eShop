using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace eShopping.PromUaApi
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductStatus
    {
        [EnumMember(Value = "on_display")]
        OnDisplay,
        [EnumMember(Value = "draft")]
        Draft,
        [EnumMember(Value = "deleted")]
        Deleted,
        [EnumMember(Value = "not_on_display")]
        NotOnDisplay,
        [EnumMember(Value = "editing_required")]
        EditingRequired,
        [EnumMember(Value = "approval_pending")]
        ApprovalPending,
        [EnumMember(Value = "deleted_by_moderatos")]
        DeletedByModerator,
    }
}