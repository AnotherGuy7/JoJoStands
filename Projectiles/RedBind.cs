using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class RedBind : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 360;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()       //all this so that the other chain doesn't draw... yare yare. It was mostly just picking out types
        {
            Projectile ownerProj = Main.projectile[(int)Projectile.ai[0]];
            float direction = ownerProj.Center.X - Projectile.Center.X;
            if (direction > 0)
            {
                Projectile.direction = -1;
                ownerProj.direction = -1;
            }
            if (direction < 0)
            {
                Projectile.direction = 1;
                ownerProj.direction = 1;
            }
            Projectile.spriteDirection = Projectile.direction;
            ownerProj.spriteDirection = ownerProj.direction;
            Vector2 rota = ownerProj.Center - Projectile.Center;
            Projectile.rotation = (-rota * Projectile.direction).ToRotation();

            if (Projectile.owner == Main.myPlayer)
            {
                float distance = Projectile.Distance(Main.MouseWorld);
                if (distance >= 20f)
                {
                    Projectile.velocity = Main.MouseWorld - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 10f;
                }
                else
                {
                    Projectile.velocity = Vector2.Zero;
                }
                Projectile.netUpdate = true;
            }

            if (!ownerProj.active || Main.mouseLeft)
            {
                Projectile.Kill();
                return;
            }
            //Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && !target.hide && !target.immortal)
                target.AddBuff(ModContent.BuffType<RedBindDebuff>(), (int)Projectile.ai[1]);

            Projectile.Kill();
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<RedBindDebuff>(), (int)Projectile.ai[1]);
            Projectile.Kill();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)     //once again, TMOd help-with-code saves the day (Scalie)
        {
            Projectile ownerProj = Main.projectile[(int)Projectile.ai[0]];
            Vector2 ownerCenterOffset = new Vector2(4f, -4.5f);
            if (ownerProj.spriteDirection == -1)
                ownerCenterOffset = new Vector2(-16f, -10f);

            Vector2 ownerCenter = ownerProj.Center + ownerCenterOffset;
            Vector2 center = Projectile.Center + new Vector2(0f, -1f);
            Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/RedBind_Part>().Value;
            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, ownerCenter) / texture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, ownerCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                Main.EntitySpriteDraw(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height), lightColor, Projectile.rotation, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}