using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using System.Collections.Generic;
using Terraria.IO;
using StructureHelper.API;
using Terraria.DataStructures;
using System.Numerics;
using Microsoft.Xna.Framework;

namespace MurphysMod.Content.Generation
{
    #region Boss Kills
    public class PostCreationGeneration : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.WallofFlesh && !Main.hardMode)
            {
                string structure = "Structures/SanctifiedIsland";
                int x = Main.maxTilesX / 2;
                int y = (int)(Main.worldSurface * .15);
                if (Generator.IsInBounds(structure, Mod, new Point16(x - 62, y)))
                {
                    Generator.GenerateStructure(structure, new Point16(x - 62, y), Mod);
                    Main.NewText("A sanctified island appears in the sky, and the jungle's accursed sap surges with newfound power.", Color.Green);
                }                
            }
        }
    }
    #endregion

    #region World Events

    //TODO: items used, special tiles broken (like shadow orbs), and other events will spawn certian structures

    #endregion
}