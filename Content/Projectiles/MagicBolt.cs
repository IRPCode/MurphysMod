using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework.Graphics;

namespace MurphysMod.Content.Enemies
{
    public class MagicBolt : ModProjectile
    {
        public override String Texture => "MurphysMod/Content/Projectiles/FrigidBolt";
        public ref float DelayTimer => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Magic;

            Projectile.aiStyle = -1;
            Projectile.penetrate = 3;
        }
        public override void AI()
        {
            if (Projectile.ai[0] >= 50)
            {
                Projectile.velocity *= 1.01f;
            }
            else
            {
                Projectile.velocity = new Vector2(1,1);
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("MurphysMod/Assets/MagicFlare2");

            Color color = Color.Aquamarine;


            Main.spriteBatch.End();

            float rotation = Math.Clamp(MathHelper.Lerp(-25,25,Projectile.rotation), -25f, 25f);

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp,
            DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 1; i <= 2; i++)
            {
                Main.spriteBatch.Draw
                (
                    texture,
                    Projectile.Center - Main.screenPosition + new Vector2(0f, 1f), //smallest star
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    color * (i * .75f),
                    Projectile.rotation + rotation,
                    texture.Size() * 0.5f,
                    Projectile.Size / 100,
                    SpriteEffects.None,
                    0f
                );
            }

            Main.spriteBatch.End();

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
            DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            return false;

        }

#pragma warning disable CS0672 // Member overrides obsolete member
        public override void Kill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, Projectile.position);
        }
    }

}