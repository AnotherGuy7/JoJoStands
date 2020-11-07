using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class StickyFingersZipperPoint2 : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.timeLeft = 1200;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
        }

        private bool stopped = false;
        private float recoveredTime = 0f;
        private Vector2 zipperLine = Vector2.Zero;
        private bool playedSound = false;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            //Main.NewText(zipperLine + "; L:" + zipperLine.Length());
            zipperLine = projectile.Center - player.Center;
            zipperLine.Normalize();
            float dist = Vector2.Distance(player.Center, projectile.Center);
            if (!playedSound && JoJoStands.SoundsLoaded)
            {
                Main.PlaySound(JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Zip"));
                playedSound = true;
            }
            if (dist >= 495f)       //about 30 tiles
            {
                stopped = true;
                projectile.velocity = Vector2.Zero;
            }
            if (stopped)
            {
                player.velocity = zipperLine * 12f;
                if (recoveredTime == 0f && projectile.ai[0] == 0f)      //max here can be 9 seconds 
                {
                    recoveredTime = dist * 1.0909f;     //1.0909 is gotten by doing (number of seconds * 60) / maxDistance  =  540 / 495
                }
                if (recoveredTime == 0f && projectile.ai[0] == 1f)      //max here can be 4 seconds
                {
                    recoveredTime = dist * 0.4848f;     ////0.4848 is gotten by doing (number of seconds * 60) / maxDistance  =  240 / 495
                }
                if (dist <= 45f)
                {
                    player.AddBuff(mod.BuffType("AbilityCooldown"), (int)recoveredTime);
                    projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            stopped = true;
            projectile.velocity = Vector2.Zero;
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 ownerCenter = Main.player[projectile.owner].Center;
            Vector2 center = projectile.Center;
            Vector2 distToProj = ownerCenter - projectile.Center;
            Texture2D texture = mod.GetTexture("Projectiles/Zipper_Part");
            float projRotation = distToProj.ToRotation();
            float distance = distToProj.Length();
            while (distance > 30f && !float.IsNaN(distance))
            {
                distToProj.Normalize();                 //get unit vector
                distToProj *= 6f;                      //speed = 24
                center += distToProj;                   //update draw position
                distToProj = ownerCenter - center;    //update distance
                distance = distToProj.Length();

                spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projRotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}