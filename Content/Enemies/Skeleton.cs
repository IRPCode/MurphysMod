using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using System.Linq;
using MurphysMod.Content.Ambience;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MurphysMod.Content.LuckHandlers;

namespace MurphysMod.Content.Enemies
{
    public class SkeletonNPC : GlobalProjectile //TODO: Make the clinger have a cursed flame flamethrower like the 1.4.4 flamethrower rework
    {
        public override bool InstancePerEntity => true;

        //numerically identified (non NPCID values) are bone throwing skeletons
        //all neg ints are other skeleton variants

        public bool shatter;
        public bool burning;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {

            if (projectile.type == ProjectileID.SkeletonBone && source is EntitySource_Parent parentSource && parentSource.Entity is NPC)
            {
                shatter = false;
                burning = false;

                Player player = Main.LocalPlayer;
                LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
                float luckVal = luckHandler.luckValue();

                if (luckVal >= 1f)
                {

                }
                else if (luckVal >= .75f)
                {
                    shatter = true;
                    burning = true;
                }

                // change to bone knives

                else if (luckVal >= .25f)
                {


                    if (luckVal >= .5f)
                    {
                        shatter = true;
                    }


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
}
