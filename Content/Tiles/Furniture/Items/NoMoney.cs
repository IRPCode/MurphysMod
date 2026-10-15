using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Items.Placeable.Furniture
{
	public class NoLegendary : ModItem //TODO: Change the item description using localization files
	{
        public override String Texture => "MurphysMod/Assets/Textures/Tiles/Paintings/NoMoneyItem";
		public override void SetDefaults() {
			
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.NoMoney>());

			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);

			
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "Tooltip Line", "He is no squid, and he certainly is not handsome."));
        }
	}
}