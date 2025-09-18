using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MurphysMod.Content.Items.Placeables;
using System;

namespace MurphysMod.Content.Tools
{
    public class CursedAxe : ModItem

    {
        public override String Texture => "MurphysMod/Assets/Textures/Items/Tools/CursedAxe";
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.value = Item.buyPrice(silver: 5, copper: 50);
            Item.rare = ItemRarityID.Green;

            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.axe = 65;
            Item.damage = 24;
            Item.knockBack = 2f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CursedBar>(4)
                .AddRecipeGroup(RecipeGroupID.Wood, 10)
                .AddTile(TileID.Anvils)
                .Register();  
        }
    }
}