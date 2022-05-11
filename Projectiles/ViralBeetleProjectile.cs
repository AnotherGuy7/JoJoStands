using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class ViralBeetleProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 360;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 3;
        }

        private bool changedPos = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            //player.ownedProjectileCounts[ModContent.ProjectileType<ViralBeetleProjectile>()]
            if (player.HasBuff(ModContent.BuffType<ViralBeetleBuff>()))
            {
                Projectile.timeLeft = 10;
            }
            float radius = Projectile.ai[0] * 24f;     //Radius, in which ai[0] is it's spawn number
            if (Projectile.ai[0] != 2f)
            {
                Projectile.ai[1] += 0.07f;     //Rotation
            }
            else
            {
                Projectile.ai[1] -= 0.07f;
            }
            if (Projectile.ai[0] == 3f && !changedPos)
            {
                Projectile.ai[1] += 36f;
                changedPos = true;
            }
            Vector2 offset = player.Center + (Projectile.ai[1].ToRotationVector2() * radius) + new Vector2(-6f, 0f);
            Projectile.position = offset;
            Lighting.AddLight(Projectile.Center, 2.55f / 3f, 2.14f / 3f, 0.88f / 3f);

            if (Projectile.position.X > player.position.X)
            {
                Projectile.direction = 1;
            }
            else
            {
                Projectile.direction = -1;
            }
            Projectile.spriteDirection = Projectile.direction;

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile otherProj = Main.projectile[p];
                if (otherProj.active)
                {
                    if (Projectile.Hitbox.Intersects(otherProj.Hitbox) && otherProj.type != Projectile.type && (otherProj.hostile || !otherProj.friendly))
                    {
                        otherProj.velocity *= -1f;
                        otherProj.penetrate -= 1;
                        Projectile.penetrate -= 1;
                    }
                }
            }

            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 15)
            {
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
                Projectile.frameCounter = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/ViralBeetleProjectile>().Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.direction == -1)
            {
                effects = SpriteEffects.FlipVertically;
            }
            Main.EntitySpriteDraw(texture, Projectile.position - Main.screenPosition, new Rectangle(0, Projectile.frame, Projectile.width, frameHeight), Color.White, Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, effects, 0f);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d < 10; d++)
            {
                Main.dust[Dust.NewDust(Projectile.Center + new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)), Projectile.width, Projectile.height, 232)].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
        }
    }
}