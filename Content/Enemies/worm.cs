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
    public class wormNPC : GlobalNPC
    {

        public override void AI(NPC npc)
        {
            if (npc.aiStyle == 6)
            {
                NPC head = Main.npc[npc.realLife];

                Player player = Main.LocalPlayer;
                LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
                float luckVal = luckHandler.luckValue();
                var knockback = 2f;
                int x = 0;

                int moduloNum = -1;

                Vector2 playerLocation = player.Center - npc.Center;
                playerLocation.Normalize();

                if (luckVal >= 1f) //changes the amount of time it takes for worms to spawn spit
                {
                    moduloNum = 100;
                }
                else if (luckVal >= .75f)
                {
                    moduloNum = 150;
                }
                else if (luckVal >= .5f)
                {
                    moduloNum = 250;
                }
                else if (luckVal >= .25f)
                {
                    moduloNum = 300;
                }

                if (npc == head && Collision.CanHitLine(npc.Center, 1, 1, player.Center, 1, 1) && npc.type != NPCID.EaterofWorldsHead) //wooooooooooorm spit :O
                {
                    npc.ai[2]++;

                    if (npc.ai[2] % moduloNum == 0 && moduloNum != -1)
                    {
                        int projID = Projectile.NewProjectile(
                           npc.GetSource_FromAI(),
                           npc.Center,
                           playerLocation * 10f,
                           ModContent.ProjectileType<WormSpit>(),
                           50, //for damage balancing
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
}