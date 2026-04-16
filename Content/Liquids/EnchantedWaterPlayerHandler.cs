using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using MurphysMod.Content.Buffs;

public class EnchantedWaterPlayerHandler : ModPlayer //TODO: add a check that makes sure that the player/npc doesn't already have the buff applied
{

    //Buff biome order: Sky, Day Purity, Night Purity, Snow, Evil, Jungle, Desert, Ocean, Underground, Underworld, Hallow, Dungeon, Aether, Mushroom.
    public int[] playerBuffTypes = {BuffID.Featherfall, BuffID.Swiftness, BuffID.NightOwl, BuffID.Warmth, BuffID.Battle, BuffID.Calm, BuffID.Hunter,
     BuffID.Fishing, BuffID.Shine, BuffID.ObsidianSkin, BuffID.ManaRegeneration, BuffID.Dangersense, BuffID.WaterWalking, BuffID.Invisibility};

    //CursedInferno and Ichor are only for hardmode, otherwise it will be weak, Pre-HM is poison for jungle, HM is AcidVenom, 
    public int[] NPCDebuffTypes = {BuffID.Confused, BuffID.Oiled, BuffID.Frostburn, BuffID.ShadowFlame, BuffID.CursedInferno, BuffID.Ichor, BuffID.Poisoned,
     BuffID.Venom, BuffID.Midas, BuffID.Slimed, BuffID.BloodButcherer, BuffID.OnFire3, BuffID.GelBalloonBuff, BuffID.OnFire, BuffID.Shimmer, BuffID.Confused};
    public override void PostUpdate()
    {
        try
        {
            if (Main.myPlayer == Player.whoAmI && !Main.dedServ)
            {
                float x = Player.Center.X;
                float y = Player.Center.Y - 1;

                Tile playerLoc = Framing.GetTileSafely((int)(x / 16), (int)(y / 16));

                if (playerLoc.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "EnchantedWater").Type)
                {
                    int buffSelected = BuffID.Swiftness;

                    if (Player.ZoneSkyHeight || Player.ZoneNormalSpace)
                        buffSelected = playerBuffTypes[0];
                    else if (Player.ZonePurity)
                        if (Main.dayTime)
                            buffSelected = playerBuffTypes[1];
                        else
                            buffSelected = playerBuffTypes[2];
                    else if (Player.ZoneSnow)
                        buffSelected = playerBuffTypes[3];
                    else if (Player.ZoneCorrupt || Player.ZoneCrimson)
                        buffSelected = playerBuffTypes[4];
                    else if (Player.ZoneJungle)
                        buffSelected = playerBuffTypes[5];
                    else if (Player.ZoneDesert || Player.ZoneUndergroundDesert)
                        buffSelected = playerBuffTypes[6];
                    else if (Player.ZoneBeach)
                        buffSelected = playerBuffTypes[7];
                    else if (Player.ZoneDirtLayerHeight || Player.ZoneRockLayerHeight)
                        buffSelected = playerBuffTypes[8];
                    else if (Player.ZoneUnderworldHeight)
                        buffSelected = playerBuffTypes[9];
                    else if (Player.ZoneHallow)
                        buffSelected = playerBuffTypes[10];
                    else if (Player.ZoneDungeon)
                        buffSelected = playerBuffTypes[11];
                    else if (Player.ZoneShimmer)
                        buffSelected = playerBuffTypes[12];
                    else if (Player.ZoneGlowshroom)
                        buffSelected = playerBuffTypes[13];

                    Player.AddBuff(buffSelected, 900); //15 secs
                }

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    Tile npcLoc = Framing.GetTileSafely((int)npc.Center.X / 16, (int)npc.Center.Y / 16);
                    if (npc.active)
                    {
                        if (npcLoc.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "EnchantedWater").Type)
                        {
                            int buffSelected = BuffID.OnFire;

                            if (Player.ZoneSkyHeight || Player.ZoneNormalSpace)
                                buffSelected = NPCDebuffTypes[0];
                            else if (Player.ZonePurity)
                                    buffSelected = NPCDebuffTypes[1];
                            else if (Player.ZoneSnow)
                                buffSelected = NPCDebuffTypes[2];

                            else if (Player.ZoneCorrupt || Player.ZoneCrimson)
                            {
                                if (Main.hardMode && Player.ZoneCorrupt)
                                    buffSelected = NPCDebuffTypes[4];
                                else if (Main.hardMode && Player.ZoneCrimson)
                                    buffSelected = NPCDebuffTypes[5];
                                else
                                    buffSelected = NPCDebuffTypes[3];
                            }

                            else if (Player.ZoneJungle)
                            {
                                if (Main.hardMode)
                                    buffSelected = NPCDebuffTypes[7];
                                else
                                    buffSelected = NPCDebuffTypes[6];
                            }
                              
                            else if (Player.ZoneDesert || Player.ZoneUndergroundDesert)
                                buffSelected = NPCDebuffTypes[8];
                            else if (Player.ZoneBeach)
                                buffSelected = NPCDebuffTypes[9];
                            else if (Player.ZoneDirtLayerHeight || Player.ZoneRockLayerHeight)
                                buffSelected = NPCDebuffTypes[10];
                            else if (Player.ZoneUnderworldHeight)
                                buffSelected = NPCDebuffTypes[11];
                            else if (Player.ZoneHallow)
                                buffSelected = NPCDebuffTypes[12];
                            else if (Player.ZoneDungeon)
                                buffSelected = NPCDebuffTypes[13];
                            else if (Player.ZoneShimmer)
                                buffSelected = NPCDebuffTypes[14];
                            else if (Player.ZoneGlowshroom)
                                buffSelected = NPCDebuffTypes[15];

                            npc.AddBuff(buffSelected, 300); //5 secs
                        }
                    }
                }
            }
        }
        catch
        {
            Main.NewText("Error thrown due to MurphysMod EnchantedWaterPlayerHandler mechanics.");
        }
    }
}