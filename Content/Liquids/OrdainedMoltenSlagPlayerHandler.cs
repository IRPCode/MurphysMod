using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using MurphysMod.Content.Buffs;

public class OrdainedMoltenSlagPlayerHandler : ModPlayer
{

    public bool liquidVelCheck = false;
    public bool velocityType;
    public override void PostUpdate()
    {
        try
        {
            if (Main.myPlayer == Player.whoAmI && !Main.dedServ)
            {
                float x = Player.Center.X;
                float y = Player.Center.Y;

                Tile playerLoc = Framing.GetTileSafely((int)(x / 16), (int)(y / 16));

                if (playerLoc.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "OrdainedMoltenSlag").Type)
                {

                    if (Player.unlockedBiomeTorches)
                    {
                        Player.slowFall = true;
                        Player.velocity.Y = -15f;
                    }
                    else
                    {
                        Player.AddBuff(ModContent.BuffType<OrdainedFlames>(), 15);
                        Player.velocity.Y = -10f;
                        //Also all gravestone/headstone projectiles
                    }
                }

                //TODO: condense this section down into a singular for loop, and use if statements to determine what it is impacting.

                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    Tile projectileLoc = Framing.GetTileSafely((int)projectile.position.X / 16, (int)projectile.position.Y / 16);
                    if (projectile.active)
                    {
                        if (projectileLoc.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "OrdainedMoltenSlag").Type)
                        {
                            projectile.velocity.Y = Math.Abs(projectile.velocity.Y) * -1;

                            if (projectile.type == ProjectileID.Headstone || projectile.type == ProjectileID.Tombstone || projectile.type == ProjectileID.Obelisk || (projectile.type >= 527 && projectile.type <= 531))
                            {
                                projectile.Kill(); //TODO: currently doesn't kill tombstones.
                            }

                            if (Math.Abs(projectile.velocity.Y) < 1f)
                            {
                                projectile.Kill();
                            }
                        }
                    }
                }

                for (int i = 0; i < Main.maxItems; i++)
                {
                    Item item = Main.item[i];
                    Tile itemLoc = Framing.GetTileSafely((int)item.position.X / 16, (int)item.position.Y / 16);
                    if (item.active)
                    {
                        if (itemLoc.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "OrdainedMoltenSlag").Type)
                        {
                            item.velocity.Y = Math.Abs(item.velocity.Y) * -1.5f;

                            if (Math.Abs(item.velocity.Y) < 1f)
                            {
                                item.velocity += new Microsoft.Xna.Framework.Vector2(5, 0);
                            }
                        }
                    }
                }

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    Tile npcLoc = Framing.GetTileSafely((int)npc.position.X / 16, (int)npc.position.Y / 16);
                    if (npc.active)
                    {
                        if (npcLoc.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "OrdainedMoltenSlag").Type) //prevents boss deaths
                        {
                            npc.velocity.Y = -15f;
                            npc.AddBuff(ModContent.BuffType<OrdainedFlames>(), 300);
                        }
                    }
                }
            }
            if (Player.velocity.Y < -3f)
            {
                Player.slowFall = false;
            }
        }
        catch
        {
            Main.NewText("Error thrown due to MurphysMod OrdainedMoltenSlag physics mechanics.");
        }
    }
}