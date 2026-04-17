using JoJoStands.Projectiles;
using System;
using System.Collections.Generic;
using JoJoStands;
using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        private const int BARRIER_DURATION = 900;
        private const int BARRIER_CD_MAX = 1200;
        private bool barrierActive = false;
        private int barrierTimer = 0;
        private int barrierCooldown = 0;
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
            if (barrierCooldown > 0) barrierCooldown--;

            SnapAbovePlayer(player);
            ApplyStuns();
            CheckTrapTriggers(mPlayer);
            ExpireTraps();

            if (barrierActive)
            {
                barrierTimer++;
                if (barrierTimer >= BARRIER_DURATION)
                {
                    barrierActive = false; barrierTimer = 0; barrierCooldown = BARRIER_CD_MAX;
                    if (barrierProjIdx >= 0 && barrierProjIdx < Main.maxProjectiles)
                        Main.projectile[barrierProjIdx].Kill();
                    barrierProjIdx = -1; Projectile.netUpdate = true;
                }
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                currentAnimationState = AnimationState.Attack;
                RainVisuals(RAIN_W, RAIN_DOWN, RAIN_UP);
                AreaDamage(mPlayer, player);
                PassiveTrapBuilder(RAIN_W, RAIN_DOWN, RAIN_UP);
            }
            else
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft)
                    {
                        currentAnimationState = AnimationState.Attack;
                        FirePrecise(player);
                    }
                    else currentAnimationState = AnimationState.Idle;

                    if (Main.mouseRight && ctrlDropActive == 0)
                    {
                        FireControllableDrop(player);
                        ctrlDropActive = 1;
                    }
                    if (!Main.mouseRight) ctrlDropActive = 0;

                    if (SpecialKeyPressed() && !barrierActive && barrierCooldown <= 0)
                    {
                        barrierActive = true; barrierTimer = 0;
                        barrierProjIdx = Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(), player.Center, Vector2.Zero,
                            ModContent.ProjectileType<RainBarrier>(),
                            newPunchDamage * 4, 0f, Main.myPlayer, Projectile.whoAmI);
                        Main.projectile[barrierProjIdx].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                }
            }

            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
        }
    }
}
