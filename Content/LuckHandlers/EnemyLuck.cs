using System.Linq;
using MurphysMod.Content.Buffs;
using Terraria;
using Terraria.GameContent.Bestiary;
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
                amount -= .01f;
                amount = Utils.Clamp(amount, 0, float.MaxValue);
            }

            if (goldCritterList.Contains(npc.type))
            {
                amount += .02f;
                length += 90 * 5;
            }

            if (npc.type == NPCID.LadyBug)
            {
                amount += .04f;
                length += 90 * 10;
            }

            else if (npc.type == NPCID.GoldLadyBug)
            {
                amount += .08f;
                length += 90 * 20;
            }

            amount = System.Math.Round(amount, 2);

            Main.NewText("Amount: " + amount);
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