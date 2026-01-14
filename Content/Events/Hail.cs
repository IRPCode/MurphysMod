using System;
using Microsoft.Xna.Framework;
using MurphysMod.Content.Enemies;
using MurphysMod.Content.LuckHandlers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content
{
    public class Hail : ModPlayer
    {
        public static bool rainFlag;
        public static int timeCheck = 240; //number of seconds per check
        public override void PostUpdate()
        {
            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckVal = luckHandler.luckValue();
            //rainFlag = true;

            if (Main.raining && Main.maxRain >= .2f && Main.GameUpdateCount % (ulong)(60 * (timeCheck * (1 + (luckVal * 2)))) == 0) //modifies amount of time based on luck
            {
                if (Main.rand.Next(1, 6) == 1) //20% chance of hail every X number of minutes it rains
                {
                    rainFlag = true;
                }
                else
                {
                    rainFlag = false;
                }
                    
            }

            int hailintensity = 10 - Utils.Clamp((int)((Main.maxRaining * 5) * Utils.Clamp((1 + luckVal), 1, 2)), 0, 9);
            int rand = Main.rand.Next(0, Utils.Clamp(hailintensity, 1, int.MaxValue));

            if (player.ZoneRain && rainFlag == true && rand <= 1)
            {
                float windspeed = Main.windSpeedCurrent;
                if(Main.windSpeedCurrent < 0f)
                    windspeed *= -1;
                
                    float x = player.position.X / 16f + (Main.rand.Next((int)(-200f * (1f + windspeed)), (int)(200f * (1f + windspeed))));
                    float y = player.position.Y / 16f + -75;

                    Vector2 velocity = new Vector2((Main.windSpeedCurrent * 20), Main.rand.Next(60, 101) / 10);

                    int projID = Projectile.NewProjectile(
                                 default,
                                 new Vector2(x * 16f, (y + 1) * 16f),
                                 velocity,
                                 ModContent.ProjectileType<Hailstone>(),
                                 10,
                                 default,
                                 -1);

                    Projectile proj = Main.projectile[projID];
                    proj.damage = 15 * (int)(1 + luckVal);
                    proj.scale = .75f;
                    proj.hostile = true;
                    proj.friendly = true;
                    proj.tileCollide = true;
                    proj.netUpdate = true;
            }
        }
    }
}