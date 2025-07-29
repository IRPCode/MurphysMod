using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace MurphysMod.Content.Tiles //TODO: https://www.youtube.com/watch?v=vjrIH2v90WM spawn ores on gen
{
    internal class MagmaGem : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;

            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileShine[Type] = 900;
            Main.tileShine2[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 400; //above plat, below meteorite for metal detector

            AddMapEntry(new Color(255, 111, 0), CreateMapEntryName());

            DustType = DustID.OrangeTorch;
            HitSound = SoundID.LiquidsHoneyLava;

            MineResist = 2f;
            MinPick = 55; //60% strength needed

        }

        public override void PostDraw(int x, int y, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[x, y];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = tile.TileFrameY == 36 ? 18 : 16;
            spriteBatch.Draw(ModContent.Request<Texture2D>("MurphysMod/Content/Tiles/MagmaGemGlow", AssetRequestMode.ImmediateLoad).Value,
            new Vector2(x * 16 - (int)Main.screenPosition.X, y * 16 - (int)Main.screenPosition.Y) + zero,
            new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height),
            new Color(200, 200, 200),
            0f,
            Vector2.Zero,
            1f,
            SpriteEffects.None,
            0f);
        }

        public override void NearbyEffects(int x, int y, bool closer)
        {
            int rand = Main.rand.Next(1, 500);

            if (rand == 1)
            {
                Dust.NewDust(new Vector2((x * 16), (y * 16)), 16, 16, DustID.OrangeTorch, 0, 0, 100, default, 1f);
            }
        }
    }
}