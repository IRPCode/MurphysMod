using MurphysMod.Systems;
using Terraria;
using Terraria.ModLoader;

namespace MurphysMod.Content.LuckHandlers
{
    public class OverrideVanillaLuck : ModPlayer
    {
        public override void PostUpdate()
        {
            if (!BookUsed.isPlayerCursed)
                return;

            Player player = Main.LocalPlayer;
            double luckVal = player.GetModPlayer<LuckHandler>().luckValue();

            if (luckVal <= .25f) //good/neutral luck only occurrs at or below .25, otherwise it will be counted as bad luck
                Main.LocalPlayer.luck = 1f - (float)(luckVal * 4);
            else
                Main.LocalPlayer.luck = -(float)(luckVal);
        }
    }
}