using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using MurphysMod.Content.Buffs;
using MurphysMod.Systems;
using System;

namespace MurphysMod.Content.LuckHandlers
{
    public class LuckHandler : ModPlayer //modify this class to balance it with items. Make the highest bad luck level incredibly uncommon (without adding items or new tiles to the mix (maybe something similar to a demon alter? Like a sacrifical alter or something?))
    { //if possible, change the night sky depending on your luck level (I am thinking like aurora borealis becoming more and more red or something like that)

        public static double TileLuck;
        public double multiplayerLuckPacket;
        public double totalLuck;

        public override void PostUpdate()
        {

            if (Main.netMode == NetmodeID.Server)
                multiplayerHandler();
            luckValue();
        }

        public void multiplayerHandler()
        {
            int activePlayerCount = 0;
            double playerLuck = 0;

            for (int i = 0; i < Main.maxNetPlayers; i++)
            {
                Player player = Main.player[i];

                if (player.active)
                {
                    activePlayerCount++;
                    playerLuck += player.GetModPlayer<LuckHandler>().luckValue();

                }
            }
            if (activePlayerCount > 0)
                playerLuck /= activePlayerCount;

            totalLuck = playerLuck;

            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)MessageType.totalLuckPacket);
            packet.Write(totalLuck);
            packet.Send();
        }
        public double luckValue()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return Utils.Clamp(multiplayerLuckPacket, 0, double.MaxValue);
            }
            
            double luckValue = 0;

            if (BookUsed.isPlayerCursed)
            {
                //bad luck biomes

                if (!Main.dayTime)
                {
                    luckValue += .15;
                }

                if (Player.ZoneCorrupt || Player.ZoneCrimson)
                {
                    luckValue += .25;
                }

                if (Player.ZoneUndergroundDesert || Player.ZoneJungle || Player.ZoneGraveyard || Player.ZoneDungeon)
                {
                    luckValue += .1;
                }

                if (Player.ZoneUnderworldHeight) //adds to player height luck
                {
                    luckValue += .15;
                }

                if (Player.ZoneGraveyard)
                {
                    luckValue += .15;
                }

                if (Player.ZoneWaterCandle)
                {
                    luckValue += .25;
                }

                if (Player.ZoneShadowCandle)
                {
                    luckValue += .35;
                }

                if (Player.ZonePeaceCandle)
                {
                    luckValue -= .2;
                }

                if (Player.ZoneLihzhardTemple)
                {
                    luckValue += .25;
                }

                if (Player.ZoneTowerNebula || Player.ZoneTowerSolar || Player.ZoneTowerVortex || Player.ZoneTowerStardust)
                {
                    luckValue += .35;
                }

                //good luck biomes

                if (Player.ZoneGemCave || Player.ZoneGlowshroom || Player.ZoneGranite || Player.ZoneMarble)
                {
                    luckValue -= .25;
                }

                if (Player.ZoneShimmer || Player.ZoneHallow)
                {
                    luckValue -= .1;
                }

                //bad luck events

                if (Main.bloodMoon || (Main.invasionType > 5))
                {
                    luckValue += .2;
                }

                if (Main.eclipse)
                {
                    luckValue += .25;
                }

                if ((Main.invasionType > 0 && Main.invasionType < 4) || Main.invasionType == 5)
                {
                    luckValue += .15;
                }

                //luck debuffs

                if (Player.HasBuff<Augury>())
                {
                    luckValue += .1;
                }

                if (Player.HasBuff<BadOmen>())
                {
                    luckValue += .2;
                }

                if (Player.HasBuff<Portent>())
                {
                    luckValue += .3;
                }

                if (Player.HasBuff<Sanctified>()) //ensure this is the final check to prevent luckvalue abuse
                {
                    if (luckValue >= .25)
                    {
                        luckValue -= .25;
                    }
                    else
                    {
                        luckValue = 0;
                    }
                }

                //Height luck

                HeightLuck heightLuck = new HeightLuck();

                luckValue += heightLuck.getHeightLuckVal();

                //tile luck

                luckValue += TileLuck;

                //weather and moon luck

                WeatherLuck weatherLuck = new WeatherLuck();
                luckValue += weatherLuck.getWeatherLuckVal();

                //ladybug deaths

                luckValue += Utils.Clamp((float)EnemyLuck.amount, 0, .3);

                if (EnemyLuck.length == 0)
                    EnemyLuck.amount = 0;

                luckValue -= (float)Utils.Clamp(updateProximityLuck.proximityAmount, -.2, .2);
                Utils.Clamp(luckValue, 0, float.MaxValue);

                luckDebuffHandler(luckValue);


                //buff luck (must be calculated last)

                PlayerBuffLuck playerBuffLuck = new PlayerBuffLuck();
                luckValue += playerBuffLuck.getBuffLuck();
            }




                return luckValue;

                
            
                
        }

        public void luckDebuffHandler(double luckValue)
        {
            if (luckValue > 1f && !Main.LocalPlayer.unlockedBiomeTorches)
            {
                Player.AddBuff(ModContent.BuffType<Doomed>(), int.MaxValue, quiet: false);
                Player.ClearBuff(ModContent.BuffType<Blighted>());
                Player.ClearBuff(ModContent.BuffType<IllFated>());
                Player.ClearBuff(ModContent.BuffType<Jinxed>());
            }

            else if (luckValue > .75f && !Main.LocalPlayer.unlockedBiomeTorches)
            {
                Player.AddBuff(ModContent.BuffType<Blighted>(), int.MaxValue, quiet: false);
                Player.ClearBuff(ModContent.BuffType<Doomed>());
                Player.ClearBuff(ModContent.BuffType<IllFated>());
                Player.ClearBuff(ModContent.BuffType<Jinxed>());
            }

            else if (luckValue > .5f && !Main.LocalPlayer.unlockedBiomeTorches)
            {
                Player.AddBuff(ModContent.BuffType<IllFated>(), int.MaxValue, quiet: false);
                Player.ClearBuff(ModContent.BuffType<Doomed>());
                Player.ClearBuff(ModContent.BuffType<Blighted>());
                Player.ClearBuff(ModContent.BuffType<Jinxed>());
            }

            else if (luckValue > .25 && !Main.LocalPlayer.unlockedBiomeTorches)
            {
                Player.AddBuff(ModContent.BuffType<Jinxed>(), int.MaxValue, quiet: false);
                Player.ClearBuff(ModContent.BuffType<Doomed>());
                Player.ClearBuff(ModContent.BuffType<Blighted>());
                Player.ClearBuff(ModContent.BuffType<IllFated>());
            }

            else
            {
                Player.ClearBuff(ModContent.BuffType<Doomed>());
                Player.ClearBuff(ModContent.BuffType<Blighted>());
                Player.ClearBuff(ModContent.BuffType<IllFated>());
                Player.ClearBuff(ModContent.BuffType<Jinxed>());
            }
        }
    }
}