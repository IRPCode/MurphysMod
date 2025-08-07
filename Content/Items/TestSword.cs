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
	public class TestSword : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.MurphysMod.hjson' file.
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MagicBolt>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		public override bool? UseItem(Player player)
		{
			/*var projectile = ModContent.ProjectileType<FrigidBolt>();

			Projectile.NewProjectile(
				player.GetSource_ItemUse(Item),
				player.Center,
				Vector2.Normalize(Main.MouseWorld - player.Center) * 10f,
				projectile,
				100, // damage
				10f, // knockback
				player.whoAmI
			);*/

			//Mathhelper.lerp(Main.Mouseworld - player.Center,npc.center,1f)

			return true;
		}
	}
}
