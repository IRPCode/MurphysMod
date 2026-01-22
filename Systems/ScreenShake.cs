using ExampleMod.Common.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;

public class ShakeScreen : ModSystem
{
    public static float shakeStrength;

    public static float multiplyStrength;

    public static float tick;
    public static float intensityPeak;
    public static bool active;
    public static int length;

    public static void StartShake(float IntensityPeak, int ShakeLength, float ShakeStrength)
    {
        tick = 0;
        intensityPeak = IntensityPeak;
        length = ShakeLength;
        shakeStrength = ShakeStrength;
        multiplyStrength = 1f;
        active = true;
    }

    public override void PostUpdateEverything()
    {
        tick++;
    }

    public override void ModifyTransformMatrix(ref SpriteViewMatrix transform)
    {
        if (Main.gameMenu || !ModContent.GetInstance<ClientSideConfig>().AmbientDustVisuals)
            return;

        Player player = Main.LocalPlayer;

        if (tick <= length && active == true)
        {
            float randX = Main.rand.NextFloat(-shakeStrength, shakeStrength);
            float randY = Main.rand.NextFloat(-shakeStrength, shakeStrength);

            if (tick == 1)
            {
                multiplyStrength *= shakeStrength;
            }

            if (tick < intensityPeak)
                multiplyStrength *= 1.0005f;
            else
                multiplyStrength *= .99f;

            Vector2 displaceScreen = new Vector2(randX, randY) * multiplyStrength;

            Main.screenPosition += displaceScreen;

            if (tick >= length || multiplyStrength <= .05f)
            {
                shakeStrength = 0f;
                active = false;
            }
        }
        else
        {
            multiplyStrength = 1f;
        }
    }
}