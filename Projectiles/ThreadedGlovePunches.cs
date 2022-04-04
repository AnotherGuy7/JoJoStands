using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class ThreadedGlovePunches : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 48;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.friendly = true;
            drawOriginOffsetY = 20;
            projectile.timeLeft = 40;
        }

        private Texture2D punchSpritesheet;
        private bool checkedPunchType = false;
        private int amountOfFrames = 0;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (!checkedPunchType)
            {
                if (projectile.ai[0] < 2f)
                {
                    punchSpritesheet = mod.GetTexture("Projectiles/ThreadedGloves_Punch");
                    amountOfFrames = 4;
                }
                else
                {
                    if (Main.rand.Next(0, 2) == 0)
                    {
                        punchSpritesheet = mod.GetTexture("Projectiles/ThreadedGloves_Uppercut");
                    }
                    else
                    {
                        punchSpritesheet = mod.GetTexture("Projectiles/ThreadedGloves_Undercut");
                    }
                    amountOfFrames = 5;
                }
                checkedPunchType = true;
            }

            Vector2 centerOffset = new Vector2((player.width / 2f) * player.direction, -24f);
            if (player.direction == -1)
            {
                centerOffset.X -= 24f;
            }
            projectile.position = player.Center + centerOffset;
            projectile.direction = player.direction;

            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
                if (projectile.frame >= amountOfFrames)
                {
                    projectile.frame = 0;
                    projectile.Kill();
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int frameHeight = punchSpritesheet.Height / amountOfFrames;
            Rectangle sourceRect = new Rectangle(0, projectile.frame * frameHeight, punchSpritesheet.Width, frameHeight);
            SpriteEffects effect = SpriteEffects.None;
            if (projectile.direction == -1)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw(punchSpritesheet, projectile.Center - Main.screenPosition, sourceRect, lightColor, projectile.rotation, new Vector2(sourceRect.Width / 2f, sourceRect.Height / 2f), projectile.scale, effect, 0f);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(0, 2) == 0)
            {
                target.AddBuff(mod.BuffType("Sunburn"), 20 * 60);
            }
        }
    }
}