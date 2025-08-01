using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using MurphysMod.Content.Buffs;
using MurphysMod.Systems;

namespace MurphysMod.Content.LuckHandlers
{
    public class LuckHandler : ModPlayer //modify this class to balance it with items. Make the highest bad luck level incredibly uncommon (without adding items or new tiles to the mix (maybe something similar to a demon alter? Like a sacrifical alter or something?))
    { //if possible, change the night sky depending on your luck level (I am thinking like aurora borealis becoming more and more red or something like that)

        public override void PostUpdate()
        {
            luckValue();
        }
        public float luckValue()
        {
            float luckValue = 0.0f;

            BookUsed bookUsed = ModContent.GetInstance<BookUsed>(); //prevents naturally occuring bad luck
            if (bookUsed.isPlayerCursed)
            {

                //bad luck biomes

                if (!Main.dayTime)
                {
                    luckValue += .30f;
                }

                if (Player.ZoneCorrupt || Player.ZoneCrimson)
                {
                    luckValue += .25f;
                }

                if (Player.ZoneUndergroundDesert || Player.ZoneJungle || Player.ZoneGraveyard || Player.ZoneDungeon)
                {
                    luckValue += .15f;
                }

                if (Player.ZoneNormalUnderground)
                {
                    luckValue += .1f;
                }

                if (Player.ZoneUnderworldHeight)
                {
                    luckValue += .35f;
                }

                if (Player.ZoneGraveyard)
                {
                    luckValue += .25f;
                }

                if (Player.ZoneWaterCandle)
                {
                    luckValue += .25f;
                }

                if (Player.ZoneShadowCandle)
                {
                    luckValue += .35f;
                }

                if (Player.ZoneLihzhardTemple)
                {
                    luckValue += .25f;
                }

                if (Player.ZoneRain || Player.ZoneSandstorm)
                {
                    luckValue += .15f;
                }

                if (Player.ZoneTowerNebula || Player.ZoneTowerSolar || Player.ZoneTowerVortex || Player.ZoneTowerStardust)
                {
                    luckValue += .4f;
                }

                //good luck biomes

                if (Player.ZoneGemCave || Player.ZoneGlowshroom || Player.ZoneGranite || Player.ZoneMarble)
                {
                    luckValue -= .25f;
                }

                if (Player.ZoneShimmer || Player.ZoneHallow)
                {
                    luckValue -= .1f;
                }

                //bad luck events

                if (Main.bloodMoon || (Main.invasionType > 5))
                {
                    luckValue += .25f;
                }

                if (Main.eclipse)
                {
                    luckValue += .35f;
                }

                if ((Main.invasionType > 0 && Main.invasionType < 4) || Main.invasionType == 5)
                {
                    luckValue += .15f;
                }

                //luck debuffs

                if (Player.HasBuff<Augury>())
                {
                    luckValue += .15f;
                }

                if (Player.HasBuff<BadOmen>())
                {
                    luckValue += .25f;
                }

                if (Player.HasBuff<Portent>())
                {
                    luckValue += .5f;
                }
            }

            luckDebuffHandler(luckValue);
            return luckValue;
        }

        public void luckDebuffHandler(float luckValue)
        {
            if (luckValue > 1f)
            {
                Player.AddBuff(ModContent.BuffType<Doomed>(), int.MaxValue, quiet: false);
                Player.ClearBuff(ModContent.BuffType<Blighted>());
                Player.ClearBuff(ModContent.BuffType<IllFated>());
                Player.ClearBuff(ModContent.BuffType<Jinxed>());
            }

            else if (luckValue > .75f)
            {
                Player.AddBuff(ModContent.BuffType<Blighted>(), int.MaxValue, quiet: false);
                Player.ClearBuff(ModContent.BuffType<Doomed>());
                Player.ClearBuff(ModContent.BuffType<IllFated>());
                Player.ClearBuff(ModContent.BuffType<Jinxed>());
            }

            else if (luckValue > .5f)
            {
                Player.AddBuff(ModContent.BuffType<IllFated>(), int.MaxValue, quiet: false);
                Player.ClearBuff(ModContent.BuffType<Doomed>());
                Player.ClearBuff(ModContent.BuffType<Blighted>());
                Player.ClearBuff(ModContent.BuffType<Jinxed>());
            }

            else if (luckValue > .25)
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