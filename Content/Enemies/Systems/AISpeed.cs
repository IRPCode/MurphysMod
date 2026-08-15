using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MurphysMod.Content.Buffs;
using Terraria.ID;
using System.Collections;
using System.Collections.Specialized;
using System;
using MurphysMod.Content.LuckHandlers;

namespace MurphysMod.Content //this class needs to be fixed to work in multiplayer. Enemies are currently buggy, and ensure that luck doesn't add other players, and all enemies are synched across all clients.
{

    public class AISpeed : GlobalNPC //make another class where whenever you spend money at the goblin tinkerer, depending on the price, your character drops coin projectiles that bounce everywhere
    { //give the headless horseman a jousting lance at worse levels of luck
        static Vector2 originalPos; //do aistyles 23 and 25, give 29 projectiles like man eaters
                                    //change 62 and 73 to shoot more bullets
                                    //edit 75 to be custom
                                    //make all pillar enemies to be custom (except 74)
                                    //make 87 custom
                                    //make 103 custom
                                    //make 119 custom
        public override void AI(NPC npc)
        {
            //for slimes, check slime.cs

            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            Vector2 playerLoc = npc.DirectionTo(player.Center);
            float luckVal = (float)luckHandler.luckValue();
            Vector2 vectorLuck = new Vector2(luckVal, luckVal);

            if (npc.aiStyle == 19 || npc.aiStyle == 22 || npc.aiStyle == 38 || npc.aiStyle == 39 || npc.aiStyle == 41 || npc.aiStyle == 42 || npc.aiStyle == 49) //fighter AI
            {
                //npc.aiStyle == 3 || 
                //if (luckVal >= 1f)
                // {
                //     luckVal = 1f;
                // }
                npc.velocity.X += MathHelper.Lerp(0f, npc.direction * ((luckVal + 1) * 2) - npc.velocity.X, .1f);
            }

            if (npc.aiStyle == 5 || (npc.aiStyle == 10 && (npc.Center - playerLoc).Length() >= 25) || npc.aiStyle == 14 || npc.aiStyle == 16 || npc.aiStyle == 17 || npc.aiStyle == 40 || npc.aiStyle == 44 || npc.aiStyle == 56 || npc.aiStyle == 63 || npc.aiStyle == 74 || npc.aiStyle == 122) //flying AI, cursed skulls and bat AI
            {
                float lerpVal = .1f;

                if (npc.aiStyle == 14)
                {
                    lerpVal = .2f; //gets bats to turn faster
                }

                if (luckVal >= 1f) //gets aggressive if bad luck is too high
                {
                    npc.velocity = Vector2.Lerp(npc.velocity, (playerLoc * (vectorLuck + new Vector2(1, 1)) * 3), lerpVal);
                    //change back to .1f and .001f if .05/0005 causes issues
                }
                else
                {
                    npc.velocity = Vector2.Lerp(npc.velocity, (playerLoc * (vectorLuck + new Vector2(1, 1)) * 3), ((lerpVal / 100) / (1 - luckVal)));
                }
            }

            npc.netUpdate = true;

            //for AI type 6 (worms), OnHitPlayer adds debuffs to give players debuffs
            //for AI type 8 (caster), their spells (AI type 9) will be affected by the luck, and not the casters themselves
            //for type 11/12 (dungeon guardian / skeletron), this is in a seperate file for more advanced tweaks.
            //check maneater file for ai13
            //all bosses and minibosses need changes
        }

        public override void OnHitPlayer(NPC npc, Player player, Player.HurtInfo hurtInfo)
        {
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            float luckVal = (float)luckHandler.luckValue();

            if (npc.aiStyle == 6)
            {
                if (luckVal >= .5f)
                {
                    player.AddBuff(ModContent.BuffType<BadOmen>(), 45);
                }

                else
                {
                    player.AddBuff(ModContent.BuffType<Augury>(), 30);
                }
            }

            if (npc.aiStyle == 9)
            {
                npc.StrikeInstantKill();
            }
            npc.netUpdate = true;
        }

        public override bool PreAI(NPC npc) //ensures npcs skip their original ai and follows what the code tells them
        {
            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            float luckVal = (float)luckHandler.luckValue();
            npc.netUpdate = true;

            if (npc.aiStyle == 9 && luckVal >= 1f && npc.type != NPCID.VileSpit && npc.type != NPCID.SolarFlare && npc.type != NPCID.VileSpitEaterOfWorlds)
            {
                npc.rotation += 1f;

                if (npc.type == NPCID.WaterSphere)
                {
                    npc.alpha = 50;
                }

                return false;
            }
            else if (npc.type == NPCID.VileSpit || npc.type == NPCID.VileSpitEaterOfWorlds)
            {
                npc.rotation += 1f;
            }

            return true;
        }

        public override void PostAI(NPC npc)
        {
            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            Vector2 playerLoc = npc.DirectionTo(player.Center);
            float luckVal = (float)luckHandler.luckValue();
            Vector2 vectorLuck = new Vector2(luckVal, luckVal);

            if (npc.type == NPCID.WaterSphere || npc.type == NPCID.ChaosBall || npc.type == NPCID.BurningSphere)
            {
                if (luckVal >= 1f)
                {
                    npc.velocity = playerLoc;
                    npc.velocity *= 7.5f;

                    int dustType;

                    switch (npc.type)
                    {
                        case 25:
                            dustType = DustID.Torch;
                            break;
                        case 30:
                            dustType = DustID.PurpleTorch;
                            break;
                        case 33:
                            dustType = DustID.BlueTorch;
                            break;
                        default:
                            dustType = DustID.Smoke;
                            break;
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, dustType, npc.velocity.X * .75f, npc.velocity.Y * .75f, 100, default, 1.5f);
                    }
                }

                else if (luckVal >= 1f && (npc.type == NPCID.VileSpit || npc.type == NPCID.VileSpitEaterOfWorlds))
                {
                    npc.velocity *= 1.02f;
                }

                else if (luckVal >= .75f)
                {
                    npc.velocity *= 1.01f;
                }

                else if (luckVal >= .5f)
                {
                    npc.velocity *= 1.005f;
                }

                else if (luckVal >= .25f)
                {
                    npc.velocity *= 1.0025f;
                }

                else
                {
                    npc.velocity *= 1.001f;
                }

                npc.netUpdate = true;

            }
        }
    }
}