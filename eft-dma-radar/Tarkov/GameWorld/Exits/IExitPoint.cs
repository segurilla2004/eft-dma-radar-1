using Common.Maps;
using Common.Unity;
using LonesEFTRadar.UI.ESP;
using LonesEFTRadar.UI.Radar;

namespace LonesEFTRadar.Tarkov.GameWorld.Exits
{
    /// <summary>
    /// Defines a contract for a point that can be used to exit the map.
    /// </summary>
    public interface IExitPoint : IWorldEntity, IMapEntity, IMouseoverEntity, IESPEntity
    {
    }
}
