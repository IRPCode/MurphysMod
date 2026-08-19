using Microsoft.Xna.Framework;
using MurphysMod.Backgrounds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MurphysMod.Common.Biomes;
using System;

namespace MurphysMod.Content.Biomes
{
    public class Drippy : ModBiome
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Audio/Music/TheDripThatNeverDries");

        public override SceneEffectPriority Priority => SceneEffectPriority.Event;

        public override bool IsBiomeActive(Player player)
        {
            int x = 0;

            for (int i = 0; i < player.armor.Length; i++)
            {
                if (player.armor[i].type == ModContent.Find<ModItem>("MurphysMod", "TheBigDart").Type ||
                 player.armor[i].type == ItemID.TerrasparkBoots || player.armor[i].type == ItemID.Sunglasses)
                {
                    x++;
                }
            }

            if (x >= 3 && (Main.maxRaining < .6f || (Main.maxRaining >= .6 && Math.Abs(Main.windSpeedCurrent) <= .4f)) && Main.maxRaining > 0f && player.position.Y / 16 <= Main.worldSurface && !player.ZoneSkyHeight)
                return true;
            else
                return false;
        }
    }
}