using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using MonoMod;
using On;
using UnityEngine;
using Zorro.Recorder;
using BepInEx.Configuration;
using BepInEx.Logging;


namespace CWLiveLeak
{
    [ContentWarningPlugin("CWLiveleak", "1.0.0", true)]
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{

		internal static ConfigEntry<string> Icon;
		internal static ConfigEntry<string> ffmpegDebugging;
		public static string pluginPath;
		internal static ManualLogSource? LiveleakLogger; 
		private void Awake()
		{
			LiveleakLogger = base.Logger;
			Icon = Config.Bind("General",      // The section under which the option is shown
				"Icon",  // The key of the configuration option in the configuration file
				"liveleak", // The default value
				"What Icon is shown, available choices are: liveleak, hypercam, spookycam "); // Description of the option to show in the config file
			ffmpegDebugging = Config.Bind("Debugging",
				"Display FFmpeg Window",
				"false", 
				"So you can watch ffmpeg shit itself in real time!");
			LiveleakLogger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
			Init();
			// looks like i dont need to harmony :yay:
			On.Zorro.Recorder.FfmpegEncoder.RunFfmpeg += FfmpegHook.FfmpegEncoderOnRunFfmpeg;
			On.SurfaceNetworkHandler.RPCM_StartGame += SelectionManager.SurfaceNetworkHandlerOnRPCM_StartGame;
		}



		private void Init()
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			pluginPath = Path.GetDirectoryName(callingAssembly.Location);
		}
		
		


	}
}