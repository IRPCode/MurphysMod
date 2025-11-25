using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using MurphysMod.Content.Buffs;

public class AccursedSapPlayerHandler : ModPlayer
{
    public override void PostUpdate()
    {
        try
        {
            if (Main.myPlayer == Player.whoAmI && !Main.dedServ)
            {
                float x = Player.Center.X;
                float y = Player.Center.Y;

                Tile playerLoc = Framing.GetTileSafely((int)(x / 16), (int)(y / 16));

                if (playerLoc.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "AccursedSap").Type)
                {
                    Player.AddBuff(ModContent.BuffType<Oleaginous>(), 1800);

                    if (Player.HasBuff(ModContent.BuffType<Sanctified>()))
                        Player.ClearBuff(ModContent.BuffType<Sanctified>());
                }

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    Tile npcLoc = Framing.GetTileSafely((int)npc.position.X / 16, (int)npc.position.Y / 16);
                    if (npc.active)
                    {
                        if (npcLoc.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "AccursedSap").Type)
                        {
                            npc.AddBuff(ModContent.BuffType<Oleaginous>(), 900);
                        }
                    }
                }
            }
        }
        catch
        {
            Main.NewText("Error thrown due to MurphysMod BlessedWaterPlayerHandler mechanics.");
        }
    }
}