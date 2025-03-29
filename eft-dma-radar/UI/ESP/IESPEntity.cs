using Common.Unity;
using LonesEFTRadar.Tarkov.EFTPlayer;

namespace LonesEFTRadar.UI.ESP
{
    /// <summary>
    /// Defines an entity that can be drawn on Fuser ESP.
    /// </summary>
    public interface IESPEntity : IWorldEntity
    {
        /// <summary>
        /// Draw this Entity on Fuser ESP.
        /// </summary>
        /// <param name="canvas">SKCanvas instance to draw on.</param>
        void DrawESP(SKCanvas canvas, LocalPlayer localPlayer);
    }
}
