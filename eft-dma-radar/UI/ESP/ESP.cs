using LonesEFTRadar.UI.Misc;

namespace LonesEFTRadar.UI.ESP
{
    internal static class ESP
    {
        /// <summary>
        /// ESP Configuration.
        /// </summary>
        public static ESPConfig Config { get; } = Program.Config.ESP;
    }
}
