using LonesEFTRadar.Tarkov.Features;
using LonesEFTRadar.Tarkov.GameWorld;
using Common.Features;
using Common.Misc;
using Common.Unity;
using Common.DMA.ScatterAPI;

namespace LonesEFTRadar.Tarkov.Features.MemoryWrites
{
    public sealed class NoVisor : MemWriteFeature<NoVisor>
    {
        public override bool Enabled
        {
            get => MemWrites.Config.NoVisor;
            set => MemWrites.Config.NoVisor = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromMilliseconds(500);

        public override void TryApply(ScatterWriteHandle writes)
        {
            try
            {
                if (Enabled && Memory.Game is LocalGameWorld game)
                {
                    const float newVisor = 0f;
                    ulong fps = game.CameraManager?.FPSCamera ?? 0x0;
                    fps.ThrowIfInvalidVirtualAddress();
                    var visorEffect = MonoBehaviour.GetComponent(fps, "VisorEffect");
                    if (visorEffect != 0x0)
                    {
                        var currentVisor = Memory.ReadValue<float>(visorEffect + Offsets.VisorEffect.Intensity);
                        if (currentVisor != newVisor)
                        {
                            writes.AddValueEntry(visorEffect + Offsets.VisorEffect.Intensity, newVisor);
                            LoneLogging.WriteLine($"NoVisor -> {newVisor}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring NoVisor: {ex}");
            }
        }
    }
}
