using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;

namespace MurphysMod.Systems
{
    public class DisplayLargeText : ModSystem
    {
        public string largeText;
        public int timer;
        public int timerOriginal;
        public Vector2 textLocation;
        public Color textColor;
        public float textScale;

        public bool animatedText;
        public Byte alpha = 255;

        public float movement = .05f;

        public void message(string text, int timeAmount, Color color, Vector2 location, float scale, bool animated)
        {
            if (text == default)
            {
                text = "error";
            }

            if (timeAmount == default)
            {
                timeAmount = 600; // 10 seconds
            }

            if (color == default)
            {
                color = Color.IndianRed;
            }

            if (location == default)
            {
                location = new Vector2(Main.screenWidth / 2f, (int)(Main.screenHeight / 2f));
            }

            if (scale == default)
            {
                scale = 1f;
            }

            if (animated == default)
            {
                animated = true;
            }

            largeText = text;
            timer = timeAmount;
            timerOriginal = timer;
            textColor = color;
            textLocation = location;
            textScale = scale;
            animatedText = animated;
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if (largeText != null && timer > 0)
            {
                if (animatedText == true && (timer <= timerOriginal / 5))
                {
                    movement *= 1.05f;
                    textLocation.Y += movement;

                    if (timer == 1)
                    {
                        movement = .05f;
                    }
                }

                Vector2 getSize = ChatManager.GetStringSize(FontAssets.DeathText.Value, largeText, Vector2.One);

                Vector2 drawnPosition = new Vector2(textLocation.X - getSize.X * 1.5f, textLocation.Y);

                Utils.DrawBorderStringBig(spriteBatch, largeText, drawnPosition, textColor, textScale, 0f, 0f);

                timer--;
            }
        }
    }
}
