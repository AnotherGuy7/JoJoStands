using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.MagiciansRed
{
    public class MagiciansRedStandT1 : StandClass
    {
        public override float shootSpeed => 8f;
        public override StandType standType => StandType.Ranged;
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
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (!attackFrames)
                StayBehind();
            else
                GoInFront();

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    if (!mPlayer.canStandBasicAttack)
                    {
                        idleFrames = true;
                        attackFrames = false;
                        return;
                    }

                    attackFrames = true;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<FireAnkh>(), newProjectileDamage, 3f, Projectile.owner, chanceToDebuff, debuffDuration);
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        idleFrames = true;
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
                    idleFrames = false;

                    Projectile.direction = 1;
                    if (target.position.X - Projectile.Center.X < 0f)
                        Projectile.direction = -1;
                    Projectile.spriteDirection = Projectile.direction;

                    Projectile.velocity = target.Center - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 4f;

                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = target.position - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<FireAnkh>(), newProjectileDamage, 3f, Projectile.owner, chanceToDebuff, debuffDuration);
                            Main.projectile[proj].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    idleFrames = true;
                    attackFrames = false;
                }
            }

            if (Main.rand.Next(0, 20 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(Projectile.position + new Vector2(0, -halfStandHeight), Projectile.width, halfStandHeight * 2, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, Scale: 2.1f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 1.4f;
            }
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/MagiciansRed/MagiciansRed_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 15, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newShootTime / 2, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}