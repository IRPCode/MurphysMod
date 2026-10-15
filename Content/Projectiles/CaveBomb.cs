using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MurphysMod.Content.LuckHandlers;
using static MurphysMod.Content.CaveTremors;

namespace MurphysMod.Content.Enemies
{
    public class CaveBomb : ModProjectile //this section is a modified version of this: https://github.com/tModLoader/tModLoader/blob/1.4.5/ExampleMod/Content/Projectiles/ExampleExplosive.cs
    {
        public override String Texture => "Terraria/Images/Projectile_28";
        private const int DefaultWidthHeight = 22;
        public int ExplosionWidthHeight = 250;
        public int Projdamage = 100;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Explosive[Type] = false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 25;
            Projectile.damage = 0;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300; //Change this with luck level;
            DrawOffsetX = -2;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.expertMode || Main.masterMode)
                if (target.type >= NPCID.EaterofWorldsBody && target.type <= NPCID.EaterofWorldsTail)
                    modifiers.FinalDamage /= 5;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.soundDelay == 0)
            {
                SoundEngine.PlaySound(SoundID.Dig);
            }

            Projectile.soundDelay = 10;

            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();

            float mod = -.4f - ((float)luckHandler.luckValue() / 4);


            if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
                Projectile.velocity.X = oldVelocity.X * mod;

            if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
                Projectile.velocity.Y = oldVelocity.Y * mod;

            return false;
        }

        public override void AI()
        {
            Projectile.spriteDirection = -1;


            if (Projectile.owner != Main.myPlayer && Projectile.timeLeft < 5)
                Projectile.timeLeft = 5;

            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.PrepareBombToBlow();
                Projectile.Kill();
                
                return;
            }

            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                dust.scale = 0.1f + Main.rand.Next(5) * 0.1f;
                dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
                dust.noGravity = true;
                dust.position = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;

                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Main.rand.Next(-1000, 1000) / 250, Main.rand.Next(-1000, 1000) / 250, 100, default, 1f);
                dust.scale = 1f + Main.rand.Next(5) * 0.1f;
                dust.noGravity = true;
                dust.position = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;
            }

            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 10f)
            {
                Projectile.ai[1] = 10f;
                // Roll speed dampening.
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.96f;

                    if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                // Delayed gravity
                Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
            }
            // Rotation increased by velocity.X
            Projectile.rotation += Projectile.velocity.X * 0.1f;
        }

        public override void PrepareBombToBlow()
        {
            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();

            ExplosionWidthHeight *= (int)(1 + (luckHandler.luckValue()) / 2);
            Projectile.Resize(ExplosionWidthHeight, ExplosionWidthHeight);
            Projdamage *= (int)(1 + luckHandler.luckValue()); // Bomb: 100, Dynamite: 250
            Projectile.knockBack = 10f * (int)(1 + luckHandler.luckValue()); // Bomb: 8f, Dynamite: 10f
        }

        public override void OnKill(int timeLeft)
        {
             Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();

            if (Vector2.Distance(Main.LocalPlayer.Center, Projectile.Center) <= ExplosionWidthHeight / 2)
                Main.LocalPlayer.Hurt(PlayerDeathReason.ByProjectile(Main.myPlayer, Projectile.whoAmI), Projdamage, Main.LocalPlayer.Center.X < Projectile.Center.X ? -1 : 1);

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            checkTileDestruction.tileBroken += (int)(25 * (1 + luckHandler.luckValue())); //adds to tremor risk

             if(Vector2.Distance(Projectile.Center, Main.LocalPlayer.Center) <= 15 * 16)
                ShakeScreen.StartShake(2, 20, MathHelper.Clamp(8f * MathHelper.Clamp(1f - Vector2.Distance(Projectile.Center, Main.LocalPlayer.Center) / 160f, 0f, 1f), 0f, 2f));

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (Vector2.Distance(npc.Center, Projectile.Center) <= ExplosionWidthHeight / 2)
                {
                    NPC.HitInfo damage = new NPC.HitInfo
                    {
                        Damage = Projdamage,
                        Knockback = 1f,
                        HitDirection = npc.Center.X < Projectile.Center.X ? -1 : 1,
                        Crit = true
                    };
                    npc.StrikeNPC(damage);
                    if (Main.netMode != NetmodeID.SinglePlayer)
                        NetMessage.SendStrikeNPC(npc, damage);
                }
            }

            // Smoke Dust spawn
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
            }

            for (int i = 0; i < 80; i++)
            {
                int dustType = Dust.NewDust(Projectile.Center, default, default, DustID.Torch, Main.rand.Next(-3000,3000)/750, Main.rand.Next(-3000,3000)/750, 100, default, 2f);
                Dust dust = Main.dust[dustType];
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust.velocity *= 3f;
                
            }

            for (int g = 0; g < 2; g++)
            {
                var goreSpawnPosition = new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f);
                Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X += 1.5f;
                gore.velocity.Y += 1.5f;
                gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X -= 1.5f;
                gore.velocity.Y += 1.5f;
                gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X += 1.5f;
                gore.velocity.Y -= 1.5f;
                gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X -= 1.5f;
                gore.velocity.Y -= 1.5f;
            }
            Projectile.Resize(DefaultWidthHeight, DefaultWidthHeight);
            ExplosionWidthHeight = 250;
        }
    }
}