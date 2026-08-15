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
    public class manEaterNPC : GlobalNPC //TODO: Make the clinger have a cursed flame flamethrower like the 1.4.4 flamethrower rework
    {
        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.ManEater || npc.type == NPCID.Snatcher || npc.type == NPCID.JungleCreeper)
            {
                if (npc.target != -1 && npc.damage > 0)
                {
                    Player player = Main.LocalPlayer;
                    LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
                    double luckVal = luckHandler.luckValue();
                    var knockback = 2f;
                    int x = 0;

                    int moduloNum = -1;

                    Vector2 playerLocation = player.Center - npc.Center;
                    playerLocation.Normalize();

                    if (luckVal >= 1f) //changes the amount of time it takes for worms to spawn spit
                    {
                        moduloNum = 200;
                    }
                    else if (luckVal >= .75f)
                    {
                        moduloNum = 300;
                    }
                    else if (luckVal >= .5f)
                    {
                        moduloNum = 400;
                    }
                    else if (luckVal >= .25f)
                    {
                        moduloNum = 500;
                    }
                    npc.localAI[0]++;

                    if (npc.localAI[0] % moduloNum == 0 && moduloNum != -1)
                    {
                        int projID = Projectile.NewProjectile(
                           npc.GetSource_FromAI(),
                           npc.Center,
                           playerLocation * 10f,
                           ModContent.ProjectileType<ToxicJungleSpore>(),
                           25, //for damage balancing
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