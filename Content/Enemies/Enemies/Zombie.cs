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

namespace MurphysMod.Content.Enemies
{
    //When 1.4.5 gets to tmodloader, add code for the moss zombie here
    public class Zombie : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int[] acceptedNPCs = { 431, 432, 433, 434, 435, 436, 3, -26, -27, 430, 132, -28, -29, 186, -30, -31, 188, 34, -35, 189, -36, -37, 200, -44, -45, 319, 320, 321, 331, 332, 489,
        NPCID.TheBride, NPCID.TheGroom, NPCID.DoctorBones, NPCID.ZombieMushroom, NPCID.ZombieMushroomHat, NPCID.Eyezor };
        public int[] slimeZombieNPCs = { 187, -32, -33 };

        public int[] torchZombiesNPCs = { NPCID.TorchZombie, NPCID.ArmedTorchZombie };

        public static int swingCounter = 0;

        public override void PostAI(NPC npc)
        {
            if (!BookUsed.isPlayerCursed)
                return;
            if (!acceptedNPCs.Contains(npc.type) && !npc.active)
                return;
            Player target = Main.player[npc.target];
            LuckHandler luckHandler = target.GetModPlayer<LuckHandler>();

            //npc speed
            if (!acceptedNPCs.Contains(npc.type))
                return;

            float mult = 1 * (float)Math.Pow(luckHandler.luckValue() + 1, 1.6);
            npc.velocity.X = MathHelper.Lerp(npc.oldVelocity.X, npc.direction * mult, .025f);
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (!BookUsed.isPlayerCursed)
                return;

            int debuffType = -1;

            if (npc.type == NPCID.ArmedZombieEskimo || npc.type == NPCID.ZombieEskimo)
                debuffType = BuffID.Chilled;

            else if (torchZombiesNPCs.Contains(npc.type))
                debuffType = BuffID.OnFire;

            else if (npc.type == NPCID.BloodZombie)
                debuffType = BuffID.Bleeding;

            else if (npc.type == NPCID.ZombieRaincoat)
                debuffType = BuffID.Wet;

            else if (npc.type == NPCID.MaggotZombie)
                debuffType = BuffID.Stinky;

            else if (slimeZombieNPCs.Contains(npc.type))
                debuffType = BuffID.Slimed;

            else
                return;

            LuckHandler luckHandler = target.GetModPlayer<LuckHandler>();
            int debuffTime = (int)(Math.Pow(1 + luckHandler.luckValue(), 4.907) * 60); //approx 1-30 seconds\

            target.AddBuff(debuffType, debuffTime);
        }

        public override void AI(NPC npc)
        {
            if (!BookUsed.isPlayerCursed)
                return;

            if (!npc.active)
                return;

            if (npc.type != NPCID.ArmedTorchZombie)
                return;

            if (npc.frame.Y != 204) //204 for start of swing animation
                return;

            Player target = Main.player[npc.target];
            LuckHandler luckHandler = target.GetModPlayer<LuckHandler>();

            int facing = 32 * npc.direction;
            swingCounter++;

            if (swingCounter % (int)(10 * (2 - luckHandler.luckValue())) != 0 || luckHandler.luckValue() < .5)
                return;

            int projID = Projectile.NewProjectile(
                   npc.GetSource_FromAI(),
                   new Vector2(npc.Center.X + facing, npc.Center.Y),
                   new Vector2((float)Main.rand.Next(0, 5000) / 1000 * npc.direction, (float)Main.rand.Next(-3500, -1500) / 1000),
                   ProjectileID.MolotovFire3,
                   npc.damage,
                   1f,
                   -1);

            Projectile proj = Main.projectile[projID];
            proj.hostile = true;
            proj.friendly = false;
            proj.tileCollide = true;
            proj.netUpdate = true;
        }

        public override void OnKill(NPC npc)
        {
            if (!BookUsed.isPlayerCursed)
                return;

            Vector2 npcCenter = npc.Center;
            if (!slimeZombieNPCs.Contains(npc.type) && !torchZombiesNPCs.Contains(npc.type) && npc.type != NPCID.ZombieMushroom && npc.type != NPCID.ZombieMushroomHat)
                return;

            Player target = Main.player[npc.target];
            LuckHandler luckHandler = target.GetModPlayer<LuckHandler>();
            int multiplyValue = getluckMultiplier(luckHandler.luckValue());

            if (multiplyValue == 0)
                return;

            if ((npc.type == NPCID.ZombieMushroom || npc.type == NPCID.ZombieMushroomHat) && luckHandler.luckValue() >= .5)
            {
                int mushiSpore = NPC.NewNPC(npc.GetSource_Death(), (int)(npcCenter.X), (int)((npcCenter.Y + 2)), NPCID.FungiSpore);

                if (mushiSpore > 0)
                {
                    NPC spore = Main.npc[mushiSpore];
                    spore.velocity = new Vector2(Main.rand.Next(-1000, 1000) / 1000, Main.rand.Next(-7000, -6000) / 1000);
                    spore.netUpdate = true;
                }

                return;
            }

            var projectile = ModContent.ProjectileType<SlimeGel>();

            if (torchZombiesNPCs.Contains(npc.type))
                projectile = ProjectileID.MolotovFire3;

            for (int i = 0; i < multiplyValue; i++)
            {
                int projID = Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        npc.Center,
                        new Vector2((float)Main.rand.Next(-5000, 5000) / 1000, (float)Main.rand.Next(-5000, -2000) / 1000),
                        projectile,
                        npc.damage,
                        1f,
                        -1);

                Projectile proj = Main.projectile[projID];
                proj.hostile = true;
                proj.friendly = true;
                proj.tileCollide = true;
                proj.netUpdate = true;
            }

        }

        private static int getluckMultiplier(double luckVal)
        {
            if (luckVal < .25f)
                return 0;
            else if (luckVal < .75f)
                return 1;
            else
                return 3;
        }
    }
}