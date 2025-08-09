using System.Numerics;
using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class OrdainedMoltenSlagPlayerHandler : ModPlayer
{
    public override void PostUpdate()
    {
        if (Main.myPlayer == Player.whoAmI && !Main.dedServ)
        {
            float x = Player.Center.X;
            float y = Player.Center.Y;

            Tile playerLoc = Framing.GetTileSafely((int)(x / 16), (int)(y / 16));

            if (playerLoc.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "OrdainedMoltenSlag").Type)
            {
                Player.velocity.Y = -10f;
                //Player.AddBuff(BuffID.);
            }
        }
    }
}