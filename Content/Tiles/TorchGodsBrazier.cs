using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MurphysMod.Content.Tiles
{
    internal class TorchGodsBrazier : ModTile
    {
        public override String Texture => "MurphysMod/Assets/Textures/Tiles/TorchGodsBrazier";

        public static int offsetY;
        public static bool flag;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileID.Sets.DisableSmartCursor[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Width = 7;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.Origin = new Point16(3, 4);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };

            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.CoordinatePaddingFix = new Terraria.DataStructures.Point16(0, 2);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);

            TileObjectData.addTile(Type);

            AddMapEntry(new Microsoft.Xna.Framework.Color(200, 200, 200), CreateMapEntryName());
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(
                new EntitySource_TileBreak(i, j),
                i * 16,
                j * 16,
                 112,
                 80,
                 ModContent.ItemType<Items.Placeables.TorchGodsBrazier>()
            );
        }

        public override bool PreDraw(int x, int y, SpriteBatch spriteBatch) //flame beam
        {
            if (Main.LocalPlayer.unlockedBiomeTorches)
            {






                Tile tile = Main.tile[x, y];
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);





                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                y -= 2;
                for (int i = 0; i < 5; i++)
                {
                    y -= 3;
                    spriteBatch.Draw(ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Tiles/Torch_God's_Brazier_Flame_Top", AssetRequestMode.ImmediateLoad).Value,
                new Vector2(x * 16 - (int)Main.screenPosition.X, y * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
                new Color(255, 255, 255),
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f);
                }

                //forge clover





            }
            return true;
        }

        public override void PostDraw(int x, int y, SpriteBatch spriteBatch) //flame base
        {
            if (Main.LocalPlayer.unlockedBiomeTorches)
            {

                Tile tile = Main.tile[x, y];
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                int height = tile.TileFrameY == 36 ? 18 : 16;
                spriteBatch.Draw(ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Tiles/Torch_God's_Brazier_Flame_Base", AssetRequestMode.ImmediateLoad).Value,
                new Vector2(x * 16 - (int)Main.screenPosition.X, y * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height),
                new Color(255, 255, 255),
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f);

                //flame base

                 if (offsetY <= 0)
                    flag = true;
                else if (offsetY >= 32000)
                    flag = false;


                if (flag)
                        offsetY++;
                    else
                        offsetY--;


                if (Main.drawToScreen) //TODO: fix this from jumping from tile to tile, and make sure it always draws above the beam as it currently clips
                {
                    zero = Vector2.Zero;
                }
                spriteBatch.Draw(ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Tiles/ForgeClover_1", AssetRequestMode.ImmediateLoad).Value,
                new Vector2(x * 16 - (int)Main.screenPosition.X, (float)(y - (offsetY / 1000)) * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
                new Color(255, 255, 255),
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f);


               
            }
        }
    }
}