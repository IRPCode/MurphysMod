using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using System;

namespace MurphysMod.Content.Items.Ingredients
{
    internal class CursedGoldFragment : ModItem
    {
        public override String Texture => "MurphysMod/Assets/Textures/Items/Ingredients/CursedGoldFragment";
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 59;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 9999;
            Item.consumable = false;
            Item.value = Item.buyPrice(silver: 3, copper: 50);

            Item.useStyle = ItemUseStyleID.None;
        }
    }
}