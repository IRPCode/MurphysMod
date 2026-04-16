using MurphysMod.Content.Liquids;
using ModLiquidLib.ID;
using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace MurphysMod.Content.Items
{
	public class EnchantedWaterBucket : ModItem
	{
		public override String Texture => "MurphysMod/Assets/Textures/Items/BlessedWaterBucket";

		public override void SetStaticDefaults()
		{
			ItemID.Sets.IsLavaImmuneRegardlessOfRarity[Type] = true;
			ItemID.Sets.AlsoABuildingItem[Type] = true;
			ItemID.Sets.DuplicationMenuToolsFilter[Type] = true;
			LiquidID_TLmod.Sets.CreateLiquidBucketItem[LiquidLoader.LiquidType<EnchantedWater>()] = Type;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}


		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 24;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tip = new TooltipLine(Mod, "toolTip1", "A magical liquid whose essence is tied to the biome you're in.");
			tooltips.Add(tip);
		}
		public override void HoldItem(Player player)
		{
			if (!player.JustDroppedAnItem)
			{
				if (player.whoAmI != Main.myPlayer)
				{
					return;
				}

				if (player.noBuilding ||
					!(player.position.X / 16f - (float)Player.tileRangeX - (float)Item.tileBoost <= (float)Player.tileTargetX) ||
					!((player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)Item.tileBoost - 1f >= (float)Player.tileTargetX) ||
					!(player.position.Y / 16f - (float)Player.tileRangeY - (float)Item.tileBoost <= (float)Player.tileTargetY) ||
					!((player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)Item.tileBoost - 2f >= (float)Player.tileTargetY))
				{
					return;
				}

				if (!Main.GamepadDisableCursorItemIcon)
				{
					player.cursorItemIconEnabled = true;
					Main.ItemIconCacheUpdate(Item.type);
				}

				if (!player.ItemTimeIsZero || player.itemAnimation <= 0 || !player.controlUseItem)
				{
					return;
				}

				Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
				if (tile.LiquidAmount >= 200)
				{
					return;
				}

				if (tile.HasUnactuatedTile)
				{
					bool[] tileSolid = Main.tileSolid;
					if (tileSolid[tile.TileType])
					{
						bool[] tileSolidTop = Main.tileSolidTop;
						if (!tileSolidTop[tile.TileType])
						{
							if (tile.TileType != TileID.Grate)
							{
								return;
							}
						}
					}
				}

				if (tile.LiquidAmount != 0)
				{
					if (tile.LiquidType != LiquidLoader.LiquidType<EnchantedWater>())
					{
						return;
					}
				}

				SoundEngine.PlaySound(SoundID.SplashWeak, player.position);
				tile.LiquidType = LiquidLoader.LiquidType<EnchantedWater>();
				tile.LiquidAmount = byte.MaxValue;
				WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY);
				Item.stack--;
				player.PutItemInInventoryFromItemUsage(ItemID.EmptyBucket, player.selectedItem);
				player.ApplyItemTime(Item);

				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
				}
			}
		}
	}
}