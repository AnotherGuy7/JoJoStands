using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.MagiciansRed
{
    public class MagiciansRedStandT1 : StandClass
    {
        public override float shootSpeed => 8f;
        public override int standType => 2;
        public override int projectileDamage => 16;
        public override int shootTime => 20;
        public override int halfStandHeight => 35;
        public override int standOffset => 0;
        public override string poseSoundName => "ThePowerToWieldFlameAtWill";
        public override string spawnSoundName => "Magicians Red";

        private int chanceToDebuff = 25;
        private int debuffDuration = 300;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                projectile.timeLeft = 2;

            if (!attackFrames)
                StayBehind();
            else
                GoInFront();

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    attackFrames = true;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("FireAnkh"), newProjectileDamage, 3f, projectile.owner, chanceToDebuff, debuffDuration);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        normalFrames = true;
                        attackFrames = false;
                    }
                }
            }
            if (mPlayer.standAutoMode)
            {
                NPC target = FindNearestTarget(350f);
                if (target != null)
                {
                    attackFrames = true;
                    normalFrames = false;

                    projectile.direction = 1;
                    if (target.position.X - projectile.Center.X < 0f)
                    {
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    projectile.velocity = target.Center - projectile.Center;
                    projectile.velocity.Normalize();
                    projectile.velocity *= 4f;

                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = target.position - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("FireAnkh"), newProjectileDamage, 3f, projectile.owner, chanceToDebuff, debuffDuration);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    normalFrames = true;
                    attackFrames = false;
                }
            }
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/MagiciansRed/MagiciansRed_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 2, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 2, newShootTime, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}