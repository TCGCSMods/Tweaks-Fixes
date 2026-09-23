using HarmonyLib;
using Object = UnityEngine.Object;

namespace TweaksAndFixes.Patches;

[HarmonyPatch(typeof(LightManager), nameof(LightManager.GoNextDay))]
public static class NextDayPatch {
    // The game runs 1 in-game hour as 60 real seconds.
    // The transition is 21:00 -> 08:00 so 11 in-game hours.
    private const float OvernightSeconds = 11.0F * 60F;

    private static void Postfix() {
        var openers = Object.FindObjectsOfType<InteractableAutoPackOpener>();

        foreach (var opener in openers) AdvanceOpener(opener, OvernightSeconds);
    }

    private static void AdvanceOpener(InteractableAutoPackOpener opener, float elapsedSeconds) {
        if (opener == null || !opener.m_IsProcessing || opener.m_StoredItemList == null || opener.m_StoredItemList.Count == 0 || opener.m_PackOpenTime <= 0.0F) return;

        opener.m_PackOpenTimer += elapsedSeconds;
        opener.m_PackOpenTimerSecond = 0.0F;

        // A single Update() only opens one pack and resets the timer.
        //
        // So we need to do the equivalent repeatedly so a large overnight jump can finish
        // multiple packs instead of only causing one pack to open.
        while (opener.m_PackOpenTimer >= opener.m_PackOpenTime && opener.m_StoredItemList.Count > 0) {
            opener.m_PackOpenTimer -= opener.m_PackOpenTime;

            opener.OpenPack();
            opener.RemoveItem(opener.m_StoredItemList[0]);

            opener.m_AutoCardOpenerUI.UpdateProcessingFillBar(1.0F - (float)opener.m_StoredItemList.Count / opener.m_MaxPackCount);
        }

        if (opener.m_StoredItemList.Count > 0) {
            // If there is any leftover partial pack progress for the next day.
            // I don't believe there ever would be, but just in case.
            opener.m_AutoCardOpenerUI.UpdateProcessingTimeLeftText(opener.m_PackOpenTime * opener.m_StoredItemList.Count - opener.m_PackOpenTimer);
        }
        else {
            // The game's "all packs processed" state.
            opener.m_CurrentState = 2;
            opener.m_AutoCardOpenerUI.SetUIState(2);
        }
    }
}