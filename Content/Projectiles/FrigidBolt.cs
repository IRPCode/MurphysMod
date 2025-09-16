using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace MurphysMod.Content.Enemies
{
    public class FrigidBolt : ModProjectile
    {
        public override String Texture => "Terraria/Images/Projectile_118";
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

        private NPC Homing
        {
            get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0] - 1];

            set
            {
                Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
            }
        }

        public bool isValidTarget(NPC target)
        {
            return target.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 1, 1, target.position, target.width, target.height);
        }

        public NPC findClosestNPC(float maxDetectDistance)
        {
            NPC closest = null;

            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            foreach (var target in Main.ActiveNPCs)
            {
                if (isValidTarget(target))
                {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closest = target;
                    }
                }
            }
            return closest;
        }

        public override void AI()
        {

            float radius = 400f;

            if (DelayTimer < 10)
            {
                DelayTimer++;
                return;
            }

            if (Homing == null)
            {
                Homing = findClosestNPC(radius);
            }

            if (Homing != null && !isValidTarget(Homing))
            {
                Projectile.velocity *= 1.001f;
            }

            float length = Projectile.velocity.Length();
            float targetAngle = Projectile.AngleTo(Homing.Center);
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(3)).ToRotationVector2() * length;
            Projectile.rotation = Projectile.velocity.ToRotation();




            /*Projectile.ai[0]++;

            if (Projectile.ai[0] < 60f)
            {
                Projectile.velocity *= 1.01f;
            }
            else
            {
                Projectile.velocity *= 1.05f;

                if (Projectile.ai[0] >= 180f)
                {
                    Projectile.Kill();
                }
            }

            float rotateSpeed = 0.35f * (float)Projectile.direction;

            Projectile.rotation += rotateSpeed; */

            Lighting.AddLight(Projectile.Center, new Vector3(0.75f, 0.75f, 1f));

            if (Main.rand.NextBool(2))
            {
                int numToSpawn = Main.rand.Next(5);

                for (int i = 0; i < numToSpawn * 2; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SnowflakeIce, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 1f);
                }
            }
        }

#pragma warning disable CS0672 // Member overrides obsolete member
        public override void Kill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, Projectile.position);
        }
    }

}