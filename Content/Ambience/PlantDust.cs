using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System.Net.PeerToPeer.Collaboration;
using ModLiquidLib.Utils;
using System.Linq;
using MurphysMod.Content.Ambience.Dusts;
using System;

namespace MurphysMod //Add a check for the TorchGodLava to add custom particles
{
    public class PlantDust : ModPlayer
    {

        public static int[] acceptedPlantSources = { TileID.Plants, TileID.Plants2, TileID.Vines, TileID.VineFlowers}; //add other biomes
        public override void PostUpdate()
        {
            if (Main.myPlayer == Player.whoAmI && !Main.dedServ) //local only
            {
                for (int i = 0; i < 100; i++)
                {


                    int x = (int)(Player.Center.X / 16) + Main.rand.Next(-60, 60); //adjust these values based off of velocity (xvel = 60 * player.velocity.x, negxvel = xvel * -1)
                    int y = (int)(Player.Center.Y / 16) + Main.rand.Next(-36, 36); //also adjust this based off of screen size

                    int rand = Main.rand.Next(0, 51);


                    Tile tile = Framing.GetTileSafely(x, y);
                    //bool branchOrTop = treebranchOrTreetop(tile.TileFrameX, tile.TileFrameY);

                    bool branchOrTop = true;




                    #region GemTrees

                    if (branchOrTop && rand <= 5)
                    {
                        if (tile.TileType == TileID.TreeTopaz)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.GemTopaz, 0f, 0f, default);
                            Dust dust = Main.dust[dustIndex];
                            dust.noGravity = true;
                        }

                        if (tile.TileType == TileID.TreeAmethyst)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.GemAmber, 0f, 0f, default);
                            Dust dust = Main.dust[dustIndex];
                            dust.noGravity = true;

                        }

                        if (tile.TileType == TileID.TreeSapphire)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.GemSapphire, 0f, 0f, default);
                            Dust dust = Main.dust[dustIndex];
                            dust.noGravity = true;
                        }

                        if (tile.TileType == TileID.TreeEmerald)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.GemEmerald, 0f, 0f, default);
                            Dust dust = Main.dust[dustIndex];
                            dust.noGravity = true;
                        }

                        if (tile.TileType == TileID.TreeRuby)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.GemRuby, 0f, 0f, default);
                            Dust dust = Main.dust[dustIndex];
                            dust.noGravity = true;
                        }

                        if (tile.TileType == TileID.TreeDiamond)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.GemDiamond, 0f, 0f, default);
                            Dust dust = Main.dust[dustIndex];
                            dust.noGravity = true;
                        }

                        if (tile.TileType == TileID.TreeAmber)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.GemAmber, 0f, 0f, default);
                            Dust dust = Main.dust[dustIndex];
                            dust.noGravity = true;
                        }
                    }


                    if (tile.TileType == TileID.GemSaplings) //only spawn white shiny dust particles
                    {
                        int dustIndex = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.Silver, 0f, 0f, default);
                        Dust dust = Main.dust[dustIndex];
                        dust.noGravity = true;
                    }

                    #endregion


                }

                #region windyDay

                if (Math.Abs(Main.windSpeedCurrent) >= .25f)
                {
                    for (int i = 0; i < (int)(5 * (Math.Abs(Main.windSpeedCurrent) + 1)); i++)
                    {
                        int x = (int)(Player.Center.X / 16) + Main.rand.Next(-60, 60); //adjust these values based off of velocity (xvel = 60 * player.velocity.x, negxvel = xvel * -1)
                        int y = (int)(Player.Center.Y / 16) + Main.rand.Next(-36, 36); //also adjust this based off of screen size

                        if (acceptedPlantSources.Contains(Framing.GetTileSafely(x, y).TileType) && Framing.GetTileSafely(x,y).WallType != 0)
                        {
                            int dustIndex = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<PlantParticleDust>(), 0f, 0f, default);
                            Dust dust = Main.dust[dustIndex];
                            dust.noGravity = false;
                        }

                    }
                }

                #endregion
            }
        }


        public bool treebranchOrTreetop(int x, int y)
        {
            int[] gemtreeTiles = { 583, 584, 585, 586, 587, 588, 589 };

            Tile tile = Framing.GetTileSafely(x, y);

            int frameX = tile.TileFrameX;
            int frameY = tile.TileFrameY;

            if (gemtreeTiles.Contains(Framing.GetTileSafely(x, y).TileType) && (gemtreeTiles.Contains(Framing.GetTileSafely(x - 1, y).TileType) ||
                gemtreeTiles.Contains(Framing.GetTileSafely(x + 1, y).TileType))) //checks if it is a branch
            {
                return true;
            }
            else if (gemtreeTiles.Contains(Framing.GetTileSafely(x, y).TileType) && !gemtreeTiles.Contains(Framing.GetTileSafely(x, y + 1).TileType))
            { //checks to see if it is a tree top
                return true;
            }
            return false;
        }
    }
}