using BeatSaberMarkupLanguage.Settings;
using BeatSaberMarkupLanguage.Util;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using SongRankedBadge.Configuration;
using SongRankedBadge.UI;
using System;
using System.IO;
using System.Reflection;
using Conf = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;

namespace SongRankedBadge
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    [NoEnableDisable]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; } = null!;
        internal static IPALogger Log { get; private set; } = null!;

        private readonly Harmony _harmony = new Harmony("com.github.qe201020335.SongRankedBadge");

        private readonly ModSettings _modSettings;

        [Init]
        public Plugin(IPALogger logger, Conf conf)
        {
            Instance = this;
            Log = logger;
            RankStatusManager.Instance.Init();
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            _modSettings = new ModSettings();
            Log.Debug("Config loaded");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            MainMenuAwaiter.MainMenuInitializing += OnMenuLoad;
            Log.Info("SongRankedBadge initialized.");
        }

        [OnEnable]
        public void OnMenuLoad()
        {
            // Use the actual game directory to find the Playlists folder
            string playlistPath = Path.Combine(Environment.CurrentDirectory, "Playlists");

            // This fills the static list inside ChallengeLoader
            ChallengeLoader.LoadAllChallengePlaylists(playlistPath);

            BSMLSettings.Instance.AddSettingsMenu("Ranked Badge", "SongRankedBadge.UI.configMenu.bsml", _modSettings);
        }
    }
}