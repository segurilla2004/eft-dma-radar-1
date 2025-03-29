using Common.Misc.Data;

namespace LonesEFTRadar.UI.LootFilters
{
    public sealed class UserLootFilter
    {
        [JsonPropertyName("enabled")] public bool Enabled { get; set; } = true;

        [JsonInclude]
        [JsonPropertyName("entries")]
        public List<LootFilterEntry> Entries { get; init; } = new();
    }
}