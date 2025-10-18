using System.Linq;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.LuckHandlers
{
    public class EnemyLuck : GlobalNPC
    {
        public static double amount;
        public static double proximityAmount;
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

        public override void PostAI(NPC npc) //TODO: make this only trigger one per instance, and have the luckhandler class properly update
        {
            Player player = Main.LocalPlayer;
            if (Vector2.Distance(npc.Center, player.Center) <= 2 && goldCritterList.Contains(npc.type))
            {
                proximityAmount -= .02;
            }

            else if (Vector2.Distance(npc.Center, player.Center) <= 2 && (npc.type == NPCID.LadyBug || npc.type == NPCID.GoldLadyBug))
            {
                proximityAmount -= .04;
                if (npc.type == NPCID.GoldLadyBug)
                    proximityAmount -= .04;
            }

            if (Vector2.Distance(npc.Center, player.Center) >= 20 && (goldCritterList.Contains(npc.type) || npc.type == NPCID.LadyBug || npc.type == NPCID.GoldLadyBug) && proximityAmount < 0)
            {
                proximityAmount += .02;
                if (npc.type == NPCID.LadyBug)
                    proximityAmount += .02;
                else if (npc.type == NPCID.GoldLadyBug)
                    proximityAmount += .04;
            }

            proximityAmount = Utils.Clamp(proximityAmount, 0, double.MaxValue);
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
}