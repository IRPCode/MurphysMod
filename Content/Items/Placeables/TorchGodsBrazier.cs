using Terraria.ModLoader;
using Terraria.ID;

namespace MurphysMod.Content.Items.Placeables
{
    internal class TorchGodsBrazier : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;

            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Expert;

            Item.autoReuse = true;

            Item.useTurn = true;

            Item.maxStack = 1;
            Item.consumable = true;

            Item.createTile = ModContent.TileType<Tiles.TorchGodsBrazier>();
            Item.placeStyle = 0;
        }
        
    }
}