using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class MetallicNunchucksProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 6;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        private bool living = true;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (Main.player[projectile.owner].dead)
            {
                projectile.Kill();
                return;
            }

            Vector2 rota = player.Center - projectile.Center;
            projectile.rotation = (-rota).ToRotation();
            float direction = player.Center.X - projectile.Center.X;
            if (direction > 0)
            {
                projectile.direction = -1;
                player.direction = -1;
            }
            if (direction < 0)
            {
                projectile.direction = 1;
                player.direction = 1;
            }

            float diffX = player.Center.X - projectile.Center.X;
            float diffY = player.Center.Y - projectile.Center.Y;
            float distanceFromPlayer = (float)Math.Sqrt((double)(diffX * diffX + diffY * diffY));
            if (living)
            {
                if (distanceFromPlayer > 5f * 16f)
                {
                    living = false;
                }
                //projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
                projectile.ai[1] += 1f;
                if (projectile.ai[1] > 5f)
                {
                    projectile.alpha = 0;
                }
                if (projectile.ai[1] >= 10f)
                {
                    projectile.ai[1] = 15f;
                }
            }
            else if (!living)
            {
                projectile.tileCollide = false;
                //projectile.rotation = (float)Math.Atan2((double)num167, (double)num166) - 1.57f;
                float num169 = 20f;
                if (distanceFromPlayer < 50f)
                {
                    projectile.Kill();
                }
                distanceFromPlayer = num169 / distanceFromPlayer;
                diffX *= distanceFromPlayer;
                diffY *= distanceFromPlayer;
                projectile.velocity.X = diffX;
                projectile.velocity.Y = diffY;
            }
            int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 169);
            Main.dust[dustIndex].noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            living = false;
            if (Main.rand.Next(0, 2) == 0)
            {
                target.AddBuff(mod.BuffType("Sunburn"), 12 * 60);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        private Texture2D chainTexture;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];

            if (Main.netMode != NetmodeID.Server && chainTexture == null)
                chainTexture = mod.GetTexture("Projectiles/ChainedClaw_Chain");

            Vector2 linkCenter = player.Center;
            Vector2 center = projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / chainTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(chainTexture, pos, new Rectangle(0, 0, chainTexture.Width, chainTexture.Height), lightColor, rotation, new Vector2(chainTexture.Width * 0.5f, chainTexture.Height * 0.5f), projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}