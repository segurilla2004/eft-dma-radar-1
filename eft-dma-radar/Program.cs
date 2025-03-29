global using SkiaSharp;
global using SkiaSharp.Views.Desktop;
global using System.ComponentModel;
global using System.Data;
global using System.Reflection;
global using System.Diagnostics;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Numerics;
global using System.Collections.Concurrent;
global using System.Net;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Collections;
global using System.Buffers;
global using System.Buffers.Binary;
global using SDK;
global using eft_dma_shared;
global using eft_dma_shared.Common;
using System.Runtime.Versioning;
using eft_dma_radar.UI.Radar;
using eft_dma_radar.UI.ESP;
using eft_dma_shared.Common.UI;
using LonesEFTRadar.Tarkov.Features;
using LonesEFTRadar.Tarkov.Features.MemoryWrites.Patches;
using LonesEFTRadar;
using LonesEFTRadar.UI.Misc;
using Common.Features;
using Common.Maps;
using Common.Misc.Data;

[assembly: AssemblyTitle(Program.Name)]
[assembly: AssemblyProduct(Program.Name)]
[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyCopyright("BSD Zero Clause License ©2025 lone-dma")]
[assembly: SupportedOSPlatform("Windows")]

namespace LonesEFTRadar
{
    internal static class Program
    {
        internal const string Name = "EFT DMA Radar";


        /// <summary>
        /// Global Program Configuration.
        /// </summary>
        public static Config Config { get; }

        /// <summary>
        /// Path to the Configuration Folder in %AppData%
        /// </summary>
        public static DirectoryInfo ConfigPath { get; } =
            new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "eft-dma-radar"));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                ConfigureProgram();
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        #region Private Members

        static Program()
        {
            try
            {
                TryImportLoneCfg();
                ConfigPath.Create();
                var config = Config.Load();
                SharedProgram.Initialize(ConfigPath, config);
                Config = config;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// If user is a former managed Lone EFT User, try import their config.
        /// </summary>
        private static void TryImportLoneCfg()
        {
            try
            {
                DirectoryInfo loneCfgPath = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lones-Client"));
                if (!ConfigPath.Exists && loneCfgPath.Exists)
                {
                    ConfigPath.Create();
                    foreach (var file in loneCfgPath.EnumerateFiles())
                        file.CopyTo(Path.Combine(ConfigPath.FullName, file.Name));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR Importing Lone Config(s)." +
                    $"Exception Info: {ex}",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Configure Program Startup.
        /// </summary>
        private static void ConfigureProgram()
        {
            ApplicationConfiguration.Initialize();
            using var loading = LoadingForm.Create();
            loading.UpdateStatus("Loading Tarkov.Dev Data...", 15);
            EftDataManager.ModuleInitAsync(loading).GetAwaiter().GetResult();
            loading.UpdateStatus("Loading Map Assets...", 35);
            LoneMapManager.ModuleInit();
            loading.UpdateStatus("Starting DMA Connection...", 50);
            ModuleInit();
            loading.UpdateStatus("Loading Remaining Modules...", 75);
            FeatureManager.ModuleInit();
            ResourceJanitor.ModuleInit(new Action(CleanupWindowResources));
            RuntimeHelpers.RunClassConstructor(typeof(MemPatchFeature<FixWildSpawnType>).TypeHandle);
            loading.UpdateStatus("Loading Completed!", 100);
        }

        private static void CleanupWindowResources()
        {
            MainForm.Window?.PurgeSKResources();
            EspForm.Window?.PurgeSKResources();
        }

        #endregion
    }
}