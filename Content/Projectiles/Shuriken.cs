using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;

namespace MurphysMod.Content.Enemies
{
    public class Shuriken : ModProjectile
    {
        public override String Texture => "Terraria/Images/Item_42";
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
            AIType = 2;
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
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position); //fix this to find the real sound needed for arrows hitting the ground

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
    }

}