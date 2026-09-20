using HarmonyLib;
using UnityEngine;

namespace TweaksAndFixes.Patches;

[HarmonyPatch(typeof(CardOpeningSequenceUI), nameof(CardOpeningSequenceUI.Update))]
public static class CardOpeningSequenceUIPatch {
    [HarmonyPrefix]
    private static void Prefix(CardOpeningSequenceUI __instance) {
        if (!__instance.m_IsShowingTotalValue) return;

        // Original animation rate:
        // Time.deltaTime * 0.5F
        //
        // Adding another 0.5F makes the total rate 1.0F,
        // resulting in a fixed 1-second animation.
        __instance.m_TotalValueLerpTimer += Time.deltaTime * 0.5F;
    }
}