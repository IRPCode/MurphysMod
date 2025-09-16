using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience;

namespace MurphysMod.Content.Ambience.Dusts
{

    public class PinkHallowFireflyDust : ModDust
    {
        public override string Texture => "MurphysMod/Assets/Textures/Dusts/PinkHallowFireflyDust";

        public static bool trigger = false;
        public override void OnSpawn(Dust dust)
        {

            dust.velocity = new Vector2(Main.rand.Next(5, 10) / 5, Main.rand.Next(-200, 200) / 150);

            if (Main.rand.Next(0, 2) == 1)
            {
                dust.velocity *= new Vector2(-1, 1);
            }
            dust.fadeIn = 0f;
            dust.noGravity = true;
            dust.scale = 1.5f;
            dust.alpha = 0;
        }

        public override bool Update(Dust dust)
        {
            int x = Tick.globalTick;

            if (trigger == true)
            {
                dust.alpha += 1;
            }

            Lighting.AddLight(dust.position, new Vector3(1.0f, 0.295f, 0.727f) * (1f - (dust.alpha / 255f)));

            dust.position += dust.velocity * new Vector2(1,-1.5f);

            if (x % 25 == 0)
            {
                return true;
            }
            else if (x % 1 == 0 && trigger == false)
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