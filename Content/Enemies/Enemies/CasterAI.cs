using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MurphysMod.Content.Projectiles;
using System;
using MurphysMod.Content.LuckHandlers;
using System.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using MurphysMod.Systems;
using Terraria.DataStructures;
using MurphysMod.Content.Buffs;
using Terraria.GameContent.Events;

namespace MurphysMod.Content.Enemies
{
    //Impacts projectiles and caster DR
    public class CasterAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int[] acceptedNPCs = { NPCID.DarkCaster, NPCID.DiabolistRed, NPCID.DiabolistWhite, NPCID.Necromancer, NPCID.NecromancerArmored, NPCID.RaggedCaster, NPCID.RaggedCasterOpenCoat,
        NPCID.FireImp, NPCID.GoblinSorcerer, NPCID.Tim, NPCID.DesertBeast};
        public int[] acceptedNPCProj = { };
        public Vector2 savedVel;

        public static int baseDefense = -1;

        public override void PostAI(NPC npc)
        {
            if (!BookUsed.isPlayerCursed)
                return;
            if (!acceptedNPCs.Contains(npc.type) && !npc.active)
                return;

            Player target = Main.player[npc.target];
            LuckHandler luckHandler = target.GetModPlayer<LuckHandler>();
            double luckValue = luckHandler.luckValue();
        }
    }
    public class CasterProjectiles : GlobalProjectile //TODO: remove extra ticks for necromancer's proj.
    {
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (!BookUsed.isPlayerCursed)
                return;

            Player target = Main.LocalPlayer;
            LuckHandler luckHandler = target.GetModPlayer<LuckHandler>();
            double luckValue = luckHandler.luckValue();

            if (projectile.type == ProjectileID.ShadowBeamHostile)
            {
                projectile.extraUpdates = Math.Clamp((int)(100 - (((((Math.Pow(10, (1 + luckValue))) / 60) * 20) - .29f) * 3)), 1, 33);
                projectile.timeLeft *= 2;
            }
            else if (projectile.type == ProjectileID.InfernoHostileBlast)
            {
                if (luckValue >= .25)

                    for (int i2 = 0; i2 < (int)(((luckValue) * 100) / 25); i2++)
                    {
                        int projID = Projectile.NewProjectile(
                                  projectile.GetSource_FromAI(),
                                  projectile.Center,
                                  new Vector2(Main.rand.Next(-10000, 10000) / 2500, Main.rand.Next(-10000, 10000) / 2500),
                                  ProjectileID.GreekFire3,
                                  40, //for damage balancing
                                  1f,
                                  -1);

                        Projectile proj = Main.projectile[projID];
                        proj.hostile = true;
                        proj.tileCollide = true;
                        proj.netUpdate = true;
                    }
            }
        }

        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            if (!BookUsed.isPlayerCursed)
                return true;

            LuckHandler luckHandler = target.GetModPlayer<LuckHandler>();
            double luckValue = luckHandler.luckValue();

            if (projectile.type == ProjectileID.LostSoulHostile)
            {
                if (projectile.Hitbox.Intersects(target.Hitbox) && !(target.HasBuff(ModContent.BuffType<Portent>()) || (target.HasBuff(ModContent.BuffType<BadOmen>())) || (target.HasBuff(ModContent.BuffType<Augury>()))))
                {
                    if (luckValue >= .7)
                        target.AddBuff(ModContent.BuffType<Portent>(), 60 * 5);
                    else if (luckValue >= .4)
                        target.AddBuff(ModContent.BuffType<BadOmen>(), 60 * 5);
                    else
                        target.AddBuff(ModContent.BuffType<Augury>(), 60 * 5);
                }
                projectile.Kill();
                return false;
            }
            return true;
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (!BookUsed.isPlayerCursed)
                return;

            if (projectile.type == ProjectileID.LostSoulHostile)
            {
                if (!target.HasBuff(BuffID.Blackout))
                {
                    target.ClearBuff(BuffID.Blackout);
                }
            }
        }

        public override void AI(Projectile projectile)
        {
            if (!BookUsed.isPlayerCursed)
                return;

            Player target = Main.LocalPlayer;
            LuckHandler luckHandler = target.GetModPlayer<LuckHandler>();
            double luckValue = luckHandler.luckValue() + 1;
            Vector2 vectorLuck = new Vector2((float)luckValue, (float)luckValue);
            Vector2 playerLoc = projectile.DirectionTo(target.Center);

            if (projectile.type == ProjectileID.DesertDjinnCurse)
            {
                vectorLuck *= 2f;
                projectile.velocity = playerLoc * vectorLuck;
            }

            else if (projectile.type == ProjectileID.RuneBlast)
            {
            uint x = Main.GameUpdateCount;
            
            projectile.velocity *= (float)(1.01 - (luckValue / 100));
            Vector2 playerLoc1 = projectile.DirectionTo(target.Center) / (float)((2 - luckValue));
            Vector2 vectorLuck1 = new Vector2((float)luckValue, (float)luckValue);
            playerLoc1.Normalize();
            playerLoc1 *= (float)((1 + luckValue) * 1.25);

            Vector2 oldVel = projectile.velocity;
            projectile.velocity = oldVel + (playerLoc1 / 20);

            //projectile.velocity.X += (float)Math.Sin(x * 5);
            //projectile.velocity.Y += (float)Math.Cos(x * 5);

            projectile.velocity.X = (float)Math.Clamp(projectile.velocity.X, -15, 15);
            projectile.velocity.Y = (float)Math.Clamp(projectile.velocity.Y, -15, 15);
            }
        }
    }
}