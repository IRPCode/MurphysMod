using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using MurphysMod.Content.Ambience;
using Terraria.ObjectData;
using Terraria.GameContent;

namespace MurphysMod.Content.Tiles
{
    internal class AetherGrass : ModTile
    {
        public override String Texture => "MurphysMod/Assets/Textures/Tiles/AetherGrass";
        public static double timer;
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;

            Main.tileSolid[Type] = true;
            Main.tileMerge[Type][TileID.Stone] = false;
            Main.tileMerge[TileID.Stone][Type] = false;
            Main.tileLighted[Type] = false;


            TileID.Sets.Grass[Type] = true;

            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(209, 194, 255), CreateMapEntryName());

            DustType = DustID.ShimmerSpark;
            HitSound = SoundID.Tink;

            MineResist = 2f;
            MinPick = 55; //60% strength needed
        }

        public override void RandomUpdate(int i, int j) //Courtesy of steviegt6, GinYuH, and Ozzatron from the CalamityMod Dev Team
        {
            Tile tile = Framing.GetTileSafely(i, j);
            Tile tile1 = Framing.GetTileSafely(i, j - 1);
            Tile tile2 = Framing.GetTileSafely(i, j - 2);

            if (WorldGen.genRand.NextBool(1) && !tile1.HasTile && !tile2.HasTile && !(tile1.LiquidAmount > 0 && tile2.LiquidAmount > 0) && !tile.LeftSlope && !tile.RightSlope && !tile.IsHalfBlock)
            {
                tile1.TileType = (ushort)ModContent.TileType<AetherGrassBlades>();
                tile1.HasTile = true;
                tile1.TileFrameY = 0;

                tile1.TileFrameX = (short)(WorldGen.genRand.Next(5) * 18);
                WorldGen.SquareTileFrame(i, j - 1, true);
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
            }


            if (Main.tile[i, j + 1] != null)
            {
                if (!Main.tile[i, j + 1].HasTile && Main.tile[i, j + 1].TileType != (ushort)ModContent.TileType<AetherVines>())
                {

                    bool canGrowVine = false;
                    for (int k = j; k > j - 10; k--)
                    {
                        if (Main.tile[i, k].BottomSlope)
                        {
                            canGrowVine = false;
                            break;
                        }
                        if (Main.tile[i, k].HasTile && !Main.tile[i, k].BottomSlope)
                        {
                            canGrowVine = true;
                            break;
                        }
                    }
                    if (canGrowVine)
                    {
                        int vineX = i;
                        int vineY = j + 1;
                        Tile vineXY = Framing.GetTileSafely(vineX, vineY);
                        Main.tile[vineX, vineY].TileFrameX = (short)(WorldGen.genRand.Next(8) * 18);
                        Main.tile[vineX, vineY].TileFrameY = (short)(4 * 18);
                        Main.tile[vineX, vineY + 1].TileFrameX = (short)(WorldGen.genRand.Next(12) * 18);
                        Main.tile[vineX, vineY + 1].TileFrameY = (short)(WorldGen.genRand.Next(4) * 18);
                        vineXY.HasTile = true;
                        Main.tile[vineX, vineY].TileType = (ushort)ModContent.TileType<AetherVines>();
                        WorldGen.SquareTileFrame(vineX, vineY, true);
                        WorldGen.SquareTileFrame(vineX, vineY - 1, true);
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendTileSquare(-1, vineX, vineY, 3, TileChangeType.None);
                            NetMessage.SendTileSquare(-1, vineX, vineY - 1, 3, TileChangeType.None);
                        }

                    }
                }
            }

        }



        public override bool CanPlace(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            if (tile.TileType == TileID.Stone)
                return true;
            else
                return false;
        }

        //Shimmer dark color: (152, 107, 233)
        //Shimmer light color: (185, 166, 246)

        public override void PostDraw(int x, int y, SpriteBatch spriteBatch)
        {
            Lighting.AddLight(new Vector2(x + .5f, y + .5f) * 16f, colorOsciliator().ToVector3());
        }

        public Color colorOsciliator()
        {
            timer++;
            double lerpAmount = Math.Sin(timer * (Math.PI / (180 * 150)));
            return Color.Lerp(new Color(209, 194, 255), new Color(135, 130, 153), (float)lerpAmount);
        }
    }
}