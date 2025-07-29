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

		//TODO: This file messes up the camera when the player teleports.

		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.MurphysMod.hjson' file.
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.consumable = false; //TODO: make this true

			Item.useTime = 24;
			Item.useAnimation = 24;

			//Item.UseSound = book;

			Item.shootSpeed = .2f;
		}

		public override bool? UseItem(Player player)
		{
			BookUsed bookUsed = ModContent.GetInstance<BookUsed>();
			Main.NewText($"{bookUsed.isPlayerCursed}");

			//if (!bookUsed.isPlayerCursed)
			//{
				Main.NewText("ReturnedTrue");
				bookUsed.isPlayerCursed = true;

				ModContent.GetInstance<DisplayLargeText>().message("You have been cursed", 600, Color.IndianRed, default, default, true);

				//Projectile.NewProjectile(player.GetSource_ItemUse(Item), new Vector2(player.position.X, player.position.Y), Vector2.Zero, ModContent.ProjectileType<CursedTomeOfTheAncientsProjectile>(), 0, 0f, player.whoAmI);
				return true;
			//}
			//else
			//{
			//	return false;
			//}

		}

		public class CursedTomeOfTheAncientsProjectile : ModProjectile
		{
			public override string Texture => "MurphysMod/Content/Items/CursedTomeOfTheAncients";
			float musicVolume;

			public int alphaAmount = 0;
			public int amount = 10;


			public static readonly SoundStyle book = new("MurphysMod/Content/Audio/CursedBookClosingRift")
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
					//musicVolume = Main.musicVolume;



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
					Main.rainTime = 1794;
					Main.raining = true;
				}

				if (Projectile.ai[0] % 175 == 0)
				{
					Main.windSpeedTarget += .1f;
					Main.maxRaining += .1f;
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

						int bookDust = Dust.NewDust(dustLocation, 16, 16, DustID.SteampunkSteam, default); //try DustID.Wraith

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
					Main.NewText("You have deeply upset the gods. You have been cursed.", new Color(255, 80, 80));
					ModContent.GetInstance<DisplayLargeText>().message("You have been cursed", 600, new Color(255, 80, 80), default, default, true);
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
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("MurphysMod/Content/Items/CursedTomeOfTheAncientsGlowMask");

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
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("MurphysMod/Assets/MagicFlare");

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

				Main.spriteBatch.Draw
				(
					texture,
					Projectile.Center - Main.screenPosition + new Vector2(0f, 1f), //smallest star
					new Rectangle(0, 0, texture.Width, texture.Height),
					color * .75f,
					Projectile.rotation + (rotation / 2f),
					texture.Size() * 0.5f,
					size * 0.25f,
					SpriteEffects.None,
					0f
				);

				Main.spriteBatch.Draw
				(
					texture,
					Projectile.Center - Main.screenPosition + new Vector2(0f, 1f), //medium star
					new Rectangle(0, 0, texture.Width, texture.Height),
					color,
					Projectile.rotation + (-rotation / 3f),
					texture.Size() * 0.5f,
					size * 0.5f,
					SpriteEffects.None,
					0f
				);

				Main.spriteBatch.Draw
				(
					texture,
					Projectile.Center - Main.screenPosition + new Vector2(0f, 1f), //largest star
					new Rectangle(0, 0, texture.Width, texture.Height),
					color * 2,
					Projectile.rotation + (rotation / 4f),
					texture.Size() * 0.5f,
					size,
					SpriteEffects.None,
					0f
				);

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
				if (Main.projectile[sourceProjectile].active && Main.projectile[sourceProjectile].type == ModContent.ProjectileType<CursedTomeOfTheAncientsProjectile>())
				{
					float randX = Main.rand.Next((int)-shakeStrength, (int)shakeStrength);
					float randY = Main.rand.Next((int)-shakeStrength, (int)shakeStrength);

					multiplyStrength *= 1.00005f;

					Vector2 displaceScreen = new Vector2(randX, randY) * multiplyStrength;

					Main.screenPosition += (displaceScreen);

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

		/*public class disableMusic : ModSceneEffect //TODO: Make music turn off, forums.terraria.org/index.php?threads/boss-music-mod-help.60120/
		{
			public override void UpdateMusic(ref int music, ref MusicPriority priority)
			{
				if (NPC.AnyNPCs(ModContent.ProjectileType<CursedTomeOfTheAncientsProjectile>()))
				{
				}
			}

			public override int Music => -1;
		}*/
	}
}