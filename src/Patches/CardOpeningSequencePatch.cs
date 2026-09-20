using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace TweaksAndFixes.Patches;

[HarmonyPatch(typeof(CardOpeningSequence), nameof(CardOpeningSequence.Update))]
public static class CardOpeningSequencePatch {
    private static ConfigEntry<float> _beforeNextPackDelay;
    private static ConfigEntry<float> _newCardPitchIncrease;

    private static CardOpeningSequence _currentSequence;

    public static void Initialize(ConfigFile config) {
        _beforeNextPackDelay = config.Bind(
            "Card Opening",
            "BeforeNextPackDelay",
            0.5F,
            "Delay before the next-pack prompt becomes available. Original: 1 second."
        );

        _newCardPitchIncrease = config.Bind(
            "Card Opening",
            "NewCardPitchIncrease",
            0.15F,
            "Additional pitch applied to the final gift sound when the pack contains a new card."
        );
    }

    [HarmonyPrefix]
    private static void Prefix(CardOpeningSequence __instance) {
        _currentSequence = __instance;

        if (!__instance.m_IsScreenActive) return;

        if (__instance.m_StateIndex == 9)
            AccelerateTimer(ref __instance.m_Slider, 1.0F, _beforeNextPackDelay.Value);
    }

    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        if (instructions == null) {
            Plugin.Logger?.LogError("CardOpeningSequence transpiler received null instructions.");
            return [];
        }

        var codes = new List<CodeInstruction>(instructions);
        var playAudio = AccessTools.Method(typeof(SoundManager), nameof(SoundManager.PlayAudio), [typeof(string), typeof(float), typeof(float)]);

        if (playAudio == null) {
            Plugin.Logger?.LogError("Could not find SoundManager.PlayAudio(string, float, float).");
            return codes;
        }

        var replacement = AccessTools.Method(typeof(CardOpeningSequencePatch), nameof(PlayFinalGiftAudio), [typeof(string), typeof(float), typeof(float)]);

        if (replacement == null) {
            Plugin.Logger?.LogError("Could not find CardOpeningSequencePatch.PlayFinalGiftAudio(string, float, float).");
            return codes;
        }

        var replacements = 0;

        for (var i = 0; i < codes.Count; i++) {
            var current = codes[i];

            if (current == null)
                continue;

            if (current.opcode != OpCodes.Ldstr)
                continue;

            if (!string.Equals(current.operand as string, "SFX_Gift", StringComparison.Ordinal))
                continue;

            // Looking forward from "SFX_Gift" until the next string literal.
            for (var j = i + 1; j < codes.Count; j++) {
                var candidate = codes[j];

                if (candidate == null)
                    continue;

                if (candidate.opcode == OpCodes.Call || candidate.opcode == OpCodes.Callvirt)
                    if (candidate.operand is MethodInfo method && method == playAudio) {
                        candidate.operand = replacement;
                        replacements++;
                        break;
                    }

                // So we don't accidentally match a later unrelated string.
                if (candidate.opcode == OpCodes.Ldstr)
                    break;
            }
        }

        Plugin.Logger?.LogInfo($"Booster pack final SFX sound patched. Replaced {replacements} call(s).");
        return codes;
    }

    private static void AccelerateTimer(ref float timer, float originalDuration, float desiredDuration) {
        if (desiredDuration <= 0.0F) {
            timer = originalDuration;
            return;
        }

        var extraRate = originalDuration / desiredDuration - 1.0F;
        if (extraRate > 0.0F) timer += Time.deltaTime * extraRate;
    }

    private static void PlayFinalGiftAudio(string soundName, float volume, float originalPitch) {
        var sequence = _currentSequence;

        if (sequence == null) {
            SoundManager.PlayAudio(soundName, volume, originalPitch);
            Plugin.Logger.LogInfo("sequence is null");
            return;
        }

        var hasNewCard = sequence.m_ManualOpenCardDataList.Any(card => card.isNew) || sequence.m_IsNewlList.Any(isNew => isNew);

        var newPitch = originalPitch + _newCardPitchIncrease.Value;
        SoundManager.PlayAudio(soundName, volume, !hasNewCard ? originalPitch : newPitch);
    }
}