using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StickyFingersFistExtended : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        private bool living = true;
        private bool playedSound = false;

        public override void AI()       //all this so that the other chain doesn't draw... yare yare. It was mostly just picking out types
        {
            Projectile ownerProj = Main.projectile[(int)projectile.ai[0]];
            if (Main.player[projectile.owner].dead || !ownerProj.active)
            {
                projectile.Kill();
                return;
            }
            float direction = ownerProj.Center.X - projectile.Center.X;
            if (direction > 0)
            {
                projectile.direction = -1;
                ownerProj.direction = -1;
            }
            if (direction < 0)
            {
                projectile.direction = 1;
                ownerProj.direction = 1;
            }
            projectile.spriteDirection = projectile.direction;
            Vector2 rota = ownerProj.Center - projectile.Center;
            projectile.rotation = (-rota * projectile.direction).ToRotation();
            if (!playedSound && JoJoStands.SoundsLoaded)
            {
                Main.PlaySound(JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Zip"));
                playedSound = true;
            }

            if (projectile.alpha == 0)
            {
                if (projectile.position.X + (float)(projectile.width / 2) > ownerProj.position.X + (float)(ownerProj.width / 2))
                {
                    ownerProj.direction = ownerProj.spriteDirection = 1;
                }
                else
                {
                    ownerProj.direction = ownerProj.spriteDirection = -1;
                }
            }
            Vector2 vector14 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num166 = ownerProj.position.X + (float)(ownerProj.width / 2) - vector14.X;
            float num167 = ownerProj.position.Y - vector14.Y;
            float num168 = (float)Math.Sqrt((double)(num166 * num166 + num167 * num167));
            if (living)     //while it's living
            {
                if (num168 > 700f)
                {
                    living = false;
                }
                //projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
                projectile.ai[1] += 1f;
                if (projectile.ai[1] > 5f)
                {
                    projectile.alpha = 0;
                }
                if (projectile.ai[1] >= 10f)
                {
                    projectile.ai[1] = 15f;
                }
            }
            else if (!living)        //dead stuff
            {
                projectile.tileCollide = false;
                //projectile.rotation = (float)Math.Atan2((double)num167, (double)num166) - 1.57f;
                float num169 = 20f;
                if (num168 < 50f)
                {
                    projectile.Kill();
                }
                num168 = num169 / num168;
                num166 *= num168;
                num167 *= num168;
                projectile.velocity.X = num166;
                projectile.velocity.Y = num167;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(0, 101) <= 50)
            {
                target.AddBuff(mod.BuffType("Zipped"), 300);
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(0, 101) <= 50)
            {
                target.AddBuff(mod.BuffType("Zipped"), 300);
            }
        }

        private Texture2D stickyFingersZipperPart;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)     //once again, TMOd help-with-code saves the day (Scalie)
        {
            Projectile ownerProj = Main.projectile[(int)projectile.ai[0]];
            Vector2 ownerCenterOffset = Vector2.Zero;
            if (ownerProj.spriteDirection == -1)
            {
                ownerCenterOffset = new Vector2(-16f, -10f);
            }
            if (ownerProj.spriteDirection == 1)
            {
                ownerCenterOffset = new Vector2(4f, -4.5f);
            }
            if (Main.netMode != NetmodeID.Server)
                stickyFingersZipperPart = mod.GetTexture("Projectiles/Zipper_Part");

            Vector2 ownerCenter = ownerProj.Center + ownerCenterOffset;
            Vector2 center = projectile.Center + new Vector2(0f, -1f);
            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, ownerCenter) / stickyFingersZipperPart.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, ownerCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                spriteBatch.Draw(stickyFingersZipperPart, pos, new Rectangle(0, 0, stickyFingersZipperPart.Width, stickyFingersZipperPart.Height), lightColor, projectile.rotation, new Vector2(stickyFingersZipperPart.Width * 0.5f, stickyFingersZipperPart.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}