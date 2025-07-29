using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace MurphysMod.Content.Enemies
{
    public class WormSpit : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_39";
        public override void SetDefaults() //TODO: Make it so players can break this projectile with tools and weapons, and make it so the texture changes depending on 
        //what is spawning the projectile (remember, bone serpents and wyverns also use this AI type.)
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 500;
            Projectile.aiStyle = 1;
            Projectile.alpha = default;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public override void AI()
        {
             Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Mud, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
        }
        public override bool CanHitPlayer(Player target)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();

            return false;
        }

#pragma warning disable CS0672 // Member overrides obsolete member
        public override void OnKill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position); //fix this to find the real sound needed for arrows hitting the ground

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Mud, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
            }
        }
    }

}