using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MurphysMod.Content.LuckHandlers;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Beard)] //TODO: fix torch being yellow, and make it run through the colors
    public class TheOrdainedDart : ModItem
    {
        public Vector3 torchColor;
        public Vector3 selectedColor;
        int dustType;
        public override String Texture => "MurphysMod/Assets/Textures/Items/Accessories/TheBigDart/OrdainedDart/OrdainedDart";
        Texture2D glowMaskOnPlayer = ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Items/Accessories/TheBigDart/OrdainedDart/OrdainedTorchGlow", AssetRequestMode.ImmediateLoad).Value;



        public override void SetStaticDefaults()
        {
            ArmorIDs.Beard.Sets.UseHairColor[Item.beardSlot] = false;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 28;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(copper: 10);
            Item.vanity = true;
            Item.accessory = true;
            Item.maxStack = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tip1 = new TooltipLine(Mod, "toolTip1", "Take one, mighty puff.");
            tooltips.Add(tip1);
            TooltipLine tip2 = new TooltipLine(Mod, "toolTip2", "'Let there be light', and light it I did.");
            tooltips.Add(tip2);
            TooltipLine tip3 = new TooltipLine(Mod, "toolTip3", "Color of torch changes depending on the biome.");
            tooltips.Add(tip3);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            updateTorch();
            Texture2D texture = ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Items/Accessories/TheBigDart/OrdainedTorchDart/OrdainedTorchGlow", AssetRequestMode.ImmediateLoad).Value;

            Vector2 position = Main.item[whoAmI].position - Main.screenPosition + new Vector2(Main.item[whoAmI].width / 2, Main.item[whoAmI].height - texture.Height / 2f);

            spriteBatch.Draw(texture,
              position,
               null,
                new Color(200, 200, 200),
                 0f,
                  texture.Size() / 2,
                   1f,
                    SpriteEffects.None,
                     0f);

            Lighting.AddLight(position, torchColor / 2);
        }

        public void updateTorch()
        {
            Player player = Main.LocalPlayer;

            if (player.ZoneCorrupt)
            {
                dustType = DustID.CorruptTorch;
                selectedColor = Color.Purple.ToVector3();
            }

            if (player.ZoneCrimson)
            {
                dustType = DustID.CrimsonTorch;
                selectedColor = Color.Red.ToVector3();
            }

            if (player.ZoneDesert || player.ZoneUndergroundDesert)
            {
                dustType = DustID.DesertTorch;
                selectedColor = Color.Orange.ToVector3();
            }

            if (player.ZoneJungle || player.ZoneLihzhardTemple)
            {
                dustType = DustID.JungleTorch;
                selectedColor = Color.LimeGreen.ToVector3();
            }

            if (player.ZoneSnow)
            {
                dustType = DustID.IceTorch;
                selectedColor = Color.Cyan.ToVector3();
            }

            if (player.ZoneGlowshroom)
            {
                dustType = DustID.MushroomTorch;
                selectedColor = Color.DarkBlue.ToVector3();
            }

            if (player.ZoneUnderworldHeight)
            {
                dustType = DustID.DemonTorch;
                selectedColor = Color.Purple.ToVector3();
            }

            if (player.ZoneShimmer)
            {
                dustType = DustID.ShimmerTorch;
                selectedColor = Color.Purple.ToVector3();
            }

            if (player.ZoneDungeon)
            {
                dustType = DustID.BoneTorch;
                selectedColor = Color.MediumPurple.ToVector3();
            }

            if (player.ZoneHallow)
            {
                dustType = DustID.HallowedTorch;
                selectedColor = Color.Pink.ToVector3();
            }
            else
            {
                dustType = DustID.Torch;
                selectedColor = Color.Orange.ToVector3();
            }

            torchColor.X = MathHelper.Lerp(torchColor.X, selectedColor.X, .05f);
            torchColor.Y = MathHelper.Lerp(torchColor.Y, selectedColor.Y, .05f);
            torchColor.Z = MathHelper.Lerp(torchColor.Z, selectedColor.Z, .05f);
        }


        public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
        {
            updateTorch();
            if (Item.beardSlot < 0)
                return true;

            drawInfo.DrawDataCache.Add(drawData);

            drawInfo.DrawDataCache.Add(drawData with
            {
                texture = glowMaskOnPlayer,
                color = new Color(torchColor)
            });

            Player player = drawInfo.drawPlayer;

            if (!player.active || player.whoAmI < 0)
                return false;

            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();

            Lighting.AddLight(player.Center, torchColor / 2);

            Vector2 dustPos = new Vector2(player.Center.X + ((12f)), player.Center.Y - (11.75f));
            if (player.direction == -1)
                dustPos.X -= 32.25f;

            int luckValueFinal = (int)(Math.Pow(1 + luckHandler.luckValue(), 11)) * 10;

            if (Main.rand.Next(0, 1000000 / luckValueFinal) % 1000 == 0)
                Dust.NewDust(dustPos, default, default, dustType);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Torch)
                .Register();
        }
    }
}