using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using System.Collections.Generic;
using Terraria.IO;
using StructureHelper.API;
using Terraria.DataStructures;

namespace MurphysMod.Content.Generation
{
    public class Generation : ModSystem
    {
        int randX = Main.rand.Next(1, 50);
        int randY = Main.rand.Next(50, 150);

        #region GENERATION

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
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
                    if (Generator.IsInBounds(structure, Mod, new Point16(x, y)) && Main.rand.Next(0, 60) == 1 && miningShackShouldSpawn(x, y))
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