using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ArmWormholeNail : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/WormholeNail"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }


        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            Projectile.rotation += MathHelper.ToRadians(13f * Projectile.direction);

            if (player.whoAmI == Main.myPlayer && mPlayer.tuskShootCooldown <= 0)
            {
                if (Main.mouseLeft && !player.channel)
                {
                    player.channel = true;
                    mPlayer.tuskShootCooldown += 35 - mPlayer.standSpeedBoosts;
                    SoundStyle shootSound = SoundID.Item67;
                    shootSound.Volume = 0.33f;
                    SoundEngine.PlaySound(shootSound);
                    Vector2 shootVelocity = Main.MouseWorld - Projectile.Center;
                    shootVelocity.Normalize();
                    shootVelocity *= 4f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + shootVelocity, shootVelocity, ModContent.ProjectileType<ControllableNail>(), (int)(128 * mPlayer.standDamageBoosts) + ((22 + mPlayer.equippedTuskAct - 3) * mPlayer.equippedTuskAct - 3), 6f, player.whoAmI);
                }
                if (Main.mouseRight || mPlayer.tuskActNumber != 3)
                {
                    Projectile.Kill();
                    mPlayer.tuskShootCooldown += 30;
                }
            }

            int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 240);
            Main.dust[dustIndex].noGravity = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}