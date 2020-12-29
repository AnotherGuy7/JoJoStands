using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.Cream
{
    public class CreamStandT2 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 11;
        }

        public override int punchDamage => 96;
        public override float punchKnockback => 2f;
        public override int punchTime => 24;
        public override int halfStandHeight => 32;
        public override float fistWhoAmI => 11f;
        public override int standOffset => 0;
        public override int standType => 1;

        private int updateTimer = 0;
        private Vector2 velocityAddition;
        private float mouseDistance;

        public override void AI()
        {

            SelectAnimation();
            UpdateStandInfo();
            updateTimer = 0;
            if (shootCount > 0)
                shootCount--;
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (modPlayer.VoidMode)
            {
                projectile.hide = true;
            }
            if (modPlayer.ExposingMode)
            {
                projectile.hide = true;
            }
            if (!modPlayer.VoidMode && !modPlayer.ExposingMode)
            {
                projectile.hide = false;
            }
            projectile.frameCounter++;
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }
            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !modPlayer.VoidMode && !modPlayer.ExposingMode)
                {
                    HandleDrawOffsets();
                    attackFrames = true;
                    normalFrames = false;
                    Main.mouseRight = false;
                    projectile.netUpdate = true;
                    float rotaY = Main.MouseWorld.Y - projectile.Center.Y;
                    projectile.rotation = MathHelper.ToRadians((rotaY * projectile.spriteDirection) / 6f);
                    if (Main.MouseWorld.X > projectile.position.X)
                    {
                        projectile.spriteDirection = 1;
                        projectile.direction = 1;
                    }
                    if (Main.MouseWorld.X < projectile.position.X)
                    {
                        projectile.spriteDirection = -1;
                        projectile.direction = -1;
                    }
                    velocityAddition = Main.MouseWorld - projectile.position;
                    velocityAddition.Normalize();
                    velocityAddition *= 5f;
                    mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        projectile.velocity = player.velocity + velocityAddition;
                    }
                    if (mouseDistance <= 40f)
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                    if (shootCount <= 0 && (projectile.frame == 0 || projectile.frame == 4))
                    {
                        shootCount += newPunchTime;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), newPunchDamage, punchKnockback, projectile.owner, fistWhoAmI);
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
                if (Main.mouseRight && player.ownedProjectileCounts[mod.ProjectileType("Void")] <= 0 && !modPlayer.VoidMode && modPlayer.VoidCounter != 0 && modPlayer.VoidCooldown <= 0)
                {
                    modPlayer.VoidCooldown += 60;
                    Main.PlaySound(SoundID.Item78);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("Void"), (int)((64 * modPlayer.CreamPower) * modPlayer.standDamageBoosts), 6f, player.whoAmI);
                }
            }
            if (modPlayer.StandAutoMode && !modPlayer.VoidMode)
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
                AnimationStates(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 6, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 2, true);
            }
        }
    }
}