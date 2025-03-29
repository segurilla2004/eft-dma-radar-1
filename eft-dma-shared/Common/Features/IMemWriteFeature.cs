using Common.DMA.ScatterAPI;

namespace Common.Features
{
    public interface IMemWriteFeature : IFeature
    {
        /// <summary>
        /// Apply the MemWrite feature via Scatter Write.
        /// Must not throw.
        /// </summary>
        /// <param name="writes"></param>
        void TryApply(ScatterWriteHandle writes);
    }
}
