using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience;

namespace MurphysMod.Content.Ambience.Dusts
{

    public class BlueMushroomDust : ModDust
    {
        public override string Texture => "MurphysMod/Content/Ambience/Dusts/BlueMushroomDust";

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.scale = .5f;
            dust.fadeIn = 1f;
            dust.alpha = 0;
        }

        public override bool Update(Dust dust)
        {
            dust.scale *= .999f;
            Lighting.AddLight(dust.position, new Vector3(0.0f, 0.03f, 1.0f) * dust.scale);
            int x = Tick.globalTick;
            dust.position += dust.velocity;

            if (x % 15 == 0)
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