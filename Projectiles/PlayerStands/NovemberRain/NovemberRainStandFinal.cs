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
    public class NovemberRainStandFinal : NovemberRainStandT1
    {
        public override int TierNumber => 4;
        public override int PunchDamage => 95;
        public override int PunchTime => 5;

        protected override float RAIN_W => 155f;
        protected override float RAIN_DOWN => 280f;
        protected override float RAIN_UP => 260f;
        protected override float RAIN_SLOW => 0.50f;
        protected override int HIT_INTERVAL => 16;
        protected override int PRECISE_CD => 5;

        private const int BARRIER_DURATION = 900;
        private const int BARRIER_CD_MAX = 1200;
        private bool barrierActive = false;
        private int barrierTimer = 0;
        private int barrierCooldown = 0;
        private int barrierProjIdx = -1;

        private const float MAEL_W = 300f;
        private const float MAEL_DOWN = 480f;
        private const float MAEL_UP = 500f;
        private const int MAEL_DURATION = 600;
        private const int MAEL_DRAIN = 330;
        private const int MAEL_CD_MAX = 1800;
        private const int MAEL_HIT_INTERVAL = 14;
        private const float MAEL_SLOW = 0.20f;

        private bool maelActive = false;
        private int maelTimer = 0;
        private int drainTimer = 0;
        private int maelCooldown = 0;
        private int maelVisTimer = 0;
        private int[] maelNpcTimers = new int[Main.maxNPCs];
        private bool[] maelNpcWas = new bool[Main.maxNPCs];

        private int ctrlDropActive = 0;

        private bool InMaelDome(Vector2 pos)
        {
            float dx = Math.Abs(pos.X - Projectile.Center.X);
            float dy = pos.Y - Projectile.Center.Y;
            if (dy <= 0)
            {
                float nx = dx / MAEL_W;
                float ny = (-dy) / MAEL_UP;
                return nx * nx + ny * ny < 1f;
            }
            return dx < MAEL_W && dy < MAEL_DOWN;
        }

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut) Projectile.timeLeft = 2;
            if (barrierCooldown > 0) barrierCooldown--;
            if (maelCooldown > 0) maelCooldown--;
            if (drainTimer > 0) drainTimer--;

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

            if (maelActive)
            {
                maelTimer++;
                MaelstromVisuals();
                MaelstromDamage(mPlayer, player);
                PassiveTrapBuilder(MAEL_W, MAEL_DOWN, MAEL_UP);
                if (maelTimer >= MAEL_DURATION)
                {
                    maelActive = false; maelTimer = 0;
                    drainTimer = MAEL_DRAIN;
                    maelCooldown = MAEL_CD_MAX;
                    for (int i = 0; i < maelNpcTimers.Length; i++) { maelNpcTimers[i] = 0; maelNpcWas[i] = false; }
                    Projectile.netUpdate = true;
                }
            }

            bool canUseRain = drainTimer <= 0 && !maelActive;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                if (canUseRain)
                {
                    currentAnimationState = AnimationState.Attack;
                    RainVisuals(RAIN_W, RAIN_DOWN, RAIN_UP);
                    AreaDamage(mPlayer, player);
                    PassiveTrapBuilder(RAIN_W, RAIN_DOWN, RAIN_UP);
                }
                else currentAnimationState = AnimationState.Idle;
            }
            else
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && canUseRain)
                    {
                        currentAnimationState = AnimationState.Attack;
                        FirePrecise(player);
                    }
                    else currentAnimationState = AnimationState.Idle;

                    if (Main.mouseRight && ctrlDropActive == 0 && canUseRain)
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

                    if (SecondSpecialKeyPressed() && !maelActive && maelCooldown <= 0 && drainTimer <= 0)
                    {
                        maelActive = true; maelTimer = 0; Projectile.netUpdate = true;
                    }
                }
            }

            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
        }

        private void MaelstromVisuals()
        {
            maelVisTimer++;
            if (maelVisTimer < 1) return;
            maelVisTimer = 0;
            Vector2 c = Projectile.Center;
            for (int i = 0; i < 8; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.Pi, MathHelper.TwoPi);
                float r = Main.rand.NextFloat(0.5f, 1.0f);
                int d = Dust.NewDust(new Vector2(c.X + (float)Math.Cos(angle) * MAEL_W * r, c.Y + (float)Math.Sin(angle) * MAEL_UP * r),
                    5, 20, DustID.Water, Main.rand.NextFloat(-0.4f, 0.4f), Main.rand.NextFloat(14f, 22f), 80, default, Main.rand.NextFloat(1.0f, 1.5f));
                Main.dust[d].noGravity = true;
            }
            for (int i = 0; i < 7; i++)
            {
                int d = Dust.NewDust(new Vector2(c.X + Main.rand.NextFloat(-MAEL_W, MAEL_W), c.Y + Main.rand.NextFloat(0f, MAEL_DOWN * 0.6f)),
                    5, 20, DustID.Water, Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(14f, 22f), 80, default, Main.rand.NextFloat(1.0f, 1.5f));
                Main.dust[d].noGravity = true;
            }
            Lighting.AddLight(Projectile.Center, 0.1f, 0.25f, 0.5f);
        }

        private void MaelstromDamage(MyPlayer mPlayer, Player player)
        {
            if (Projectile.owner != Main.myPlayer) return;
            float boost = Math.Max(0.5f, mPlayer.standDamageBoosts);
            int interval = Math.Max((int)(MAEL_HIT_INTERVAL / boost), 8);
            int dmgBase = (int)(newPunchDamage * 1.35f);

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage) { maelNpcTimers[i] = 0; maelNpcWas[i] = false; continue; }
                bool inArea = InMaelDome(npc.Center);
                if (inArea)
                {
                    if (!maelNpcWas[i]) { maelNpcWas[i] = true; maelNpcTimers[i] = interval - 1; if (!npc.boss) npc.velocity *= MAEL_SLOW; }
                    maelNpcTimers[i]++;
                    if (maelNpcTimers[i] >= interval)
                    {
                        maelNpcTimers[i] = 0;
                        if (!npc.boss) npc.velocity *= MAEL_SLOW;
                        bool crit = Main.rand.Next(100) < player.GetTotalCritChance<MeleeDamageClass>();
                        int dmg = crit ? dmgBase * 2 : dmgBase;
                        npc.SimpleStrikeNPC(dmg, Projectile.direction, crit: crit, knockBack: 1.2f);
                        npc.velocity += new Vector2(Projectile.direction * 0.8f, 0.4f);
                        for (int d = 0; d < 5; d++)
                            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Water, Main.rand.NextFloat(-3f, 3f), -2f, 0, default, 1.1f);
                        Projectile.netUpdate = true;
                    }
                }
                else { maelNpcTimers[i] = 0; maelNpcWas[i] = false; }
            }
        }
    }
}
