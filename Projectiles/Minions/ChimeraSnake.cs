using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private int frames = 0;
        private int hitcooldown = 60;
        private int changeframe = 0;
        public override void AI()
        {
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
            projectile.frame = frames;
            changeframe += 1;
            if (changeframe == 10)
            {
                changeframe = 0;
                frames += 1;
            }
            if (frames > 3)
            {
                frames = 0;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (player.dead || !player.active || projectile.timeLeft == 0 || !modPlayer.doobiesskullEquipped)
            {
                projectile.Kill();
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
            target.AddBuff(BuffID.Poisoned, 300);
            hitcooldown += 60;
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 300);
            hitcooldown += 60;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            Main.PlaySound(5, (int)projectile.position.X, (int)projectile.position.Y);
        }
    }
}