using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;
using Terraria.DataStructures;

namespace MurphysMod.Content.Enemies
{
    public class RabidSpit : ModProjectile
    {
        public override String Texture => "Terraria/Images/Projectile_" + ProjectileID.WebSpit;
        public override void SetDefaults() 
        {
           Projectile.CloneDefaults(ProjectileID.WebSpit);
           AIType = ProjectileID.WebSpit;
           Projectile.hostile = true;
           Projectile.friendly = false;
        }

        public override void OnKill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            SoundEngine.PlaySound(SoundID.Item17, Projectile.position); //fix this to find the real sound needed for arrows hitting the ground

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FoodPiece, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if(!Main.expertMode || !Main.masterMode)
                return;
             target.AddBuff(BuffID.Rabies, 100);
        }

        public override void OnSpawn(IEntitySource source)
        {
             for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FoodPiece, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
    }
}