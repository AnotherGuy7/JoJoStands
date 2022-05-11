using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class HermitPurpleWhip : ModProjectile
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
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private bool living = true;

        public override void AI()       //all this so that the other chain doesn't draw... yare yare. It was mostly just picking out types
        {
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);     //aiStyle 13 without the types
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
            //Projectile.spriteDirection = Projectile.direction;
            Vector2 rota = player.Center - Projectile.Center;
            Projectile.rotation = (-rota).ToRotation();
            if (Projectile.alpha == 0)
            {
                if (Projectile.position.X + (float)(Projectile.width / 2) > player.position.X + (float)(player.width / 2))
                {
                    player.ChangeDir(1);
                }
                else
                {
                    player.ChangeDir(-1);
                }
            }
            Vector2 projectileCenter = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
            float playerDifferenceX = player.position.X + (float)(player.width / 2) - projectileCenter.X;
            float playerDifferenceY = player.position.Y + (float)(player.height / 2) - projectileCenter.Y;
            float distance = (float)Math.Sqrt((double)(playerDifferenceX * playerDifferenceX + playerDifferenceY * playerDifferenceY));
            if (living)
            {
                if (distance > 120f)
                {
                    living = false;
                }
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] > 5f)
                {
                    Projectile.alpha = 0;
                }
                if (Projectile.ai[1] >= 10f)
                {
                    Projectile.ai[1] = 15f;
                }
            }
            else if (!living)
            {
                Projectile.tileCollide = false;
                float num169 = 20f;
                if (distance < 50f)
                {
                    Projectile.Kill();
                }
                distance = num169 / distance;
                playerDifferenceX *= distance;
                playerDifferenceY *= distance;
                Projectile.velocity.X = playerDifferenceX;
                Projectile.velocity.Y = playerDifferenceY;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        private Texture2D hermitPurpleVinePartTexture;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            if (Main.netMode != NetmodeID.Server)
                hermitPurpleVinePartTexture = Mod.Assets.Request<Texture2D>("Projectiles/HermitPurpleVine_Part").Value;

            Vector2 offset = new Vector2(12f * player.direction, 0f);
            Vector2 linkCenter = player.Center + offset;
            Vector2 center = Projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / hermitPurpleVinePartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                Main.EntitySpriteDraw(hermitPurpleVinePartTexture, pos, new Rectangle(0, 0, hermitPurpleVinePartTexture.Width, hermitPurpleVinePartTexture.Height), lightColor, rotation, new Vector2(hermitPurpleVinePartTexture.Width * 0.5f, hermitPurpleVinePartTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}