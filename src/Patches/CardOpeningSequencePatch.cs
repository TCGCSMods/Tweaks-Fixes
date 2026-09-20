using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace TweaksAndFixes.Patches;

[HarmonyPatch(typeof(CardOpeningSequence), nameof(CardOpeningSequence.Update))]
public static class CardOpeningSequencePatch {
    private static ConfigEntry<float> _beforeNextPackDelay;

    public static void Initialize(ConfigFile config) {
        _beforeNextPackDelay = config.Bind(
            "Card Opening",
            "BeforeNextPackDelay",
            0.5F,
            "Delay before the next-pack prompt becomes available. Original: 1 second."
        );
    }

    [HarmonyPrefix]
    private static void Prefix(CardOpeningSequence __instance) {
        if (!__instance.m_IsScreenActive)
            return;

        if (__instance.m_StateIndex == 9) AccelerateTimer(ref __instance.m_Slider, 1.0F, _beforeNextPackDelay.Value);
    }

    private static void AccelerateTimer(ref float timer, float originalDuration, float desiredDuration) {
        if (desiredDuration <= 0.0F) {
            timer = originalDuration;
            return;
        }

        var extraRate = originalDuration / desiredDuration - 1.0F;

        if (extraRate > 0.0F)
            timer += Time.deltaTime * extraRate;
    }
}