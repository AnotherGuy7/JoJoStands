using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Cream
{
    public class CreamStandT2 : StandClass
    {
        public override int PunchDamage => 68;
        public override float PunchKnockback => 9f;
        public override int PunchTime => 26;
        public override int HalfStandHeight => 36;
        public override int FistID => 11;
        public override int TierNumber => 2;
        public override string PoseSoundName => "Cream";
        public override string SpawnSoundName => "Cream";
        public override Vector2 StandOffset => Vector2.Zero;
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        public new enum AnimationState
        {
            Idle,
            Attack,
            ExposedIdle,
            Transform,
            TransformToVoid,
            Pose
        }

        private Vector2 velocity;
        private int creamCustomFrameCounter = 0;
        private int dashproj = 0;
        private bool dashprojspawn = false;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            Projectile.hide = mPlayer.creamVoidMode;
            if (mPlayer.creamExposedMode)
                Projectile.hide = false;
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual && !mPlayer.creamDash)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && mPlayer.canStandBasicAttack && !mPlayer.creamVoidMode && !mPlayer.creamExposedMode && !mPlayer.creamExposedToVoid && !mPlayer.creamNormalToExposed && !mPlayer.creamDash)
                    {
                        attacking = true;
                        currentAnimationState = AnimationState.Attack;
                        Vector2 targetPosition = Main.MouseWorld;
                        if (JoJoStands.StandAimAssist)
                        {
                            float lowestDistance = 4f * 16f;
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                NPC npc = Main.npc[n];
                                if (npc.active && npc.CanBeChasedBy(this, false))
                                {
                                    float distance = Vector2.Distance(npc.Center, Main.MouseWorld);
                                    if (distance < lowestDistance && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                                    {
                                        targetPosition = npc.Center;
                                        lowestDistance = distance;
                                    }
                                }
                            }
                        }
                        float rotaY = targetPosition.Y - Projectile.Center.Y;
                        Projectile.rotation = MathHelper.ToRadians((rotaY * Projectile.spriteDirection) / 6f);
                        Projectile.spriteDirection = Projectile.direction = targetPosition.X > Projectile.Center.X ? 1 : -1;
                        Vector2 velocityAddition = targetPosition - Projectile.position;
                        velocityAddition.Normalize();
                        velocityAddition *= 5f + mPlayer.standTier;

                        float mouseDistance = Vector2.Distance(targetPosition, Projectile.Center);
                        if (mouseDistance > 12f)
                            Projectile.velocity = player.velocity + velocityAddition;
                        else
                            Projectile.velocity = Vector2.Zero;
                        if (shootCount <= 0 && Projectile.frame == 2)
                        {
                            shootCount += newPunchTime / 2;
                            Vector2 shootVel = targetPosition - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), newPunchDamage, PunchKnockback, Projectile.owner, FistID);
                            Main.projectile[projIndex].netUpdate = true;
                        }
                        Projectile.netUpdate = true;
                        LimitDistance();
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
                if (!attacking)
                    StayBehind();

                if (SpecialKeyPressed() && player.ownedProjectileCounts[ModContent.ProjectileType<Void>()] <= 0 && !mPlayer.creamVoidMode && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid && !mPlayer.creamDash && Projectile.owner == Main.myPlayer)
                {
                    mPlayer.creamFrame = 0;
                    if (mPlayer.creamExposedMode)
                        mPlayer.creamExposedToVoid = true;

                    if (!mPlayer.creamExposedMode)
                    {
                        mPlayer.creamNormalToExposed = true;
                        mPlayer.creamNormalToVoid = true;
                    }
                    Projectile.netUpdate = true;
                }
                if (Main.mouseRight && !Main.mouseLeft && player.ownedProjectileCounts[ModContent.ProjectileType<Void>()] <= 0 && !mPlayer.creamVoidMode! && !mPlayer.creamExposedMode && !mPlayer.creamExposedToVoid && !mPlayer.creamNormalToExposed && !mPlayer.creamDash && mPlayer.voidCounter >= 4 && Projectile.owner == Main.myPlayer)
                {
                    mPlayer.voidCounter -= 4;
                    mPlayer.creamDash = true;
                }
            }
            float playerDistance = Vector2.Distance(player.Center, Projectile.Center);
            if (mPlayer.creamDash)
            {
                if (Projectile.owner == Main.myPlayer && !dashprojspawn && player.ownedProjectileCounts[ModContent.ProjectileType<DashVoid>()] <= 0)
                {
                    SoundEngine.PlaySound(SoundID.Item78, Projectile.Center);
                    Vector2 shootVelocity = Main.MouseWorld - Projectile.Center;
                    velocity = Main.MouseWorld;
                    if (shootVelocity == Vector2.Zero)
                        shootVelocity = new Vector2(0f, 1f);
                    shootVelocity.Normalize();
                    shootVelocity *= 8f + (mPlayer.creamTier * 2f);
                    dashproj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVelocity, ModContent.ProjectileType<DashVoid>(), (int)((PunchDamage * 1.3f) * mPlayer.standDamageBoosts), 6f, Projectile.owner, Projectile.whoAmI, 0);
                    Main.projectile[dashproj].netUpdate = true;
                    Projectile.netUpdate = true;
                }
                if (dashprojspawn && player.ownedProjectileCounts[ModContent.ProjectileType<DashVoid>()] <= 0)
                {
                    if (Projectile.velocity.X < 0)
                        Projectile.spriteDirection = -1;
                    else
                        Projectile.spriteDirection = 1;
                    Projectile.velocity = player.Center - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 6f + mPlayer.creamTier + player.moveSpeed;
                    Projectile.netUpdate = true;
                    if (playerDistance <= 40f)
                    {
                        SoundEngine.PlaySound(SoundID.Item78, Projectile.Center);
                        mPlayer.voidCounter += 2;
                        mPlayer.creamDash = false;
                        dashprojspawn = false;
                    }
                }
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<DashVoid>()] >= 1)
            {
                dashprojspawn = true;
                Vector2 vector = Main.projectile[dashproj].Center;
                Projectile.Center = Vector2.Lerp(Projectile.Center, vector, 1f);
                Projectile.hide = true;
                if (Vector2.Distance(velocity, Projectile.Center) <= 10f || playerDistance >= 800f || player.dead || !mPlayer.standOut || playerDistance >= 1200f)
                {
                    if (mPlayer.creamDash && playerDistance >= 1200f)
                        mPlayer.standOut = false;
                    Main.projectile[dashproj].Kill();
                    SoundEngine.PlaySound(SoundID.Item78, Projectile.Center);
                }
            }
            if (mPlayer.creamNormalToExposed)
            {
                PlayAnimation("Transform");
                if (mPlayer.creamFrame >= 5 && !mPlayer.creamAnimationReverse)
                {
                    if (mPlayer.creamNormalToVoid)
                    {
                        mPlayer.creamExposedToVoid = true;
                        mPlayer.creamNormalToVoid = false;
                    }
                    mPlayer.creamNormalToExposed = false;
                    mPlayer.creamFrame = 0;
                    SoundEngine.PlaySound(SoundID.Item78, Projectile.Center);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Top, shootVelocity, ModContent.ProjectileType<ExposingCream>(), 0, 6f, player.whoAmI);
                }
            }
            if (mPlayer.creamExposedToVoid)
            {
                PlayAnimation("TransformToVoid");
                if (mPlayer.creamFrame >= 7 && !mPlayer.creamAnimationReverse)
                {
                    mPlayer.creamExposedToVoid = false;
                    SoundEngine.PlaySound(SoundID.Item78, Projectile.Center);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Top, shootVelocity, ModContent.ProjectileType<Void>(), (int)((PunchDamage * 0.5f) * mPlayer.standDamageBoosts), 6f, player.whoAmI);
                }
                if (mPlayer.creamFrame <= 0 && mPlayer.creamAnimationReverse)
                {
                    if (mPlayer.creamNormalToExposed)
                    {
                        mPlayer.creamNormalToExposed = false;
                        mPlayer.creamAnimationReverse = false;
                    }
                    if (mPlayer.creamExposedToVoid)
                    {
                        mPlayer.creamExposedToVoid = false;
                        if (!mPlayer.creamNormalToVoid)
                            mPlayer.creamAnimationReverse = false;

                        if (mPlayer.creamNormalToVoid)
                        {
                            mPlayer.creamFrame = 5;
                            mPlayer.creamNormalToExposed = true;
                            mPlayer.creamNormalToVoid = false;
                        }
                    }
                }
            }
            if (mPlayer.creamExposedToVoid || mPlayer.creamVoidMode || mPlayer.creamExposedMode)
            {
                Vector2 vector131 = player.Center;
                vector131.X += (float)((player.width / 2) * player.direction);
                vector131.Y -= -35 + HalfStandHeight;
                Projectile.Center = Vector2.Lerp(Projectile.Center, vector131, 1f);
                Projectile.velocity *= 0.5f;
                Projectile.direction = (Projectile.spriteDirection = player.direction);
                Projectile.rotation = 0;
                LimitDistance();
            }
            if (mPlayer.creamExposedToVoid || mPlayer.creamNormalToExposed)
            {
                creamCustomFrameCounter += 1;
                if (creamCustomFrameCounter >= 15)
                {
                    mPlayer.creamFrame += mPlayer.creamAnimationReverse ? -1 : 1;
                    creamCustomFrameCounter = 0;
                }
                Projectile.frame = mPlayer.creamFrame;
            }
            if (mPlayer.creamNormalToExposed)
                currentAnimationState = AnimationState.Transform;
            else if (mPlayer.creamExposedToVoid)
                currentAnimationState = AnimationState.TransformToVoid;
            else if (mPlayer.creamExposedMode && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid || mPlayer.creamDash)
                currentAnimationState = AnimationState.ExposedIdle;
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto && !mPlayer.creamVoidMode && !mPlayer.creamExposedMode && !mPlayer.creamExposedToVoid && !mPlayer.creamNormalToExposed && !mPlayer.creamDash)
            {
                BasicPunchAI();
                if (!attacking)
                    currentAnimationState = AnimationState.Idle;
                else
                    currentAnimationState = AnimationState.Attack;
            }
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            bool creamdash = true;
            if (mPlayer.creamDash)
                creamdash = false;
            return creamdash;
        }
        public override void SendExtraStates(BinaryWriter writer)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            writer.Write(mPlayer.creamNormalToExposed);
            writer.Write(mPlayer.creamExposedToVoid);
            writer.Write(mPlayer.creamExposedMode);
            writer.Write(mPlayer.creamVoidMode);
            writer.Write(mPlayer.creamDash);
            writer.Write(dashprojspawn);
            writer.Write(dashproj);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.creamNormalToExposed = reader.ReadBoolean();
            mPlayer.creamExposedToVoid = reader.ReadBoolean();
            mPlayer.creamExposedMode = reader.ReadBoolean();
            mPlayer.creamVoidMode = reader.ReadBoolean();
            mPlayer.creamDash = reader.ReadBoolean();
            dashprojspawn = reader.ReadBoolean();
            dashproj = reader.ReadInt32();
        }

        public override byte SendAnimationState() => (byte)currentAnimationState;
        public override void ReceiveAnimationState(byte state) => currentAnimationState = (AnimationState)state;

        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
                Projectile.netUpdate = true;
            }

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Attack)
                PlayAnimation("Attack");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
            else if (currentAnimationState == AnimationState.Transform)
                PlayAnimation("Transform");
            else if (currentAnimationState == AnimationState.TransformToVoid)
                PlayAnimation("TransformToVoid");
            else if (currentAnimationState == AnimationState.ExposedIdle)
                PlayAnimation("ExposedIdle");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/Cream/Cream_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 30, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime / 2, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 2, true);
            else if (animationName == "TransformToVoid")
                AnimateStand(animationName, 8, 99999, true);
            else if (animationName == "Transform")
                AnimateStand(animationName, 6, 99999, true);
            else if (animationName == "ExposedIdle")
                AnimateStand(animationName, 4, 30, true);
        }

        public override void StandKillEffects()
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            mPlayer.creamDash = false;
        }
    }
}