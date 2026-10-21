using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace MurphysMod.Content.Projectiles
{
    public class SalamanderSpit : ModProjectile
    {
        public override String Texture => "Terraria/Images/Projectile_" + ProjectileID.SalamanderSpit;
        private int bounceCount;
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.SalamanderSpit);
            AIType = ProjectileID.SalamanderSpit;
            Projectile.hostile = true;
            Projectile.friendly = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = -oldVelocity * .6f;
            bounceCount++;

            if (bounceCount >= 3)
            {
                bounceCount = 0;
                return true;
            }
            else
                return false;
        }
        public override void OnKill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Poisoned, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (!Main.expertMode || !Main.masterMode)
                return;
            target.AddBuff(BuffID.Poisoned, 100);
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Poisoned, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
    }
}
