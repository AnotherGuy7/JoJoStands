using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.SilverChariot
{
    public class SilverChariotAfterImage : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }

        public override void SetDefaults()
        {
            projectile.alpha = 127;
        }

        public override float maxDistance => 98f;
        public override int punchDamage => 32;
        public override int punchTime => 7;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 10f;
        public override int standType => 1;

        public int updateTimer = 0;


        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (modPlayer.Shirtless)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            BasicPunchAI();
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
                standTexture = mod.GetTexture("Projectiles/PlayerStands/SilverChariot/SilverChariot_Shirtless_" + animationName);
            {
                if (animationName == "Idle")
                {
                    AnimationStates(animationName, 4, 30, true);
                }
                if (animationName == "Attack")
                {
                    AnimationStates(animationName, 5, newPunchTime, true);
                }
                if (animationName == "Pose")
                {
                    AnimationStates(animationName, 1, 10, true);
                }
            }
        }
    }
}
