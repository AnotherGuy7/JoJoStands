using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.Cream
{
    public class CreamStandT3 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 11;
        }

        public override int punchDamage => 116;
        public override float punchKnockback => 10f;
        public override int punchTime => 24;
        public override int halfStandHeight => 36;
        public override float fistWhoAmI => 11f;
        public override int standOffset => 0;
        public override int standType => 1;

        private int updateTimer = 0;
        private Vector2 velocityAddition;
        private float mouseDistance;
        private int framechangecounter = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer = 0;
            if (shootCount > 0)
                shootCount--;
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.creamVoidMode)
                projectile.hide = true;
            if (mPlayer.creamExposedMode)
                projectile.hide = false;
            if (!mPlayer.creamVoidMode)
                projectile.hide = false;
            if (mPlayer.standOut)
                projectile.timeLeft = 2;

            if (updateTimer >= 90)
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !mPlayer.creamVoidMode && !mPlayer.creamExposedMode && !mPlayer.creamExposedToVoid && !mPlayer.creamNormalToExposed)
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
                        projectile.spriteDirection = -1;
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

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
                if (Main.mouseRight && !mPlayer.creamExposedMode && player.ownedProjectileCounts[mod.ProjectileType("ExposingCream")] <= 0 && player.ownedProjectileCounts[mod.ProjectileType("Void")] <= 0 && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid)
                {
                    mPlayer.creamFrame = 0;
                    if (!mPlayer.creamVoidMode)
                    {
                        mPlayer.creamNormalToExposed = true;
                    }
                }
                if (SpecialKeyPressed() && player.ownedProjectileCounts[mod.ProjectileType("Void")] <= 0 && !mPlayer.creamVoidMode && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid)
                {
                    mPlayer.creamFrame = 0;
                    if (mPlayer.creamExposedMode)
                    {
                        mPlayer.creamExposedToVoid = true;
                    }
                    if (!mPlayer.creamExposedMode)
                    {
                        mPlayer.creamNormalToExposed = true;
                        mPlayer.creamNormalToVoid = true;
                    }
                }
            }
            if (mPlayer.creamNormalToExposed)
            {
                PlayAnimation("Transform");
                if (mPlayer.creamFrame >= 5 && !mPlayer.creamAnimationReverse)
                {
                    if (mPlayer.creamNormalToVoid)
                    {
                        mPlayer.creamExposedToVoid = true;
                        mPlayer.creamNormalToVoid = false;
                    }
                    mPlayer.creamNormalToExposed = false;
                    mPlayer.creamFrame = 0;
                    Main.PlaySound(SoundID.Item78);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("ExposingCream"), 0, 6f, player.whoAmI);
                }
            }
            if (mPlayer.creamExposedToVoid)
            {
                PlayAnimation("Transform2");
                if (mPlayer.creamFrame >= 7 && !mPlayer.creamAnimationReverse)
                {
                    mPlayer.creamExposedToVoid = false;
                    Main.PlaySound(SoundID.Item78);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("Void"), (int)((50 * mPlayer.creamTier) * mPlayer.standDamageBoosts), 6f, player.whoAmI);
                }
                if (mPlayer.creamFrame <= 0 && mPlayer.creamAnimationReverse && mPlayer.creamNormalToExposed)
                {
                    mPlayer.creamNormalToExposed = false;
                    mPlayer.creamAnimationReverse = false;
                }
                if (mPlayer.creamFrame <= 0 && mPlayer.creamAnimationReverse && mPlayer.creamExposedToVoid)
                {
                    mPlayer.creamExposedToVoid = false;
                    if (!mPlayer.creamNormalToVoid)
                    {
                        mPlayer.creamAnimationReverse = false;
                    }
                    if (mPlayer.creamNormalToVoid)
                    {
                        mPlayer.creamFrame = 5;
                        mPlayer.creamNormalToExposed = true;
                        mPlayer.creamNormalToVoid = false;
                    }
                }
            }
            if (mPlayer.creamExposedToVoid || mPlayer.creamVoidMode || mPlayer.creamExposedMode)
            {
                HandleDrawOffsets();
                Vector2 vector131 = player.Center;
                vector131.X += (float)((player.width / 2) * player.direction);
                vector131.Y -= -35 + halfStandHeight;
                projectile.Center = Vector2.Lerp(projectile.Center, vector131, 1f);
                projectile.velocity *= 0.5f;
                projectile.direction = (projectile.spriteDirection = player.direction);
                projectile.rotation = 0;
                LimitDistance();
            }
            if (mPlayer.creamExposedToVoid || mPlayer.creamNormalToExposed)
            {
                projectile.frame = mPlayer.creamFrame;
                if (mPlayer.creamAnimationReverse)
                {
                    framechangecounter += 1;
                    if (framechangecounter == 15)
                    {
                        mPlayer.creamFrame -= 1;
                        framechangecounter = 0;
                    }
                }
                if (!mPlayer.creamAnimationReverse)
                {
                    framechangecounter += 1;
                    if (framechangecounter == 15)
                    {
                        mPlayer.creamFrame += 1;
                        framechangecounter = 0;
                    }
                }
            }
            if (mPlayer.standAutoMode && !mPlayer.creamVoidMode && !mPlayer.creamExposedMode && !mPlayer.creamExposedToVoid && !mPlayer.creamNormalToExposed)
            {
                BasicPunchAI();
            }
        }

        public override void SelectAnimation()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (attackFrames && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid && !mPlayer.creamExposedMode)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid && !mPlayer.creamExposedMode)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid && !mPlayer.creamExposedMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
            if (mPlayer.creamExposedMode && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid)
            {
                PlayAnimation("Idle2");
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
            if (animationName == "Transform2")
            {
                AnimateStand(animationName, 8, 99999, true);
            }
            if (animationName == "Transform")
            {
                AnimateStand(animationName, 6, 9999, true);
            }
            if (animationName == "Idle2")
            {
                AnimateStand(animationName, 4, 30, true);
            }
        }
    }
}