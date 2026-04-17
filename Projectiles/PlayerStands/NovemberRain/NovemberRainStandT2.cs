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
    public class NovemberRainStandT2 : NovemberRainStandT1
    {
        public override int TierNumber => 2;
        public override int PunchDamage => 35;
        public override int PunchTime => 7;

        protected override float RAIN_W => 155f;
        protected override float RAIN_DOWN => 280f;
        protected override float RAIN_UP => 260f;
        protected override float RAIN_SLOW => 0.60f;
        protected override int HIT_INTERVAL => 20;
        protected override int PRECISE_CD => 7;

        private int ctrlDropActive = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut) Projectile.timeLeft = 2;

            SnapAbovePlayer(player);
            ApplyStuns();
            CheckTrapTriggers(mPlayer);
            ExpireTraps();

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
                }
            }

            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
        }
    }
}
