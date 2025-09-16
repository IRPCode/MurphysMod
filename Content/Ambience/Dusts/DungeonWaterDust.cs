using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience;

namespace MurphysMod.Content.Ambience.Dusts
{

    public class DungeonWaterDust : ModDust
    {
        public override string Texture => "MurphysMod/Assets/Textures/Dusts/DungeonWaterDust";

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.scale = .75f;
            dust.alpha = 50;
        }

        public override bool Update(Dust dust)
        {
            dust.scale *= .999f;
            Lighting.AddLight(dust.position, new Vector3(0.096f, 0.643f, 1f) * dust.scale);
            
            int x = Tick.globalTick;
            dust.position += dust.velocity;

            if (x % 20 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
    }
}