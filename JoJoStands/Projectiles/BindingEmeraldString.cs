using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class BindingEmeraldString : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 14;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.frameCounter += 1;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            projectile.rotation = projectile.velocity.ToRotation();
            if (Main.rand.Next(0, 1 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 61, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.noGravity)
                target.velocity.X = 0f;
            else
                target.velocity = Vector2.Zero;
            target.GetGlobalNPC<JoJoGlobalNPC>().stunnedByBindingEmerald = true;
            target.GetGlobalNPC<JoJoGlobalNPC>().bindingEmeraldDurationTimer = (int)projectile.ai[0] * 60;
        }
    }
}