using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using MurphysMod.Content.Items.Placeables;
using MurphysMod.Content.Items.Ingredients;

namespace MurphysMod.Content.DropTables
{
    public class CursedGoldFragmentDropTable : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.CursedSkull)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CursedGoldFragment>(), Main.rand.Next(1,4)));
        }
    }
}