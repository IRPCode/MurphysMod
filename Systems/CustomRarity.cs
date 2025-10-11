using System;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MurphysMod.Systems
{
    public class CustomRarity : ModSystem
    {
        public static int timer;

        public override void PostUpdateEverything()
        {
            timer++;
        }
        public static Color getRarity(Color color1, Color color2)
        {
            
            float lerpAmount = (float)Math.Sin(timer * (Math.PI / (180 * 300)));
            return Color.Lerp(color1, color2, lerpAmount);
        }
    }
}