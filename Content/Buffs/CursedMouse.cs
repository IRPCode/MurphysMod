using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Mono.CompilerServices.SymbolWriter;
using MurphysMod.Content.Ambience;

namespace MurphysMod.Content.Buffs
{
	public class CursedMouse : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = "Removes your mouse cursor.";
		}
	}

	public class PoorAimHandler : ModSystem
	{
		public override void PreUpdatePlayers()
		{

			bool hasBuff = Main.LocalPlayer.GetModPlayer<playerHasBuff>().hasBuff();
			int tick = Tick.globalTick;

			if (hasBuff == true)
			{
				Main.cursorScale = 0;
			}
		}
	}

	public class playerHasBuff : ModPlayer
	{
		public bool hasBuff()
		{
			if (Player.HasBuff(ModContent.BuffType<CursedMouse>()))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}