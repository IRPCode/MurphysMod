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
    [AutoloadEquip(EquipType.Beard)] //just to be on the face
    public class TheBigDart : ModItem
    {
        public override String Texture => "MurphysMod/Assets/Textures/Items/Accessories/TheBigDart/TorchDart";
        Texture2D glowMaskOnPlayer = ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Items/Accessories/TheBigDart/TorchDartVanityGlowMask", AssetRequestMode.ImmediateLoad).Value;


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
            TooltipLine tip1 = new TooltipLine(Mod, "toolTip1", "I'll take a pack of torches, please.");
			tooltips.Add(tip1);
            TooltipLine tip2 = new TooltipLine(Mod, "toolTip2", "The worse your luck, the more you chuff back on that fat dart.");
			tooltips.Add(tip2);
            TooltipLine tip3 = new TooltipLine(Mod, "toolTip3", "Provides a small amount of light.");
			tooltips.Add(tip3);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Items/Accessories/TheBigDart/TorchDartGlowMask", AssetRequestMode.ImmediateLoad).Value;

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

            Lighting.AddLight(position, Color.Orange.ToVector3() / 2);
        }


        public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
        {
            if (Item.beardSlot < 0)
                return true;

            drawInfo.DrawDataCache.Add(drawData);

            drawInfo.DrawDataCache.Add(drawData with
            {
                texture = glowMaskOnPlayer,
                color = Color.White
            });

            Player player = drawInfo.drawPlayer;

            if(!player.active || player.whoAmI < 0)
                return false;
                
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();

            Lighting.AddLight(player.Center, Color.Orange.ToVector3() / 2);

            Vector2 dustPos = new Vector2(player.Center.X + ((12f)), player.Center.Y - (11.75f));
            if (player.direction == -1)
                dustPos.X -= 32.25f;

            int luckValueFinal = (int)(Math.Pow(1 + luckHandler.luckValue(), 11)) * 10;

            if (Main.rand.Next(0, 1000000 / luckValueFinal) % 1000 == 0)
                Dust.NewDust(dustPos, default, default, DustID.Torch);

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