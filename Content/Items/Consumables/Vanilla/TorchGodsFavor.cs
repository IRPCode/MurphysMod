using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MurphysMod.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Items.Vanila
{
    public class TorchGodsFavor : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) //TODO: Take a look at this repo: https://github.com/TheFifthCircle/WrathOfTheGodsPublic/blob/main/Content/Rarities/NamelessDeityRarity.cs
        {
            if (item.type == ItemID.TorchGodsFavor)
            {
                tooltips.Clear();

                TooltipLine tip = new TooltipLine(Mod, "tooltip1", "Torch God's Favor") { OverrideColor = CustomRarity.getRarity(Color.White, new Color(255, 241, 145)) };
                TooltipLine tip2 = new TooltipLine(Mod, "tooltip2", "Unlocks an ability toggle to the left of the inventory");
                TooltipLine tip3 = new TooltipLine(Mod, "tooltip3", "When enabled normal torches change according to your biome");
                TooltipLine tip4 = new TooltipLine(Mod, "tooltip4", "Unlocks crafting at the Torch God's Brazier") { OverrideColor = CustomRarity.getRarity(new Color(47, 163, 255), new Color(254, 121, 2)) };

                tooltips.Add(tip);
                tooltips.Add(tip2);
                tooltips.Add(tip3);
                tooltips.Add(tip4);
            }
        }
    }
}