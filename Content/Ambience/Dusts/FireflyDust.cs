using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience;

namespace MurphysMod.Content.Ambience.Dusts
{

    public class FireflyDust : ModDust
    {
        public override string Texture => "MurphysMod/Content/Ambience/Dusts/FireflyDust";

        public override void OnSpawn(Dust dust)
        {
            dust.velocity = new Vector2(0,0);
            dust.noGravity = true;
            dust.scale = 1.25f;
            dust.alpha = 50;
        }

        public override bool Update(Dust dust)
        {
            dust.scale *= .999f;
            Lighting.AddLight(dust.position, new Vector3(0.973f, 0.911f, 0.364f) * (dust.scale / 2));
            
            int x = Tick.globalTick;
            dust.position += dust.velocity;

            if (x % 3 == 0)
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