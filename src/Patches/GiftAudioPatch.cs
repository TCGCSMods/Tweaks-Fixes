using System.Linq;
using BepInEx.Configuration;
using HarmonyLib;

namespace TweaksAndFixes.Patches;

[HarmonyPatch(typeof(SoundManager), nameof(SoundManager.PlayAudio), typeof(string), typeof(float), typeof(float))]
public static class GiftAudioPatch {
    private static ConfigEntry<bool> _enabled;
    private static ConfigEntry<float> _newCardPitchIncrease;

    public static void Initialize(ConfigFile config) {
        _enabled = config.Bind(
            "Card Opening",
            "NewCardPitchEnabled",
            true,
            "Enables a higher-pitched final gift sound when the pack contains a new card."
        );

        _newCardPitchIncrease = config.Bind(
            "Card Opening",
            "NewCardPitchIncrease",
            0.15F,
            "Additional pitch applied to the final gift sound when the pack contains a new card."
        );
    }

    [HarmonyPrepare]
    private static bool Prepare() {
        return _enabled.Value;
    }

    [HarmonyPrefix]
    private static void Prefix(string audioName, ref float pitch) {
        if (audioName != "SFX_Gift") return;

        var sequence = CSingleton<CardOpeningSequence>.Instance;
        if (sequence == null) return;

        if (!sequence.m_ManualOpenCardDataList.Any(card => card.isNew)) return;

        pitch += _newCardPitchIncrease.Value;
    }
}