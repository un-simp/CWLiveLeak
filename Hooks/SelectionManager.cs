using System.IO;
using UnityEngine;

namespace CWLiveLeak;

public class SelectionManager
{
	internal static void SurfaceNetworkHandlerOnRPCM_StartGame(On.SurfaceNetworkHandler.orig_RPCM_StartGame orig, SurfaceNetworkHandler self)
	{
		
		var myLoadedAssetBundle = AssetBundle.LoadFromFile("cwliveleak");
		if (myLoadedAssetBundle == null)
		{
			Debug.Log("Failed to load AssetBundle!");
			return;
		}

		var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("MyObject");
		Instantiate(prefab);

		myLoadedAssetBundle.Unload(false);

		orig(self);
	}
}