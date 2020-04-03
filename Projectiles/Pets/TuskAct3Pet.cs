using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Pets
{
    public class TuskAct3Pet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 26;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (player.dead)
            {
                modPlayer.TuskAct3Pet = false;
            }
            if (modPlayer.TuskAct3Pet)
            {
                projectile.timeLeft = 2;
            }
            Vector2 vector131 = player.Center;
            vector131.X -= (float)((12 + player.width / 2) * player.direction);
            vector131.Y -= 25f;
            projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
            projectile.velocity *= 0.8f;
            projectile.direction = (projectile.spriteDirection = player.direction);
            if (projectile.frameCounter >= 10)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 4)
            {
                projectile.frame = 0;
            }
            projectile.netUpdate = true;
        }
    }
}