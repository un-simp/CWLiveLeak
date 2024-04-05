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
using FfmpegEncoder = On.Zorro.Recorder.FfmpegEncoder;


namespace CWLiveLeak
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{

		private ConfigEntry<string> Icon;
		public string pluginPath;
		static internal ManualLogSource? LiveleakLogger;

		private void Awake()
		{
			LiveleakLogger = base.Logger;
			Icon = Config.Bind("General",      // The section under which the option is shown
				"Icon",  // The key of the configuration option in the configuration file
				"liveleak", // The default value
				"What Icon is shown, available choices are: liveleak, hypercam, spookycam "); // Description of the option to show in the config file
			LiveleakLogger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
			Init();
			// looks like i dont need to harmony :yay:
			On.Zorro.Recorder.FfmpegEncoder.RunFfmpeg += FfmpegEncoderOnRunFfmpeg;
		}

		private void Init()
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			pluginPath = Path.GetDirectoryName(callingAssembly.Location);
		}
		
		private IEnumerator FfmpegEncoderOnRunFfmpeg(FfmpegEncoder.orig_RunFfmpeg orig, Zorro.Recorder.FfmpegEncoder self, string arguments, bool displaywindow)
		{
			LiveleakLogger.LogInfo($"Adding {Icon.Value} In");
			switch (Icon.Value)
			{
				// special case for liveleak as i think it looks nicer like this
				case "liveleak":
					arguments = arguments + $" -i {pluginPath}\\{Icon.Value.ToLower()}.png -filter_complex \"[0:v][3:v] overlay=25:25:enable='between(t,0,20)'\"";
					break;
				default:
					arguments = arguments + $" -i {pluginPath}\\{Icon.Value.ToLower()}.png -filter_complex \"[0:v][3:v] overlay=0:0:enable='between(t,0,20)'\"";
					break;
			}
			
			
			return orig(self,arguments,displaywindow);
		}


	}
}