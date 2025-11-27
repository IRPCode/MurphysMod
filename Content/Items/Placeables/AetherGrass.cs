using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;

namespace MurphysMod.Content.Items.Placeables
{
    internal class AetherGrass : ModItem
    {
public override String Texture => "MurphysMod/Assets/Textures/Items/Placeables/CursedOre";
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
            ItemID.Sets.SortingPriorityMaterials[Type] = 58;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.value = Item.buyPrice(silver: 1);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.createTile = ModContent.TileType<Tiles.AetherGrass>();
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Tiles/GlowMasks/AetherGrass", AssetRequestMode.ImmediateLoad).Value;

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
        }

       

    }
}