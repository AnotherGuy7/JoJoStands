using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class RedBind : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.timeLeft = 360;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public override void AI()       //all this so that the other chain doesn't draw... yare yare. It was mostly just picking out types
        {
            Projectile ownerProj = Main.projectile[(int)projectile.ai[0]];
            float direction = ownerProj.Center.X - projectile.Center.X;
            if (direction > 0)
            {
                projectile.direction = -1;
                ownerProj.direction = -1;
            }
            if (direction < 0)
            {
                projectile.direction = 1;
                ownerProj.direction = 1;
            }
            projectile.spriteDirection = projectile.direction;
            Vector2 rota = ownerProj.Center - projectile.Center;
            projectile.rotation = (-rota * projectile.direction).ToRotation();

            if (projectile.owner == Main.myPlayer)
            {
                projectile.velocity = Main.MouseWorld - projectile.Center;
                projectile.velocity.Normalize();
                projectile.velocity *= 10f;
                projectile.netUpdate = true;
            }

            if (!ownerProj.active)
            {
                projectile.Kill();
                return;
            }
            if (projectile.alpha == 0)
            {
                if (projectile.position.X + (float)(projectile.width / 2) > ownerProj.position.X + (float)(ownerProj.width / 2))
                {
                    ownerProj.direction = ownerProj.spriteDirection = 1;
                }
                else
                {
                    ownerProj.direction = ownerProj.spriteDirection = -1;
                }
            }
            //projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("RedBindDebuff"), (int)projectile.ai[1]);
            projectile.Kill();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)     //once again, TMOd help-with-code saves the day (Scalie)
        {
            Projectile ownerProj = Main.projectile[(int)projectile.ai[0]];
            Vector2 ownerCenterOffset = Vector2.Zero;
            if (ownerProj.spriteDirection == -1)
            {
                ownerCenterOffset = new Vector2(-16f, -10f);
            }
            if (ownerProj.spriteDirection == 1)
            {
                ownerCenterOffset = new Vector2(4f, -4.5f);
            }
            Vector2 ownerCenter = ownerProj.Center + ownerCenterOffset;
            Vector2 center = projectile.Center + new Vector2(0f, -1f);
            Texture2D texture = mod.GetTexture("Projectiles/RedBind_Part");
            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, ownerCenter) / texture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, ownerCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height), lightColor, projectile.rotation, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}