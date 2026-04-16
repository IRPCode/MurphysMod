using Microsoft.Xna.Framework;
using MurphysMod.Backgrounds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MurphysMod.Common.Biomes;

namespace MurphysMod.Content.Biomes
{
    public class OrdainedForgeBiome : ModBiome //TODO: Change the background if the player is in the 406x200 structure block
    {
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<ForgeBackgroundStyle>(); //TODO: Make this work underground
        //public override ModBackgroundStyle ModBackgroundStyle => ModContent.GetInstance<ForgeBackgroundStyle>(); ----- ?????

        public override int Music => MusicID.OtherworldlyUnderground;
        //change Music to MusicLoader.GetMusicSlot(Mod, "Assets/Music/SongName");

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;

        public override bool IsBiomeActive(Player player)
        {
            if (player.Center.Y / 16f <= (Main.maxTilesY / 2) + 100 && player.Center.Y / 16f >= (Main.maxTilesY / 2) - 100 && player.Center.X / 16f <= (Main.maxTilesX / 2) + 203 && player.Center.X / 16f >= (Main.maxTilesX / 2) - 203)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class PreventForgeBuilding : ModPlayer
    {
        public override void PostUpdate()
        {
            //Player player = Main.LocalPlayer;
            if (Player.InModBiome<OrdainedForgeBiome>())
            {
                Player.noBuilding = true;
                Player.AddBuff(BuffID.NoBuilding, 1);
            } else 
            Player.noBuilding = false;
                
        }
    }

    public class spawnRatesForForge : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.InModBiome<OrdainedForgeBiome>())
            {
                spawnRate = int.MinValue;
                maxSpawns = 0;
            }
        }
    }
}