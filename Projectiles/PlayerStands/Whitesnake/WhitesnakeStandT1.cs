using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.Whitesnake
{
    public class WhitesnakeStandT1 : StandClass
    {
        public override int punchDamage => 16;
        public override int punchTime => 14;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 9f;
        public override int standType => 1;
        public override int standOffset => -20;
        public override float maxDistance => 147f;      //1.5x the normal range cause Whitesnake is considered a long-range stand with melee capabilities
        public override string poseSoundName => "YouWereTwoSecondsTooLate";
        public override string spawnSoundName => "Whitesnake";

        private int updateTimer = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                Projectile.netUpdate = true;
            }

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
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
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = Mod.GetTexture("Projectiles/PlayerStands/Whitesnake/Whitesnake_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 3, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 10, true);
            }
        }
    }
}