using System;
using MurphysMod.Content.Ambience;
using Terraria;
using Terraria.ModLoader;

public class HeightLuck : ModSystem
{
    public override void PostUpdateEverything()
    {
        getHeightLuckVal();
    }
    public double getHeightLuckVal()
    {
        double HluckVal = 0;
        Player player = Main.LocalPlayer;

        if (player.position.Y / 16 >= Main.worldSurface)
        {
            HluckVal = Math.Round(((player.position.Y / 16) - Main.worldSurface) / (Main.maxTilesY - Main.worldSurface) / 4, 2);
        }
        return HluckVal;
    }
}