using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MurphysMod.Content.Tiles //TODO: https://www.youtube.com/watch?v=vjrIH2v90WM spawn ores on gen
{
    internal class CursedOre : ModTile
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
            Main.tileOreFinderPriority[Type] = 350; //above plat, below meteorite for metal detector

            AddMapEntry(new Color(200, 200, 200), CreateMapEntryName());

            DustType = DustID.Tungsten;
            HitSound = SoundID.Tink;
            RegisterItemDrop(ModContent.ItemType<Items.Placeables.CursedOre>());

            MineResist = 1.5f;
            MinPick = 60; //60% strength needed

        }
    }
}