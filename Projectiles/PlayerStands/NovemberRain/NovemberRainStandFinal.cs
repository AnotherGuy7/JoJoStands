using JoJoStands.Projectiles;
using JoJoStands.Buffs.Debuffs;
using System;
using System.Collections.Generic;
using JoJoStands;
using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework;
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
        protected override int TRAP_SPAWN_TICKS => 120;
        protected override int TRAP_BASE_TICKS => 900;
        protected override int TRAP_MAX_TICKS => 1500;

        private const int BARRIER_DURATION = 900;
        private const int BARRIER_CD_SECS = 20;
        private bool barrierActive = false;
        private int barrierTimer = 0;
        private int barrierProjIdx = -1;

        private const float MAEL_W = 300f;
        private const float MAEL_DOWN = 480f;
        private const float MAEL_UP = 500f;
        private const int MAEL_DURATION = 600;
        private const int MAEL_DRAIN_SECS = 5;
        private const int MAEL_CD_SECS = 30;
        private const int MAEL_HIT_INTERVAL = 14;
        private const float MAEL_SLOW = 0.20f;

        private bool maelActive = false;
        private int maelTimer = 0;
        private int maelVisTimer = 0;
        private int[] maelNpcTimers = new int[Main.maxNPCs];
        private bool[] maelNpcWas = new bool[Main.maxNPCs];
        private int ctrlDropActive = 0;
        private int maelTrapFormTimer = 0;

        private bool InMaelDome(Vector2 pos)
        {
            float dx = Math.Abs(pos.X - Projectile.Center.X);
            float dy = pos.Y - Projectile.Center.Y;
            if (dy <= 0) { float nx = dx / MAEL_W; float ny = (-dy) / MAEL_UP; return nx * nx + ny * ny < 1f; }
            return dx < MAEL_W && dy < MAEL_DOWN;
        }

        private bool IsSolidF(int tx, int ty) { if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) return false; var t = Main.tile[tx, ty]; return t.HasTile && (Main.tileSolid[t.TileType] || TileID.Sets.Platforms[t.TileType]); }
        private bool IsPlatformF(int tx, int ty) { if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) return false; var t = Main.tile[tx, ty]; return t.HasTile && TileID.Sets.Platforms[t.TileType]; }
        private bool IsAirF(int tx, int ty) { if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) return true; var t = Main.tile[tx, ty]; return !t.HasTile || (!Main.tileSolid[t.TileType] && !TileID.Sets.Platforms[t.TileType]); }
        private bool HasWallF(int tx, int ty) { if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) return false; return Main.tile[tx, ty].WallType > 0; }

        private List<TrapSurface> ScanMaelSurfaces(Vector2 playerCenter)
        {
            var surfaces = new List<TrapSurface>();
            int cx = (int)(Projectile.Center.X / 16f), cy = (int)(Projectile.Center.Y / 16f);
            int rX = (int)(MAEL_W / 16f) + 2, rDown = (int)(MAEL_DOWN / 16f) + 2, rUp = (int)(MAEL_UP / 16f) + 2;
            int playerTileY = (int)(playerCenter.Y / 16f);
            for (int tx = cx - rX; tx <= cx + rX; tx++)
            {
                for (int ty = cy - rUp; ty <= cy + rDown; ty++)
                {
                    if (tx < 0 || tx >= Main.maxTilesX || ty < 0 || ty >= Main.maxTilesY) continue;
                    if (!InMaelDome(new Vector2(tx * 16f + 8f, ty * 16f + 8f))) continue;
                    bool solid = IsSolidF(tx, ty);
                    if (solid)
                    {
                        if (ty >= playerTileY && IsAirF(tx, ty - 1))
                            surfaces.Add(new TrapSurface { WorldPos = new Vector2(tx * 16f + 8f, ty * 16f), Type = SurfaceType.Floor, DripTimerOffset = Main.rand.Next(0, TRAP_CYCLE) });
                        else if (ty < playerTileY && IsAirF(tx, ty + 1))
                        {
                            bool isPlatformTile = IsPlatformF(tx, ty);
                            float ceilBottomY = isPlatformTile ? ty * 16f + 8f : ty * 16f + 16f;
                            int landY = 0;
                            int stx = tx, startTY = ty + 1;
                            for (int scanTy = startTY; scanTy < startTY + 60; scanTy++)
                            {
                                if (scanTy < 0 || scanTy >= Main.maxTilesY) break;
                                var tt = Main.tile[stx, scanTy];
                                if (tt.HasTile && (Main.tileSolid[tt.TileType] || TileID.Sets.Platforms[tt.TileType])) { landY = scanTy * 16; break; }
                            }
                            if (landY == 0) landY = (ty + 1) * 16 + 200;
                            surfaces.Add(new TrapSurface { WorldPos = new Vector2(tx * 16f + 8f, ceilBottomY), Type = SurfaceType.Ceiling, DripLandY = landY, DripLandFound = true, DripTimerOffset = Main.rand.Next(0, TRAP_CYCLE) });
                        }
                    }
                    else if (!solid && HasWallF(tx, ty))
                        surfaces.Add(new TrapSurface { WorldPos = new Vector2(tx * 16f + 8f, ty * 16f + 8f), Type = SurfaceType.BackWall, DripTimerOffset = Main.rand.Next(0, TRAP_CYCLE) });
                }
            }
            return surfaces;
        }

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

            bool hasDrain = player.HasBuff(ModContent.BuffType<MaelstromDrain>());
            bool hasMaelCD = player.HasBuff(ModContent.BuffType<MaelstromCooldown>());
            bool hasBarrierCD = player.HasBuff(ModContent.BuffType<RainBarrierCooldown>());

            if (barrierActive)
            {
                barrierTimer++;
                if (barrierTimer >= BARRIER_DURATION)
                {
                    barrierActive = false; barrierTimer = 0;
                    if (barrierProjIdx >= 0 && barrierProjIdx < Main.maxProjectiles) Main.projectile[barrierProjIdx].Kill();
                    barrierProjIdx = -1;
                    player.AddBuff(ModContent.BuffType<RainBarrierCooldown>(), mPlayer.AbilityCooldownTime(BARRIER_CD_SECS));
                    Projectile.netUpdate = true;
                }
            }

            if (maelActive)
            {
                maelTimer++;
                MaelstromVisuals();
                MaelstromDamage(mPlayer, player);
                MaelstromTraps(player);
                if (maelTimer >= MAEL_DURATION)
                {
                    maelActive = false; maelTimer = 0;
                    for (int i = 0; i < maelNpcTimers.Length; i++) { maelNpcTimers[i] = 0; maelNpcWas[i] = false; }
                    player.AddBuff(ModContent.BuffType<MaelstromDrain>(), MAEL_DRAIN_SECS * 60);
                    player.AddBuff(ModContent.BuffType<MaelstromCooldown>(), mPlayer.AbilityCooldownTime(MAEL_CD_SECS));
                    Projectile.netUpdate = true;
                }
            }

            bool canUseRain = !hasDrain && !maelActive;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                if (canUseRain)
                {
                    currentAnimationState = AnimationState.Idle;
                    RainVisuals(RAIN_W, RAIN_DOWN, RAIN_UP, Projectile.Center);
                    AreaDamage(mPlayer, player, RAIN_W, RAIN_DOWN, RAIN_UP, HIT_INTERVAL, RAIN_SLOW, Projectile.Center);
                    BuildTraps(player);
                }
                else currentAnimationState = AnimationState.Idle;
            }
            else
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && preciseTimer <= 0 && canUseRain) { currentAnimationState = AnimationState.Idle; FirePrecise(mPlayer); preciseTimer = PRECISE_CD; }
                    else currentAnimationState = AnimationState.Idle;

                    if (Main.mouseRight && ctrlDropActive == 0 && canUseRain) { FireControllableDrop(mPlayer); ctrlDropActive = 1; }
                    if (!Main.mouseRight) ctrlDropActive = 0;

                    if (SpecialKeyPressed() && !barrierActive && !hasBarrierCD)
                    {
                        barrierActive = true; barrierTimer = 0;
                        barrierProjIdx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Vector2.Zero,
                            ModContent.ProjectileType<RainBarrier>(), newPunchDamage * 2, 0f, Main.myPlayer, Projectile.whoAmI);
                        Main.projectile[barrierProjIdx].netUpdate = true; Projectile.netUpdate = true;
                    }

                    if (SecondSpecialKeyPressed() && !maelActive && !hasMaelCD && !hasDrain)
                    { maelActive = true; maelTimer = 0; maelTrapFormTimer = 0; Projectile.netUpdate = true; }
                }
            }

            if (mPlayer.posing) currentAnimationState = AnimationState.Pose;
        }

        private void MaelstromTraps(Player player)
        {
            if (Projectile.owner != Main.myPlayer || ActiveTraps.Count >= MAX_TRAP_AREAS) return;
            maelTrapFormTimer++;
            if (maelTrapFormTimer < TRAP_SPAWN_TICKS) return;
            maelTrapFormTimer = 0;
            var surfaces = ScanMaelSurfaces(player.Center);
            if (surfaces.Count == 0) return;
            ActiveTraps.Add(new TrapArea { Center = Projectile.Center, DurationTicks = TRAP_BASE_TICKS, BaseDuration = TRAP_BASE_TICKS, MaxDuration = TRAP_MAX_TICKS, Surfaces = surfaces, PlayerNearby = true });
            Projectile.netUpdate = true;
        }

        private void MaelstromVisuals()
        {
            maelVisTimer++;
            if (maelVisTimer < 1) return;
            maelVisTimer = 0;
            Vector2 c = Projectile.Center;
            for (int i = 0; i < 8; i++) { float a = Main.rand.NextFloat(MathHelper.Pi, MathHelper.TwoPi); float r = Main.rand.NextFloat(0.5f, 1.0f); int d = Dust.NewDust(new Vector2(c.X + (float)Math.Cos(a) * MAEL_W * r, c.Y + (float)Math.Sin(a) * MAEL_UP * r), 5, 20, DustID.Water, Main.rand.NextFloat(-0.4f, 0.4f), Main.rand.NextFloat(14f, 22f), 80, default, Main.rand.NextFloat(1.0f, 1.5f)); Main.dust[d].noGravity = true; }
            for (int i = 0; i < 7; i++) { int d = Dust.NewDust(new Vector2(c.X + Main.rand.NextFloat(-MAEL_W, MAEL_W), c.Y + Main.rand.NextFloat(0f, MAEL_DOWN * 0.6f)), 5, 20, DustID.Water, Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(14f, 22f), 80, default, Main.rand.NextFloat(1.0f, 1.5f)); Main.dust[d].noGravity = true; }
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
                if (InMaelDome(npc.Center))
                {
                    if (!maelNpcWas[i]) { maelNpcWas[i] = true; maelNpcTimers[i] = interval - 1; if (!npc.boss) npc.velocity *= MAEL_SLOW; }
                    maelNpcTimers[i]++;
                    if (maelNpcTimers[i] >= interval)
                    {
                        maelNpcTimers[i] = 0;
                        if (!npc.boss) npc.velocity *= MAEL_SLOW;
                        bool crit = Main.rand.Next(100) < player.GetTotalCritChance<MeleeDamageClass>();
                        npc.SimpleStrikeNPC(dmgBase, Projectile.direction, crit: crit, knockBack: 1.2f);
                        npc.velocity += new Vector2(Projectile.direction * 0.8f, 0.4f);
                        for (int d = 0; d < 5; d++) Dust.NewDust(npc.position, npc.width, npc.height, DustID.Water, Main.rand.NextFloat(-3f, 3f), -2f, 0, default, 1.1f);
                        Projectile.netUpdate = true;
                    }
                }
                else { maelNpcTimers[i] = 0; maelNpcWas[i] = false; }
            }
        }
    }
}
