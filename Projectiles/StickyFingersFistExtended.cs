using JoJoStands.Buffs.Debuffs;
using JoJoStands.NPCs;
using JoJoStands.Projectiles.PlayerStands.StickyFingers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StickyFingersFistExtended : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private bool living = true;
        private bool playedSound = false;
        private const float MaxTravelDistance = 40f * 16f;
        private const float DespawnDistance = 3f * 16f;

        public override void AI()       //all this so that the other chain doesn't draw... yare yare. It was mostly just picking out types
        {
            Projectile ownerProj = Main.projectile[(int)Projectile.ai[0]];
            if (Main.player[Projectile.owner].dead || !ownerProj.active)
            {
                Projectile.Kill();
                return;
            }
            Projectile.direction = ownerProj.Center.X < Projectile.Center.X ? 1 : -1;
            Projectile.spriteDirection = ownerProj.direction = Projectile.direction;
            Vector2 rota = ownerProj.Center - Projectile.Center;
            Projectile.rotation = (-rota * Projectile.direction).ToRotation();
            if (!playedSound && JoJoStands.SoundsLoaded)
            {
                SoundEngine.PlaySound(StickyFingersStandFinal.ZipperSound);
                playedSound = true;
            }

            if (Projectile.alpha == 0)
            {
                if (Projectile.position.X + (float)(Projectile.width / 2) > ownerProj.position.X + (float)(ownerProj.width / 2))
                    ownerProj.direction = ownerProj.spriteDirection = 1;
                else
                    ownerProj.direction = ownerProj.spriteDirection = -1;
            }
            float distanceFromOwner = Vector2.Distance(ownerProj.Center, Projectile.Center);
            if (living)     //while it's living
            {
                if (distanceFromOwner > MaxTravelDistance)
                    living = false;
                //Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] > 5f)
                    Projectile.alpha = 0;
                if (Projectile.ai[1] >= 10f)
                    Projectile.ai[1] = 15f;
            }
            else
            {
                Projectile.tileCollide = false;
                if (distanceFromOwner < DespawnDistance)
                    Projectile.Kill();
                Vector2 returnVelocity = ownerProj.Center - Projectile.Center;
                returnVelocity.Normalize();
                returnVelocity *= 16f;
                Projectile.velocity = returnVelocity;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            living = false;
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.Next(0, 101) <= 50)
            {
                target.GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner = Projectile.owner;
                target.AddBuff(ModContent.BuffType<Zipped>(), 5 * 60);
            }
            living = false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                living = false;
                if (Main.rand.Next(1, 100 + 1) <= 50)
                    target.AddBuff(ModContent.BuffType<Zipped>(), 5 * 60);
            }
        }

        private Texture2D stickyFingersZipperPart;

        public override bool PreDraw(ref Color lightColor)     //once again, TMOd help-with-code saves the day (Scalie)
        {
            Projectile ownerProj = Main.projectile[(int)Projectile.ai[0]];
            Vector2 ownerCenterOffset;
            if (ownerProj.spriteDirection == 1)
                ownerCenterOffset = new Vector2(4f, -4f);
            else
                ownerCenterOffset = new Vector2(-12f, -6f);

            if (Main.netMode != NetmodeID.Server)
                stickyFingersZipperPart = ModContent.Request<Texture2D>("JoJoStands/Projectiles/Zipper_Part").Value;

            Vector2 ownerCenter = ownerProj.Center + ownerCenterOffset;
            Vector2 center = Projectile.Center + new Vector2(0f, -1f);
            for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, ownerCenter) / stickyFingersZipperPart.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
            {
                Vector2 pos = Vector2.Lerp(center, ownerCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                Main.EntitySpriteDraw(stickyFingersZipperPart, pos, new Rectangle(0, 0, stickyFingersZipperPart.Width, stickyFingersZipperPart.Height), lightColor, Projectile.rotation, new Vector2(stickyFingersZipperPart.Width * 0.5f, stickyFingersZipperPart.Height * 0.5f), 1f, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}