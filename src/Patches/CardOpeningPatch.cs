using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace TweaksAndFixes.Patches;

[HarmonyPatch(typeof(CardOpeningSequence), nameof(CardOpeningSequence.Update))]
public static class CardOpeningPatch {
    private static ConfigEntry<bool> _enabled;
    private static ConfigEntry<float> _beforeNextPackDelay;


    public static void Initialize(ConfigFile config) {
        _enabled = config.Bind(
            "Card Opening",
            "ShortenNextPackDelayEnabled",
            true,
            "Enables shortening the delay before you can proceed to the next pack after the final card reveal."
        );

        _beforeNextPackDelay = config.Bind(
            "Card Opening",
            "BeforeNextPackDelay",
            0.5F,
            "Delay in seconds before input to continue is accepted after the final card reveal. Original: 1 second."
        );
    }

    [HarmonyPrepare]
    private static bool Prepare() {
        return _enabled.Value;
    }

    [HarmonyPrefix]
    private static void Prefix(CardOpeningSequence __instance) {
        if (!__instance.m_IsScreenActive) return;

        if (__instance.m_StateIndex == 9)
            AccelerateTimer(ref __instance.m_Slider, 1.0F, _beforeNextPackDelay.Value);
    }

    private static void AccelerateTimer(ref float timer, float originalDuration, float desiredDuration) {
        if (desiredDuration <= 0.0F) {
            timer = originalDuration;
            return;
        }

        var extraRate = originalDuration / desiredDuration - 1.0F;
        if (extraRate > 0.0F) timer += Time.deltaTime * extraRate;
    }
}