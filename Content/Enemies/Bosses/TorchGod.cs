using Microsoft.Xna.Framework;
using MurphysMod.Content.LuckHandlers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Enemies
{
    public class TorchGod : GlobalProjectile
    {

        public override void PostAI(Projectile projectile)
        {
            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            float luckVal = luckHandler.luckValue();
            float rand = Main.rand.Next(0, 101);


            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];


                if (proj.type == ProjectileID.TorchGod)
                {
                    proj.tileCollide = false;


                    if (luckVal >= 1f)
                    {
                        projectile.tileCollide = true;
                        proj.aiStyle = modifyTorchProjectile(luckVal);
                        proj.velocity *= 1.00025f;
                    }
                    else if (luckVal >= .75f)
                    {
                        proj.velocity *= 1.0002f;
                    }
                    else if (luckVal >= .5f)
                    {
                        proj.velocity *= 1.00015f;
                    }
                    else if (luckVal >= .25f)
                    {
                        proj.velocity *= 1.0001f;
                    }
                }
            }
        }

        public int modifyTorchProjectile(float luckVal)
        {
            int projectileNum = 0;
            int rand = Main.rand.Next(0, 101);

            if (luckVal >= 1f)
            {
                if (rand > 97)
                {
                    projectileNum = ProjectileID.InfernoHostileBlast;
                }
                else if (rand > 90)
                {
                    projectileNum = ProjectileID.BouncyGrenade;
                }
                else if (rand > 85)
                {
                    projectileNum = ProjectileID.DD2BetsyFireball;
                }

            }
            else if (luckVal >= .75f)
            {

            }
            else if (luckVal >= .5f)
            {

            }
            else if (luckVal >= .25f)
            {

            }

            return projectileNum;
        }
    }
}