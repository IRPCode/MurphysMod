//Objects that should glow faintly:

//Sunflowers at night, gemcorn trees, hellstone brick (ancient too), 

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Ambience
{

    public class TileGlow : GlobalTile
    {
        public float intensity;
        public Boolean flag;
        public static int[] acceptedGemTreeSources = { TileID.TreeAmber, TileID.TreeAmethyst, TileID.TreeDiamond, TileID.TreeEmerald, TileID.TreeRuby, TileID.TreeSapphire, TileID.TreeTopaz };
        public static int[] acceptedGemBunnyCages = {TileID.AmberBunnyCage, TileID.AmethystBunnyCage, TileID.DiamondBunnyCage, TileID.EmeraldBunnyCage, TileID.RubyBunnyCage, TileID.SapphireBunnyCage, TileID.TopazBunnyCage};
        public static int[] acceptedGemSquirrelCages = {TileID.AmberSquirrelCage, TileID.AmethystSquirrelCage, TileID.DiamondSquirrelCage, TileID.EmeraldSquirrelCage, TileID.RubySquirrelCage, TileID.SapphireSquirrelCage, TileID.TopazSquirrelCage};

        public static Color[] colors = { Color.Orange, Color.Purple, Color.White, Color.Green, Color.Red, Color.Blue, Color.Yellow };

        public override void PostDraw(int i, int j, int type, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

            #region gemcorn trees

            if (acceptedGemTreeSources.Contains(type) || acceptedGemBunnyCages.Contains(type) || acceptedGemSquirrelCages.Contains(type))
            {
                for (int x = 0; x < acceptedGemTreeSources.Length; x++)
                {
                    if (type == acceptedGemTreeSources[x] || type == acceptedGemBunnyCages[x] || type == acceptedGemSquirrelCages[x])
                    {
                        if (colors[x] == Color.Blue)
                            Lighting.AddLight(new Vector2(i + .5f, j + .5f) * 16f, (new Vector3(colors[x].R, colors[x].G * intensity, colors[x].B * intensity) / 255f) * .75f * intensity);

                        else if (colors[x] == Color.Red)
                            Lighting.AddLight(new Vector2(i + .5f, j + .5f) * 16f, (new Vector3(colors[x].R  * intensity, colors[x].G * intensity, colors[x].B) / 255f) * .75f * intensity);
                        else
                            Lighting.AddLight(new Vector2(i + .5f, j + .5f) * 16f, (new Vector3(colors[x].R , colors[x].G * intensity, colors[x].B * intensity) / 255f) * .75f * intensity);

                        strength();
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