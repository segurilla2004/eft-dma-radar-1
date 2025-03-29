﻿using LonesEFTRadar.Tarkov.EFTPlayer;

namespace LonesEFTRadar.Tarkov.Loot
{
    public sealed class LootCorpse : LootContainer
    {
        public override string Name => PlayerObject?.Name ?? "Body";
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of corpse (example: Killa).</param>
        public LootCorpse(IReadOnlyList<LootItem> loot) : base(loot)
        {
        }

        /// <summary>
        /// Corpse container's associated player object (if any).
        /// </summary>
        public Player PlayerObject { get; init; }
    }
}