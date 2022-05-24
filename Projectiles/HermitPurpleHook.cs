using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HermitPurpleHook : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/HermitPurpleVine_End"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 12;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        private const float DistanceLimit = 36f * 16f;

        private bool distanceLimitReached = false;
        private bool attachedToTile = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            float direction = player.Center.X - Projectile.Center.X;
            if (direction > 0)
            {
                Projectile.direction = -1;
                player.ChangeDir(-1);
            }
            if (direction < 0)
            {
                Projectile.direction = 1;
                player.ChangeDir(1);
            }

            if (!attachedToTile && !distanceLimitReached)
            {
                float distance = Projectile.Distance(player.Center);
                if (distance >= DistanceLimit)
                    distanceLimitReached = true;

                if (Collision.SolidCollision(Projectile.Center, 1, 1))
                {
                    attachedToTile = true;
                    Projectile.velocity = Vector2.Zero;
                }
            }
            if (distanceLimitReached)
            {
                attachedToTile = false;
                Projectile.tileCollide = false;

                float distance = Projectile.Distance(player.Center);
                if (distance <= 20f)
                {
                    Projectile.Kill();
                    return;
                }

                Vector2 velocity = player.position - Projectile.position;
                velocity.Normalize();
                velocity *= 16f;
                Projectile.velocity = velocity;

            }

            if (attachedToTile)
            {
                if (player.controlJump)
                {
                    Projectile.Kill();
                    return;
                }

                Vector2 velocity = Projectile.position - player.position;
                velocity.Normalize();
                velocity *= 12f;
                player.velocity = velocity;
            }
        }

        private Texture2D hermitPurpleVinePartTexture;

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            if (Main.netMode != NetmodeID.Server && hermitPurpleVinePartTexture == null)
                hermitPurpleVinePartTexture = ModContent.Request<Texture2D>("JoJoStands/Projectiles/HermitPurpleVine_Part", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            Vector2 offset = new Vector2(12f * player.direction, 0f);
            Vector2 linkCenter = player.Center + offset;
            Vector2 center = Projectile.Center;
            float rotation = (linkCenter - center).ToRotation();
            float loopIncrement = 1 / (Vector2.Distance(center, linkCenter) / hermitPurpleVinePartTexture.Width);
            float lightLevelIndex = 0f;
            Color drawColor = lightColor;

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / hermitPurpleVinePartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                lightLevelIndex += loopIncrement;
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;
                if (lightLevelIndex >= 0.1f)
                {
                    drawColor = Lighting.GetColor((int)(pos.X + Main.screenPosition.X) / 16, (int)(pos.Y + Main.screenPosition.Y) / 16);
                    lightLevelIndex = 0f;
                }

                Main.EntitySpriteDraw(hermitPurpleVinePartTexture, pos, new Rectangle(0, 0, hermitPurpleVinePartTexture.Width, hermitPurpleVinePartTexture.Height), drawColor, rotation, new Vector2(hermitPurpleVinePartTexture.Width * 0.5f, hermitPurpleVinePartTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}