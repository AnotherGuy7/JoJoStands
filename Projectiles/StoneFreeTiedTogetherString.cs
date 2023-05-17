using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StoneFreeTiedTogetherString : ModProjectile
    {
        public override string Texture => Mod.Name + "/Extras/EmptyTexture";

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private NPC heldNPC;
        private bool living = true;
        private float previousDistance = 0f;
        private int strangleTimer = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile ownerProj = Main.projectile[(int)Projectile.ai[0]];
            if (Projectile.ai[0] < 0 || Projectile.ai[0] >= Main.maxProjectiles || !ownerProj.active)
            {
                Projectile.Kill();
                return;
            }


            float direction = player.Center.X - Projectile.Center.X;
            if (direction > 0)
            {
                Projectile.direction = -1;
                player.direction = -1;
                ownerProj.direction = -1;
            }
            if (direction < 0)
            {
                Projectile.direction = 1;
                player.direction = 1;
                ownerProj.direction = 1;
            }
            Vector2 rota = player.Center - Projectile.Center;
            Projectile.rotation = (-rota).ToRotation();
            float distance = Vector2.Distance(player.Center, Projectile.Center);

            if (heldNPC != null)
            {
                if (!heldNPC.active || distance > 30f * 16f || !Main.mouseRight)
                {
                    living = false;
                    heldNPC = null;
                    return;
                }

                Projectile.timeLeft = 300;
                Projectile.position = heldNPC.Center - new Vector2(Projectile.width / 2f, Projectile.height / 2f);

                float mouseDistance = heldNPC.Distance(Main.MouseWorld);
                if (mouseDistance > previousDistance)
                {
                    Vector2 pullVelocity = Projectile.Center - ownerProj.Center;
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
                    NPC.HitInfo hitInfo = new NPC.HitInfo()
                    {
                        Damage = strangleDamage,
                        HitDirection = -heldNPC.direction
                    };
                    heldNPC.StrikeNPC(hitInfo);
                }
            }

            if (living)
            {
                if (distance > 30f * 16f)
                    living = false;
            }
            else if (!living)
            {
                Projectile.tileCollide = false;
                Vector2 returnVel = player.Center - Projectile.Center;
                returnVel.Normalize();
                returnVel *= 9f;
                Projectile.velocity = returnVel;

                if (distance < 50f)
                    Projectile.Kill();
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
                stringTexture = ModContent.Request<Texture2D>("JoJoStands/Projectiles/StoneFreeString_Part", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (heldNPC == null)
                heldNPC = target;

            Projectile.damage = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}