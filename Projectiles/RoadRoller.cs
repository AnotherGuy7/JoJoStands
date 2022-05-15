using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class RoadRoller : ModProjectile
    {
        private Vector2 velocityAdd = Vector2.Zero;
        private float damageMult = 1f;
        private bool landed = false;

        public override void SetDefaults()
        {
            Projectile.width = 144;
            Projectile.height = 74;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1800;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Projectile.timeLeft < 256)
            {
                Projectile.alpha = -Projectile.timeLeft + 255;
            }
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation = (Projectile.velocity * new Vector2(Projectile.spriteDirection)).ToRotation();

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile otherProj = Main.projectile[p];
                if (p == Projectile.whoAmI)
                    continue;

                if (otherProj.active && otherProj.type == ModContent.ProjectileType<Fists>())
                {
                    if (Projectile.Hitbox.Intersects(otherProj.Hitbox) && mPlayer.timestopActive)
                    {
                        velocityAdd += otherProj.velocity / 75f;
                        if (damageMult <= 4f)
                        {
                            damageMult += otherProj.damage / 50;
                        }
                        Dust.NewDust(otherProj.position, Projectile.width, Projectile.height, DustID.FlameBurst, otherProj.velocity.X * -0.5f, otherProj.velocity.Y * -0.5f);
                        SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/GameSounds/Punch_land").WithVolume(.3f));
                        otherProj.Kill();
                    }
                }
            }

            if (landed && Projectile.timeLeft < 180)
            {
                Projectile.damage = 0;
            }
            if (!mPlayer.timestopActive)
            {
                Projectile.velocity.Y += 0.1f;
                if (velocityAdd != Vector2.Zero)
                {
                    Projectile.velocity += velocityAdd;
                    Projectile.damage *= (int)damageMult;
                    damageMult = 1f;
                    velocityAdd = Vector2.Zero;
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.boss)
                damage *= 3;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            landed = true;
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            return !mPlayer.timestopActive;
        }
    }
}