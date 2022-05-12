using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StickyFingersZipperPoint : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1200;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.damage = 0;
        }

        private bool stopped = false;
        private float recoveredTime = 0f;
        private Vector2 zipperLine = Vector2.Zero;
        private bool playedSound = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            //Main.NewText(zipperLine + "; L:" + zipperLine.Length());
            zipperLine = Projectile.Center - player.Center;
            zipperLine.Normalize();
            float dist = Vector2.Distance(player.Center, Projectile.Center);
            if (!playedSound && JoJoStands.SoundsLoaded)
            {
                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(JoJoStands.JoJoStandsSounds, "Sounds/SoundEffects/Zip"));
                playedSound = true;
            }
            if (dist >= 495f)       //about 30 tiles
            {
                stopped = true;
                Projectile.velocity = Vector2.Zero;
            }
            if (stopped)
            {
                player.velocity = zipperLine * 12f;
                if (recoveredTime == 0f && Projectile.ai[0] == 0f)      //max here can be 9 seconds 
                {
                    recoveredTime = dist * 1.0909f;     //1.0909 is gotten by doing (number of seconds * 60) / maxDistance  =  540 / 495
                }
                if (recoveredTime == 0f && Projectile.ai[0] == 1f)      //max here can be 4 seconds
                {
                    recoveredTime = dist * 0.4848f;     ////0.4848 is gotten by doing (number of seconds * 60) / maxDistance  =  240 / 495
                }
                if (dist <= 45f)
                {
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), (int)recoveredTime);
                    Projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            stopped = true;
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        private Texture2D stickyFingersZipperPart;

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.netMode != NetmodeID.Server)
                stickyFingersZipperPart = ModContent.Request<Texture2D>("JoJoStands/Projectiles/Zipper_Part").Value;

            Vector2 ownerCenter = Main.player[Projectile.owner].Center;
            Vector2 center = Projectile.Center;
            Vector2 distToProj = ownerCenter - Projectile.Center;
            float projRotation = distToProj.ToRotation();
            float distance = distToProj.Length();
            while (distance > 30f && !float.IsNaN(distance))
            {
                distToProj.Normalize();                 //get unit vector
                distToProj *= 6f;                      //speed = 24
                center += distToProj;                   //update draw position
                distToProj = ownerCenter - center;    //update distance
                distance = distToProj.Length();

                Main.EntitySpriteDraw(stickyFingersZipperPart, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y), new Rectangle(0, 0, stickyFingersZipperPart.Width, stickyFingersZipperPart.Height), Color.White, projRotation, new Vector2(stickyFingersZipperPart.Width / 2f, stickyFingersZipperPart.Height / 2f), 1f, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}