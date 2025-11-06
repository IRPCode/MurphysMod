using System;
using MurphysMod.Content.LuckHandlers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class ShopLuck : ModPlayer
{
    public override void PreUpdate()
    {
        if (Player.talkNPC != -1)
        {
            float luckVal = Utils.Clamp(((float)Player.GetModPlayer<LuckHandler>().luckValue() * .5f) - .1f, -.1f, 1f);

            var currentSettings = Player.currentShoppingSettings;
            currentSettings.PriceAdjustment = 1f + luckVal;
            Player.currentShoppingSettings = currentSettings;
        }
    }
}