using MurphysMod.Content.LuckHandlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using System;
using Mono.Cecil;
using System.Runtime.CompilerServices;
using System.Numerics;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using MurphysMod.Content.Ambience;

namespace MurphysMod.Common.UI
{
    internal class LuckBar : UIState
    {
        private UIText text;
        private UIElement area;
        private UIImage barFrame;
        private UIImage barBack;
        private float timer;
        private int newSteps;

        public override void OnInitialize()
        {
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 600, 1f);
            area.Top.Set(30f, 0f);
            area.Width.Set(182, 0f);
            area.Height.Set(60, 0f);

            barFrame = new UIImage(ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/UI/LuckBarFront"));
            barFrame.Left.Set(22, 0);
            barFrame.Top.Set(0, 0f);
            barFrame.Width.Set(138, 0f);
            barFrame.Height.Set(34, 0f);

            barBack = new UIImage(ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/UI/LuckBarBack"));
            barBack.Left.Set(22, 0);
            barBack.Top.Set(0, 0f);
            barBack.Width.Set(138, 0f);
            barBack.Height.Set(34, 0f);

            text = new UIText("Luck Level", 0.8f);
            text.Width.Set(138, 0f);
            text.Height.Set(34, 0f);
            text.Top.Set(40, 0f);
            text.Left.Set(0, 0f);

            area.Append(barBack);
            area.Append(text);
            area.Append(barFrame);

            Append(area);
        }

        public Color colorOsciliator()
        {
            Color color1 = new Color(47, 163, 255);
            Color color2 = new Color(254, 121, 2);

            timer++;
            float lerpAmount = (float)Math.Sin(timer * (Math.PI / (180 * 300)));

            return Color.Lerp(color1, color2, lerpAmount);
        }

        public float smoothBar(float current, float old)
        {
            if (old < current)
            {
                old++;
                return old;
            }
            else if (old > current)
            {
                old--;
                return old;
            }
            else
                return current;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckVal = luckHandler.luckValue();

            luckVal = Math.Round((Utils.Clamp(luckVal, 0f, 1f)), 2);

            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 25;
            hitbox.Width -= 30;
            hitbox.Y += 6;
            hitbox.Height -= 14;

            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * luckVal);
            int finalSteps = steps;

            if (Tick.globalTick % 60 == 0) //TODO: figure out how to lerp these
            {
                newSteps = steps;
            }

            finalSteps = (int)smoothBar(newSteps, steps);

            Main.NewText(finalSteps);

            text.SetText("Luck Level: " + (luckVal) * 100 + "%");
            colorOsciliator();

            for (int i = 0; i < finalSteps; i += 1)
            {
                float percent = (float)i / (right - left);

                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(Color.White, colorOsciliator(), percent));
            }
        }

        [Autoload(Side = ModSide.Client)]
        internal class LuckResourceUISystem : ModSystem
        {
            private UserInterface LuckBarUI;
            internal LuckBar LuckBar;

            public override void Load()
            {
                if (Main.dedServ)
                    return;
                LuckBar = new();
                LuckBarUI = new();
                LuckBarUI.SetState(LuckBar);
            }

            public override void UpdateUI(GameTime gameTime)
            {
                LuckBarUI?.Update(gameTime);
            }

            public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
            {
                int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
                if (resourceBarIndex != -1)
                {
                    layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                        "MurphysMod: Luck Resource Bar",
                        delegate
                        {
                            LuckBarUI.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }
    }
}