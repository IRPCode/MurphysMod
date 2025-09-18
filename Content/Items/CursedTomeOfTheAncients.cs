using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MurphysMod.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
//using Luminance;

namespace MurphysMod.Content.Items
{
	public class CursedTomeOfTheAncients : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.MurphysMod.hjson' file.
		public override String Texture => "MurphysMod/Assets/Textures/Items/CursedTomeOfTheAncients";
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.consumable = false;

			Item.useTime = 24;
			Item.useAnimation = 24;

			Item.shootSpeed = .2f;
			Item.rare = ItemRarityID.Master;
		}

		public override bool? UseItem(Player player)
		{
			BookUsed bookUsed = ModContent.GetInstance<BookUsed>();

			if (!bookUsed.isPlayerCursed)
			{
				bookUsed.isPlayerCursed = true;
				Projectile.NewProjectile(player.GetSource_ItemUse(Item), new Vector2(player.position.X, player.position.Y), Vector2.Zero, ModContent.ProjectileType<CursedTomeOfTheAncientsProjectile>(), 0, 0f, player.whoAmI);
				return true;
			}
			else
			{
				return false;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			BookUsed bookUsed = ModContent.GetInstance<BookUsed>();
			if (!bookUsed.isPlayerCursed)
			{
				TooltipLine tip = new TooltipLine(Mod, "toolTip1", "It feels... Powerful.");
				tooltips.Add(tip);
				TooltipLine tip2 = new TooltipLine(Mod, "toolTip2", "This cannot be undone.") {OverrideColor = Color.DarkRed};
				tooltips.Add(tip2);
			}
			else
			{
				TooltipLine tip = new TooltipLine(Mod, "toolTip1", "It lies dormant; waiting.") { OverrideColor = Color.DimGray};
				tooltips.Add(tip);
			}
		}

		public class CursedTomeOfTheAncientsProjectile : ModProjectile
		{
			public override string Texture => "MurphysMod/Assets/Textures/Items/CursedTomeOfTheAncients";

			public int alphaAmount = 0;
			public int amount = 10;


			public static readonly SoundStyle book = new("MurphysMod/Assets/Audio/CursedBookClosingRift")
			{
				Volume = 1f,
				Pitch = 0f,
				PitchVariance = .0f
			};

			public override void SetDefaults()
			{
				Projectile.width = 28;
				Projectile.height = 28;
				Projectile.hostile = false;
				Projectile.friendly = true;
				Projectile.tileCollide = false;
				Projectile.timeLeft = 1795;
				Projectile.aiStyle = -1;
				AIType = -1;
				Projectile.alpha = default;
			}

			public override void AI()
			{
				if (Projectile.ai[0] == 0)
				{
					SoundEngine.PlaySound(book, Projectile.Center);

					//reset to prevent crashes
					Main.windSpeedTarget = 0f;
					Main.windSpeedCurrent = 0f;
					Main.maxRaining = 0f;
					Main.raining = false;
					Main.rainTime = 0;
				}

				Projectile.ai[0]++;

				//glowmask alpha
				if (Projectile.ai[0] % 7 == 0)
				{
					alphaAmount++;
				}

				//movement

				if (Projectile.ai[0] == 1)
				{
					Projectile.velocity.Y = -1f;
					Main.rainTime = 1793;
					Main.raining = true;
				}

				if (Projectile.ai[0] % 175 == 0)
				{
					Main.windSpeedTarget = MathHelper.Clamp(Main.windSpeedTarget + .1f, -1f, 1f);
					Main.maxRaining = MathHelper.Clamp(Main.maxRaining + .1f, -1f, 1f);
				}

				else if (Projectile.ai[0] <= 300)
				{
					Projectile.velocity.Y *= .99f;
				}

				else
				{
					Projectile.velocity.X = 0f;
					Projectile.velocity.Y = 0f;

					if (Projectile.ai[0] % 359 == 0)
					{
						amount++;
					}

					for (int i = 0; i <= 1 * amount; i++)
					{

						Vector2 dustLocation = new Vector2(Projectile.Center.X + Main.rand.Next(-100, 100) * 16f, Projectile.Center.Y + Main.rand.Next(-100, 100) * 16f);

						Vector2 direction = Projectile.Center - dustLocation;
						direction.Normalize();

						int bookDust = Dust.NewDust(dustLocation, 16, 16, DustID.SteampunkSteam, default);

						Main.dust[bookDust].velocity = direction * (8f + Projectile.ai[0] * .005f);
						Main.dust[bookDust].noGravity = true;
					}
				}

				if (Projectile.ai[0] == 300)
				{
					ModContent.GetInstance<ScreenShake>().strengthAndTime(1, Projectile.whoAmI);
				}

				if (Projectile.ai[0] == 1790)
				{
					shrinkGraphic(true);
				}

				//dust to book
				if (Projectile.ai[0] == 1794)
				{
					Main.windSpeedTarget = 0f;
					Main.raining = false;
					ModContent.GetInstance<DisplayLargeText>().message("You have been cursed", 600, Color.IndianRed, default, default, true);
				}

			}

#pragma warning disable CS0672 // Member overrides obsolete member
			public override void OnKill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
			{
				ModContent.GetInstance<ScreenShake>().strengthAndTime(0f, 0);
			}

			public override void PostDraw(Color lightColor)
			{
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Items/CursedTomeOfTheAncientsGlowMask");

				float normalizedAlpha = Utils.Clamp(alphaAmount / 255f, 0f, 1f);

				Color alpha = Color.White * normalizedAlpha;

				Main.spriteBatch.Draw
				(
					texture,
					Projectile.Center - Main.screenPosition + new Vector2(0f, 1f), //fixes offset
					new Rectangle(0, 0, texture.Width, texture.Height),
					alpha,
					Projectile.rotation,
					texture.Size() * 0.5f,
					Projectile.scale,
					SpriteEffects.None,
					0f
				);
			}

			public float rotation = 0;
			public bool shrink = false;
			public float size;

			public override bool PreDraw(ref Color lightColor)
			{
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("MurphysMod/Assets/Effects/MagicFlare");

				float alpha = alphaAmount / 255f;

				Color color = Color.White * alpha;

				if (shrink == true)
				{
					size *= .9f;
					Projectile.scale *= .9f;
				}
				else
				{
					size = Projectile.scale * alphaAmount / 750f;
				}

				rotation += .001f;

				Main.spriteBatch.End();

				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp,
				DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

				float rotationSpeed = 2f;
				float spriteSize = .25f;

				for (int i = 1; i <= 3; i++)
				{
					if (i == 2) rotationSpeed *= -1; //inverse

					Main.spriteBatch.Draw
					(
						texture,
						Projectile.Center - Main.screenPosition + new Vector2(0f, 1f), //smallest star
						new Rectangle(0, 0, texture.Width, texture.Height),
						color * (i * .75f),
						Projectile.rotation + (rotation / rotationSpeed),
						texture.Size() * 0.5f,
						size * spriteSize,
						SpriteEffects.None,
						0f
					);

					if (i == 2) rotationSpeed *= -1; //undo
					rotationSpeed++;
					if (i == 3) rotationSpeed++; //keep i == 1 speed and i == 3 speed stylistically nice
					spriteSize *= 2f;
				}

				Main.spriteBatch.End();

				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
				DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

				return true;
			}

			public void shrinkGraphic(bool begin)
			{
				shrink = begin;
			}
		}
		public class ScreenShake : ModSystem
		{
			private float shakeStrength;

			private float multiplyStrength;

			private int sourceProjectile;
			public override void ModifyTransformMatrix(ref SpriteViewMatrix transform)
			{
				if (Main.projectile[sourceProjectile].active && Main.projectile[sourceProjectile].type == ModContent.ProjectileType<CursedTomeOfTheAncientsProjectile>() && sourceProjectile >= 0 && sourceProjectile <= Main.maxProjectiles)
				{
					float randX = Main.rand.Next((int)-shakeStrength, (int)shakeStrength);
					float randY = Main.rand.Next((int)-shakeStrength, (int)shakeStrength);

					multiplyStrength *= 1.00005f;

					Vector2 displaceScreen = new Vector2(randX, randY) * multiplyStrength;

					Main.screenPosition += displaceScreen;

					if (Main.gameMenu)
					{
						return;
					}

				}
				else
				{
					multiplyStrength = 1f;
				}
			}

			public void strengthAndTime(float strength, int projectileIndex)
			{
				shakeStrength = strength;
				sourceProjectile = projectileIndex;
			}
		}

		public class disableMusic : ModSceneEffect
		{
			public override int Music => 0;
			public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
			public override bool IsSceneEffectActive(Player player)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					if (Main.projectile[i].type == ModContent.ProjectileType<CursedTomeOfTheAncientsProjectile>() && Main.projectile[i].active)
					{
						return true;
					}
				}
				return false;
			}
		}
	}
}