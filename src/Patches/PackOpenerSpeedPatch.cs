using BepInEx.Configuration;
using HarmonyLib;

namespace TweaksAndFixes.Patches;

[HarmonyPatch(typeof(InteractableAutoPackOpener), nameof(InteractableAutoPackOpener.Awake))]
public class PackOpenerSpeedPatch {
    private static ConfigEntry<bool> _enabled;
    private static ConfigEntry<float> _packOpenTimeMultiplier;

    public static void Initialize(ConfigFile config) {
        _enabled = config.Bind(
            "AutoPackOpener",
            "Enabled",
            true,
            "Speeds up how long auto pack openers take to process each pack."
        );

        _packOpenTimeMultiplier = config.Bind(
            "AutoPackOpener",
            "PackOpenTimeMultiplier",
            0.25F,
            "Multiplies how long each auto pack opener takes to process a single pack. Original: 1.0"
        );
    }

    [HarmonyPrepare]
    private static bool Prepare() {
        return _enabled.Value;
    }

    [HarmonyPostfix]
    private static void Postfix(InteractableAutoPackOpener __instance) {
        __instance.m_PackOpenTime *= _packOpenTimeMultiplier.Value;
    }
}