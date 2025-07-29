using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria.IO;
using System;
using rail;
using StructureHelper.API;
using Terraria.DataStructures; //add structures :)

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
            }
        }

        private void GenerateHouse(GenerationProgress progress, GameConfiguration configuration) //find something on github that isn't garbage
        {
            progress.Message = "Creating a lovely home.";

            int x = Main.maxTilesX / 2 + 50;
            int y = 0;

            string structure = "Structures/AbandonedHouse";

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