using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace MurphysMod.Content.Enemies
{
    public class SlimyBoulder : ModProjectile
    {
        public override String Texture => "MurphysMod/Assets/Textures/Projectiles/SlimyBoulder";
        public override void SetDefaults() 
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = Main.rand.Next(100, 300);
            Projectile.aiStyle = 14;
            Projectile.alpha = default;
            AIType = ProjectileID.Glowstick;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public override void AI()
        {
            Projectile.velocity.Y += .5f;
            Projectile.velocity.X += .2f;
            Projectile.rotation += Projectile.velocity.X * .0005f;
        }
        public override bool CanHitPlayer(Player target)
        {
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X = 0f;
            Projectile.velocity.Y = 0f;
            
            return false;
        }

#pragma warning disable CS0672 // Member overrides obsolete member
        public override void Kill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position); //fix this to find the real sound needed for arrows hitting the ground

            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SnowBlock, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, Color.Blue, 1f);
            }
        }
    }

}