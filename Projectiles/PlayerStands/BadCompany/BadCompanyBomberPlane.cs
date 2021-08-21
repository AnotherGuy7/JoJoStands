using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.BadCompany
{
    public class BadCompanyBomberPlane : StandClass
    {

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 26;
            projectile.timeLeft = 180;
        }

        public override int standType => 2;

        public int updateTimer = 0;

        private int bombDamage = 0;
        private bool setStats = false;
        private int bombsDropped = 0;
        private int bombDropTime = 0;
        private int bombDropTimer = 0;

        public override void AI()
        {
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut && mPlayer.badCompanyTier != 0)
            {
                projectile.timeLeft = 2;
            }
            PlayAnimation("BomberPlane");

            if (!setStats)
            {
                if (projectile.ai[0] == 3f)
                {
                    bombDamage = 62;
                }
                else if (projectile.ai[0] == 4f)
                {
                    bombDamage = 87;
                }
                bombDropTime = (int)((Main.screenWidth / 4f) / Math.Abs(projectile.velocity.X)) + Main.rand.Next(-30, 30 + 1);
                setStats = true;
            }

            bombDropTimer++;
            if (bombDropTimer >= bombDropTime && bombsDropped < 3)
            {
                bombsDropped++;
                Projectile.NewProjectile(projectile.Center, projectile.velocity, mod.ProjectileType("BadCompanyBomb"), 0, 3f, projectile.owner, bombDamage * (float)mPlayer.standDamageBoosts);
                bombDropTimer = 0;
            }

            if (projectile.velocity.X <= 0)
            {
                projectile.spriteDirection = projectile.direction = -1;
            }
            else
            {
                projectile.spriteDirection = projectile.direction = 1;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/BadCompany/BadCompanyBomberPlane");

            AnimateStand(animationName, 3, 11, true);
        }
    }
}