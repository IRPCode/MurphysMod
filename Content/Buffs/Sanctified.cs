using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using MurphysMod.Content.Ambience.Dusts;
using System.Linq;

namespace MurphysMod.Content.Buffs
{
	public class Sanctified : ModBuff
	{
		public override String Texture => "MurphysMod/Assets/Textures/BuffIcons/Sanctified";
		public override void SetStaticDefaults() //Check luckhandler.cs for luck modifications
		{
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = "You are drenched in the holiest of water.";
		}

		public override void Update(Player player, ref int buffIndex)
		{
			int rand = Main.rand.Next(0,11);
			player.AddBuff(BuffID.NightOwl, 1);

			if (rand == 0)
			{
				Dust.NewDust(new Vector2(player.position.X, player.position.Y), 16, 16, ModContent.DustType<BlessedWaterDust>(), 0, 0, 100, default, 1f);
			}

			
			player.GetModPlayer<SanctifiedPlayer>().Sanctified = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			int[] HallowedNPCs = { 75, 80, 84, 86, 120, 122, 137, 138, 171, 290, 475, 636, 657, 658, 659, 660, 661, 676, 677 }; //all hallowed npcs
			int[] UnholyNPCs = { 62, 66, 156, 158, 159, 162, 251, 253, 315, 316, 326, 329, 330, 351, 379, 380, 438, 441, 460, 461, 462, 463, 466, 467, 468, 469, 534, 662 }; //cursed / classical monsters

			if (HallowedNPCs.Contains(npc.type))
			{
				npc.lifeRegen += 50; //heals hallowed npcs significantly
			}

			else if (UnholyNPCs.Contains(npc.type))
			{
				npc.lifeRegen -= 100; //harms unholy npcs significantly
			}

			else if (npc.aiStyle == 7) //town NPCs, town pets, and critters
			{
				npc.lifeRegen += 5;
			}

			else
			{
				npc.lifeRegen -= 5;
			}

			int rand = Main.rand.Next(0,11);


			if (rand == 0)
			{
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 16, 16, ModContent.DustType<BlessedWaterDust>(), 0, 0, 100, default, 1f);
			}
        }
	}

	public class SanctifiedPlayer : ModPlayer{
		public bool Sanctified;
		public override void ResetEffects()
		{
			Sanctified = false;
		}
        public override void UpdateLifeRegen() //slightly boosts healing
		{
			if (Sanctified)
			{
				Player.lifeRegenTime++;
				Player.lifeRegen += 2;

			}
		}   
	}
}