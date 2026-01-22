using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Build.Evaluation;

namespace MurphysMod.Content.Enemies
{
    public class GiantHailstone : ModProjectile
    {
        public bool hitGround = false;
        public override string Texture => "MurphysMod/Assets/Textures/Projectiles/LargeHailstone";
        public override void SetDefaults() //TODO: Make it so players can break this projectile with tools and weapons, and make it so the texture changes depending on 
        //what is spawning the projectile (remember, bone serpents and wyverns also use this AI type.)
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 500;
            AIType = ProjectileID.Glowstick;
            Projectile.alpha = default;
            Projectile.friendly = true;
            Projectile.hostile = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }
        public override void AI()
        {
            if (hitGround)
            {
                 Projectile.damage = 0;
                 Projectile.velocity = Vector2.Zero;
            }

            Projectile.rotation += Projectile.velocity.X;             
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.velocity.Y >= 4f && !(Projectile.velocity.X >= 2f))
            {
                Texture2D texture = TextureAssets.Projectile[Type].Value;
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY / 2);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                }
            }

            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) //TODO: Make screen shake a little when the player is within 10 tiles of the hailstone hitting the ground.
        {
            Player player = Main.LocalPlayer;

            if (!hitGround)
            {
                SoundEngine.PlaySound(SoundID.Item50, Projectile.Center); //fix this to find the real sound needed for arrows hitting the ground
                 SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
                Projectile.velocity.Y /= 3;

                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
                }
            }
            hitGround = true;

            Projectile.velocity = Vector2.Zero;

            if(Vector2.Distance(Projectile.Center, player.Center) <= 15 * 16) //15 tiles
                ShakeScreen.StartShake(2, 20, 3f / (Vector2.Distance(Projectile.Center, player.Center) / 20));// TODO: make this falloff less aggressive
            return false;
        }

#pragma warning disable CS0672 // Member overrides obsolete member
        public override void OnKill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
        {

            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Ice, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
            }
        }
    }

}