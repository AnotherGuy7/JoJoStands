using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.StickyFingers
{
    public class StickyFingersStandT3 : StandClass
    {
        public override int punchDamage => 60;
        public override int altDamage => 52;
        public override int punchTime => 10;
        public override int halfStandHeight => 39;
        public override float fistWhoAmI => 4f;
        public override float tierNumber => 3f;
        public override int standType => 1;
        public override string punchSoundName => "Ari";
        public override string poseSoundName => "Arrivederci";
        public override string spawnSoundName => "Sticky Fingers";

        private int updateTimer = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!mPlayer.standAutoMode)
            {
                secondaryAbilityFrames = player.ownedProjectileCounts[mod.ProjectileType("StickyFingersFistExtended")] != 0;
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && player.ownedProjectileCounts[mod.ProjectileType("StickyFingersFistExtended")] == 0)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    if (!secondaryAbilityFrames)
                        StayBehind();
                    else
                        GoInFront();
                }
                if (Main.mouseRight && shootCount <= 0 && player.ownedProjectileCounts[mod.ProjectileType("StickyFingersFistExtended")] == 0 && projectile.owner == Main.myPlayer)
                {
                    shootCount += 120;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("StickyFingersFistExtended"), (int)(altDamage * mPlayer.standDamageBoosts), 6f, projectile.owner, projectile.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                    projectile.netUpdate = true;
                }
                if (SpecialKeyPressed() && shootCount <= 0 && !secondaryAbilityFrames && player.ownedProjectileCounts[mod.ProjectileType("StickyFingersZipperPoint")] == 0)
                {
                    shootCount += 20;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed * 3;
                    int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("StickyFingersZipperPoint"), 0, 0f, projectile.owner, 0f);
                    Main.projectile[proj].netUpdate = true;
                    projectile.netUpdate = true;
                }
            }
            if (mPlayer.standAutoMode)
            {
                PunchAndShootAI(mod.ProjectileType("StickyFingersFistExtended"), shootMax: 1);
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
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
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
                standTexture = mod.GetTexture("Projectiles/PlayerStands/StickyFingers/StickyFingers_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 1, 10, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 10, true);
            }
        }
    }
}