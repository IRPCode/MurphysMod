using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience;

namespace MurphysMod.Content.Ambience.Dusts
{

    public class BlessedWaterDust : ModDust
    {
        public override string Texture => "MurphysMod/Assets/Textures/Dusts/BlessedWaterDust";

        public override void OnSpawn(Dust dust)
        {
            dust.velocity = new Vector2(Main.rand.Next(-10, 10) / 9, Main.rand.Next(2, 8) / 5);
            dust.position += new Vector2((Main.rand.Next(-12, 12) / 10f) * 16f, Main.rand.Next(0,3) * 16f);
            dust.noGravity = false;
            dust.scale = 2f;
            //dust.fadeIn = 2.5f;
            dust.alpha = 50;
        }

        public override bool Update(Dust dust)
        {
            dust.scale *= .97f;
            Lighting.AddLight(dust.position, (new Vector3(0.6f, 0.88f, 0.96f) / 1.1f) * dust.scale);

            int x = Tick.globalTick;
            dust.position -= dust.velocity * new Vector2(1, 1.3f);


            if (dust.velocity.X != 0)
            {
                dust.rotation += .05f * dust.velocity.X;
            }
            else
            {
                dust.rotation += .02f;
            }



            if (x % 1 == 0)
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