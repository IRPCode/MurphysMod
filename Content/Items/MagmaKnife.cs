using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MurphysMod.Content.Buffs;
using MurphysMod.Content.Enemies;
using MurphysMod.Content.Items.Placeables;
using MurphysMod.Content.LuckHandlers;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Items
{
	public class MagmaKnife : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.MurphysMod.hjson' file.
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.useTime = 20;
			Item.useAnimation = 20;

			Item.DamageType = DamageClass.Ranged;
			Item.damage = 6;
			Item.knockBack = 2f;
			Item.consumable = true;
			Item.autoReuse = true;
			Item.maxStack = 9999;

			Item.shoot = ModContent.ProjectileType<MagmaKnifeProjectile>();
			Item.shootSpeed = 5f;
			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(100);
			recipe.AddIngredient(ItemID.ThrowingKnife, 100);
			recipe.AddIngredient(ModContent.ItemType<MagmaGem>());
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
			float luckVal = luckHandler.luckValue();

			float[] itemStats = modifyKnifeStats();

			//modifies based off of luck

			Item.damage = (int)itemStats[0];
			Item.knockBack = itemStats[1];
			Item.useTime = (int)itemStats[2];
			Item.useAnimation = (int)itemStats[3];
			Item.shootSpeed = itemStats[4];

			TooltipLine tip = new TooltipLine(Mod, "toolTip1", "Quite hot to the touch.");
			tooltips.Add(tip);

			TooltipLine tip2 = new TooltipLine(Mod, "toolTip2", "Your bad luck imbues this weapon with strength.") { OverrideColor = Color.Violet };
			tooltips.Add(tip2);
		}

		public override void HoldItem(Player player)
		{
			float[] itemStats = modifyKnifeStats();

			//modifies based off of luck

			Item.damage = (int)itemStats[0];
			Item.knockBack = itemStats[1];
			Item.useTime = (int)itemStats[2];
			Item.useAnimation = (int)itemStats[3];
			Item.shootSpeed = itemStats[4];
		}

		public float[] modifyKnifeStats()
		{
			Player player = Main.LocalPlayer;
			LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
			float luckVal = luckHandler.luckValue();

			float[] itemStats = [0, 0, 0, 0, 0]; //damage, knockback, useTime, useAnimation, shootSpeed

			if (luckVal >= 1f)
			{
				itemStats[0] = 18f;
				itemStats[1] = 6f;
				itemStats[2] = 8f;
				itemStats[3] = 8f;
				itemStats[4] = 22f;
			}
			else if (luckVal >= .75f)
			{
				itemStats[0] = 16f;
				itemStats[1] = 5f;
				itemStats[2] = 14f;
				itemStats[3] = 14f;
				itemStats[4] = 18f;
			}
			else if (luckVal >= .5f)
			{
				itemStats[0] = 14f;
				itemStats[1] = 4f;
				itemStats[2] = 16f;
				itemStats[3] = 16f;
				itemStats[4] = 15f;
			}
			else if (luckVal >= .25f)
			{
				itemStats[0] = 10f;
				itemStats[1] = 3f;
				itemStats[2] = 18f;
				itemStats[3] = 18f;
				itemStats[4] = 12f;
			}
			else
			{
				itemStats[0] = 8f;
				itemStats[1] = 2f;
				itemStats[2] = 20f;
				itemStats[3] = 20f;
				itemStats[4] = 10f;
			}

			return itemStats;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("MurphysMod/Content/Items/MagmaKnifeGlowMask");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
	}
}
