using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.ManhattanTransfer
{
    public abstract class ManhattanTransferStand : StandClass
    {
        protected virtual int RedirectCooldown => 30;
        private const float RedirectSpeed = 18f;
        private const int FrameSpeed = 12;
        private const float FlySpeed = 10f;
        private const float RotationLerpSpeed = 0.18f;
        private const float InteractRadius = 40f;

        protected abstract string IdleTexture { get; }
        protected abstract string DeflectTexture { get; }
        protected virtual int IdleFrameCount => 4;
        protected virtual int DeflectFrameCount => 4;

        protected virtual float TierRange => 150f;
        protected virtual bool CanLockTarget => false;
        protected virtual bool CanDeflect => false;
        protected virtual bool CanRedirectShots => true;

        public override string Texture => IdleTexture;

        public float StandRange(MyPlayer mPlayer) => TierRange + mPlayer.standRangeBoosts * 8f;

        private int redirectTimer = 0;
        private int lockedTargetWhoAmI = -1;

        private Vector2 targetPosition = Vector2.Zero;
        private bool hasTarget = false;

        private bool DeflectMode => Projectile.ai[0] == 1f;

        private int ActiveFrameCount => (CanDeflect && DeflectMode) ? DeflectFrameCount : IdleFrameCount;

        private NPC LockedTarget
        {
            get
            {
                if (!CanLockTarget || lockedTargetWhoAmI < 0 || lockedTargetWhoAmI >= Main.maxNPCs)
                    return null;
                NPC n = Main.npc[lockedTargetWhoAmI];
                return (n.active && n.lifeMax > 5 && !n.immortal && !n.townNPC) ? n : null;
            }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = IdleFrameCount;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.timeLeft = 2;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (redirectTimer > 0)
                redirectTimer--;

            if (LockedTarget == null)
                lockedTargetWhoAmI = -1;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= FrameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % ActiveFrameCount;
            }

            DrawScanRing(mPlayer);

            if (Projectile.owner == Main.myPlayer)
                HandleInput(player, mPlayer);

            FlyToTarget(player, mPlayer);

            if (CanDeflect && DeflectMode)
                DoDeflectMode(mPlayer);
            else if (CanRedirectShots)
                DoRedirectMode(mPlayer);
        }

        private void FlyToTarget(Player player, MyPlayer mPlayer)
        {
            if (hasTarget)
            {
                Vector2 toTarget = targetPosition - Projectile.Center;
                float dist = toTarget.Length();

                if (dist <= FlySpeed)
                {
                    Projectile.Center = targetPosition;
                    hasTarget = false;
                    Projectile.velocity = Vector2.Zero;
                    Projectile.netUpdate = true;
                }
                else
                {
                    Vector2 dir = toTarget / dist;
                    Projectile.velocity = dir * FlySpeed;

                    float targetRot = dir.ToRotation();
                    Projectile.rotation = LerpAngle(Projectile.rotation, targetRot, RotationLerpSpeed);
                }
            }
            else
            {
                Projectile.rotation = LerpAngle(Projectile.rotation, 0f, RotationLerpSpeed * 0.5f);
                Projectile.velocity = Vector2.Zero;
                ClampToRange(player, mPlayer);
            }
        }

        private static float LerpAngle(float from, float to, float amount)
        {
            float diff = to - from;
            while (diff > MathHelper.Pi) diff -= MathHelper.TwoPi;
            while (diff < -MathHelper.Pi) diff += MathHelper.TwoPi;
            return from + diff * amount;
        }

        private void HandleInput(Player player, MyPlayer mPlayer)
        {
            if (Main.mouseRight && mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                Vector2 toMouse = Main.MouseWorld - player.Center;
                float range = StandRange(mPlayer);
                if (toMouse.Length() > range)
                {
                    toMouse.Normalize();
                    toMouse *= range;
                }
                targetPosition = player.Center + toMouse;
                hasTarget = true;
                Projectile.netUpdate = true;
            }

            if (SpecialKeyPressed())
            {
                int hoveredNPC = -1;
                float closestDist = float.MaxValue;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (!n.active || n.lifeMax <= 5 || n.immortal || n.hide || n.townNPC)
                        continue;
                    if (!n.getRect().Contains(Main.MouseWorld.ToPoint()))
                        continue;
                    float d = Vector2.Distance(n.Center, Main.MouseWorld);
                    if (d < closestDist)
                    {
                        closestDist = d;
                        hoveredNPC = i;
                    }
                }
                if (hoveredNPC >= 0)
                {
                    SetLockedTarget(hoveredNPC);
                    Projectile.netUpdate = true;
                    SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);
                    for (int k = 0; k < 20; k++)
                        Dust.NewDust(Main.npc[hoveredNPC].position, Main.npc[hoveredNPC].width, Main.npc[hoveredNPC].height, DustID.Electric);
                }
            }

            if (SecondSpecialKeyPressed(false))
            {
                ToggleDeflectMode();
                SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
                int dustCount = 30;
                for (int k = 0; k < dustCount; k++)
                {
                    float rot = MathHelper.ToRadians((360f / dustCount) * k);
                    Vector2 dp = Projectile.Center + rot.ToRotationVector2() * 24f;
                    int di = Dust.NewDust(dp, 1, 1, DustID.Electric);
                    Main.dust[di].noGravity = true;
                    Main.dust[di].velocity = rot.ToRotationVector2() * 3f;
                }
            }
        }

        private void DrawScanRing(MyPlayer mPlayer)
        {
            // Ring shows the movement range around the player, not the stand
            float range = StandRange(mPlayer);
            int ringDusts = 36;
            for (int i = 0; i < ringDusts; i++)
            {
                if (Main.rand.NextBool(4))
                    continue;
                Player player = Main.player[Projectile.owner];
                float rot = MathHelper.ToRadians((360f / ringDusts) * i);
                Vector2 dp = player.Center + rot.ToRotationVector2() * range;
                int di = Dust.NewDust(dp, 1, 1, DustID.Electric);
                Main.dust[di].noGravity = true;
                Main.dust[di].velocity = Vector2.Zero;
                Main.dust[di].scale = 0.7f;
                Main.dust[di].fadeIn = 0.4f;
            }
        }

        private void ClampToRange(Player player, MyPlayer mPlayer)
        {
            float range = StandRange(mPlayer);
            float dist = Vector2.Distance(Projectile.Center, player.Center);
            if (dist > range)
            {
                Vector2 dir = Projectile.Center - player.Center;
                dir.Normalize();
                Projectile.position = player.Center + dir * range - Projectile.Size / 2f;
                Projectile.netUpdate = true;
            }
        }

        private void DoRedirectMode(MyPlayer mPlayer)
        {
            if (redirectTimer > 0)
                return;

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile other = Main.projectile[p];
                if (!IsFriendlyRangedShot(other))
                    continue;

                // Only redirect projectiles the stand physically touches
                if (Vector2.Distance(other.Center, Projectile.Center) > InteractRadius)
                    continue;

                NPC target = LockedTarget ?? ClosestNPC(Projectile.Center);
                if (target == null)
                    break;

                RedirectProjectile(other, target.Center, RedirectSpeed);
                other.penetrate = Math.Max(other.penetrate, 1) + 1;
                other.GetGlobalProjectile<JoJoGlobalProjectile>().kickedBySexPistols = true;

                SpawnRedirectDusts(other.Center);
                SoundEngine.PlaySound(SoundID.Tink.WithPitchOffset(3f), other.Center);

                redirectTimer += Math.Max(0, RedirectCooldown - (2 * mPlayer.standSpeedBoosts));
                break;
            }
        }

        private void DoDeflectMode(MyPlayer mPlayer)
        {
            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile other = Main.projectile[p];
                if (!IsEnemyProjectile(other))
                    continue;

                // Only deflect projectiles the stand physically touches
                if (Vector2.Distance(other.Center, Projectile.Center) > InteractRadius)
                    continue;

                NPC target = LockedTarget ?? ClosestNPC(Projectile.Center);

                if (target != null)
                    RedirectProjectile(other, target.Center, RedirectSpeed);
                else
                {
                    other.velocity = -other.velocity;
                    other.netUpdate = true;
                }

                other.friendly = true;
                other.hostile = false;
                other.owner = Projectile.owner;

                SpawnRedirectDusts(other.Center, 18);
                SoundEngine.PlaySound(SoundID.Tink.WithPitchOffset(-2f), other.Center);
            }
        }

        private static void RedirectProjectile(Projectile proj, Vector2 dest, float speed)
        {
            Vector2 dir = dest - proj.Center;
            if (dir == Vector2.Zero)
                return;
            dir.Normalize();
            proj.velocity = dir * speed;
            proj.netUpdate = true;
        }

        private static void SpawnRedirectDusts(Vector2 pos, int amount = 12)
        {
            for (int i = 0; i < amount; i++)
            {
                float rot = MathHelper.ToRadians((360f / amount) * i);
                Vector2 dp = pos + rot.ToRotationVector2() * 5f;
                int di = Dust.NewDust(dp, 1, 1, DustID.Electric);
                Main.dust[di].noGravity = true;
                Main.dust[di].velocity = rot.ToRotationVector2() * 2f;
                Main.dust[di].scale = 1.1f;
            }
        }

        private bool IsFriendlyRangedShot(Projectile proj)
            => proj.active
            && proj.type != Projectile.type
            && proj.owner == Projectile.owner
            && proj.friendly
            && proj.DamageType == DamageClass.Ranged;

        private static bool IsEnemyProjectile(Projectile proj)
            => proj.active && proj.hostile && !proj.friendly && proj.damage > 0;

        private static NPC ClosestNPC(Vector2 origin, float maxDist = float.MaxValue)
        {
            NPC best = null;
            float bestDist = maxDist;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (!n.active || n.lifeMax <= 5 || n.immortal || n.hide || n.townNPC)
                    continue;
                float d = Vector2.Distance(n.Center, origin);
                if (d < bestDist) { bestDist = d; best = n; }
            }
            return best;
        }

        public void SetLockedTarget(int npcWhoAmI)
        {
            if (CanLockTarget)
                lockedTargetWhoAmI = npcWhoAmI;
        }

        public void ToggleDeflectMode()
        {
            if (!CanDeflect)
                return;
            Projectile.ai[0] = Projectile.ai[0] == 0f ? 1f : 0f;
            Projectile.frame = 0;
            Projectile.frameCounter = 0;
            Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> texAsset = (CanDeflect && DeflectMode)
                ? ModContent.Request<Texture2D>(DeflectTexture)
                : ModContent.Request<Texture2D>(IdleTexture);

            Texture2D tex = texAsset.Value;
            int frameCount = (CanDeflect && DeflectMode) ? DeflectFrameCount : IdleFrameCount;
            int frameHeight = tex.Height / frameCount;
            Rectangle source = new Rectangle(0, Projectile.frame * frameHeight, tex.Width, frameHeight);
            Vector2 origin = new Vector2(tex.Width / 2f, frameHeight / 2f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(tex, drawPos, source, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override void SendExtraAI(System.IO.BinaryWriter writer)
        {
            writer.Write(lockedTargetWhoAmI);
            writer.Write(hasTarget);
            if (hasTarget)
            {
                writer.Write(targetPosition.X);
                writer.Write(targetPosition.Y);
            }
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader)
        {
            lockedTargetWhoAmI = reader.ReadInt32();
            hasTarget = reader.ReadBoolean();
            if (hasTarget)
                targetPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }
    }
}