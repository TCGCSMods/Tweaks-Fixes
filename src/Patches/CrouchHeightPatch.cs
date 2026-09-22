using BepInEx.Configuration;
using HarmonyLib;

namespace TweaksAndFixes.Patches;

[HarmonyPatch(typeof(InteractionPlayerController), nameof(InteractionPlayerController.EvaluateCameraLerp) )]
public static class CrouchHeightPatch {
    private static ConfigEntry<bool> _enabled;
    private static ConfigEntry<float> _crouchCameraDropY;

    public static void Initialize(ConfigFile config) {
        _enabled = config.Bind(
            "Crouch Height",
            "Enabled",
            true,
            "Overrides how far the camera drops down while crouching."
        );

        _crouchCameraDropY = config.Bind(
            "Crouch Height",
            "CameraDropY",
            -0.6F,
            "Local Y offset applied to the camera while crouching. The game's default is -1.0."
        );
    }

    [HarmonyPrepare]
    private static bool Prepare() {
        return _enabled.Value;
    }

    [HarmonyPrefix]
    private static void Prefix(InteractionPlayerController __instance, out bool __state) {
        // Whether it is about to perform a camera-position interpolation.
        __state = __instance.IsCrouching() && __instance.m_CameraBlendPosTimer < 1.0F;
    }

    [HarmonyPostfix]
    private static void Postfix(InteractionPlayerController __instance, bool __state) {
        // If no interpolation was performed, we don't care.
        if (!__state)
            return;

        const float defaultCrouchY = -1.0F;

        var desiredCrouchY = _crouchCameraDropY.Value;
        var difference = desiredCrouchY - defaultCrouchY;
        
        // The method has already executed:
        // m_CurrentCameraPosY = Mathf.Lerp(m_CurrentCameraPosY, -1.0F, m_CameraBlendPosTimer);
        //
        // So we'll adjust the result so it is equivalent to interpolating toward the configured crouch height instead.
        var correction = difference * __instance.m_CameraBlendPosTimer;

        __instance.m_CurrentCameraPosY += correction;
        __instance.m_CurrentCameraPos.y = __instance.m_CurrentCameraPosY;

        __instance.m_CameraController.transform.localPosition = __instance.m_CurrentCameraPos;

        __instance.m_LookAtTransform.transform.localPosition = __instance.m_CurrentCameraPos;
        
        // I'm unsure if there's a better way of doing it, but I've tried like 5 different ways, and none of them worked until this.
        // This games code is genuinely worse than an AAA slop game... which isn't a compliment to AAA games.
    }
}