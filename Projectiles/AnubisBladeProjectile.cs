using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class AnubisBladeProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 28;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.aiStyle = -1;
            Projectile.scale = 2.5f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.channel || player.noItems || player.CCed || player.dead)
            {
                Projectile.Kill();
                return;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            Vector2 direction = Main.MouseWorld - player.MountedCenter;
            if (direction != Vector2.Zero)
                direction.Normalize();
            Projectile.velocity = direction;
            Projectile.rotation = direction.ToRotation() + MathHelper.PiOver4;
            Projectile.Center = player.MountedCenter + (direction * 20f);
            player.ChangeDir(direction.X > 0 ? 1 : -1);
            player.itemRotation = (direction * player.direction).ToRotation();
            player.itemTime = 2;
            player.itemAnimation = 2;
            Projectile.timeLeft = 2;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int sizeIncrease = 40;
            hitbox.Inflate(sizeIncrease, sizeIncrease);
        }
    }
}