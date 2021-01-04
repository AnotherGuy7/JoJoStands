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

        public override int punchDamage => 68;
        public override float punchKnockback => 9f;
        public override int punchTime => 26;
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
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (modPlayer.creamVoidMode)
            {
                projectile.hide = true;
            }
            if (modPlayer.creamExposedMode)
            {
                projectile.hide = false;
            }
            if (!modPlayer.creamVoidMode)
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
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !modPlayer.creamVoidMode && !modPlayer.creamExposedMode && !modPlayer.creamExposedToVoid && !modPlayer.creamNormalToExposed)
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
                if (SpecialKeyPressed() && player.ownedProjectileCounts[mod.ProjectileType("Void")] <= 0 && !modPlayer.creamVoidMode && !modPlayer.creamNormalToExposed && !modPlayer.creamExposedToVoid)
                {
                    modPlayer.creamFrame = 0;
                    if (modPlayer.creamExposedMode)
                    {
                        modPlayer.creamExposedToVoid = true;
                    }
                    if (!modPlayer.creamExposedMode)
                    {
                        modPlayer.creamNormalToExposed = true;
                        modPlayer.creamNormalToVoid = true;
                    }
                }
            }
            if (modPlayer.creamNormalToExposed)
            {
                PlayAnimation("Transform");
                if (modPlayer.creamFrame >= 5 && !modPlayer.creamAnimationReverse)
                {
                    if (modPlayer.creamNormalToVoid)
                    {
                        modPlayer.creamExposedToVoid = true;
                        modPlayer.creamNormalToVoid = false;
                    }
                    modPlayer.creamNormalToExposed = false;
                    modPlayer.creamFrame = 0;
                    Main.PlaySound(SoundID.Item78);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("ExposingCream"), 0, 6f, player.whoAmI);
                }
            }
            if (modPlayer.creamExposedToVoid)
            {
                PlayAnimation("Transform2");
                if (modPlayer.creamFrame >= 7 && !modPlayer.creamAnimationReverse)
                {
                    modPlayer.creamExposedToVoid = false;
                    Main.PlaySound(SoundID.Item78);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("Void"), (int)((50 * modPlayer.creamTier) * modPlayer.standDamageBoosts), 6f, player.whoAmI);
                }
                if (modPlayer.creamFrame <= 0 && modPlayer.creamAnimationReverse && modPlayer.creamNormalToExposed)
                {
                    modPlayer.creamNormalToExposed = false;
                    modPlayer.creamAnimationReverse = false;
                }
                if (modPlayer.creamFrame <= 0 && modPlayer.creamAnimationReverse && modPlayer.creamExposedToVoid)
                {
                    modPlayer.creamExposedToVoid = false;
                    if (!modPlayer.creamNormalToVoid)
                    {
                        modPlayer.creamAnimationReverse = false;
                    }
                    if (modPlayer.creamNormalToVoid)
                    {
                        modPlayer.creamFrame = 5;
                        modPlayer.creamNormalToExposed = true;
                        modPlayer.creamNormalToVoid = false;
                    }
                }
            }
            if (modPlayer.creamExposedToVoid || modPlayer.creamVoidMode || modPlayer.creamExposedMode)
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
            if (modPlayer.creamExposedToVoid || modPlayer.creamNormalToExposed)
            {
                projectile.frame = modPlayer.creamFrame;
                if (modPlayer.creamAnimationReverse)
                {
                    framechangecounter += 1;
                    if (framechangecounter == 15)
                    {
                        modPlayer.creamFrame -= 1;
                        framechangecounter = 0;
                    }
                }
                if (!modPlayer.creamAnimationReverse)
                {
                    framechangecounter += 1;
                    if (framechangecounter == 15)
                    {
                        modPlayer.creamFrame += 1;
                        framechangecounter = 0;
                    }
                }
            }
            if (modPlayer.StandAutoMode && !modPlayer.creamVoidMode && !modPlayer.creamExposedMode && !modPlayer.creamExposedToVoid && !modPlayer.creamNormalToExposed)
            {
                BasicPunchAI();
            }
        }

        public override void SelectAnimation()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (attackFrames && !modPlayer.creamNormalToExposed && !modPlayer.creamExposedToVoid && !modPlayer.creamExposedMode)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames && !modPlayer.creamNormalToExposed && !modPlayer.creamExposedToVoid && !modPlayer.creamExposedMode)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode && !modPlayer.creamNormalToExposed && !modPlayer.creamExposedToVoid && !modPlayer.creamExposedMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
            if (modPlayer.creamExposedMode && !modPlayer.creamNormalToExposed && !modPlayer.creamExposedToVoid)
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
                AnimationStates(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 2, true);
            }
            if (animationName == "Transform2")
            {
                AnimationStates(animationName, 8, 99999, true);
            }
            if (animationName == "Transform")
            {
                AnimationStates(animationName, 6, 99999, true);
            }
            if (animationName == "Idle2")
            {
                AnimationStates(animationName, 4, 30, true);
            }
        }
    }
}