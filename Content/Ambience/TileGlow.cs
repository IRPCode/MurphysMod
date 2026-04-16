//Objects that should glow faintly:

//Sunflowers at night, gemcorn trees, hellstone brick (ancient too), 

using System;
using System.Linq;
using ExampleMod.Common.Configs;
using ExampleMod.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Ambience
{

    public class TileGlow : GlobalTile
    {
        public float intensity;
        public Boolean flag;

        public static int[] gemTypes = { 0, 1, 2, 3, 4, 5, 6 };
        public static int[] acceptedGemTreeSources = { TileID.TreeAmber, TileID.TreeAmethyst, TileID.TreeDiamond, TileID.TreeEmerald, TileID.TreeRuby, TileID.TreeSapphire, TileID.TreeTopaz };
        public static int[] acceptedGemBunnyCages = { TileID.AmberBunnyCage, TileID.AmethystBunnyCage, TileID.DiamondBunnyCage, TileID.EmeraldBunnyCage, TileID.RubyBunnyCage, TileID.SapphireBunnyCage, TileID.TopazBunnyCage };
        public static int[] acceptedGemSquirrelCages = { TileID.AmberSquirrelCage, TileID.AmethystSquirrelCage, TileID.DiamondSquirrelCage, TileID.EmeraldSquirrelCage, TileID.RubySquirrelCage, TileID.SapphireSquirrelCage, TileID.TopazSquirrelCage };

        public static Color[] colors = { Color.Orange, Color.Purple, Color.White, Color.Green, Color.Red, Color.Blue, Color.Yellow };

        public static bool tileGlow = ModContent.GetInstance<ClientSideConfig>().TileGlow;

        public override void PostDraw(int i, int j, int type, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (!ModContent.GetInstance<ClientSideConfig>().TileGlow)
                return;

            #region gemcorn trees
            strength();



            if (acceptedGemTreeSources.Contains(type) || acceptedGemBunnyCages.Contains(type) || acceptedGemSquirrelCages.Contains(type) || type == TileID.ExposedGems)
            {

                if(type == TileID.ExposedGems)
                {
                    //int gemType = type.tileFramingX / 18;

                }



                for (int x = 0; x < acceptedGemTreeSources.Length; x++)
                {
                    if (type == acceptedGemTreeSources[x] || type == acceptedGemBunnyCages[x] || type == acceptedGemSquirrelCages[x])
                    {
                        if (colors[x] == Color.Blue)
                            Lighting.AddLight(new Vector2(i + .5f, j + .5f) * 16f, (new Vector3(colors[x].R, colors[x].G * intensity, colors[x].B * intensity) / 255f) * .75f * intensity);

                        else if (colors[x] == Color.Red)
                            Lighting.AddLight(new Vector2(i + .5f, j + .5f) * 16f, (new Vector3(colors[x].R * intensity, colors[x].G * intensity, colors[x].B) / 255f) * .75f * intensity);
                        else
                            Lighting.AddLight(new Vector2(i + .5f, j + .5f) * 16f, (new Vector3(colors[x].R, colors[x].G * intensity, colors[x].B * intensity) / 255f) * .75f * intensity);


                    }
                }
            }

            #endregion


        }

        public void strength()
        {
            int x = Tick.globalTick;

            if (x % 2 == 0)
            {
                if (flag)
                    intensity += .00005f;
                else
                    intensity -= .00005f;
            }

            if (intensity < .9f)
            {
                flag = true;
                intensity = .9f;
            }
            else if (intensity > 1.1f)
            {
                flag = false;
                intensity = 1.1f;
            }
        }
    }
}