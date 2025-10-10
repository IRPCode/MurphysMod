using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MurphysMod.Systems;
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

        public static float offsetY;
        public static bool flag;
        public static int timer;
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
            if (Main.LocalPlayer.unlockedBiomeTorches && BookUsed.isPlayerCursed)
            {
                Tile tile = Main.tile[x, y];
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                for (int i = 0; i < 3; i++)
                {
                    y -= 5;
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
            }
            return true;
        }

        public override void SpecialDraw(int x, int y, SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.unlockedBiomeTorches && BookUsed.isPlayerCursed)
            {
                Tile tile = Main.tile[x, y];
                Texture2D cloverTex = ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Tiles/ForgeClover_1", AssetRequestMode.ImmediateLoad).Value;
                Vector2 positionInWorld = new Vector2((x + 12) * 16, (y + 9) * 16);

                Vector2 spritePos = positionInWorld - Main.screenPosition - new Vector2(0, (offsetY));

                if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
                {
                spriteBatch.Draw(
                   cloverTex,
                   spritePos,
                   null,
                   Color.White,
                   0f,
                   Vector2.Zero,
                   1f,
                   SpriteEffects.None,
                   .9f);
                }
            }
        }

        public override void PostDraw(int x, int y, SpriteBatch spriteBatch) //flame base
        {
            if (Main.LocalPlayer.unlockedBiomeTorches && BookUsed.isPlayerCursed)
            {
                Tile tile = Main.tile[x, y];

                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                Lighting.AddLight(new Vector2(x * 16,y * 16), new Color(255, 249, 181).ToVector3());

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

                //clover movement
                offsetY = (float)Math.Sin(timer * (Math.PI / (180 * 30))) * 24f;
                timer++;               

                if (Main.netMode != NetmodeID.Server)
                {
                    Main.instance.TilesRenderer.AddSpecialLegacyPoint(x, y);
                }
            }
        }
    }
}