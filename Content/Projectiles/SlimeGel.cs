using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;


namespace MurphysMod.Content.Projectiles
{
    public class SlimeGel : ModProjectile
    {
        public override String Texture => "MurphysMod/Content/Projectiles/SlimeGel";
        public override void SetDefaults() //for slime gel projectiles
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = Main.rand.Next(100, 300);
            Projectile.aiStyle = 14;
            Projectile.alpha = 75;
            AIType = ProjectileID.Glowstick;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }
        public override bool CanHitPlayer(Player target)
        {
            return true;
        }

#pragma warning disable CS0672 // Member overrides obsolete member
        public override void Kill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SnowBlock, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, Color.Blue, 1f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }

}