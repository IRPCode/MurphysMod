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
using System.Linq;
using System;

namespace MurphysMod.Content.Generation
{
    public class Generation : ModSystem //531 -- 265
    {
        int randX = Main.rand.Next(1, 50);
        int randY = Main.rand.Next(50, 150);

        #region GENERATION

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.RemoveAll(genPass => genPass.Name == "Floating Islands");

            int shiniesIndex = tasks.FindIndex(GenPass => GenPass.Name.Equals("Shinies"));
            if (shiniesIndex != -1)
            {
                tasks.Insert(shiniesIndex + 1, new PassLegacy("Spawning Cursed Ore", GenerateCursedOre));
                tasks.Insert(shiniesIndex + 1, new PassLegacy("Spawning Magma Gems", GenerateMagmaGems));
                //TODO: fix these
                //tasks.Insert(shiniesIndex + 1, new PassLegacy("Spawning Hot Moss", generateLavaMoss));
                //tasks.Insert(shiniesIndex + 1, new PassLegacy("Trimming Moss", killMoss));
            }

            int structureIndex = tasks.FindIndex(GenPass => GenPass.Name.Equals("Micro Biomes"));

            if (structureIndex != -1)
            {
                tasks.Insert(structureIndex + 1, new PassLegacy("Spawning a home", GenerateHouse));
                tasks.Insert(structureIndex + 1, new PassLegacy("Something Karl would be interested in.", GenerateMinerShacks));
                tasks.Insert(structureIndex + 1, new PassLegacy("Disgusting sap fills the jungle", generateAccursedSapGrove));
                tasks.Insert(structureIndex + 1, new PassLegacy("Filling a forge with Ordained Slag", generateForge));
                tasks.Insert(structureIndex + 1, new PassLegacy("Ruining your future arenas", generateSkyIslands));
            }

            int islandIndex = tasks.FindIndex(GenPass => GenPass.Name.Equals("Floating Islands"));

            if (islandIndex != -1)
            {
                tasks.Insert(structureIndex + 1, new PassLegacy("Ruining your future arenas", generateSkyIslands));
            }
        }

        public static List<string> SkyIslands = new List<String>{ "Structures/SkyIslands/Islands/IslandBase0", "Structures/SkyIslands/Islands/IslandBase1",
         "Structures/SkyIslands/Islands/IslandBase2", "Structures/SkyIslands/Islands/IslandBase3", "Structures/SkyIslands/Islands/IslandBase4",
          "Structures/SkyIslands/Islands/IslandBase5", "Structures/SkyIslands/Islands/IslandBase6", "Structures/SkyIslands/Islands/IslandBase7",
           "Structures/SkyIslands/Islands/IslandBase8" };
        public static List<string> SkyBuildings = new List<String> { "Structures/SkyIslands/Buildings/SkyBuilding0", "Structures/SkyIslands/Buildings/SkyBuilding1",
         "Structures/SkyIslands/Buildings/SkyBuilding2", "Structures/SkyIslands/Buildings/SkyBuilding3", "Structures/SkyIslands/Buildings/SkyBuilding4",
          "Structures/SkyIslands/Buildings/SkyBuilding5", "Structures/SkyIslands/Buildings/SkyBuilding6", "Structures/SkyIslands/Buildings/SkyBuilding7",
           "Structures/SkyIslands/Buildings/SkyBuilding8", "Structures/SkyIslands/Buildings/SkyBuilding9" };

        public static List<Vector2> BuildingOffsets = new List<Vector2>{new Vector2(18, -2), new Vector2(24, 4), new Vector2(11, 5), new Vector2(54, -18),
        new Vector2(43, 9), new Vector2(24, -5), new Vector2(52, -7), new Vector2(30, -4), new Vector2 (0,0)}; //last element is the skylake, only needed for algorithm

        private void generateSkyIslands(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Adding sky islands to block your arena.";

            int x = 0;
            int y = 0;
            int sizeFraction = 1;
            int min = 0;
            int max = 0;


            y = (int)Main.worldSurface;

            if (Main.maxTilesX >= 2400)
            {
                sizeFraction = 7;
                min = 325;
                max = 550;
            }
            else if (Main.maxTilesX >= 1800)
            {
                sizeFraction = 5;
                min = 225;
                max = 350;
            }
            else
            {
                sizeFraction = 4;
                min = 125;
                max = 275;
            }

            int placementIncrement = Main.maxTilesX / (sizeFraction + 1);

            for (int i = 0; i <= sizeFraction; i++)
            {
                int selectedIsland = Main.rand.Next(0, SkyIslands.Count);
                int selectedBuilding = Main.rand.Next(0, SkyBuildings.Count);

                x += placementIncrement;
                y = (int)Main.worldSurface - Main.rand.Next(min, max + 1);

                if (Generator.IsInBounds(SkyIslands[selectedIsland], Mod, new Point16(x, y)))
                {
                    Generator.GenerateStructure(SkyIslands[selectedIsland], new Point16(x, y), Mod);
                }

                if (SkyIslands[selectedIsland] != "Structures/SkyIslands/Islands/IslandBase8")
                {
                    Vector2 offset = BuildingOffsets[selectedIsland];
                    if (Generator.IsInBounds(SkyBuildings[selectedBuilding], Mod, new Point16(x + (int)offset.X, y - (int)offset.Y)))
                    {
                        Generator.GenerateStructure(SkyBuildings[selectedBuilding], new Point16(x + (int)offset.X, y - (int)offset.Y - 1), Mod);
                    }
                }

                //after placing island
                SkyIslands.RemoveAt(selectedIsland);
                BuildingOffsets.RemoveAt(selectedIsland);
                SkyBuildings.RemoveAt(selectedBuilding);
            }
        }

        private void generateAccursedSapGrove(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Filling a forge with Ordained Slag";

            string structure = "Structures/AccursedSapGrove";

            int x = 0;
            int y = 0;

            y = (int)Main.worldSurface + 75;

            for (int iX = 0; iX < Main.maxTilesX; iX++)
            {
                for (int iY = 0; iY < Main.maxTilesY; iY++)
                {
                    if (WorldGen.SolidTile(iX, iY) && WorldGen.TileType(iX, iY) == TileID.LivingMahoganyLeaves)
                    {
                        if (iX > Main.maxTilesX / 2)
                            x = iX - (Main.maxTilesX / 17);
                        else
                            x = iX + (Main.maxTilesX / 17);
                        break;
                    }
                }
            }

            if (Generator.IsInBounds(structure, Mod, new Point16(x, y)))
            {
                Generator.GenerateStructure(structure, new Point16(x, y), Mod);
            }
        }

        private void generateForge(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Filling a forge with Ordained Slag";

            string structure = "Structures/OrdainedForge";

            int x = 0;
            int y = 0;

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (j == (Main.maxTilesY / 2f))
                    {
                        y = j;
                        break;
                    }
                }

                if (i == (Main.maxTilesX / 2f))
                {
                    x = i;
                    break;
                }
            }

            if (Generator.IsInBounds(structure, Mod, new Point16(x, y)))
            {
                Generator.GenerateStructure(structure, new Point16(x - 266, y - 100), Mod); //substract the structure's height plus some
            }
        }

        private void GenerateMinerShacks(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Can I get a rock and stone?";

            string structure = "Structures/MiningShack";

            int x = 0;
            int y = 0;

            for (int i = (int)(Main.maxTilesX / 3.5f); i < (Main.maxTilesX - (int)(Main.maxTilesX / 3.5f)); i++)
            {
                x = WorldGen.genRand.Next(Main.maxTilesX / 5, Main.maxTilesX - (Main.maxTilesX / 5));
                y = WorldGen.genRand.Next((int)(Main.maxTilesY / 2), (int)(Main.maxTilesY - (Main.maxTilesY / 5)));

                int rand = Main.rand.Next(1, 4);

                switch (rand) //get miningshack type
                {
                    case 1:
                        structure += "1";
                        break;
                    case 2:
                        structure += "2";
                        break;
                    case 3:
                        structure += "3";
                        break;
                }

                if (WorldGen.SolidTile(i, y) && WorldGen.TileType(i, y) == TileID.Stone)
                {
                    x = i;

                    for (int j = y; i < (Main.maxTilesY - (Main.maxTilesY / 5)); j++)
                    {
                        if (WorldGen.SolidTile(i, j) && WorldGen.TileType(i, j) == TileID.Stone)
                        {
                            y = i;
                            continue;
                        }
                    }
                    if (Generator.IsInBounds(structure, Mod, new Point16(x, y)) && Main.rand.Next(0, 80) == 1 && miningShackShouldSpawn(x, y))
                    {
                        Generator.GenerateStructure(structure, new Point16(x, y), Mod);
                    }
                }
                structure = "Structures/MiningShack"; //reset
            }
        }

        private bool miningShackShouldSpawn(int x, int y)
        {
            for (int i1 = x; i1 < x + 15; i1++) //15x15 cube to not have bad blocks to ensure good spawning
            {
                for (int i2 = y; i2 < y + 15; i2++)
                {
                    int tile = WorldGen.TileType(i1, i2);

                    if (tile == TileID.JungleGrass || tile == TileID.Mud || tile == TileID.LihzahrdBrick ||
                        tile == TileID.Ash || tile == TileID.IceBlock || tile == TileID.SnowBlock || tile == TileID.WoodBlock ||
                        tile == TileID.Sand || tile == TileID.Sandstone || tile == TileID.HardenedSand ||
                        tile == TileID.BlueDungeonBrick || tile == TileID.PinkDungeonBrick || tile == TileID.GreenDungeonBrick) //prevents bad spawning
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void GenerateHouse(GenerationProgress progress, GameConfiguration configuration) //find something on github that isn't garbage
        {
            progress.Message = "Creating a lovely home.";

            int x = Main.maxTilesX / 2 + 50;
            int y = 0;

            string structure = "Structures/AbandonedHome";

            for (int i = (int)Main.worldSurface - 300; i < (int)Main.worldSurface + 200; i++) //REMINDER: The structure's coordinates is from top down. 
            {                                                                                 //Make sure you account for this when the structure generates.
                if (WorldGen.SolidTile(x, i))
                {
                    y = i;
                    break;
                }
            }

            if (Generator.IsInBounds(structure, Mod, new Point16(x, y)))
            {
                Generator.GenerateStructure(structure, new Point16(x, y - 24), Mod); //substract the structure's height plus some
            }

        }

        private void GenerateCursedOre(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Cursing the world...";
            for (int i = 0; i < (int)(double)(Main.maxTilesX * Main.maxTilesY) * 6E-05; i++)
            {
                int x = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
                int y = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 500);

                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 7), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.CursedOre>());
            }
        }

        private void GenerateMagmaGems(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Melting gems...";
            for (int i = 0; i < (int)(double)(Main.maxTilesX * Main.maxTilesY) * 6E-04; i++)
            {

                int x = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
                int y = WorldGen.genRand.Next((int)(Main.maxTilesY / 1.4f), Main.maxTilesY);

                if (WorldGen.TileType(x, y) == TileID.Stone)
                {
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(1, 3), WorldGen.genRand.Next(1, 4), ModContent.TileType<Tiles.MagmaGem>());
                }
            }
        }

        #region MOSS

        private void generateLavaMoss(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Spreading Hot Moss";
            for (int i = 0; i < (int)(double)(Main.maxTilesX * Main.maxTilesY) * 6E-01; i++)
            {

                int x = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
                int y = WorldGen.genRand.Next((int)(Main.maxTilesY / 1.4f), Main.maxTilesY);

                if (WorldGen.TileType(x, y) == TileID.Stone && (exposed(x, y) == true))
                {
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(randX, randY), WorldGen.genRand.Next(randX / 10, randY / 10), TileID.LavaMoss);
                }
            }
        }

        private void killMoss(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Spreading Hot Moss";
            for (int i = 0; i < (int)(double)(Main.maxTilesX * Main.maxTilesY) * 6E-01; i++)
            {

                int x = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
                int y = WorldGen.genRand.Next((int)(Main.maxTilesY / 1.4f), Main.maxTilesY);

                if (WorldGen.TileType(x, y) == TileID.LavaMoss && exposed(x, y) == false)
                {
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(randX, randY), WorldGen.genRand.Next(randX / 10, randY / 10), TileID.Stone);
                }
            }
        }

        private bool exposed(int x, int y)
        {
            if (Main.tile[x, y - 1].TileType == TileID.LavaMoss && Main.tile[x, y + 1].TileType == TileID.LavaMoss && Main.tile[x - 1, y].TileType == TileID.LavaMoss && Main.tile[x + 1, y].TileType == TileID.LavaMoss)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #endregion

    }
}