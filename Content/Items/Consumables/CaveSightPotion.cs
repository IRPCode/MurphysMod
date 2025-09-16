using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MurphysMod.Content.LuckHandlers;
using MurphysMod.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Items
{
    public class CaveSightPotion : ModItem //TODO: Add in the buff that tells a new ambient dust class to spawn light emitting dust particles. The worse your luck, the worse this potion will work (less light emitting dust) 
    {
        public override String Texture => "MurphysMod/Assets/Textures/Items/Potions/CaveSightPotion";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;

            ItemID.Sets.DrinkParticleColors[Type] = [
                new Color(212,210,180),
                new Color(189,185,160),
                new Color(133,130,114)
            ];
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(silver: 50);
            Item.buffTime = 5400;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(100);
            recipe.AddIngredient(ItemID.SpelunkerPotion, 2);
            recipe.AddIngredient(ItemID.Blinkroot, 1);
            recipe.AddIngredient(ItemID.Moonglow, 1);
            recipe.AddIngredient(ItemID.Hellstone, 1);
            recipe.AddTile(ModContent.TileType<TorchGodsBrazier>());
            recipe.Register();
        }
        
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{		
			TooltipLine tip = new TooltipLine(Mod, "toolTip1", "Shows the location of empty space.");
			tooltips.Add(tip);

			TooltipLine tip2 = new TooltipLine(Mod, "toolTip2", "Your bad luck hurts the strength of this potion.") { OverrideColor = Color.IndianRed };
			tooltips.Add(tip2);
		}
    }

  
}