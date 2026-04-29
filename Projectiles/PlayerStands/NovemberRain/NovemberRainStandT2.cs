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
        protected override int TRAP_SPAWN_TICKS => 300;
        protected override int TRAP_BASE_TICKS => 600;
        protected override int TRAP_MAX_TICKS => 900;

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
                }
            }

            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
        }
    }
}
