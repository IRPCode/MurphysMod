using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace MurphysMod.Content.Buffs
{
	public class Augury : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}


		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = "Adds a small amount of bad luck.";
		}
	}
}