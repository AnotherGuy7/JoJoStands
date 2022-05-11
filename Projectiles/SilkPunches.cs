using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class SilkPunches : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.friendly = true;
            DrawOriginOffsetY = 20;
            Projectile.timeLeft = 80;
        }

        private Texture2D punchSpritesheet;
        private bool checkedPunchType = false;
        private int amountOfFrames = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!checkedPunchType)
            {
                if (Projectile.ai[0] < 2f)
                {
                    punchSpritesheet = Mod.Assets.Request<Texture2D>("Projectiles/SilkGloves_Punch").Value;
                    amountOfFrames = 4;
                }
                else
                {
                    if (Main.rand.Next(0, 2) == 0)
                    {
                        punchSpritesheet = Mod.Assets.Request<Texture2D>("Projectiles/SilkGloves_Uppercut").Value;
                    }
                    else
                    {
                        punchSpritesheet = Mod.Assets.Request<Texture2D>("Projectiles/SilkGloves_Undercut").Value;
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
            Projectile.position = player.Center + centerOffset;
            Projectile.direction = player.direction;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= amountOfFrames)
                {
                    Projectile.frame = 0;
                    Projectile.Kill();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            int frameHeight = punchSpritesheet.Height / amountOfFrames;
            Rectangle sourceRect = new Rectangle(0, Projectile.frame * frameHeight, punchSpritesheet.Width, frameHeight);
            SpriteEffects effect = SpriteEffects.None;
            if (Projectile.direction == -1)
                effect = SpriteEffects.FlipVertically;

            Main.EntitySpriteDraw(punchSpritesheet, Projectile.Center - Main.screenPosition, sourceRect, lightColor, Projectile.rotation, new Vector2(sourceRect.Width / 2f, sourceRect.Height / 2f), Projectile.scale, effect, 0);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(0, 2) == 0)
                target.AddBuff(ModContent.BuffType<Sunburn>(), 12 * 60);
        }
    }
}