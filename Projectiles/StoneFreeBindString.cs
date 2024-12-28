using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StoneFreeBindString : ModProjectile
    {
        public override string Texture => Mod.Name + "/Extras/EmptyTexture";
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 360;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private NPC boundNPC;
        private bool boundToNPC = false;

        public override void AI()
        {
            Projectile ownerProj = Main.projectile[(int)Projectile.ai[0]];
            if (boundToNPC)
            {
                if (!ownerProj.active || (boundNPC == null || boundNPC.life <= 0))
                {
                    Projectile.Kill();
                    return;
                }

                Projectile.Center = boundNPC.Center;
                return;
            }

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
                if (distance >= 16f)
                {
                    Projectile.velocity = Main.MouseWorld - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 24f;
                }
                else
                    Projectile.velocity = Vector2.Zero;
                Projectile.netUpdate = true;
            }

            if (!ownerProj.active)
            {
                Projectile.Kill();
                return;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.active && target.life > 0 && !target.hide && !target.immortal)
            {
                boundNPC = target;
                boundToNPC = true;
                boundNPC.GetGlobalNPC<JoJoGlobalNPC>().boundByStrings = true;
                if (Projectile.owner == Main.myPlayer)
                    boundNPC.GetGlobalNPC<JoJoGlobalNPC>().SyncEffect(JoJoGlobalNPC.Sync_BoundByStrings);
                Projectile.damage = 0;
                Projectile.timeLeft = (int)Projectile.ai[1] * 60;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (boundToNPC && boundNPC != null && boundNPC.life > 0)
            {
                boundNPC.GetGlobalNPC<JoJoGlobalNPC>().boundByStrings = false;
                if (Projectile.owner == Main.myPlayer)
                    boundNPC.GetGlobalNPC<JoJoGlobalNPC>().SyncEffect(JoJoGlobalNPC.Sync_BoundByStrings);
            }
        }

        private Texture2D stringTexture;
        private Color drawColor;
        private Rectangle stringSourceRect;
        private Vector2 stringOrigin;

        public override bool PreDraw(ref Color lightColor)
        {
            if (stringTexture == null)
            {
                stringTexture = ModContent.Request<Texture2D>("JoJoStands/Projectiles/StoneFreeString_Part").Value;
                stringSourceRect = new Rectangle(0, 0, stringTexture.Width, stringTexture.Height);
                stringOrigin = new Vector2(stringTexture.Width * 0.5f, stringTexture.Height * 0.5f);
            }

            Projectile ownerProj = Main.projectile[(int)Projectile.ai[0]];
            Vector2 linkCenter = ownerProj.Center + new Vector2(12f * ownerProj.direction, 0f);
            Vector2 projectileCenter = Projectile.Center;
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

                Main.EntitySpriteDraw(stringTexture, pos, stringSourceRect, drawColor, stringRotation, stringOrigin, Projectile.scale * stringScale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}