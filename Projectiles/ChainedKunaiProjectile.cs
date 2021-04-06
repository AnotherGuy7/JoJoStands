using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class ChainedKunaiProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 10;
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
            Vector2 rota = player.Center - projectile.Center;
            projectile.rotation = (-rota).ToRotation();
            Vector2 vector14 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num166 = player.position.X + (float)(player.width / 2) - vector14.X;
            float num167 = player.position.Y + (float)(player.height / 2) - vector14.Y;
            float num168 = (float)Math.Sqrt((double)(num166 * num166 + num167 * num167));
            if (living)
            {
                if (num168 > 20f * 16f)
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
                if (num168 < 50f)
                {
                    projectile.Kill();
                }
                num168 = num169 / num168;
                num166 *= num168;
                num167 *= num168;
                projectile.velocity.X = num166;
                projectile.velocity.Y = num167;
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

        private Vector2 offset;
        private Texture2D stringPartTexture;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];

            if (Main.netMode != NetmodeID.Server && stringPartTexture == null)
                stringPartTexture = mod.GetTexture("Projectiles/ChainedKunai_StringPart");

            Vector2 linkCenter = player.Center + offset;
            Vector2 center = projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / stringPartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(stringPartTexture, pos, new Rectangle(0, 0, stringPartTexture.Width, stringPartTexture.Height), lightColor, rotation, new Vector2(stringPartTexture.Width * 0.5f, stringPartTexture.Height * 0.5f), projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}