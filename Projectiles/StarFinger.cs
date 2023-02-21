using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.Debuffs;

namespace JoJoStands.Projectiles
{
    public class StarFinger : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private Projectile ownerProj;
        private bool living = true;
        private bool playedSound = false;

        public override void AI()       //all this so that the other chain doesn't draw... yare yare. It was mostly just picking out types
        {
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);     //aiStyle 13 without the types
            if (!playedSound && JoJoStands.SoundsLoaded)
            {
                SoundStyle starFinger = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/StarFinger");
                starFinger.Volume = MyPlayer.ModSoundsVolume;
                SoundEngine.PlaySound(starFinger, Projectile.Center);
                playedSound = true;
            }
            ownerProj = Main.projectile[(int)Projectile.ai[0]];
            if (Main.player[Projectile.owner].dead || !ownerProj.active)
            {
                Projectile.Kill();
                return;
            }
            float direction = ownerProj.Center.X - Projectile.Center.X;
            if (direction > 0)
            {
                Projectile.direction = -1;
                ownerProj.spriteDirection = ownerProj.direction = -1;
            }
            if (direction < 0)
            {
                Projectile.direction = 1;
                ownerProj.spriteDirection = ownerProj.direction = 1;
            }
            //Projectile.spriteDirection = Projectile.direction;
            Vector2 rota = ownerProj.Center - Projectile.Center;
            Projectile.rotation = (-rota).ToRotation();
            if (Projectile.alpha == 0)
            {
                if (Projectile.position.X + (float)(Projectile.width / 2) > ownerProj.position.X + (float)(ownerProj.width / 2))
                {
                    ownerProj.spriteDirection = ownerProj.direction = 1;
                }
                else
                {
                    ownerProj.spriteDirection = ownerProj.direction = -1;
                }
            }
            Vector2 vector14 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
            float num166 = ownerProj.position.X + (float)(ownerProj.width / 2) - vector14.X;
            float num167 = ownerProj.position.Y + (float)(ownerProj.height / 2) - vector14.Y;
            float num168 = (float)Math.Sqrt((double)(num166 * num166 + num167 * num167));
            if (living)
            {
                if (num168 > 700f)
                {
                    living = false;
                }
                //Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
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
                //Projectile.rotation = (float)Math.Atan2((double)num167, (double)num166) - 1.57f;
                float num169 = 20f;
                if (num168 < 50f)
                {
                    Projectile.Kill();
                }
                num168 = num169 / num168;
                num166 *= num168;
                num167 *= num168;
                Projectile.velocity.X = num166;
                Projectile.velocity.Y = num167;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                crit = true;
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.Next(1, 100 + 1) >= 60)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        private Vector2 offset;
        private Texture2D starFingerPartTexture;

        public override bool PreDraw(ref Color lightColor)
        {
            if (ownerProj == null)
                return false;

            if (ownerProj.direction == 1)
            {
                offset = new Vector2(20f, -2f);
            }
            if (ownerProj.direction == -1)
            {
                offset = new Vector2(-31f, -2f);
            }
            if (Main.netMode != NetmodeID.Server)
                starFingerPartTexture = ModContent.Request<Texture2D>("JoJoStands/Projectiles/StarFingerPart").Value;

            Vector2 linkCenter = ownerProj.Center + offset;
            Vector2 center = Projectile.Center;
            float rotation = (linkCenter - center).ToRotation();

            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / starFingerPartTexture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                Main.EntitySpriteDraw(starFingerPartTexture, pos, new Rectangle(0, 0, starFingerPartTexture.Width, starFingerPartTexture.Height), lightColor, rotation, new Vector2(starFingerPartTexture.Width * 0.5f, starFingerPartTexture.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}