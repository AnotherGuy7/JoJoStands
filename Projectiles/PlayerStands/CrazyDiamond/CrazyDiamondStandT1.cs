using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.CrazyDiamond
{
    public class CrazyDiamondStandT1 : StandClass
    {
        public override int PunchDamage => 21;
        public override int PunchTime => 12;
        public override int HalfStandHeight => 51;
        public override int FistWhoAmI => 12;
        public override int TierNumber => 1;
        public override string PunchSoundName => "Dora";
        public override string PoseSoundName => "CrazyDiamond";
        public override string SpawnSoundName => "Crazy Diamond";
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        public new enum AnimationState
        {
            Idle,
            Attack,
            Flick,
            Healing,
            Pose
        }

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
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    if (Main.mouseLeft)
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch();
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
                if (!attacking)
                    StayBehind();
                if (SpecialKeyPressed(false))
                {
                    restrationMode = !restrationMode;
                    if (restrationMode)
                        Main.NewText("Restoration Mode: Active");
                    else
                        Main.NewText("Restoration Mode: Disabled");
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
                BasicPunchAI();

            if (restrationMode)
            {
                int amountOfDusts = Main.rand.Next(0, 2 + 1);
                for (int i = 0; i < amountOfDusts; i++)
                {
                    int index = Dust.NewDust(Projectile.position - new Vector2(0f, HalfStandHeight), Projectile.width, HalfStandHeight * 2, DustID.IchorTorch, Scale: Main.rand.Next(8, 12) / 10f);
                    Main.dust[index].noGravity = true;
                    Main.dust[index].velocity = new Vector2(Main.rand.Next(-2, 2 + 1) / 10f, Main.rand.Next(-5, -2 + 1) / 10f);
                }

                Lighting.AddLight(Projectile.position, 11);
            }
            if (player.teleporting)
                Projectile.position = player.position;
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }


        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
                Projectile.netUpdate = true;
            }

            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Attack)
                PlayAnimation("Attack");
            else if (currentAnimationState == AnimationState.Flick)
                PlayAnimation("Flick");
            else if (currentAnimationState == AnimationState.Healing)
                PlayAnimation("Heal");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            string pathAddition = "";
            if (restrationMode)
                pathAddition = "Restoration_";

            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/CrazyDiamond", "CrazyDiamond_" + pathAddition + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 4, 12, true);
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