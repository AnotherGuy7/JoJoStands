using JoJoStands.Projectiles;
using JoJoStands.Buffs.Debuffs;
using System;
using JoJoStands;
using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.NovemberRain
{
    public class NovemberRainStandT3 : NovemberRainStandT1
    {
        public override int TierNumber => 3;
        public override int PunchDamage => 68;
        public override int PunchTime => 6;
        protected override float RAIN_W => 155f;
        protected override float RAIN_DOWN => 280f;
        protected override float RAIN_UP => 260f;
        protected override float RAIN_SLOW => 0.55f;
        protected override int HIT_INTERVAL => 18;
        protected override int PRECISE_CD => 6;
        protected override int TRAP_SPAWN_TICKS => 180;
        protected override int TRAP_BASE_TICKS => 720;
        protected override int TRAP_MAX_TICKS => 1200;

        private const int BARRIER_DURATION = 900;
        private const int BARRIER_CD_SECS = 20;
        private bool barrierActive = false;
        private int barrierTimer = 0;
        private int barrierProjIdx = -1;
        private int ctrlDropActive = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut) Projectile.timeLeft = 2;
            if (preciseTimer > 0) preciseTimer--;

            SnapAbovePlayer(player);
            ApplyStuns();
            UpdateTraps(mPlayer, player);
            CheckTrapTriggers(mPlayer);

            if (barrierActive)
            {
                barrierTimer++;
                if (barrierTimer >= BARRIER_DURATION)
                {
                    barrierActive = false; barrierTimer = 0;
                    if (barrierProjIdx >= 0 && barrierProjIdx < Main.maxProjectiles)
                        Main.projectile[barrierProjIdx].Kill();
                    barrierProjIdx = -1;
                    player.AddBuff(ModContent.BuffType<RainBarrierCooldown>(), mPlayer.AbilityCooldownTime(BARRIER_CD_SECS));
                    Projectile.netUpdate = true;
                }
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                currentAnimationState = AnimationState.Idle;
                RainVisuals(RAIN_W, RAIN_DOWN, RAIN_UP, Projectile.Center);
                AreaDamage(mPlayer, player, RAIN_W, RAIN_DOWN, RAIN_UP, HIT_INTERVAL, RAIN_SLOW, Projectile.Center);
                BuildTraps(player);
            }
            else
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && preciseTimer <= 0)
                    {
                        currentAnimationState = AnimationState.Idle;
                        FirePrecise(mPlayer);
                        preciseTimer = PRECISE_CD;
                    }
                    else currentAnimationState = AnimationState.Idle;

                    if (Main.mouseRight && ctrlDropActive == 0)
                    {
                        FireControllableDrop(mPlayer);
                        ctrlDropActive = 1;
                    }
                    if (!Main.mouseRight) ctrlDropActive = 0;

                    if (SpecialKeyPressed() && !barrierActive && !player.HasBuff(ModContent.BuffType<RainBarrierCooldown>()))
                    {
                        barrierActive = true; barrierTimer = 0;
                        barrierProjIdx = Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(), player.Center, Vector2.Zero,
                            ModContent.ProjectileType<RainBarrier>(), newPunchDamage * 2, 0f, Main.myPlayer, Projectile.whoAmI);
                        Main.projectile[barrierProjIdx].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                }
            }

            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
        }
    }
}
