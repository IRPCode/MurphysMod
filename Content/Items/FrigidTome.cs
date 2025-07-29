using Microsoft.Xna.Framework;
using MurphysMod.Content.Enemies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Items
{
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class FrigidTome : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.MurphysMod.hjson' file.
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.useStyle = ItemUseStyleID.Shoot;

			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.mana = 5;
			Item.useTime = 20;
			Item.damage = 20;
			Item.knockBack = 2f;
			Item.autoReuse = true;

			Item.shoot = ModContent.ProjectileType<FrigidBolt>();
			Item.shootSpeed = .2f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		
	}
}
