using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StoneFreeBindString : ModProjectile
    {
        public override string Texture => mod.Name + "/Projectiles/PlayerStands/StandPlaceholder";

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 0;
            projectile.timeLeft = 360;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        private NPC boundNPC;
        private bool boundToNPC = false;

        public override void AI()
        {
            Projectile ownerProj = Main.projectile[(int)projectile.ai[0]];
            if (boundToNPC)
            {
                if (!ownerProj.active || (boundNPC == null || boundNPC.life <= 0))
                {
                    projectile.Kill();
                    return;
                }

                projectile.Center = boundNPC.Center;
                return;
            }

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
            ownerProj.spriteDirection = ownerProj.direction;
            Vector2 rota = ownerProj.Center - projectile.Center;
            projectile.rotation = (-rota * projectile.direction).ToRotation();

            if (projectile.owner == Main.myPlayer)
            {
                float distance = projectile.Distance(Main.MouseWorld);
                if (distance >= 16f)
                {
                    projectile.velocity = Main.MouseWorld - projectile.Center;
                    projectile.velocity.Normalize();
                    projectile.velocity *= 24f;
                }
                else
                {
                    projectile.velocity = Vector2.Zero;
                }
                projectile.netUpdate = true;
            }

            if (!ownerProj.active)
            {
                projectile.Kill();
                return;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.active && target.life > 0 && !target.hide && !target.immortal)
            {
                boundNPC = target;
                boundToNPC = true;
                boundNPC.GetGlobalNPC<NPCs.JoJoGlobalNPC>().boundByStrings = true;
                projectile.damage = 0;
                projectile.timeLeft = (int)projectile.ai[1] * 60;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (boundToNPC && boundNPC != null && boundNPC.life > 0)
                boundNPC.GetGlobalNPC<NPCs.JoJoGlobalNPC>().boundByStrings = false;
        }

        private Texture2D stringTexture;
        private Color drawColor;
        private Rectangle stringSourceRect;
        private Vector2 stringOrigin;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (stringTexture == null)
            {
                stringTexture = mod.GetTexture("Projectiles/StoneFreeString_Part");
                stringSourceRect = new Rectangle(0, 0, stringTexture.Width, stringTexture.Height);
                stringOrigin = new Vector2(stringTexture.Width * 0.5f, stringTexture.Height * 0.5f);
            }

            Projectile ownerProj = Main.projectile[(int)projectile.ai[0]];
            Vector2 linkCenter = ownerProj.Center + new Vector2(12f * ownerProj.direction, 0f);
            Vector2 projectileCenter = projectile.Center;
            drawColor = lightColor;

            float stringRotation = (linkCenter - projectileCenter).ToRotation();
            float stringScale = 0.6f;
            float loopIncrement = 1 / (Vector2.Distance(projectileCenter, linkCenter) / (stringTexture.Width * stringScale));
            float lightLevelIndex = 0f;
            for (float k = 0; k <= 1; k += loopIncrement)     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                lightLevelIndex += loopIncrement;
                Vector2 pos = Vector2.Lerp(projectileCenter, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                if (lightLevelIndex >= 0.1f)        //Gets new light levels every 10% of the string.
                {
                    drawColor = Lighting.GetColor((int)(pos.X + Main.screenPosition.X) / 16, (int)(pos.Y + Main.screenPosition.Y) / 16);
                    lightLevelIndex = 0f;
                }

                spriteBatch.Draw(stringTexture, pos, stringSourceRect, drawColor, stringRotation, stringOrigin, projectile.scale * stringScale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}