using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ViralBeetleProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = 0;
            projectile.timeLeft = 360;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = 3;
        }

        private bool changedPos = false;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            //player.ownedProjectileCounts[mod.ProjectileType("ViralBeetleProjectile")]
            if (player.HasBuff(mod.BuffType("ViralBeetleBuff")))
            {
                projectile.timeLeft = 10;
            }
            float radius = projectile.ai[0] * 24f;     //Radius, in which ai[0] is it's spawn number
            if (projectile.ai[0] != 2f)
            {
                projectile.ai[1] += 0.07f;     //Rotation
            }
            else
            {
                projectile.ai[1] -= 0.07f;
            }
            if (projectile.ai[0] == 3f && !changedPos)
            {
                projectile.ai[1] += 36f;
                changedPos = true;
            }
            Vector2 offset = player.Center + (projectile.ai[1].ToRotationVector2() * radius) + new Vector2(-6f, 0f);
            projectile.position = offset;
            Lighting.AddLight(projectile.Center, 2.55f / 3f, 2.14f / 3f, 0.88f / 3f);

            if (projectile.position.X > player.position.X)
            {
                projectile.direction = 1;
            }
            else
            {
                projectile.direction = -1;
            }
            projectile.spriteDirection = projectile.direction;

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile otherProj = Main.projectile[p];
                if (otherProj.active)
                {
                    if (projectile.Hitbox.Intersects(otherProj.Hitbox) && otherProj.type != projectile.type && (otherProj.hostile || !otherProj.friendly))
                    {
                        otherProj.velocity *= -1f;
                        otherProj.penetrate -= 1;
                        projectile.penetrate -= 1;
                    }
                }
            }

            if (projectile.penetrate <= 0)
            {
                projectile.Kill();
            }

            projectile.frameCounter++;
            if (projectile.frameCounter >= 15)
            {
                projectile.frame++;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
                projectile.frameCounter = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/ViralBeetleProjectile");
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.direction == -1)
            {
                effects = SpriteEffects.FlipVertically;
            }
            spriteBatch.Draw(texture, projectile.position - Main.screenPosition, new Rectangle(0, projectile.frame, projectile.width, frameHeight), Color.White, projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), projectile.scale, effects, 0f);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d < 10; d++)
            {
                Main.dust[Dust.NewDust(projectile.Center + new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)), projectile.width, projectile.height, 232)].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("Infected"), 10 * 60);
        }
    }
}