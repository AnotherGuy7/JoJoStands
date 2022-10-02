using System.IO;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.CrazyDiamond
{
    public class CrazyDiamondStandT1 : StandClass
    {
        public override int PunchDamage => 21;
        public override int PunchTime => 10;
        public override int HalfStandHeight => 51;
        public override int FistWhoAmI => 12;
        public override int TierNumber => 1;
        public override StandAttackType StandType => StandAttackType.Melee;

        private bool restrationMode = false;

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

            mPlayer.crazyDiamondRestorationMode = restrationMode;
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
                    StayBehind();
                if (SpecialKeyPressedNoCooldown())
                {
                    restrationMode = !restrationMode;
                    if (restrationMode)
                        Main.NewText("Restoration Mode: Active");
                    else
                        Main.NewText("Restoration Mode: Disabled");
                }
            }
            if (restrationMode)
                Lighting.AddLight(Projectile.position, 11);
            if (mPlayer.standAutoMode)
                BasicPunchAI();
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
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().posing)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            string pathAddition = "";
            if (restrationMode)
                pathAddition = "Restoration_";

            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/CrazyDiamond", "/CrazyDiamond_" + pathAddition + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 4, 12, true);
            }
        }
        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(restrationMode);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            restrationMode = reader.ReadBoolean();
        }
    }
}