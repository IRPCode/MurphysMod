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
using MurphysMod.Content.Ambience.Dusts;
using Terraria.GameContent.Liquid;

namespace MurphysMod.Content.Liquids
{
    public class OrdainedMoltenSlag : ModLiquid
    { //Textures are kept inside of the liquids folder for the sake simplicity, as Tmodloader and LiquidLib handles this automatically 
        public static readonly SoundStyle sound = new("MurphysMod/Assets/Audio/OrdainedMoltenSlagSplash")
        {
            Volume = 1f,
            Pitch = 0f,
            PitchVariance = .2f
        };

        public override void SetStaticDefaults()
        {
            LiquidRenderer.VISCOSITY_MASK[Type] = 100;
			LiquidRenderer.WATERFALL_LENGTH[Type] = 30;
			LiquidRenderer.DEFAULT_OPACITY[Type] = 0f;

            SlopeOpacity = 0.9f;
            WaterRippleMultiplier = 100f;
            SplashDustType = DustID.SpelunkerGlowstickSparkle;
            SplashSound = sound;
            ChecksForDrowning = false;

            FishingPoolSizeMultiplier = 1.5f;

            AddMapEntry(new Color(255, 249, 207), CreateMapEntryName()); //change the color too
        }

        public override void Load()
        {
            LiquidRenderer.VISCOSITY_MASK[LiquidID.Honey] = 100;
        }

        public override int LiquidMerge(int i, int j, int otherLiquid)
        {
            if (otherLiquid == LiquidID.Water)
            {
                return TileID.Obsidian;
            }

            else if (otherLiquid == LiquidID.Lava)
            {
                return TileID.Obsidian;
            }

            else if (otherLiquid == LiquidID.Honey)
            {
                return TileID.Obsidian;
            }

            else if (otherLiquid == LiquidID.Shimmer)
            {
                return TileID.Obsidian;
            }
            else
            {
                return TileID.Obsidian;
            }
        }

        public override void LiquidMergeSound(int i, int j, int otherLiquid, ref SoundStyle? collisionSound)
        {
            collisionSound = SoundID.Splash;
            //if(otherLiquid == LiquidID.Water){colisionSound = SoundXYZ}
        }

        public override int ChooseWaterfallStyle(int i, int j)
        {
            return ModContent.GetInstance<OrdainedMoltenSlagFall>().Slot;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = .97f;
            b = .81f;
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