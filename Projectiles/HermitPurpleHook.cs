using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
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

        private const float DistanceLimit = 34f * 16f;

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
                {
                    distanceLimitReached = true;
                }

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
                velocity *= 9f;
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
                velocity *= 9f;
                player.velocity = velocity;
            }
        }

        private Texture2D hermitPurpleVinePartTexture;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            if (Main.netMode != NetmodeID.Server)
                hermitPurpleVinePartTexture = Mod.GetTexture("Projectiles/HermitPurpleVine_Part");

            Vector2 offset = new Vector2(12f * player.direction, 0f);
            Vector2 linkCenter = player.Center + offset;
            Vector2 center = Projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / hermitPurpleVinePartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(hermitPurpleVinePartTexture, pos, new Rectangle(0, 0, hermitPurpleVinePartTexture.Width, hermitPurpleVinePartTexture.Height), lightColor, rotation, new Vector2(hermitPurpleVinePartTexture.Width * 0.5f, hermitPurpleVinePartTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}