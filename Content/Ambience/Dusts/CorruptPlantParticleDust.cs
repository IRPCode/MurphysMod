using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience;
using System;

namespace MurphysMod.Content.Ambience.Dusts
{

    public class CorruptPlantParticleDust : ModDust
    {
        public override string Texture => "MurphysMod/Assets/Textures/Dusts/CorruptPlantParticleDust";
        
        public static bool trigger = false;

        public override void OnSpawn(Dust dust)
        {
            dust.velocity = new Vector2((Main.windSpeedCurrent * 3) * (1 + (Main.rand.Next(1, 100) / 100)), (100 - Math.Abs(Main.windSpeedCurrent) * 100) / 100);

            dust.noGravity = false;
            dust.scale = 1f;
            dust.alpha = 0;
        }

        public override bool Update(Dust dust)
        {
            int x = Tick.globalTick;

            Vector2 dustTileLocation = new Vector2(dust.position.X, dust.position.Y);

            int tileX = (int)(dustTileLocation.X / 16);
            int tileY = (int)(dustTileLocation.Y / 16);

            if (trigger == true)
            {
                dust.alpha += 1;
            }

            if ((Framing.GetTileSafely(tileX, tileY).HasTile && Main.tileSolid[Framing.GetTileSafely(tileX, tileY).TileType])) //stops particle
            {
                dust.velocity = new Vector2(0, 0);
                trigger = true;
            }

            dust.position += dust.velocity * new Vector2(1, 3f);
            dust.rotation += dust.velocity.X / 15f;

            if (x % 150 == 0)
            {
                return true;
            }
            else if (x % 50 == 0 && trigger == false)
            {
                trigger = true; //solution makes brain happy :o
                return false;
            }
            else
            {
                return false;
            }


        }
    }
}