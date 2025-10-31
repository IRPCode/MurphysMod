using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MurphysMod.Content.Items.Ingredients;
using MurphysMod.Content.Weapons.Projectiles;
using MurphysMod.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Weapons
{
    public class BlessedGlaive : ModItem
    {
        public override String Texture => "MurphysMod/Assets/Textures/Items/Weapons/OrdainedNaginata";
        public override void SetDefaults()
        {
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HiddenAnimation;

            Item.damage = 60;
            Item.width = 74;
            Item.height = 82;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.autoReuse = true;
            Item.UseSound = null;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shootSpeed = 0;

            Item.shoot = ModContent.ProjectileType<BlessedGlaiveProjectile>();

        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Emerald, 5)
            .AddIngredient(ItemID.GoldBar, 7)
            .AddIngredient(ModContent.ItemType<CursedGoldFragment>(), 30)
            .AddIngredient(ItemID.Spear)
            .AddTile<Tiles.TorchGodsBrazier>()
            .AddCondition(new Condition("Favor used", () => Main.LocalPlayer.unlockedBiomeTorches))
            .AddCondition(new Condition("Cursed", () => BookUsed.isPlayerCursed))
            .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.ownedProjectileCounts[type] == 0)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer);
            }
            
            return false;
        }

        public override bool MeleePrefix()
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaivePierce, player.position);
			return true;
		}
    }
}