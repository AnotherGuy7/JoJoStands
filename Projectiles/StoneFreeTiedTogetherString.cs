using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StoneFreeTiedTogetherString : ModProjectile
    {
        public override string Texture => mod.Name + "/Projectiles/PlayerStands/StandPlaceholder";

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        private int linkWhoAmI = -1;
        private NPC heldNPC;
        private bool living = true;
        private float previousDistance = 0f;
        private int strangleTimer = 0;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Projectile ownerProj = Main.projectile[(int)projectile.ai[0]];
            if (projectile.ai[0] < 0 || projectile.ai[0] >= Main.maxProjectiles || !ownerProj.active)
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
            float distance = Vector2.Distance(player.Center, projectile.Center);

            if (heldNPC != null)
            {
                if (!heldNPC.active || distance > 30f * 16f || !Main.mouseRight)
                {
                    living = false;
                    heldNPC = null;
                    return;
                }

                projectile.timeLeft = 300;
                projectile.position = heldNPC.Center - new Vector2(projectile.width / 2f, projectile.height / 2f);

                float mouseDistance = heldNPC.Distance(Main.MouseWorld);
                if (mouseDistance > previousDistance)
                {
                    Vector2 pullVelocity = projectile.Center - ownerProj.Center;
                    pullVelocity.Normalize();
                    pullVelocity *= (previousDistance - mouseDistance) / 128f;
                    heldNPC.velocity += pullVelocity;
                }
                previousDistance = mouseDistance;

                strangleTimer++;
                if (strangleTimer >= 60)
                {
                    strangleTimer = 0;
                    int strangleDamage = (int)(((30f * 16f) - (ownerProj.Distance(heldNPC.Center))) / (30f * 16f)) * 64;
                    heldNPC.StrikeNPC(strangleDamage, 0f, -heldNPC.direction);
                }
            }

            if (living)
            {
                if (distance > 30f * 16f)
                    living = false;
            }
            else if (!living)
            {
                projectile.tileCollide = false;
                Vector2 returnVel = player.Center - projectile.Center;
                returnVel.Normalize();
                returnVel *= 9f;
                projectile.velocity = returnVel;

                if (distance < 50f)
                    projectile.Kill();
            }
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (heldNPC == null)
                heldNPC = target;

            projectile.damage = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}