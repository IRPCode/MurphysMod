using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;

//Since the LiquidLib is planned to be merged into tModLoader, remove these in the future.
using ModLiquidLib.ModLoader;
using ModLiquidLib.Utils.Structs;

namespace MurphysMod.Content.Liquids
{
    public class GodlyForgeLava : ModLiquid
    {
        public override void SetStaticDefaults()
        {
            VisualViscosity = 175;
            LiquidFallLength = 30;
            DefaultOpacity = 0.95f;
            SlopeOpacity = 1f;
            WaterRippleMultiplier = 0.4f;
            SplashDustType = DustID.Astra; //TODO: Change this to a custom dust
            SplashSound = SoundID.SplashWeak; //change the sound

            ChecksForDrowning = false;
            PlayersEmitBreathBubbles = false;

            FishingPoolSizeMultiplier = 1.5f;

            AddMapEntry(new Color(200, 200, 200), CreateMapEntryName()); //change the color too
        }

        public override int LiquidMerge(int i, int j, int otherLiquid)
        {
            if (otherLiquid == LiquidID.Water)
            {
                return 0;
            }

            else if (otherLiquid == LiquidID.Lava)
            {
                return 0;
            }

            else if (otherLiquid == LiquidID.Honey)
            {
                return 0;
            }

            else if (otherLiquid == LiquidID.Shimmer)
            {
                return 0;
            }
            else
            {
                return 0;
            }
        }

        public override void LiquidMergeSound(int i, int j, int otherLiquid, ref SoundStyle? collisionSound)
        {
            collisionSound = SoundID.Splash;
            //if(otherLiquid == LiquidID.Water){colisionSound = SoundXYZ}
        }

        public override int ChooseWaterfallStyle(int i, int j)
        {
            return ModContent.GetInstance<GodlyForgeLavaFall>().Slot;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 1f;
            b = 1f;
        }

        public override LightMaskMode LiquidLightMaskMode(int i, int j)
        {
            return LightMaskMode.None;
        }

        public override bool EvaporatesInHell(int i, int j)
        {
            return false;
        }

        public override void RetroDrawEffects(int i, int j, SpriteBatch spriteBatch, ref RetroLiquidDrawInfo drawData, float liquidAmountModified, int liquidGFXQuality)
        {
            drawData.liquidAlphaMultiplier *= 1.5f;
            if (drawData.liquidAlphaMultiplier > 1f)
            {
                drawData.liquidAlphaMultiplier = 1f;
            }
        }

        #region splash
        //modify this section to intended vals
        public override bool OnNPCSplash(NPC npc, bool isEnter)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(new Vector2(npc.position.X - 6f, npc.position.Y + (npc.height / 2) - 8f), npc.width + 12, 24, SplashDustType);
                Main.dust[dust].velocity.Y -= 1f;
                Main.dust[dust].velocity.X *= 2.5f;
                Main.dust[dust].scale = 1.3f;
                Main.dust[dust].alpha = 100;
                Main.dust[dust].noGravity = true;
            }

            //if npcs are added, make sure that they don't make splash sounds

            SoundEngine.PlaySound(SplashSound, npc.position);

            return false;
        }

        public override bool OnProjectileSplash(Projectile proj, bool isEnter)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(new Vector2(proj.position.X - 6f, proj.position.Y + (proj.height / 2) - 8f), proj.width + 12, 24, SplashDustType);
                Main.dust[dust].velocity.Y -= 1f;
                Main.dust[dust].velocity.X *= 2.5f;
                Main.dust[dust].scale = 1.3f;
                Main.dust[dust].alpha = 100;
                Main.dust[dust].noGravity = true;
            }
            SoundEngine.PlaySound(SplashSound, proj.position);
            return false;
        }
        
        public override bool OnItemSplash(Item item, bool isEnter)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(new Vector2(item.position.X - 6f, item.position.Y + (item.height / 2) - 8f), item.width + 12, 24, SplashDustType);
				Main.dust[dust].velocity.Y -= 1f;
				Main.dust[dust].velocity.X *= 2.5f;
				Main.dust[dust].scale = 1.3f;
				Main.dust[dust].alpha = 100;
				Main.dust[dust].noGravity = true;
			}
			SoundEngine.PlaySound(SplashSound, item.position);
			return false;
		}

        #endregion
    }
}