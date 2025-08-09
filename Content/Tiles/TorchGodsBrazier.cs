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
            TileObjectData.newTile.CoordinateHeights = new[] {16, 16, 16, 16, 16};

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
    }
}