using System;
using Terraria;

namespace JoJoStands.Projectiles.PlayerStands.NovemberRain
{
    public class NovemberRainStandT2 : NovemberRainStandT1
    {
        public override int TierNumber => 2;
        public override int ProjectileDamage => 51;
        public override int ShootTime => 20;
        public override StandAttackType StandType => StandAttackType.Ranged;
        protected override float RAIN_W => 154f + Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standRangeBoosts * 0.5f;
        protected override float RAIN_DOWN => 280f + Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standRangeBoosts * 0.8f;
        protected override float RAIN_UP => 260f + Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standRangeBoosts * 0.4f;
        protected override float RAIN_SLOW => 0.60f;
        protected override int HIT_INTERVAL => 20;
        protected override int TRAP_SPAWN_TICKS => 300;
        protected override int TRAP_BASE_TICKS => 600;
        protected override int TRAP_MAX_TICKS => 900;
        // Trap
        protected override int MaxFloorCeilingTraps => 8;
        protected override float CONTROLABLE_RANGE => 250f;

        private int ctrlDropActive = 0;

        public override void AI()
        {
            if (!summonAnimDone)
                currentAnimationState = AnimationState.Summon;

            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut) Projectile.timeLeft = 2;

            SnapAbovePlayer(player);
            UpdateLegs(player);
            ApplyStuns();
            UpdateTraps(mPlayer, player);
            CheckTrapTriggers(mPlayer);

            if (!summonAnimDone)
                return;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                currentAnimationState = AnimationState.Idle;
                RainVisuals(RAIN_W, RAIN_DOWN, RAIN_UP, player.Center);
                AreaDamage(mPlayer, player, RAIN_W, RAIN_DOWN, RAIN_UP, HIT_INTERVAL, RAIN_SLOW, player.Center);
                BuildTraps(player);
            }
            else
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    bool leftPressed = PlayerLeftClick();
                    bool rightPressed = Main.mouseRight;

                    if (leftPressed && shootCount <= 0 && !HasActiveControlledControllableDrop())
                    {
                        currentAnimationState = AnimationState.Idle;
                        FireThreeStreams(mPlayer);
                        shootCount += newShootTime;
                    }
                    else currentAnimationState = AnimationState.Idle;

                    if (rightPressed && ctrlDropActive == 0 && !leftPressed)
                    {
                        FireControllableDrop(mPlayer);
                        ctrlDropActive = 1;
                    }
                    if (!rightPressed) ctrlDropActive = 0;
                }
            }

            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
        }
    }
}
