using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

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
        public override void SetDefaults() //snake don't work properly, later i'll improve it (C) Proos <3
        {
            Projectile.penetrate = 3;
            Projectile.width = 60;
            Projectile.height = 62;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1200;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.minion = true;
        }

        private int hitcooldown = 60;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (player.dead || !player.active || Projectile.timeLeft == 0 || !vPlayer.doobiesskullEquipped)
            {
                Projectile.Kill();
                return;
            }

            if (hitcooldown > 0)
                hitcooldown--;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (Projectile.frame > 3)
                    Projectile.frame = 0;
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitcooldown += 60;
            target.AddBuff(BuffID.Poisoned, 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                hitcooldown += 60;
                target.AddBuff(BuffID.Poisoned, 5 * 60);
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath5, Projectile.Center);
        }
        public override bool CanHitPvp(Player target)
        {
            if (hitcooldown > 0)
                return false;
            return true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (hitcooldown > 0 || target.townNPC)
                return false;
            return true;
        }
    }
}