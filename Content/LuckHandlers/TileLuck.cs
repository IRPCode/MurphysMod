//TODO: Add a handler that will add luck based buffs depending on what tiles you're close to, such as the hanging brazierusing Terraria;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.LuckHandlers
{
    public class TileLuck : ModTile
    {

        public static float tileLuck;

        public static float incrementAmount = .005f;

        public static int[] GoodLuckTiles = { TileID.GardenGnome, TileID.Sunflower, TileID.Jackolanterns, TileID.GoldBirdCage };
        public static int[] BadLuckTiles = { TileID.DemonAltar, TileID.SkullLanterns, TileID.ShadowOrbs };
        public static int[] GoldCreatures = { TileID.GoldBirdCage, TileID.GoldBunnyCage, TileID.GoldButterflyCage, TileID.GoldDragonflyJar, TileID.GoldFrogCage, TileID.GoldGoldfishBowl, TileID.GoldGrasshopperCage, TileID.GoldLadybugCage, TileID.GoldMouseCage, TileID.GoldSeahorseCage, TileID.GoldWaterStriderCage, TileID.GoldWormCage };

        public static int[] NormalTorches = { Terraria.ID.TorchID.Blue, Terraria.ID.TorchID.Green, Terraria.ID.TorchID.Orange, Terraria.ID.TorchID.Pink, Terraria.ID.TorchID.Purple, Terraria.ID.TorchID.Rainbow, Terraria.ID.TorchID.Red, Terraria.ID.TorchID.Torch, Terraria.ID.TorchID.UltraBright, Terraria.ID.TorchID.White, Terraria.ID.TorchID.Yellow };
        public override void NearbyEffects(int i, int j, bool closer) //Tile proximity ticks upwards slowly, if nothing is close, make it tick downwards
        {
            Vector2 playerLoc = Main.LocalPlayer.Center;
            Vector2 tileLoc = new Vector2(i * 16, j * 16);

            //torch luck

            if (Vector2.Distance(playerLoc, tileLoc) <= 200 && Framing.GetTileSafely(i * 16, j * 16).TileType == TileID.Torches)
                tileLuck += torchLuck(new Vector2(i * 16, j * 16));

            if (Vector2.Distance(playerLoc, tileLoc) <= 50)
            {

                #region Good Luck

                if (Framing.GetTileSafely(i * 16, j * 16).TileType == TileID.GardenGnome)
                    tileLuck += incrementAmount;

                if (Framing.GetTileSafely(i * 16, j * 16).TileType == TileID.ChineseLanterns)
                    tileLuck += incrementAmount;

                if (Framing.GetTileSafely(i * 16, j * 16).TileType == TileID.Sunflower)
                    tileLuck -= incrementAmount;

                if (Framing.GetTileSafely(i * 16, j * 16).TileType == TileID.Jackolanterns)
                    tileLuck -= incrementAmount;

                if (GoldCreatures.Contains(Framing.GetTileSafely(i * 16, j * 16).TileType)) //prevents aggressive luck stacking
                    tileLuck -= incrementAmount;

                #endregion

                #region Bad Luck

                if (Framing.GetTileSafely(i * 16, j * 16).TileType == TileID.DemonAltar)
                    tileLuck -= incrementAmount;


                if (Framing.GetTileSafely(i * 16, j * 16).TileType == TileID.SkullLanterns)
                    tileLuck -= incrementAmount;

                if (Framing.GetTileSafely(i * 16, j * 16).TileType == TileID.ShadowOrbs)
                    tileLuck -= incrementAmount;

                #endregion

                Main.NewText(tileLuck);

            }


        }
        public float torchLuck(Vector2 position)
        {
            Player player = Main.LocalPlayer;
            Tile TorchType = Framing.GetTileSafely(position);
            float luckAmount = 0f;

            if (player.ZoneBeach)
            {
                if (TorchType.TileType != Terraria.ID.TorchID.Coral)
                    luckAmount = -.1f;
                else if (TorchType.TileType == Terraria.ID.TorchID.Coral)
                    luckAmount = .1f;
            }

            if (player.ZoneCorrupt)
            {
                if (TorchType.TileType != Terraria.ID.TorchID.Corrupt || TorchType.TileType != Terraria.ID.TorchID.Cursed)
                    luckAmount = -.1f;
                else if (TorchType.TileType == Terraria.ID.TorchID.Corrupt || TorchType.TileType != Terraria.ID.TorchID.Cursed)
                    luckAmount = .1f;
            }

            if (player.ZoneCrimson)
            {
                if (TorchType.TileType != Terraria.ID.TorchID.Crimson || TorchType.TileType != Terraria.ID.TorchID.Ichor)
                    luckAmount = -.1f;
                else if (TorchType.TileType == Terraria.ID.TorchID.Crimson || TorchType.TileType != Terraria.ID.TorchID.Ichor)
                    luckAmount = .1f;
            }

            if (player.ZoneDesert)
            {
                if (TorchType.TileType != Terraria.ID.TorchID.Desert )
                    luckAmount = -.1f;
                else if (TorchType.TileType == Terraria.ID.TorchID.Desert)
                    luckAmount = .1f;
            }

            if (player.ZoneDungeon)
            {
                 if (TorchType.TileType != Terraria.ID.TorchID.Bone)
                    luckAmount = -.1f;
                else if (TorchType.TileType == Terraria.ID.TorchID.Bone)
                    luckAmount = .1f;
            }

            if (player.ZoneGlowshroom)
            {
                 if (TorchType.TileType != Terraria.ID.TorchID.Mushroom)
                    luckAmount = -.1f;
                else if (TorchType.TileType == Terraria.ID.TorchID.Mushroom)
                    luckAmount = .1f;
            }

            if (player.ZoneHallow)
            {
                 if (TorchType.TileType != Terraria.ID.TorchID.Hallowed)
                    luckAmount = -.1f;
                else if (TorchType.TileType == Terraria.ID.TorchID.Hallowed)
                    luckAmount = .1f;
            }

            if (player.ZonePurity)
            {
                if (!NormalTorches.Contains(TorchType.TileType))
                    luckAmount = -.1f;
                else
                    luckAmount = 0f;
            }

            if (player.ZoneShimmer)
            {
                if (TorchType.TileType != Terraria.ID.TorchID.Shimmer)
                    luckAmount = -.1f;
                else if (TorchType.TileType == Terraria.ID.TorchID.Shimmer)
                    luckAmount = .1f;
            }

            if (player.ZoneSnow)
            {
                if (TorchType.TileType != Terraria.ID.TorchID.Ice)
                    luckAmount = -.1f;
                else if (TorchType.TileType == Terraria.ID.TorchID.Ice)
                    luckAmount = .1f;
            }

            if (player.ZoneUnderworldHeight)
            {
                if (TorchType.TileType != Terraria.ID.TorchID.Demon)
                    luckAmount = -.1f;
                else if (TorchType.TileType == Terraria.ID.TorchID.Demon)
                    luckAmount = .1f;
            }

            return luckAmount;
        }
    }
}