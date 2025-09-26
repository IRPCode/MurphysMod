using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using System.Linq;
using MurphysMod.Content.Ambience;
using Microsoft.Xna.Framework;

namespace MurphysMod.Content.Enemies
{
    public class SkeletonNPC : GlobalNPC //TODO: Make the clinger have a cursed flame flamethrower like the 1.4.4 flamethrower rework
    {
        public override bool InstancePerEntity => true;
        int[] skeletonType = {-46, -47, -48, -49, -50, -51, -52, -53, NPCID.Skeleton, NPCID.HeadacheSkeleton, NPCID.MisassembledSkeleton, NPCID.PantlessSkeleton,
            NPCID.SkeletonTopHat, NPCID.SkeletonAstonaut, NPCID.SkeletonAlien, 449, 450, 451, 452, NPCID.SporeSkeleton};
        //numerically identified (non NPCID values) are bone throwing skeletons
        //all neg ints are other skeleton variants
        public override void AI(NPC npc)
        {
            if (skeletonType.Contains(npc.type))
            {
                int x = Tick.globalTick;
                Player player = Main.player[npc.target];
                float distanceFromPlayer = Vector2.Distance(npc.Center, player.Center);

                Vector2 playerLocation = player.Center - npc.Center;
                playerLocation.Normalize();


                if (npc.localAI[0] % 100 == 0 && distanceFromPlayer <= 100f)
                {
                    npc.localAI[0] = 0;
                    int projID = Projectile.NewProjectile(
                           npc.GetSource_FromAI(),
                           npc.Center,
                           playerLocation,
                           ModContent.ProjectileType<RustySwordProjectile>(),
                           25, //for damage balancing
                           5,
                           -1);

                    Projectile proj = Main.projectile[projID];
                    proj.hostile = true;
                    proj.friendly = false;
                    proj.tileCollide = true;
                    proj.netUpdate = true;
                }
                npc.localAI[0]++;
            }
        }
    }
}