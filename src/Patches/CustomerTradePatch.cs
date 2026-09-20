using HarmonyLib;

namespace TweaksAndFixes.Patches;

[HarmonyPatch(typeof(CustomerTradeCardScreen), nameof(CustomerTradeCardScreen.SetCustomer))]
public static class CustomerTradePatch {
    [HarmonyPostfix]
    private static void Postfix(CustomerTradeCardScreen __instance, CustomerTradeData customerTradeData) {
        // Trading uses cards instead of a price, this patch is specifically for prices.
        if (__instance.m_IsTrading) return;

        // We don't want to overwrite the data when the player returns from the "Let Me Think" option.
        if (customerTradeData != null) return;

        // Setting the default price to the customers asking price.
        var price = __instance.m_SellCardAskPrice;

        var input = CSingleton<CGameManager>.Instance.m_CurrencyType != EMoneyCurrencyType.Yen
            ? GameInstance.GetPriceString(price, useCurrencySymbol: false)
            : GameInstance.GetPriceString(price, useCurrencySymbol: false, decimalString: "F0");

        __instance.OnInputTextUpdated(input);
    }
}