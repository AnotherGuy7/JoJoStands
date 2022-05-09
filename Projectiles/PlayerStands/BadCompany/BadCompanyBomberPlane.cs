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
            Projectile.width = 40;
            Projectile.height = 26;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
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
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>());
            if (mPlayer.standOut && mPlayer.badCompanyTier != 0)
            {
                Projectile.timeLeft = 2;
            }
            PlayAnimation("BomberPlane>();

            if (!setStats)
            {
                if (Projectile.ai[0] == 3f)
                {
                    bombDamage = 62;
                }
                else if (Projectile.ai[0] == 4f)
                {
                    bombDamage = 87;
                }
                bombDropTime = (int)((Main.screenWidth / 4f) / Math.Abs(Projectile.velocity.X)) + Main.rand.Next(-30, 30 + 1);
                setStats = true;
            }

            bombDropTimer++;
            if (bombDropTimer >= bombDropTime && bombsDropped < 3)
            {
                bombsDropped++;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<BadCompanyBomb>(), 0, 3f, Projectile.owner, bombDamage * (float)mPlayer.standDamageBoosts);
                bombDropTimer = 0;
            }

            if (Projectile.velocity.X <= 0)
                Projectile.spriteDirection = Projectile.direction = -1;
            else
                Projectile.spriteDirection = Projectile.direction = 1;
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = Mod.GetTexture("Projectiles/PlayerStands/BadCompany/BadCompanyBomberPlane>();

            AnimateStand(animationName, 3, 11, true);
        }
    }
}