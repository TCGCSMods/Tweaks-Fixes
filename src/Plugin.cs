using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TweaksAndFixes.Patches;

namespace TweaksAndFixes;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    internal new static ManualLogSource Logger;

    private Harmony _harmony;

    private void Awake() {
        CardOpeningSequencePatch.Initialize(Config);
        GiftAudioPatch.Initialize(Config);
        
        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll();

        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void OnDestroy() {
        _harmony.UnpatchSelf();
    }
}