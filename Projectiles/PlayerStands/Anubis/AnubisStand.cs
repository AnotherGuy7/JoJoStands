using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Anubis
{
    public abstract class AnubisStand : StandClass
    {
        public bool IsStandActive => Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standOut
                             && IsHoldingMeleeWeapon(Main.player[Projectile.owner]);

        public abstract int MaxAdaptationStacks { get; }
        private const int StackDecayDelay = 120;
        private const int StackDecayRate = 30;

        private const float DamagePerStack = 0.01f;
        private const float SpeedPerStack = 0.008f;
        private const float CritPerStack = 0.20f;

        private const int ArmorShredHitsPerPoint = 3;
        private const int ArmorShredMaxReduction = 20;
        private const int ParryPerfectDuration = 20;
        private const int ParryGoodDuration = 60;
        private const int ParryCooldown = 150;
        private int _parryPerfectTimer;
        private int _parryGoodTimer;
        private const float ParryRadius = 180f;
        private const int DashDuration = 14;
        private const float DashSpeed = 22f;
        private const int DashCooldown = 90;

        public abstract float DashDamageMultiplier { get; }
        public abstract float BaseMeleeDamageBonusValue { get; }
        public abstract float BaseMeleeSpeedBonusValue { get; }
        public abstract float BaseCritBonusValue { get; }

        public int AdaptationStacks { get; private set; }
        private int _stackDecayTimer;
        private int _stackDecayTickTimer;
        private readonly Dictionary<int, int> _armorShredBuildup = new Dictionary<int, int>();
        private readonly Dictionary<int, int> _armorShredApplied = new Dictionary<int, int>();
        private int _parryCooldownTimer;
        private int _parryCritTimer;
        private int _dashCooldownTimer;
        private int _dashTimer;
        private Vector2 _dashDirection;
        private enum AnubisAnim : byte { Idle, Dash, Parry }
        private AnubisAnim _animState = AnubisAnim.Idle;
        private AnubisAnim _oldAnimState = AnubisAnim.Idle;

        protected abstract string TextureRoot { get; }
        protected virtual int IdleFrameCount => 2;
        protected virtual int IdleFrameSpeed => 12;
        public override int PunchDamage => 0;
        public override int ProjectileDamage => 0;
        public override int PunchTime => 60;
        public override int ShootTime => 60;
        public override StandAttackType StandType => StandAttackType.None;
        public override bool? CanCutTiles() => false;

        private int _lastHitNPCIndex = -1;

        public void RegisterMeleeHit(int npcWhoAmI)
        {
            if (_lastHitNPCIndex != -1 && _lastHitNPCIndex != npcWhoAmI)
            {
                AdaptationStacks = 0;
                Projectile.netUpdate = true;
            }

            _lastHitNPCIndex = npcWhoAmI;
            AddAdaptationStack();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 2;
        }

        public override void SelectAnimation()
        {
            if (_oldAnimState != _animState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                _oldAnimState = _animState;
                Projectile.netUpdate = true;
            }

            switch (_animState)
            {
                case AnubisAnim.Idle: PlayAnimation("Idle"); break;
                case AnubisAnim.Dash: PlayAnimation("Dash"); break;
                case AnubisAnim.Parry: PlayAnimation("Parry"); break;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                standTexture = (Texture2D)ModContent.Request<Texture2D>(
                    TextureRoot + "_" + animationName);

                int frameCount = animationName == "Idle" ? IdleFrameCount : 4;
                Projectile.height = standTexture.Height / frameCount;
            }

            int fc = animationName == "Idle" ? IdleFrameCount : 4;
            AnimateStand(animationName, fc, IdleFrameSpeed, true);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            UpdateStandInfo();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            bool holdingMelee = IsHoldingMeleeWeapon(player);

            if (mPlayer.standOut && holdingMelee)
            {
                ApplyPlayerBoosts(player, mPlayer);
                if (Projectile.owner == Main.myPlayer)
                    player.AddBuff(ModContent.BuffType<AnubisAdaptationBuff>(), 2);
            }

            if (Projectile.owner == Main.myPlayer)
            {
                TickCooldowns();

                if (holdingMelee)
                {
                    TickStackDecay();

                    if (_dashTimer > 0)
                        TickDash(player);

                    if (SpecialKeyPressed(false) && _dashCooldownTimer <= 0 && _dashTimer <= 0)
                        StartDash(player);

                    if (Main.mouseRight && _parryCooldownTimer <= 0 && Projectile.owner == Main.myPlayer)
                        TryParry(player);
                }
                else
                {
                    DecayOneStack();
                }
            }

            if (_dashTimer > 0)
                _animState = AnubisAnim.Dash;
            else if (IsInParryAnyWindow() || _parryCooldownTimer > ParryCooldown - 20)
                _animState = AnubisAnim.Parry;
            else
                _animState = AnubisAnim.Idle;

            SelectAnimation();

            if (_dashTimer <= 0)
                FollowBehindPlayer(player);

            if (IsInParryAnyWindow() && Main.GameUpdateCount % 12 == 0 && Main.netMode != NetmodeID.Server)
                SpawnCritWindowDust();

            UpdateStandSync();
        }

        private void ApplyPlayerBoosts(Player player, MyPlayer mPlayer)
        {
            bool holdingAnubisBlade = player.HeldItem.type == ModContent.ItemType<AnubisBladeItem>();
            float boostMultiplier = holdingAnubisBlade ? 1.3f : 1.0f;

            float stackDamageBonus = Math.Min(AdaptationStacks * DamagePerStack, 1.0f);
            float totalDamageBonus = (BaseMeleeDamageBonusValue + stackDamageBonus) * boostMultiplier;
            mPlayer.standDamageBoosts += totalDamageBonus;

            float stackSpeedBonus = AdaptationStacks * SpeedPerStack;
            int speedTicks = (int)((BaseMeleeSpeedBonusValue + stackSpeedBonus * 60f) * boostMultiplier);
            mPlayer.standSpeedBoosts += speedTicks;

            float stackCritBonus = AdaptationStacks * CritPerStack;
            float totalCrit = (BaseCritBonusValue + stackCritBonus) * boostMultiplier;
            mPlayer.standCritChangeBoosts += totalCrit;
        }

        private static bool IsHoldingMeleeWeapon(Player player)
        {
            Item held = player.HeldItem;
            if (held == null || held.IsAir) return false;
            return held.DamageType == DamageClass.Melee
                || held.DamageType == DamageClass.MeleeNoSpeed;
        }

        public void AddAdaptationStack()
        {
            AdaptationStacks = Math.Min(AdaptationStacks + 1, MaxAdaptationStacks);
            _stackDecayTimer = StackDecayDelay;
            _stackDecayTickTimer = StackDecayRate;
            Projectile.netUpdate = true;
        }

        public int GetAndApplyArmorShred(NPC target)
        {
            int id = target.whoAmI;
            if (!_armorShredBuildup.ContainsKey(id)) _armorShredBuildup[id] = 0;
            if (!_armorShredApplied.ContainsKey(id)) _armorShredApplied[id] = 0;

            _armorShredBuildup[id]++;

            int newPoints = _armorShredBuildup[id] / ArmorShredHitsPerPoint;
            int currentApplied = _armorShredApplied[id];
            int toApply = Math.Min(newPoints - currentApplied,
                                          ArmorShredMaxReduction - currentApplied);

            if (toApply > 0)
            {
                _armorShredApplied[id] += toApply;
                SpawnShredDust(target.Center);
                Projectile.netUpdate = true;
            }

            return toApply;
        }

        private void TickStackDecay()
        {
            if (AdaptationStacks <= 0) return;

            if (_stackDecayTimer > 0)
            {
                _stackDecayTimer--;
                return;
            }

            _stackDecayTickTimer--;
            if (_stackDecayTickTimer <= 0)
            {
                _stackDecayTickTimer = StackDecayRate;
                DecayOneStack();
            }
        }

        private void DecayOneStack()
        {
            if (AdaptationStacks <= 0) return;
            AdaptationStacks--;
            Projectile.netUpdate = true;
        }

        private void TickCooldowns()
        {
            if (_parryCooldownTimer > 0) _parryCooldownTimer--;
            if (_dashCooldownTimer > 0) _dashCooldownTimer--;

            if (_parryPerfectTimer > 0)
            {
                _parryPerfectTimer--;
                if (_parryPerfectTimer == 0)
                    _parryGoodTimer = ParryGoodDuration;
            }
            else if (_parryGoodTimer > 0)
                _parryGoodTimer--;
        }

        private void TryParry(Player player)
        {
            _parryPerfectTimer = ParryPerfectDuration;
            _parryGoodTimer = 0;
            _parryCooldownTimer = ParryCooldown;

            int stackBonus = MaxAdaptationStacks / 2;
            AdaptationStacks = Math.Min(AdaptationStacks + stackBonus, MaxAdaptationStacks);
            _stackDecayTimer = StackDecayDelay;

            SoundEngine.PlaySound(SoundID.Item37, player.Center);
            SpawnParryDust(player.Center);
            Projectile.netUpdate = true;
        }

        public bool IsInParryPerfectWindow() => _parryPerfectTimer > 0;
        public bool IsInParryGoodWindow() => _parryPerfectTimer == 0 && _parryGoodTimer > 0;
        public bool IsInParryAnyWindow() => _parryPerfectTimer > 0 || _parryGoodTimer > 0;

        private static Projectile FindParryTarget(Player player)
        {
            Projectile best = null;
            float bestDist = ParryRadius;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (!p.active || p.friendly || !p.hostile) continue;
                float d = Vector2.Distance(p.Center, player.Center);
                if (d < bestDist) { bestDist = d; best = p; }
            }
            return best;
        }

        private void StartDash(Player player)
        {
            Vector2 toCursor = Main.MouseWorld - player.Center;
            if (toCursor == Vector2.Zero) toCursor = new Vector2(player.direction, 0f);
            toCursor.Normalize();

            _dashDirection = toCursor;
            _dashTimer = DashDuration;
            _dashCooldownTimer = DashCooldown;

            SoundEngine.PlaySound(SoundID.Item1, player.Center);
            SpawnDashDust(player.Center);
            Projectile.netUpdate = true;
        }

        private void TickDash(Player player)
        {
            _dashTimer--;
            player.velocity = _dashDirection * DashSpeed;
            player.immune = true;
            player.immuneTime = Math.Max(player.immuneTime, 4);

            int dashDmg = (int)(player.HeldItem.damage
                                * DashDamageMultiplier
                                * (1f + DamagePerStack * AdaptationStacks));

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (!n.active || n.friendly || !n.CanBeChasedBy()) continue;
                if (Vector2.Distance(player.Center, n.Center) > 48f) continue;

                if (!_armorShredApplied.ContainsKey(n.whoAmI))
                    _armorShredApplied[n.whoAmI] = 0;
                int shredOnDash = Math.Min(6, ArmorShredMaxReduction - _armorShredApplied[n.whoAmI]);
                if (shredOnDash > 0)
                {
                    n.defense = Math.Max(0, n.defense - shredOnDash);
                    _armorShredApplied[n.whoAmI] += shredOnDash;
                }

                n.StrikeNPC(n.CalculateHitInfo(dashDmg, player.direction, false, 8f, DamageClass.Melee, false));
                SpawnDashHitDust(n.Center);
                break;
            }

            if (_dashTimer <= 0)
                player.velocity *= 0.3f;
        }

        public bool IsInParryCritWindow() => _parryCritTimer > 0;

        public void OnNPCKilled(NPC npc)
        {
            _armorShredBuildup.Remove(npc.whoAmI);
            _armorShredApplied.Remove(npc.whoAmI);

            if (_lastHitNPCIndex == npc.whoAmI)
                _lastHitNPCIndex = -1;
        }

        private void FollowBehindPlayer(Player player)
        {
            const float BehindOffset = 24f;
            const float FollowLerp = 0.35f;

            float dirSign = player.direction;
            Vector2 target = player.Center + new Vector2(-dirSign * BehindOffset, 0f);

            if (Vector2.Distance(Projectile.Center, target) > 16f * 20f)
            {
                Projectile.Center = target;
                Projectile.velocity = Vector2.Zero;
            }
            else
                Projectile.velocity = (target - Projectile.Center) * FollowLerp;

            Projectile.spriteDirection = player.direction;
            Projectile.rotation = 0f;
            Projectile.tileCollide = false;
            Projectile.netUpdate = true;
        }

        private static void SpawnParryDust(Vector2 center)
        {
            for (int k = 0; k < 20; k++)
            {
                float rot = MathHelper.ToRadians(360f / 20f * k);
                int di = Dust.NewDust(center + rot.ToRotationVector2() * 30f, 1, 1, DustID.GoldFlame);
                Main.dust[di].noGravity = true;
                Main.dust[di].velocity = rot.ToRotationVector2() * 4f;
            }
        }

        private static void SpawnDashDust(Vector2 center)
        {
            for (int k = 0; k < 12; k++)
            {
                int di = Dust.NewDust(center + Main.rand.NextVector2Circular(20f, 20f), 1, 1, DustID.Shadowflame);
                Main.dust[di].noGravity = true;
                Main.dust[di].velocity *= 2f;
            }
        }

        private static void SpawnDashHitDust(Vector2 center)
        {
            for (int k = 0; k < 8; k++)
            {
                int di = Dust.NewDust(center + Main.rand.NextVector2Circular(16f, 16f), 1, 1, DustID.Shadowflame);
                Main.dust[di].noGravity = true;
            }
        }

        private static void SpawnShredDust(Vector2 center)
        {
            for (int k = 0; k < 5; k++)
            {
                int di = Dust.NewDust(center + Main.rand.NextVector2Circular(12f, 12f), 1, 1, DustID.Iron);
                Main.dust[di].noGravity = false;
                Main.dust[di].velocity.Y -= 2f;
            }
        }

        private void SpawnCritWindowDust()
        {
            int di = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(20f, 20f), 1, 1, DustID.GoldFlame);
            Main.dust[di].noGravity = true;
            Main.dust[di].velocity *= 0.5f;
        }

        public static void SpawnStackGainDust(Vector2 center)
        {
            if (Main.netMode == NetmodeID.Server) return;
            for (int k = 0; k < 4; k++)
            {
                int di = Dust.NewDust(center + Main.rand.NextVector2Circular(10f, 10f), 1, 1, DustID.GoldFlame);
                Main.dust[di].noGravity = true;
                Main.dust[di].velocity = Main.rand.NextVector2Circular(2f, 2f);
                Main.dust[di].scale = 0.6f;
            }
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write((byte)_animState);
            writer.Write((byte)AdaptationStacks);
            writer.Write((byte)Math.Min(_parryPerfectTimer, 255));
            writer.Write((byte)Math.Min(_parryGoodTimer, 255));
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            _animState = (AnubisAnim)reader.ReadByte();
            AdaptationStacks = reader.ReadByte();
            _parryPerfectTimer = reader.ReadByte();
            _parryGoodTimer = reader.ReadByte();
        }

        public override void OnKill(global::System.Int32 timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            player.ClearBuff(ModContent.BuffType<AnubisAdaptationBuff>());
        }
    }
}