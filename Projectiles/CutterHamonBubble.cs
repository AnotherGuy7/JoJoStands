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
            hamonLossCounter++;
            if (projectile.timeLeft <= 550 && Main.mouseRight && Main.LocalPlayer.GetModPlayer<MyPlayer>().HamonCounter >= 1)
            {
                beingControlled = true;
            }
            if (beingControlled)
            {
                hamonLossCounter++;
                if (projectile.position.X <= Main.MouseWorld.X)        //if it's more to the right, go left
                {
                    projectile.velocity.X = 5f;
                }
                else if (projectile.position.X >= Main.MouseWorld.X)       //if it's more to the left, stay the same...
                {
                    projectile.velocity.X = -5f;
                }
                if (projectile.position.Y >= Main.MouseWorld.Y)       //if it's lower, go up to it
                {
                    projectile.velocity.Y = -5f;
                }
                else if (projectile.position.Y <= Main.MouseWorld.Y)      //if it's higher, go down to it
                {
                    projectile.velocity.Y = 5f;
                }
            }
            if (hamonLossCounter >= 120)
            {
                hamonLossCounter = 0;
                Main.LocalPlayer.GetModPlayer<MyPlayer>().HamonCounter -= 1;
            }
            if (Main.LocalPlayer.GetModPlayer<MyPlayer>().HamonCounter <= 1)
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