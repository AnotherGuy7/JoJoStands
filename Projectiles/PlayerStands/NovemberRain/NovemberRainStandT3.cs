using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.NovemberRain
{
    public class NovemberRainStandT3 : NovemberRainStandT1
    {
        public override int TierNumber => 3;
        public override int ProjectileDamage => 84;
        public override int ShootTime => 15;
        public override StandAttackType StandType => StandAttackType.Ranged;
        protected override float RAIN_W => 154f + Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standRangeBoosts * 0.5f;
        protected override float RAIN_DOWN => 280f + Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standRangeBoosts * 0.8f;
        protected override float RAIN_UP => 260f + Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standRangeBoosts * 0.4f;
        protected override float RAIN_SLOW => 0.55f;
        protected override int HIT_INTERVAL => 18;
        protected override int TRAP_SPAWN_TICKS => 180;
        protected override int TRAP_BASE_TICKS => 720;
        protected override int TRAP_MAX_TICKS => 1200;
        // Trap
        protected override int MaxFloorCeilingTraps => 10;

        private const int BARRIER_DURATION = 900;
        private const int BARRIER_CD_SECS = 20;
        private bool barrierActive = false;
        private int barrierTimer = 0;
        private int barrierProjIdx = -1;
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

            if (barrierActive)
            {
                barrierTimer++;
                if (barrierTimer >= BARRIER_DURATION)
                {
                    barrierActive = false; barrierTimer = 0;
                    if (barrierProjIdx >= 0 && barrierProjIdx < Main.maxProjectiles)
                        Main.projectile[barrierProjIdx].Kill();
                    barrierProjIdx = -1;
                    // Rain Barrier
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(BARRIER_CD_SECS));
                    Projectile.netUpdate = true;
                }
                else
                {
                    int bRemaining = BARRIER_DURATION - barrierTimer;
                    if (bRemaining > 0) player.AddBuff(ModContent.BuffType<RainBarrierActive>(), bRemaining);
                }
            }

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

                    if (SpecialKeyPressed() && !barrierActive && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                    {
                        barrierActive = true; barrierTimer = 0;
                        barrierProjIdx = Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(), player.Center, Vector2.Zero,
                            ModContent.ProjectileType<RainBarrier>(), newProjectileDamage * 2, 0f, Main.myPlayer, Projectile.whoAmI);
                        Main.projectile[barrierProjIdx].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                }
            }

            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);

            if (barrierActive)
            {
                Player player = Main.player[Projectile.owner];
                MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
                if (barrierProjIdx >= 0 && barrierProjIdx < Main.maxProjectiles)
                    Main.projectile[barrierProjIdx].Kill();
                barrierActive = false; barrierTimer = 0;
                player.ClearBuff(ModContent.BuffType<RainBarrierActive>());
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(BARRIER_CD_SECS));
            }
        }
    }
}
