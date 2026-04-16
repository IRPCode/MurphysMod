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
        public static int timeCheck = 1800; //number of seconds per check
        public override void PostUpdate()
        {
            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckVal = luckHandler.luckValue();
            //rainFlag = true;

            if (Main.raining && Main.maxRain >= .2f && Main.GameUpdateCount % (ulong)(60 * (timeCheck * (1 + (luckVal * 2)))) == 0) //modifies amount of time based on luck
            {
                if (Main.rand.Next(1, 1) == 1) //20% chance of hail every X number of minutes it rains
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
                if (Main.windSpeedCurrent < 0f)
                    windspeed *= -1;

                int projectileType =  ModContent.ProjectileType<Hailstone>();


                int massiveHailChance = 10 - Utils.Clamp((int)((Main.maxRaining * 5) * Utils.Clamp((1 + luckVal), 1, 2)), 0, 9);
                int hailRand = Main.rand.Next(1, 101 * massiveHailChance);

                float x = player.position.X / 16f + (Main.rand.NextFloat((-200f * (1f + windspeed)), (200f * (1f + windspeed))));
                float y = player.position.Y / 16f + -75;

                Vector2 velocity = new Vector2((Main.windSpeedCurrent * 20), Main.rand.Next(60, 101) / 10);

                if (hailRand == 100)
                {
                    projectileType = ModContent.ProjectileType<GiantHailstone>();
                    velocity = new Vector2((Main.windSpeedCurrent * 10), Main.rand.Next(60, 101) / 3);
                }


                int projID = Projectile.NewProjectile(
                             default,
                             new Vector2(x * 16f, (y + 1) * 16f),
                             velocity,
                             projectileType,
                             10,
                             default,
                             -1);

                Projectile proj = Main.projectile[projID];

                if (projectileType == ModContent.ProjectileType<GiantHailstone>())
                {
                    proj.damage = 40 * (int)(1 + luckVal);
                    proj.scale = 1.5f;
                }
                else
                {
                    proj.damage = 15 * (int)(1 + luckVal);
                    proj.scale = .75f;
                }
                proj.hostile = true;
                proj.friendly = true;
                proj.tileCollide = true;
                proj.netUpdate = true;
                proj.rotation = Main.rand.Next(1, 3600) / 10;


            }
        }
    }
}