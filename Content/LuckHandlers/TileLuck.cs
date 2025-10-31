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

        public static float tileLuck;

        public static float incrementAmount = .005f;

        public static int[] GoodLuckTiles = { TileID.GardenGnome, TileID.Sunflower, TileID.Jackolanterns, TileID.GoldBirdCage };
        public static int[] BadLuckTiles = { TileID.DemonAltar, TileID.SkullLanterns, TileID.ShadowOrbs };
        public static int[] GoldCreatures = { TileID.GoldBirdCage, TileID.GoldBunnyCage, TileID.GoldButterflyCage, TileID.GoldDragonflyJar, TileID.GoldFrogCage, TileID.GoldGoldfishBowl, TileID.GoldGrasshopperCage, TileID.GoldLadybugCage, TileID.GoldMouseCage, TileID.GoldSeahorseCage, TileID.GoldWaterStriderCage, TileID.GoldWormCage };

        public static int[] NormalTorches = { TorchID.Blue, TorchID.Green, TorchID.Orange, TorchID.Pink, TorchID.Purple, TorchID.Rainbow, TorchID.Red, TorchID.Torch, TorchID.UltraBright, TorchID.White, TorchID.Yellow };
        public override void PostUpdatePlayers() //Tile proximity ticks upwards slowly, if nothing is close, make it tick downwards
        {
            Player player = Main.LocalPlayer;

            Point playerLocation = player.Center.ToTileCoordinates();

            for (int x = -10; x <= 10; x++)
            {
                for (int y = -10; y <= 10; y++)
                {
                    int i = playerLocation.X + x;
                    int j = playerLocation.Y + y;

                    Tile tile = Framing.GetTileSafely(i, j);
                    if (!tile.HasTile)
                        continue;

                    if (tile.TileType == TileID.Torches)
                        tileLuck += torchLuck(i, j);

                    #region Good Luck

                    if (tile.TileType == TileID.GardenGnome)
                        tileLuck += incrementAmount;

                    if (tile.TileType == TileID.ChineseLanterns)
                        tileLuck += incrementAmount;

                    if (tile.TileType == TileID.Sunflower)
                        tileLuck -= incrementAmount;

                    if (tile.TileType == TileID.Jackolanterns)
                        tileLuck -= incrementAmount;

                    if (GoldCreatures.Contains(tile.TileType)) //prevents aggressive luck stacking
                        tileLuck -= incrementAmount;

                    #endregion

                    #region Bad Luck

                    if (tile.TileType == TileID.DemonAltar)
                        tileLuck -= incrementAmount;


                    if (tile.TileType == TileID.SkullLanterns)
                        tileLuck -= incrementAmount;

                    if (tile.TileType == TileID.ShadowOrbs)
                        tileLuck -= incrementAmount;

                    #endregion

                    Main.NewText(tileLuck);
                }
            }
        }
        public float torchLuck(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Framing.GetTileSafely(new Vector2(i, j));
            int TorchType = tile.TileFrameX / 18;
            float luckAmount = 0f;

            if (player.ZonePurity)
            {
                if (!NormalTorches.Contains(TorchType))
                    luckAmount = -.1f;
                else
                    luckAmount = 0f;
            }

            if (player.ZoneBeach)
            {
                if (TorchType != TorchID.Coral)
                    luckAmount = -.1f;
                else if (TorchType == TorchID.Coral)
                    luckAmount = .1f;
            }

            if (player.ZoneDesert)
            {
                if (TorchType != TorchID.Desert)
                    luckAmount = -.1f;
                else if (TorchType == TorchID.Desert)
                    luckAmount = .1f;
            }

            if (player.ZoneDungeon)
            {
                if (TorchType != TorchID.Bone)
                    luckAmount = -.1f;
                else if (TorchType == TorchID.Bone)
                    luckAmount = .1f;
            }

            if (player.ZoneGlowshroom)
            {
                if (TorchType != TorchID.Mushroom)
                    luckAmount = -.1f;
                else if (TorchType == TorchID.Mushroom)
                    luckAmount = .1f;
            }

            if (player.ZoneHallow)
            {
                if (TorchType != TorchID.Hallowed)
                    luckAmount = -.1f;
                else if (TorchType == TorchID.Hallowed)
                    luckAmount = .1f;
            }

            if (player.ZoneShimmer)
            {
                if (TorchType != TorchID.Shimmer)
                    luckAmount = -.1f;
                else if (TorchType == TorchID.Shimmer)
                    luckAmount = .1f;
            }

            if (player.ZoneSnow)
            {
                if (TorchType != TorchID.Ice)
                    luckAmount = -.1f;
                else if (TorchType == TorchID.Ice)
                    luckAmount = .1f;
            }

            if (player.ZoneCorrupt)
            {
                if (TorchType != TorchID.Corrupt || TorchType != TorchID.Cursed)
                    luckAmount = -.1f;
                else if (TorchType == TorchID.Corrupt || TorchType != TorchID.Cursed)
                    luckAmount = .1f;
            }

            if (player.ZoneCrimson)
            {
                if (TorchType != TorchID.Crimson || TorchType != TorchID.Ichor)
                    luckAmount = -.1f;
                else if (TorchType == TorchID.Crimson || TorchType != TorchID.Ichor)
                    luckAmount = .1f;
            }

            if (player.ZoneUnderworldHeight)
            {
                if (TorchType != TorchID.Demon)
                    luckAmount = -.1f;
                else if (TorchType == TorchID.Demon)
                    luckAmount = .1f;
            }

            return luckAmount;
        }
    }
}