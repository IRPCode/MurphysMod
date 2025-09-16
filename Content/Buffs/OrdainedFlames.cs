using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace MurphysMod.Content.Buffs
{
	public class OrdainedFlames : ModBuff
	{
		public override String Texture => "MurphysMod/Assets/Textures/BuffIcons/OrdainedFlames";
		public override void SetStaticDefaults() //TODO: make this debuff actually take health away
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = "You are melting away!";
		}

		public override void Update(Player player, ref int buffIndex)
		{
			Dust.NewDust(new Vector2(player.position.X, player.position.Y), 16, 16, DustID.SpelunkerGlowstickSparkle, 0, 0, 100, default, 1f);
			player.GetModPlayer<OrdainedFlamesPlayer>().OrdainedFlames = true;

		}

        public override void Update(NPC npc, ref int buffIndex)
        {
			npc.lifeRegen -= 15;
        }
	}

	public class OrdainedFlamesPlayer : ModPlayer{
		public bool OrdainedFlames;
		public override void ResetEffects()
		{
			OrdainedFlames = false;
		}
        public override void UpdateBadLifeRegen()
		{
			if (OrdainedFlames)
			{
				if (Player.lifeRegen > 0)
				{
					Player.lifeRegen = 0;
					Player.lifeRegenTime = 0;
				}
				Player.lifeRegen -= (int)(Player.statLifeMax / 5);
			}
		}   
	}
}