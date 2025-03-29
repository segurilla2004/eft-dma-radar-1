using Common.Maps;
using Common.Players;
using Common.Unity;
using eft_dma_radar.UI.Radar;
using eft_dma_shared.Common.ESP;
using LonesEFTRadar.Tarkov.EFTPlayer;
using LonesEFTRadar.UI.ESP;
using LonesEFTRadar.UI.Misc;

namespace LonesEFTRadar.Tarkov.GameWorld.Explosives
{
    /// <summary>
    /// Represents a Tripwire (with attached Grenade) in Local Game World.
    /// </summary>
    public sealed class Tripwire : IExplosiveItem, IWorldEntity, IMapEntity, IESPEntity
    {
        public static implicit operator ulong(Tripwire x) => x.Addr;

        /// <summary>
        /// Base Address of Grenade Object.
        /// </summary>
        public ulong Addr { get; }

        /// <summary>
        /// True if the Tripwire is in an active state.
        /// </summary>
        public bool IsActive { get; private set; }

        public Tripwire(ulong baseAddr)
        {
            Addr = baseAddr;
            IsActive = GetIsTripwireActive(false);
            if (IsActive)
            {
                _position = GetPosition(false);
            }
        }

        public void Refresh()
        {
            IsActive = GetIsTripwireActive();
            if (IsActive)
            {
                Position = GetPosition();
            }
        }

        private bool GetIsTripwireActive(bool useCache = true)
        {
            var status = (Enums.ETripwireState)Memory.ReadValue<int>(this + Offsets.TripwireSynchronizableObject._tripwireState, useCache);
            return status is Enums.ETripwireState.Wait || status is Enums.ETripwireState.Active;
        }
        private Vector3 GetPosition(bool useCache = true)
        {
            return Memory.ReadValue<Vector3>(this + Offsets.TripwireSynchronizableObject.ToPosition, useCache);
        }

        #region Interfaces

        private Vector3 _position;
        public ref Vector3 Position => ref _position;

        public void Draw(SKCanvas canvas, LoneMapParams mapParams, ILocalPlayer localPlayer)
        {
            if (!IsActive)
                return;
            var circlePosition = Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
            var size = 5 * MainForm.UIScale;
            SKPaints.ShapeOutline.StrokeWidth = SKPaints.PaintExplosives.StrokeWidth + 2f * MainForm.UIScale;
            canvas.DrawCircle(circlePosition, size, SKPaints.ShapeOutline); // Draw outline
            canvas.DrawCircle(circlePosition, size, SKPaints.PaintExplosives); // draw LocalPlayer marker
        }

        public void DrawESP(SKCanvas canvas, LocalPlayer localPlayer)
        {
            if (!IsActive)
                return;
            if (Vector3.Distance(localPlayer.Position, Position) > ESP.Config.GrenadeDrawDistance)
                return;
            if (!CameraManagerBase.WorldToScreen(ref _position, out var scrPos))
                return;
            var circleRadius = 8f * ESP.Config.LineScale;
            canvas.DrawCircle(scrPos, circleRadius, SKPaints.PaintGrenadeESP);
        }

        #endregion
    }
}
