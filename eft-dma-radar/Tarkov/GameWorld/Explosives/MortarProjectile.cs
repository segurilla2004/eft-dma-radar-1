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
    public sealed class MortarProjectile : IExplosiveItem
    {
        public static implicit operator ulong(MortarProjectile x) => x.Addr;
        private readonly ConcurrentDictionary<ulong, IExplosiveItem> _parent;

        public MortarProjectile(ulong baseAddr, ConcurrentDictionary<ulong, IExplosiveItem> parent)
        {
            _parent = parent;
            Addr = baseAddr;
            Refresh();
            if (!IsActive)
            {
                throw new Exception("Already exploded!");
            }
        }

        public ulong Addr { get; }

        public bool IsActive { get; private set; }

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
            if (!CameraManagerBase.WorldToScreen(ref _position, out var scrPos))
                return;
            var circleRadius = 8f * ESP.Config.LineScale;
            canvas.DrawCircle(scrPos, circleRadius, SKPaints.PaintGrenadeESP);
        }

        public void Refresh()
        {
            var artilleryProjectile = Memory.ReadValue<ArtilleryProjectile>(this, false);
            IsActive = artilleryProjectile.IsActive;
            if (IsActive)
            {
                _position = artilleryProjectile.Position;
            }
            else
            {
                _parent.TryRemove(this, out _);
            }
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        private readonly struct ArtilleryProjectile
        {
            [FieldOffset((int)Offsets.ArtilleryProjectileClient.Position)]
            public readonly Vector3 Position;
            [FieldOffset((int)Offsets.ArtilleryProjectileClient.IsActive)]
            public readonly bool IsActive;
        }
    }
}
