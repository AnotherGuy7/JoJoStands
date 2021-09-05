using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ArmWormholeNail : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/WormholeNail"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 25;
            projectile.height = 25;
            projectile.aiStyle = 0;
            projectile.penetrate = -1;
            projectile.timeLeft = 3600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }


        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            projectile.rotation += MathHelper.ToRadians(13f * projectile.direction);

            if (player.whoAmI == Main.myPlayer && mPlayer.tuskShootCooldown <= 0)
            {
                if (Main.mouseLeft && !player.channel)
                {
                    player.channel = true;
                    mPlayer.tuskShootCooldown += 35 - mPlayer.standSpeedBoosts;
                    Main.PlaySound(SoundID.Item67);
                    Vector2 shootVelocity = Main.MouseWorld - projectile.Center;
                    shootVelocity.Normalize();
                    shootVelocity *= 4f;
                    Projectile.NewProjectile(projectile.Center + shootVelocity, shootVelocity, mod.ProjectileType("ControllableNail"), (int)(122 * mPlayer.standDamageBoosts) + ((22 + mPlayer.equippedTuskAct - 3) * mPlayer.equippedTuskAct - 3), 6f, player.whoAmI);
                }
                if (Main.mouseRight || mPlayer.tuskActNumber != 3)
                {
                    projectile.Kill();
                    mPlayer.tuskShootCooldown += 30;
                }
            }

            int dustIndex = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 240);
            Main.dust[dustIndex].noGravity = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}