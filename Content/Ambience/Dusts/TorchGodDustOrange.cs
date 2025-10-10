using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience;
using Microsoft.Xna.Framework.Graphics;

namespace MurphysMod.Content.Ambience.Dusts
{

    public class TorchGodDustOrange : ModDust
    {
        public override string Texture => "MurphysMod/Assets/Textures/Dusts/EmberDust";

        public override void OnSpawn(Dust dust)
        {
            dust.velocity = new Vector2(Main.rand.Next(-10,10) / 9, Main.rand.Next(6, 10) / 5);
            dust.noGravity = false;
            dust.scale = 1.5f;
            //dust.fadeIn = 2.5f;
            dust.alpha = 0;
        }

        public override bool Update(Dust dust)
        {
             if (dust.scale <= .4f) //prevents graphic shimmering
            {
                dust.scale *= .7f;
            }
            else
            {
                dust.scale *= .998f;
            }

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



            if (x % 100 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public override bool PreDraw(Dust dust)
        {
            Vector2 pos = dust.position - Main.screenPosition;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Dusts/GlowMasks/TorchGodDustOrangeGlowMask");

            Main.spriteBatch.Draw(
                texture,
                pos,
                null,
                Color.White,
                dust.rotation,
                texture.Size() / 2,
                dust.scale,
                SpriteEffects.None,
                0f
            );
            return false;

        }
    }
}