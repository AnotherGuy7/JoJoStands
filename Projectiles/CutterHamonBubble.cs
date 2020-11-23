using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class CutterHamonBubble : ModProjectile
    {
        public int hamonLossCounter = 0;
        public bool beingControlled = false;
        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 18;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 2;
            projectile.maxPenetrate = 3;
        }

        public override void AI()       //make them very slightly controllable
        {
            Player player = Main.player[projectile.owner];
            Items.Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Items.Hamon.HamonPlayer>();
            hamonLossCounter++;
            if (projectile.timeLeft <= 550 && Main.mouseRight && hamonPlayer.HamonCounter >= 1 && player.whoAmI == Main.myPlayer)
            {
                beingControlled = true;
            }
            if (beingControlled)
            {
                hamonLossCounter++;
                if (projectile.owner == Main.myPlayer)
                {
                    projectile.velocity = Main.MouseWorld - projectile.Center;
                    projectile.velocity.Normalize();
                    projectile.velocity *= 10f;
                }
                projectile.netUpdate = true;
            }
            if (hamonLossCounter >= 120)
            {
                hamonLossCounter = 0;
                hamonPlayer.HamonCounter -= 1;
            }
            if (hamonPlayer.HamonCounter <= 1)
            {
                beingControlled = false;
                projectile.timeLeft--;
            }
            else
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 169, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            }
        }
    }
}