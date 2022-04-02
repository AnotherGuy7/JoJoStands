using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ViralPearl : ModProjectile
    {
        private int counter = 0;

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            counter++;
            if (counter < 120)
            {
                projectile.velocity.Y = -1f;
            }
            else
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 105);
                projectile.position.X += (float)Math.Sin(counter * 180);
                projectile.velocity = Vector2.Zero;
                if (counter >= 240)
                {
                    projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.owner == Main.myPlayer)
            {
                int item = 0;
                if (!player.ZoneDungeon)
                {
                    if (Main.rand.Next(0, 101) <= 90)
                    {
                        item = Item.NewItem(projectile.getRect(), mod.ItemType("ViralPearl"));
                    }
                    else
                    {
                        item = Item.NewItem(projectile.getRect(), mod.ItemType("CrackedPearl"));
                    }
                }
                else
                {
                    item = Item.NewItem(projectile.getRect(), mod.ItemType("EctoPearl"));
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
                }
            }
        }
    }
}