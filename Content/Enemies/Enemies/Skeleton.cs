using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using System.Linq;
using MurphysMod.Content.Ambience;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MurphysMod.Content.LuckHandlers;
using MurphysMod.Systems;

namespace MurphysMod.Content.Enemies
{
    public class SkeletonNPC : GlobalProjectile //TODO: Make the clinger have a cursed flame flamethrower like the 1.4.4 flamethrower rework
    {
        public override bool InstancePerEntity => true;
        public bool shatter;
        public bool burning;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (!BookUsed.isPlayerCursed)
                return;
            if (!projectile.active)
                return;

            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckVal = luckHandler.luckValue();


            if (projectile.type == ProjectileID.SkeletonBone && source is EntitySource_Parent parentSource && parentSource.Entity is NPC)
            {
                shatter = false;
                burning = false;


                if (luckVal >= .75)
                {
                    shatter = true;
                    burning = true;
                }
                else if (luckVal >= .5)
                {
                    shatter = true;
                }
            }

            if(luckVal >= .25)
            {
                if (projectile.type == ProjectileID.FlamingArrow && source is EntitySource_Parent arrowSource && arrowSource.Entity is NPC)
            {
                projectile.velocity *= (float)(1 + (luckVal / 2));
            }

            if (projectile.type == ProjectileID.JavelinHostile && source is EntitySource_Parent javelinSource && javelinSource.Entity is NPC)
            {
                projectile.velocity *= (float)(1 + (luckVal / 2));
            }
            }
            
        }

        public override void AI(Projectile projectile)
        {
            if (burning && (projectile.type == ProjectileID.SkeletonBone || projectile.type == ModContent.ProjectileType<BoneChunk>() || projectile.type == ModContent.ProjectileType<BoneSplinter>()))
            {
                int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BoneTorch, projectile.velocity.X * .9f, projectile.velocity.Y * .9f, default, default);
                Dust dust = Main.dust[dustIndex];
                dust.noGravity = true;
            }
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (burning && (projectile.type == ProjectileID.SkeletonBone || projectile.type == ModContent.ProjectileType<BoneChunk>() || projectile.type == ModContent.ProjectileType<BoneSplinter>()))
                target.AddBuff(BuffID.OnFire, 300);
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {

            if (shatter && projectile.type == ProjectileID.SkeletonBone)
            {
                for (int i = 0; i < 3; i++)
                {
                    int rand = Main.rand.Next(0, 3);
                    int boneFragType;

                    if (rand == 0)
                        boneFragType = ModContent.ProjectileType<BoneChunk>();
                    else
                        boneFragType = ModContent.ProjectileType<BoneSplinter>();


                    projectile.velocity.X = (projectile.oldVelocity.X * (Main.rand.Next(-100, 101) / 10)) * .1f;
                    projectile.velocity.Y *= -.5f;


                    int projID = Projectile.NewProjectile(
                           projectile.GetSource_FromAI(),
                           projectile.Center,
                           projectile.velocity,
                           boneFragType,
                           projectile.damage,
                           projectile.knockBack,
                           -1);

                    Projectile proj = Main.projectile[projID];
                    proj.hostile = true;
                    proj.friendly = false;
                    proj.tileCollide = true;
                    proj.netUpdate = true;
                }
            }
            return true;
        }
    }

    public class SkeletonNPCAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        //numerically identified (non NPCID values) are bone throwing skeletons
        //all neg ints are other skeleton variants
        public int[] skeletonIDS = [NPCID.Skeleton, NPCID.BigSkeleton, NPCID.SmallSkeleton, NPCID.BoneThrowingSkeleton, NPCID.BoneThrowingSkeleton2, NPCID.BoneThrowingSkeleton3, NPCID.BoneThrowingSkeleton4,
        NPCID.BigSkeleton, NPCID.HeadacheSkeleton, NPCID.BigHeadacheSkeleton, NPCID.SmallHeadacheSkeleton, NPCID.MisassembledSkeleton, NPCID.BigMisassembledSkeleton,
        NPCID.SmallMisassembledSkeleton, NPCID.PantlessSkeleton, NPCID.BigPantlessSkeleton, NPCID.SmallPantlessSkeleton, NPCID.SkeletonTopHat, NPCID.SkeletonAlien,
        NPCID.SkeletonAstonaut, NPCID.UndeadMiner, NPCID.ArmoredSkeleton, NPCID.ArmoredViking, NPCID.GreekSkeleton, NPCID.SporeSkeleton, NPCID.HeavySkeleton, NPCID.UndeadViking, NPCID.SkeletonArcher];

        public int[] projectileSkeletons = [NPCID.BoneThrowingSkeleton, NPCID.BoneThrowingSkeleton2, NPCID.BoneThrowingSkeleton3, NPCID.BoneThrowingSkeleton4,
        NPCID.GreekSkeleton, NPCID.SkeletonArcher];
        public bool shatter;
        public bool burning;
        bool jumpCheck;

        public override void PostAI(NPC npc)
        {
            if (!BookUsed.isPlayerCursed)
                return;
            if (!npc.active)
                return;
            if(!skeletonIDS.Contains(npc.type))
                return;

            Player player = Main.player[npc.target];
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();

            if (npc.velocity.Y == 0f)
                jumpCheck = true;

            if (npc.velocity.Y == -7.7f && jumpCheck)
            {
                npc.velocity.Y *= (float)(1 + (luckHandler.luckValue() / 3));
                jumpCheck = false;
            }

            float mult = 1 * (float)Math.Pow(luckHandler.luckValue() + 1, 1.4);
            if (npc.ai[1] == 0 && !projectileSkeletons.Contains(npc.type))
                npc.velocity.X = MathHelper.Lerp(npc.oldVelocity.X, npc.direction * mult, .025f);
        }
        public override void OnKill(NPC npc)
        {
            if (!BookUsed.isPlayerCursed)
                return;
            if (!npc.active)
                return;

            Player player = Main.player[npc.target];
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();

            if (npc.type == NPCID.UndeadMiner && luckHandler.luckValue() >= .25)
            {
                int projID = Projectile.NewProjectile(
                          npc.GetSource_FromAI(),
                          npc.Center,
                          new Vector2(Main.rand.Next(-1000, 1000) / 250, Main.rand.Next(-1000, 1000) / 250) * (float)(1f + luckHandler.luckValue()),
                          ModContent.ProjectileType<CaveBomb>(),
                          25, //for damage balancing
                          1f,
                          -1);
            }
        }
    }
}
