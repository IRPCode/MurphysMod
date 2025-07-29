using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace MurphysMod.Content.Enemies
{
    public class ToxicJungleSpore : ModProjectile
    {
        public override String Texture => "MurphysMod/Content/Projectiles/ToxicJungleSpore";

        public int x = 0;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = Main.rand.Next(450, 600);
            Projectile.aiStyle = 16;
            AIType = ProjectileID.BouncyGrenade;
            Projectile.alpha = default;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }
        public override bool CanHitPlayer(Player target)
        {
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = -Projectile.velocity.X * 0.95f;
            }

            if (oldVelocity.Y != Projectile.velocity.Y)
            {
                Projectile.velocity.X = -Projectile.velocity.X * 0.95f;
            }

            if (x % 2 == 0)
            {
                Projectile.velocity = new Vector2(0f, 0f);
            }

            x++;

            return false;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.7f, 0.8f, 0.1f));

            if (x % 10 == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.JungleSpore, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
            }

            x++;
        }

#pragma warning disable CS0672 // Member overrides obsolete member
        public override void OnKill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            SoundEngine.PlaySound(SoundID.Grass, Projectile.position);

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.JungleSpore, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
            }
        }

        public override void OnHitPlayer(Player player, Player.HurtInfo info)
        {
            player.AddBuff(BuffID.Poisoned, 600);
            Projectile.Kill();
        }
    }

}