using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StickyFingersTraversalZipper : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1200;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.damage = 0;
        }

        private bool stopped = false;
        private float recoveredTime = 0f;
        private bool playedSound = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 velocity;
            velocity = Projectile.Center - player.Center;
            velocity.Normalize();
            float dist = Vector2.Distance(player.Center, Projectile.Center);
            if (!playedSound && JoJoStands.SoundsLoaded)
            {
                SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/Zip"));
                playedSound = true;
            }

            if (dist >= 32 * 16f)
            {
                if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    stopped = true;
                    Projectile.position += -velocity * 2f;
                    Projectile.velocity = Vector2.Zero;
                }
            }
            if (stopped)
            {
                player.position += velocity * 12f;
                player.velocity = velocity * 6f;
                /*if (recoveredTime == 0f && Projectile.ai[0] == 0f)      //max here can be 9 seconds 
                {
                    recoveredTime = dist * 1.0909f;     //1.0909 is gotten by doing (number of seconds * 60) / maxDistance  =  540 / 495
                }
                if (recoveredTime == 0f && Projectile.ai[0] == 1f)      //max here can be 4 seconds
                {
                    recoveredTime = dist * 0.4848f;     ////0.4848 is gotten by doing (number of seconds * 60) / maxDistance  =  240 / 495
                }*/
                if (dist <= 3 * 16f)
                {
                    player.position += velocity * 24f;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), (int)recoveredTime);
                    Projectile.Kill();
                }
            }

            if (Collision.SolidCollision(player.position, player.width, player.height))
                player.AddBuff(BuffID.Obstructed, 2);
        }

        private Texture2D stickyFingersZipperPart;
        private Rectangle stickyFingersPartSourceRect;
        private Vector2 stickyFingersPartOrigin;
        private Color drawColor;

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.netMode != NetmodeID.Server && stickyFingersZipperPart == null)
            {
                stickyFingersZipperPart = ModContent.Request<Texture2D>("JoJoStands/Projectiles/Zipper_Part", AssetRequestMode.ImmediateLoad).Value;
                stickyFingersPartSourceRect = new Rectangle(0, 0, stickyFingersZipperPart.Width, stickyFingersZipperPart.Height);
                stickyFingersPartOrigin = new Vector2(stickyFingersZipperPart.Width * 0.5f, stickyFingersZipperPart.Height * 0.5f);
            }

            Vector2 linkCenter = Main.projectile[Projectile.owner].Center;
            Vector2 projectileCenter = Projectile.Center;
            drawColor = lightColor;

            float stringRotation = (linkCenter - projectileCenter).ToRotation();
            float stringScale = 0.6f;
            float loopIncrement = 1 / (Vector2.Distance(projectileCenter, linkCenter) / (stickyFingersZipperPart.Width * stringScale));
            float lightLevelIndex = 0f;
            for (float k = 0; k <= 1; k += loopIncrement)
            {
                lightLevelIndex += loopIncrement;
                Vector2 pos = Vector2.Lerp(projectileCenter, linkCenter, k) - Main.screenPosition;
                if (lightLevelIndex >= 0.1f)
                {
                    drawColor = Lighting.GetColor((int)(pos.X + Main.screenPosition.X) / 16, (int)(pos.Y + Main.screenPosition.Y) / 16);
                    lightLevelIndex = 0f;
                }

                Main.EntitySpriteDraw(stickyFingersZipperPart, pos, stickyFingersPartSourceRect, drawColor, stringRotation, stickyFingersPartOrigin, Projectile.scale * stringScale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}