using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace TweaksAndFixes.Patches;

[HarmonyPatch(typeof(InteractionPlayerController), nameof(InteractionPlayerController.Start))]
public static class PlayerSpawnPatch {
    private static ConfigEntry<bool> _enabled;
    private static readonly Vector3 SpawnPosition = new(6.103F, 0.007F, -7.448F);

    public static void Initialize(ConfigFile config) {
        _enabled = config.Bind(
            "Player Spawn",
            "Enabled",
            true,
            "Changes the players spawn location to the shop door instead of on the road."
        );
    }

    [HarmonyPrepare]
    private static bool Prepare() {
        return _enabled.Value;
    }

    [HarmonyPostfix]
    private static void Postfix() {
        var player = GameObject.Find("FirstPersonController");

        if (player == null) {
            Plugin.Logger.LogWarning("Could not find game-object 'FirstPersonController'.");
            return;
        }

        player.transform.position = SpawnPosition;
        player.transform.Rotate(0.0F, 33.0F, 0.0F);
    }
}