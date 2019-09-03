using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class KingCrimsonDonut : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 26;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.timeLeft = 35;
            projectile.penetrate = 99;
            projectile.ownerHitCheck = true;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 10)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
            }
            Player player = Main.player[projectile.owner];
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = Main.projFrames[projectile.type];
            }
            if (player.direction == 1)
            {
                projectile.position = player.Center - new Vector2(31f, 0f);
            }
            if (player.direction == -1)
            {
                projectile.position = player.Center;
            }
            projectile.spriteDirection = projectile.direction;
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
        }
    }
}