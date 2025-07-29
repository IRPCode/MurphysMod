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

        public bool textFade;
        public int alpha = 255;

        public void message(string text, int timeAmount, Color color, Vector2 location, float scale, bool fade) //TODO: This only works on 2560 x 1600
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

            if (fade == default)
            {
                fade = false;
            }

            largeText = text;
            timer = timeAmount;
            timerOriginal = timer;
            textColor = color;
            textLocation = location;
            textScale = scale;
            textFade = fade;
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if (largeText != null && timer > 0)
            {

                if (textFade == true && (timerOriginal / 5) >= timer)
                {
                    alpha = alpha - (255 / (timerOriginal / 5));
                    alpha = Utils.Clamp(alpha, 0, 255);
                    textColor = new Color(textColor.R, textColor.G, textColor.B, alpha);
                }

                Vector2 getSize = ChatManager.GetStringSize(FontAssets.DeathText.Value, largeText, Vector2.One);

                Vector2 drawnPosition = new Vector2(textLocation.X - getSize.X * 1.5f, textLocation.Y);

                Utils.DrawBorderStringBig(spriteBatch, largeText, drawnPosition, textColor, textScale, 0f, 0f);

                timer--;
            }
        }
    }
}
