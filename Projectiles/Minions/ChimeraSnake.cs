using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{
    public class ChimeraSnake : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.minion = true;
            projectile.penetrate = 3;
            projectile.damage = 30;
            projectile.width = 60;
            projectile.height = 62;
            projectile.aiStyle = 54;
            projectile.timeLeft = 1200;
        }

        private int hitcooldown = 60;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.dead || !player.active || projectile.timeLeft == 0 || !mPlayer.doobiesskullEquipped)
            {
                projectile.Kill();
                return;
            }

            if (hitcooldown > 0)
            {
                hitcooldown -= 1;
            }
            if (hitcooldown == 0)
            {
                projectile.damage = 30;
            }
            if (hitcooldown > 0)
            {
                projectile.damage = 0;
            }

            projectile.frameCounter++;
            if (projectile.frameCounter >= 10)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            hitcooldown += 60;
            target.AddBuff(BuffID.Poisoned, 300);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            hitcooldown += 60;
            target.AddBuff(BuffID.Poisoned, 300);
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(5, (int)projectile.position.X, (int)projectile.position.Y);
        }
    }
}