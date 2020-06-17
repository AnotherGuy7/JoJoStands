using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ViralPearl : ModProjectile
    {
        private int counter;
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
            if (counter <= 120)
            {
                projectile.velocity.Y = -1f;
            }
            if (counter <= 240 && counter >= 120)
            {
                Dust.NewDust(projectile.Center, 1, 1, 105);
                projectile.position.X += (float)Math.Sin(counter * 180);
                projectile.velocity = Vector2.Zero;
            }
            if (counter >= 240)
            {
                projectile.Kill();
            }
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Main.myPlayer];
            if (!player.ZoneDungeon)
            {
                if (Main.rand.Next(0, 101) <= 90)
                {
                    Item.NewItem(projectile.getRect(), mod.ItemType("ViralPearl"));
                }
                else
                {
                    Item.NewItem(projectile.getRect(), mod.ItemType("CrackedPearl"));
                }
            }
            else
            {
                Item.NewItem(projectile.getRect(), mod.ItemType("EctoPearl"));
            }
        }
    }
}