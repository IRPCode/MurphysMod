using System.Linq;
using Microsoft.Xna.Framework;
using MurphysMod.Content.Ambience;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.LuckHandlers
{
    public class EnemyLuck : GlobalNPC
    {
        public static double amount;
        public static int length;
        public static float inverseAmount;
        public static int[] goldCritterList = {NPCID.GoldBird, NPCID.GoldBunny, NPCID.GoldButterfly, NPCID.GoldDragonfly, NPCID.GoldFrog,
        NPCID.GoldGoldfish, NPCID.GoldGoldfishWalker, NPCID.GoldGrasshopper, NPCID.GoldMouse, NPCID.GoldSeahorse, NPCID.GoldWaterStrider,
        NPCID.GoldWorm, NPCID.SquirrelGold};
        public override void OnKill(NPC npc)
        {

            if (length == 0)
            {
                amount -= .01;
                amount = Utils.Clamp(amount, 0, double.MaxValue);
            }

            if (goldCritterList.Contains(npc.type))
            {
                amount += .02;
                length += 90 * 5;
            }

            if (npc.type == NPCID.LadyBug)
            {
                amount += .04;
                length += 90 * 10;
            }

            else if (npc.type == NPCID.GoldLadyBug)
            {
                amount += .08;
                length += 90 * 20;
            }
        }
    }

    public class updateLength : ModSystem
    {
        public override void PostUpdateEverything()
        {
            EnemyLuck.length--;
            EnemyLuck.length = Utils.Clamp(EnemyLuck.length, 0, int.MaxValue);
        }
    }

    public class updateProximityLuck : ModPlayer
    {
        public static double proximityAmount;
        public override void PreUpdate()
        {
            {
                Player player = Main.LocalPlayer;

                if (Tick.globalTick % 300 == 0)
                {
                    

                    NPC[] npc = Main.npc;

                    for (int i = 0; i < Main.maxNPCs; i++)
                    {

                        if (Vector2.Distance(npc[i].Center, player.Center) <= 50 && EnemyLuck.goldCritterList.Contains(npc[i].type))
                        {
                            proximityAmount -= .02;

                            Main.NewText(Tick.globalTick);
                        }

                        else if (Vector2.Distance(npc[i].Center, player.Center) <= 50 && (npc[i].type == NPCID.LadyBug || npc[i].type == NPCID.GoldLadyBug))
                        {
                            proximityAmount -= .04;
                            if (npc[i].type == NPCID.GoldLadyBug)
                                proximityAmount -= .04;
                        }

                        if (Vector2.Distance(npc[i].Center, player.Center) >= 50 && (EnemyLuck.goldCritterList.Contains(npc[i].type) || npc[i].type == NPCID.LadyBug || npc[i].type == NPCID.GoldLadyBug) && proximityAmount < 0)
                        {
                            proximityAmount += .02;
                            if (npc[i].type == NPCID.LadyBug)
                                proximityAmount += .02;
                            else if (npc[i].type == NPCID.GoldLadyBug)
                                proximityAmount += .04;
                        }
                    }
                }
                proximityAmount = Utils.Clamp(proximityAmount, 0, double.MaxValue);
            }
        }   //TODO: make this only trigger one per instance, and have the luckhandler class properly update
    }
}