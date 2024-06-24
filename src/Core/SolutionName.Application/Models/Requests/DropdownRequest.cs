using System.Text.Json.Serialization;

namespace SolutionName.Application.Models.Requests
{
    public sealed class DropdownRequest : PagingRequest
    {
        public DropdownRequest(string? searchKey)
        {
            SearchKey = searchKey;
        }

        public string? DependentColumnIdHash { get; set; }

        [JsonIgnore]
        public int? DependentColumnId { get { return Decrypt<int>(DependentColumnIdHash); } set { DependentColumnIdHash = Encrypt(value); } }

        public string? SearchKey { get; set; }

    }
}
