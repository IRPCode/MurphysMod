using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MurphysMod.Content.Buffs;
using MurphysMod.Content.Projectiles;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using MurphysMod.Content.LuckHandlers;
using MurphysMod.Systems;

namespace MurphysMod.Content.Enemies
{
    public class slimeNPC : GlobalNPC //TODO: For whatever reason, balloon slimes die when the balloon pops
    {
        public override bool InstancePerEntity => true;
        public float lungeVal;
        public float jumpVal;

        public bool jumpCheck;

        public int[] jumpVals = [0,0,0,0];

        public int[] regSlimes = [NPCID.GreenSlime, NPCID.BlueSlime, NPCID.RedSlime, NPCID.PurpleSlime, NPCID.BlackSlime, NPCID.IceSlime, NPCID.SandSlime,
        NPCID.JungleSlime, NPCID.LavaSlime, NPCID.Pinky, NPCID.GoldenSlime, NPCID.ShimmerSlime, NPCID.DungeonSlime, NPCID.UmbrellaSlime, NPCID.BunnySlimed,
        NPCID.SlimeRibbonRed, NPCID.SlimeRibbonGreen, NPCID.SlimeRibbonWhite, NPCID.SlimeRibbonYellow];
        public override void PostAI(NPC npc)
        {
            if (!BookUsed.isPlayerCursed)
                return;

            if (npc.aiStyle != NPCAIStyleID.Slime && !npc.active)
                return;

                Main.NewText(npc.ai[0]);

            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();

            float distX = Math.Abs(npc.Center.X - player.Center.X);
            float distY = player.Center.Y - npc.Center.Y;

            if (npc.life < npc.lifeMax && distX >= 0 * (1f + luckHandler.luckValue()) * 16 && distX <= 15 * (1f + luckHandler.luckValue()) * 16 && Math.Abs(distY) < 5 * 16 && Math.Abs(distY) >= 0)
                lungeVal = 1.5f * (float)(1f + luckHandler.luckValue());
            else
                lungeVal = 1f;

            if (npc.life < npc.lifeMax && distY < -15 * 16 && luckHandler.luckValue() >= .5)
                jumpVal = 1.5f;
            else
                jumpVal = 1f;

            if (npc.ai[0] == 0 && jumpCheck) //wait, then short
            {
                npc.velocity.X *= (float)(1 + (luckHandler.luckValue() / 3)) * (lungeVal * 1.25f);
                jumpCheck = false;
            }
            else if (npc.ai[0] == -200 && jumpCheck) //high
            {
                npc.velocity.Y *= (float)((1 + (luckHandler.luckValue() / 4)) * jumpVal);
                jumpCheck = false;
            }
            else if (npc.ai[0] == -1120 && jumpCheck) //short
            {
                npc.velocity.X *= (float)(1 + (luckHandler.luckValue() / 4)) * (lungeVal * .9f);
                jumpCheck = false;
            }
            else if (npc.ai[0] == -2120 && jumpCheck) //short
            {
                npc.velocity.X *= (float)(1 + (luckHandler.luckValue() / 3)) * (lungeVal * .85f);
                jumpCheck = false;
            }
            else if (npc.ai[0] != 0 && npc.ai[0] != -200 && npc.ai[0] != -1120 && npc.ai[0] != -2120)
            {
                jumpCheck = true;
            }

            if ((npc.ai[0] >= -200) && npc.life < npc.lifeMax && distY > 0 && luckHandler.luckValue() >= .5)
            {
                Point point = new Point((int)npc.Center.X / 16, (int)npc.Bottom.Y / 16);
                Tile tile = Framing.GetTileSafely(point.X, point.Y + 1);
                if (tile.HasTile && TileID.Sets.Platforms[tile.TileType])
                    npc.noTileCollide = true;
                else
                    npc.noTileCollide = false;
            }
            else
                npc.noTileCollide = false;
        }

        public override void OnKill(NPC npc)
        {

            if (npc.aiStyle == NPCAIStyleID.Slime)
            {

                Player player = Main.LocalPlayer;
                LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
                double luckVal = luckHandler.luckValue();
                var projectile = ModContent.ProjectileType<SlimeGel>();
                bool spawnProjectile = false;
                var knockback = 2f;
                var damageModifier = 0.0;
                int x = 0;
                int y = 0;
                int yVel = 5;
                int randomVal = 0;

                //check for luck

                if (luckVal >= 1f)
                {
                    randomVal = Main.rand.Next(1, 51);
                }

                else if (luckVal >= .75f)
                {
                    randomVal = Main.rand.Next(1, 41);
                }

                else if (luckVal >= .5f)
                {
                    randomVal = Main.rand.Next(1, 21);
                }

                else if (luckVal >= .25f)
                {
                    randomVal = Main.rand.Next(1, 11);
                }

                //apply projectile logic, L1 = 10%, L2 = 20%, L3 = 25%, L4 = 33%

                if (randomVal == 1 || randomVal == 11 || randomVal == 21 || randomVal == 41 || randomVal == 49) //gel
                {
                    y = Main.rand.Next(1, 3);
                    spawnProjectile = true;
                    knockback = 1f;
                    damageModifier = .4;
                    projectile = ModContent.ProjectileType<SlimeGel>();
                }

                else if (randomVal == 12 || randomVal == 22 || randomVal == 42) //copper
                {
                    y = Main.rand.Next(1, 5);
                    spawnProjectile = true;
                    knockback = 0f;
                    damageModifier = .3;
                    projectile = ModContent.ProjectileType<CopperCoin>();
                }

                else if (randomVal == 13 || randomVal == 23 || randomVal == 43) //silver
                {
                    y = Main.rand.Next(1, 3);
                    spawnProjectile = true;
                    knockback = 1f;
                    damageModifier = .6;
                    projectile = ModContent.ProjectileType<SilverCoin>();
                }

                else if (randomVal == 24) //gold
                {
                    y = Main.rand.Next(1, 2);
                    spawnProjectile = true;
                    knockback = 2f;
                    damageModifier = 1.5;
                    projectile = ModContent.ProjectileType<GoldCoin>();
                }

                else if (randomVal == 28 || randomVal == 48) //arrow
                {
                    y = 1;
                    knockback = 5f;
                    spawnProjectile = true;
                    damageModifier = 1;
                    projectile = ModContent.ProjectileType<Arrow>();
                }

                else if (randomVal == 29 || randomVal == 47)
                { //shuriken
                    y = Main.rand.Next(1, 2);
                    knockback = 0f;
                    spawnProjectile = true;
                    damageModifier = .6;
                    projectile = ModContent.ProjectileType<Shuriken>();
                }

                else if (randomVal == 50)
                { //slimy boulder
                    y = 1;
                    knockback = 15f;
                    spawnProjectile = true;
                    damageModifier = 2;
                    yVel = 15;
                    projectile = ModContent.ProjectileType<SlimyBoulder>();
                }

                while (x < y)
                    if (spawnProjectile == true)
                    {
                        int projID = Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        npc.Center,
                        new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-yVel, 0)),
                        projectile,
                        Convert.ToInt32(npc.damage * damageModifier), //for damage balancing
                        knockback,
                        -1);

                        Projectile proj = Main.projectile[projID];
                        proj.hostile = true;
                        proj.friendly = false;
                        proj.tileCollide = true;
                        proj.netUpdate = true;
                        x++;
                    }
            }
        }
    }
}