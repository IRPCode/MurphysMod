using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using MurphysMod.Content.Buffs;

public class BlessedWaterPlayerHandler : ModPlayer
{

    public bool liquidVelCheck = false;
    public bool velocityType;
    public override void PostUpdate()
    {
        try
        {

            if (Main.myPlayer == Player.whoAmI && !Main.dedServ)
            {
                float x = Player.Center.X;
                float y = Player.Center.Y;

                Tile playerLoc = Framing.GetTileSafely((int)(x / 16), (int)(y / 16));

                if (playerLoc.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "BlessedWater").Type)
                {
                    Player.AddBuff(ModContent.BuffType<Sanctified>(), 900);
                }

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    Tile npcLoc = Framing.GetTileSafely((int)npc.position.X / 16, (int)npc.position.Y / 16);
                    if (npc.active)
                    {
                        if (npcLoc.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "BlessedWater").Type) //prevents boss deaths
                        {
                            npc.AddBuff(ModContent.BuffType<Sanctified>(), 300);
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