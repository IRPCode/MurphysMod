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
        double MluckVal = 0;
        Player player = Main.LocalPlayer;

        //General weather

        if (player.position.Y / 16f < Main.worldSurface)
        {
            if (!Main.raining)
            {
                WluckVal -= Math.Abs(Math.Round((double)Main.windSpeedCurrent, 2) / 4);
            }
            else
            {
                WluckVal += Math.Abs(Math.Round((double)Main.windSpeedCurrent / 2, 2)) * Math.Round((double)Main.maxRaining / 2, 2);
                if (player.ZoneSnow)
                    WluckVal += .05;
            }

            if (player.ZoneSandstorm)
            {
                WluckVal += Math.Abs(Math.Round((double)Main.windSpeedCurrent / 2, 2)) + .05;
            }

            WluckVal = Math.Round(WluckVal, 2);
        }

        //Moon phases

        if (!Main.dayTime)
        {
            if (Main.moonPhase >= 4)
                MluckVal = 1 - ((Main.moonPhase - 4) * .25);
            else
                MluckVal = 1 - (Main.moonPhase * .25);
        }

        MluckVal = Math.Round(MluckVal * .05, 3);

        WluckVal -= MluckVal;

        return WluckVal;
    }
}