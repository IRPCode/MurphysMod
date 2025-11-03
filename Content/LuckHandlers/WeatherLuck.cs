using System;
using Terraria;
using Terraria.ModLoader;

public class WeatherLuck : ModSystem
{
    public override void PostUpdateEverything()
    {
        getWeatherLuckVal();
    }
    public double getWeatherLuckVal()
    {
        double WluckVal = 0;
        Player player = Main.LocalPlayer;

        if (player.position.Y / 16f < Main.worldSurface)
        {
            if (!Main.raining)
            {
                WluckVal -= Math.Abs(Math.Round((double)Main.windSpeedCurrent, 2) / 4);
            }
            else
            {
                WluckVal += Math.Abs(Math.Round((double)Main.windSpeedCurrent, 2)) * Math.Round((double)Main.maxRaining, 2);
                if (player.ZoneSnow)
                    WluckVal += .1f;
            }

            if (player.ZoneSandstorm)
            {
                WluckVal += Math.Abs(Math.Round((double)Main.windSpeedCurrent, 2)) + .1;
            }

            WluckVal = Math.Round(WluckVal, 2);
        }

        //Time

        return WluckVal;
    }
}