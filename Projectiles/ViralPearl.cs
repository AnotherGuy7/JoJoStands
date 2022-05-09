using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class ViralPearl : ModProjectile
    {
        private int counter = 0;

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            counter++;
            if (counter < 120)
            {
                Projectile.velocity.Y = -1f;
            }
            else
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 105);
                Projectile.position.X += (float)Math.Sin(counter * 180);
                Projectile.velocity = Vector2.Zero;
                if (counter >= 240)
                {
                    Projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.owner == Main.myPlayer)
            {
                int Item = 0;
                if (!player.ZoneDungeon)
                {
                    if (Main.rand.Next(0, 101) <= 90)
                    {
                        Item = Item.NewItem(Projectile.getRect(), ModContent.ItemType<ViralPearl>());
                    }
                    else
                    {
                        Item = Item.NewItem(Projectile.getRect(), ModContent.ItemType<CrackedPearl>());
                    }
                }
                else
                {
                    Item = Item.NewItem(Projectile.getRect(), ModContent.ItemType<EctoPearl>());
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && Item >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, Item, 1f);
                }
            }
        }
    }
}