﻿using Common.Maps;
using LonesArenaRadar.Arena.ArenaPlayer;

namespace LonesArenaRadar.UI.Radar
{
    /// <summary>
    /// Defines a Radar Map Mouseover Entity.
    /// </summary>
    public interface IMouseoverEntity : IMapEntity
    {
        /// <summary>
        /// Cached 'Mouseover' Position on the Radar GUI. Used for mouseover events.
        /// Uses zoomed coordinates and is refreshed on each render cycle.
        /// </summary>
        Vector2 MouseoverPosition { get; set; }


        void DrawMouseover(SKCanvas canvas, LoneMapParams mapParams, LocalPlayer localPlayer);
    }
}
