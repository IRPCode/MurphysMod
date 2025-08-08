using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience;

namespace MurphysMod.Content.Ambience.Dusts
{

    public class TorchGodDustBlue : ModDust
    {
        public override string Texture => "MurphysMod/Content/Ambience/Dusts/TorchGodDustBlue";

        public override void OnSpawn(Dust dust)
        {
            dust.velocity = new Vector2(Main.rand.Next(-10,10) / 9, Main.rand.Next(2, 8) / 5);
            dust.noGravity = false;
            dust.scale = 1.5f;
            //dust.fadeIn = 2.5f;
            dust.alpha = 0;
        }

        public override bool Update(Dust dust)
        {
            dust.scale *= .998f;
            
            int x = Tick.globalTick;
            dust.position -= dust.velocity * new Vector2(1,1.3f);


            if (dust.velocity.X != 0)
            {
                dust.rotation += .05f * dust.velocity.X;
            }
            else
            {
                dust.rotation += .02f;
            }

            

            if (x % 100 == 0)
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