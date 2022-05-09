using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles.Minions
{
    public class ChimeraSnake : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.penetrate = 3;
            Projectile.damage = 30;
            Projectile.width = 60;
            Projectile.height = 62;
            Projectile.aiStyle = 54;
            Projectile.timeLeft = 1200;
        }

        private int hitcooldown = 60;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.dead || !player.active || Projectile.timeLeft == 0 || !mPlayer.doobiesskullEquipped)
            {
                Projectile.Kill();
                return;
            }

            if (hitcooldown > 0)
            {
                hitcooldown -= 1;
            }
            if (hitcooldown == 0)
            {
                Projectile.damage = 30;
            }
            if (hitcooldown > 0)
            {
                Projectile.damage = 0;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
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
            SoundEngine.PlaySound(5, (int)Projectile.position.X, (int)Projectile.position.Y);
        }
    }
}