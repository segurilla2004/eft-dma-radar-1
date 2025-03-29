using Common.Unity.LowLevel;

namespace Common.Misc.Config
{
    public interface IConfig
    {
        LowLevelCache LowLevelCache { get; }
        ChamsConfig ChamsConfig { get; }
        bool MemWritesEnabled { get; }
        bool AdvancedMemWrites { get; }
        int MonitorWidth { get; }
        int MonitorHeight { get; }

        void Save();
        Task SaveAsync();
    }
}
