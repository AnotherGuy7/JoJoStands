using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.Cream
{
    public class CreamStandT1 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 11;
        }

        public override int punchDamage => 35;
        public override float punchKnockback => 8f;
        public override int punchTime => 28;     
        public override int halfStandHeight => 36;
        public override float fistWhoAmI => 11f;
        public override int standOffset => 0;
        public override int standType => 1;

        private int updateTimer = 0;
        private Vector2 velocityAddition;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            SelectAnimation();
            UpdateStandInfo();
            updateTimer = 0;
            if (shootCount > 0)
                shootCount--;
            if (mPlayer.standOut)
                projectile.timeLeft = 2;

            if (updateTimer >= 90)      
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    HandleDrawOffsets();
                    attackFrames = true;
                    normalFrames = false;
                    projectile.netUpdate = true;

                    float rotaY = Main.MouseWorld.Y - projectile.Center.Y;
                    projectile.rotation = MathHelper.ToRadians((rotaY * projectile.spriteDirection) / 6f);

                    projectile.direction = 1;
                    if (Main.MouseWorld.X < projectile.position.X)
                    {
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    velocityAddition = Main.MouseWorld - projectile.position;
                    velocityAddition.Normalize();
                    velocityAddition *= 5f;

                    float mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        projectile.velocity = player.velocity + velocityAddition;
                    }
                    if (mouseDistance <= 40f)
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                    if (shootCount <= 0 && projectile.frame == 2)
                    {
                        shootCount += newPunchTime;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("Fists"), newPunchDamage, punchKnockback, projectile.owner, fistWhoAmI);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                    LimitDistance();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    StayBehind();
                }
            }
            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
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
                standTexture = mod.GetTexture("Projectiles/PlayerStands/Cream/Cream_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}