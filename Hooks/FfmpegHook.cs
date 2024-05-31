using System.Collections;
using FfmpegEncoder = On.Zorro.Recorder.FfmpegEncoder;
using BepInEx.Logging;

namespace CWLiveLeak;

public class FfmpegHook
{
	internal static IEnumerator FfmpegEncoderOnRunFfmpeg(FfmpegEncoder.orig_RunFfmpeg orig, Zorro.Recorder.FfmpegEncoder self, string arguments, bool displaywindow)
	{
		Plugin.LiveleakLogger.LogInfo($"Adding {Plugin.Icon.Value} In");
		displaywindow = true;
		switch (Plugin.Icon.Value)
		{
			// special case for liveleak as i think it looks nicer like this
			case "liveleak":
				arguments = arguments + $" -i \"{Plugin.pluginPath}\\{Plugin.Icon.Value.ToLower()}.png\" -filter_complex \"[0:v][3:v] overlay=25:25:enable='between(t,0,20)'\"";
				break;
			default:
				arguments = arguments + $" -i \"{Plugin.pluginPath}\\{Plugin.Icon.Value.ToLower()}.png\" -filter_complex \"[0:v][3:v] overlay=0:0:enable='between(t,0,20)'\"";
				break;
		}
			
			
		return orig(self,arguments,displaywindow);
	}
}