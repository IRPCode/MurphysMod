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

namespace MurphysMod.Content.Enemies
{
    public class slimeNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {

            if (npc.aiStyle == 1)
            {

                Player player = Main.LocalPlayer;
                LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
                float luckVal = luckHandler.luckValue();
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
                        (Convert.ToInt32(npc.damage * damageModifier)), //for damage balancing
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