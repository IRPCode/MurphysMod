using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience;

namespace MurphysMod.Content.Ambience.Dusts
{

    public class AetherflyDust : ModDust
    {
        public override string Texture => "MurphysMod/Assets/Textures/Dusts/AetherflyDust"; //TODO: seperate this file into 3 sprites to make them emit different colors

        public static bool trigger = false;
        public override void OnSpawn(Dust dust)
        {
            dust.velocity = new Vector2(Main.rand.Next(-200, 200) / 200, Main.rand.Next(5, 10) / 5);

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

            Color dustColor = dust.color;

            Lighting.AddLight(dust.position, Color.White.ToVector3() * (1f - (dust.alpha / 255f)));

            dust.position += dust.velocity;

            if (x % 10 == 0)
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