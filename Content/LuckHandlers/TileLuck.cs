//TODO: Add a handler that will add luck based buffs depending on what tiles you're close to, such as the hanging brazierusing Terraria;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.LuckHandlers
{
    public class TileLuck : ModSystem
    {
        public static double incrementAmount = .005;
        public static double torchLuckAmount = .01;

        public static int[] GoodLuckTiles = { TileID.GardenGnome, TileID.Sunflower, TileID.Jackolanterns, TileID.GoldBirdCage };
        public static int[] BadLuckTiles = { TileID.DemonAltar, TileID.SkullLanterns, TileID.ShadowOrbs };
        public static int[] GoldCreatures = { TileID.GoldBirdCage, TileID.GoldBunnyCage, TileID.GoldButterflyCage, TileID.GoldDragonflyJar, TileID.GoldFrogCage, TileID.GoldGoldfishBowl, TileID.GoldGrasshopperCage, TileID.GoldLadybugCage, TileID.GoldMouseCage, TileID.GoldSeahorseCage, TileID.GoldWaterStriderCage, TileID.GoldWormCage };

        public static int[] NormalTorches = { TorchID.Blue, TorchID.Green, TorchID.Orange, TorchID.Pink, TorchID.Purple, TorchID.Rainbow, TorchID.Red, TorchID.Torch, TorchID.UltraBright, TorchID.White, TorchID.Yellow };
        public override void PostUpdatePlayers()
        {
            Player player = Main.LocalPlayer;

            double tileLuck = 0;

            Point playerLocation = player.Center.ToTileCoordinates();

            for (int x = -60; x <= 60; x++)
            {
                for (int y = -40; y <= 40; y++)
                {
                    int i = playerLocation.X + x;
                    int j = playerLocation.Y + y;

                    Tile tile = Framing.GetTileSafely(i, j);
                    if (!tile.HasTile)
                        continue;

                    if (tile.TileType == TileID.Torches)
                        tileLuck += Utils.Clamp(torchLuck(i, j), -.3, .3);

                    #region Good Luck

                    if (tile.TileType == TileID.GardenGnome)
                        tileLuck -= incrementAmount * 5;

                    if (tile.TileType == TileID.ChineseLanterns)
                        tileLuck -= incrementAmount / 2;

                    if (tile.TileType == TileID.Sunflower)
                        tileLuck -= incrementAmount / 5;

                    if (tile.TileType == TileID.Jackolanterns)
                        tileLuck -= incrementAmount;

                    if (GoldCreatures.Contains(tile.TileType)) //prevents aggressive luck stacking
                        tileLuck -= incrementAmount * 2;

                    if (tile.TileType == TileID.PinkFairyJar || tile.TileType == TileID.GreenFairyJar || tile.TileType == TileID.BlueFairyJar)
                        tileLuck -= incrementAmount * 3;

                    if (tile.TileType == TileID.Heart)
                        tileLuck -= incrementAmount * 3;

                    if (tile.TileType == TileID.LifeFruit)
                        tileLuck -= incrementAmount * 4;

                    #endregion

                    #region Bad Luck

                    if (tile.TileType == TileID.DemonAltar)
                        tileLuck += incrementAmount;

                    if (tile.TileType == TileID.SkullLanterns)
                        tileLuck += incrementAmount * 4;

                    if (tile.TileType == TileID.ShadowOrbs)
                        tileLuck += incrementAmount * 2;

                    if (tile.TileType == TileID.EmpressButterflyJar)
                        tileLuck += incrementAmount * 3;

                    if (tile.TileType == TileID.Demonite || tile.TileType == TileID.Crimtane || tile.TileType == TileID.DemoniteBrick || tile.TileType == TileID.CrimtaneBrick)
                        tileLuck += incrementAmount / 2;

                    #endregion

                    LuckHandler.TileLuck = Utils.Clamp(tileLuck, -.7f, .7f);
                }
            }
        }
        public double torchLuck(int i, int j)

        {
            Player player = Main.LocalPlayer;
            Tile tile = Framing.GetTileSafely(i, j);
            int TorchType = tile.TileFrameY / 22;
            double luckAmount = 0f;

            if (player.ZonePurity)
                luckAmount += NormalTorches.Contains(TorchType) ? 0 : torchLuckAmount;

            else if (player.ZoneBeach)
                luckAmount += (TorchType == TorchID.Coral) ? -torchLuckAmount : torchLuckAmount;

            else if (player.ZoneDesert)
                luckAmount += (TorchType == TorchID.Desert) ? -torchLuckAmount : torchLuckAmount;

            else if (player.ZoneDungeon)
                luckAmount += (TorchType == TorchID.Bone) ? -torchLuckAmount : torchLuckAmount;

            else if (player.ZoneGlowshroom)
                luckAmount += (TorchType == TorchID.Mushroom) ? -torchLuckAmount : torchLuckAmount;

            else if (player.ZoneHallow)
                luckAmount += (TorchType == TorchID.Hallowed) ? -torchLuckAmount : torchLuckAmount;

            else if (player.ZoneShimmer)
                luckAmount += (TorchType == TorchID.Shimmer) ? -torchLuckAmount : torchLuckAmount;

            else if (player.ZoneSnow)
                luckAmount += (TorchType == TorchID.Ice) ? -torchLuckAmount : torchLuckAmount;

            else if (player.ZoneJungle)
                luckAmount += (TorchType == TorchID.Jungle) ? -torchLuckAmount : torchLuckAmount;

            else if (player.ZoneCorrupt)
                luckAmount += (TorchType == TorchID.Corrupt || TorchType == TorchID.Cursed) ? -torchLuckAmount : torchLuckAmount;

            else if (player.ZoneCrimson)
                luckAmount += (TorchType == TorchID.Crimson || TorchType == TorchID.Ichor) ? -torchLuckAmount : torchLuckAmount;

            else if (player.ZoneUnderworldHeight)
                luckAmount += (TorchType == TorchID.Demon) ? -torchLuckAmount : torchLuckAmount;

                //TODO: add a setting in the mod where it will subtract the bad luck amount if it is not in the purity biome and is a normal torch
                //I.E. normal torch types are neutral and will not impact luck

            //Main.NewText(luckAmount);

            return luckAmount;
        }
    }
}