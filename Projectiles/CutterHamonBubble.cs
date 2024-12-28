using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class CutterHamonBubble : ModProjectile
    {
        private int hamonLossCounter = 0;
        private bool canBeControlled = true;

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.maxPenetrate = 3;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            hamonLossCounter++;
            if (Main.mouseRight && canBeControlled && Projectile.owner == Main.myPlayer)
            {
                hamonLossCounter++;
                Projectile.velocity = Main.MouseWorld - Projectile.Center;
                Projectile.velocity.Normalize();
                Projectile.velocity *= 9f;
                Projectile.netUpdate = true;
            }
            if (hamonLossCounter >= 120)
            {
                hamonLossCounter = 0;
                hamonPlayer.amountOfHamon -= 1;
            }
            if (hamonPlayer.amountOfHamon <= 1)
            {
                if (Projectile.timeLeft > 60)
                    Projectile.timeLeft = 60;
                Projectile.timeLeft--;
                canBeControlled = false;
            }
            else
            {
                int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.IchorTorch, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                Main.dust[dustIndex].noGravity = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.scale = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f);
        }
    }
}