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
        public override int FistWhoAmI => 11;
        public override int TierNumber => 2;
        public override int StandOffset => 0;
        public override StandAttackType StandType => StandAttackType.Melee;

        private Vector2 velocityAddition;
        private Vector2 velocity;
        private int framechangecounter = 0;
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
            if (!mPlayer.standAutoMode && !mPlayer.creamDash)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && mPlayer.canStandBasicAttack && !mPlayer.creamVoidMode && !mPlayer.creamExposedMode && !mPlayer.creamExposedToVoid && !mPlayer.creamNormalToExposed && !mPlayer.creamDash)
                {
                    HandleDrawOffsets();
                    attackFrames = true;
                    idleFrames = false;
                    Projectile.netUpdate = true;
                    float rotaY = Main.MouseWorld.Y - Projectile.Center.Y;
                    Projectile.rotation = MathHelper.ToRadians((rotaY * Projectile.spriteDirection) / 6f);

                    if (mouseX > Projectile.position.X)
                        Projectile.direction = 1;
                    if (mouseX < Projectile.position.X)
                        Projectile.direction = -1;

                    Projectile.spriteDirection = Projectile.direction;

                    velocityAddition = Main.MouseWorld - Projectile.position;
                    velocityAddition.Normalize();
                    velocityAddition *= 5f + mPlayer.standTier;
                    float mouseDistance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        Projectile.velocity = player.velocity + velocityAddition;
                    }
                    if (mouseDistance <= 40f)
                    {
                        Projectile.velocity = Vector2.Zero;
                    }
                    if (shootCount <= 0 && Projectile.frame == 2)
                    {
                        shootCount += newPunchTime / 2;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= ProjectileSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), newPunchDamage, PunchKnockback, Projectile.owner, FistWhoAmI);
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                    LimitDistance();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    StayBehind();
                }
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
                }
                if (Main.mouseRight && !Main.mouseLeft && player.ownedProjectileCounts[ModContent.ProjectileType<Void>()] <= 0 && !mPlayer.creamVoidMode! && !mPlayer.creamExposedMode && !mPlayer.creamExposedToVoid && !mPlayer.creamNormalToExposed && !mPlayer.creamDash && mPlayer.voidCounter >= 4 && Projectile.owner == Main.myPlayer)
                {
                    mPlayer.voidCounter -= 4;
                    mPlayer.creamDash = true;
                }
            }
            float distance = Vector2.Distance(player.Center, Projectile.Center);
            if (mPlayer.creamDash)
            {
                if (Projectile.owner == Main.myPlayer && !dashprojspawn && player.ownedProjectileCounts[ModContent.ProjectileType<DashVoid>()] <= 0)
                {
                    SoundEngine.PlaySound(SoundID.Item78);
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
                    if (distance <= 40f)
                    {
                        SoundEngine.PlaySound(SoundID.Item78);
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
                if (Vector2.Distance(velocity, Projectile.Center) <= 10f || distance >= 800f || player.dead || !mPlayer.standOut || distance >= 1200f)
                {
                    if (mPlayer.creamDash && distance >= 1200f)
                        mPlayer.standOut = false;
                    Main.projectile[dashproj].Kill();
                    SoundEngine.PlaySound(SoundID.Item78);
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
                    SoundEngine.PlaySound(SoundID.Item78);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Top, shootVelocity, ModContent.ProjectileType<ExposingCream>(), 0, 6f, player.whoAmI);
                }
            }
            if (mPlayer.creamExposedToVoid)
            {
                PlayAnimation("Transform2");
                if (mPlayer.creamFrame >= 7 && !mPlayer.creamAnimationReverse)
                {
                    mPlayer.creamExposedToVoid = false;
                    SoundEngine.PlaySound(SoundID.Item78);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Top, shootVelocity, ModContent.ProjectileType<Void>(), (int)((PunchDamage * 0.5f) * mPlayer.standDamageBoosts), 6f, player.whoAmI);
                }
                if (mPlayer.creamFrame <= 0 && mPlayer.creamAnimationReverse && mPlayer.creamNormalToExposed)
                {
                    mPlayer.creamNormalToExposed = false;
                    mPlayer.creamAnimationReverse = false;
                }
                if (mPlayer.creamFrame <= 0 && mPlayer.creamAnimationReverse && mPlayer.creamExposedToVoid)
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
            if (mPlayer.creamExposedToVoid || mPlayer.creamVoidMode || mPlayer.creamExposedMode)
            {
                HandleDrawOffsets();
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
                Projectile.frame = mPlayer.creamFrame;
                if (mPlayer.creamAnimationReverse)
                {
                    framechangecounter += 1;
                    if (framechangecounter == 15)
                    {
                        mPlayer.creamFrame -= 1;
                        framechangecounter = 0;
                    }
                }
                if (!mPlayer.creamAnimationReverse)
                {
                    framechangecounter += 1;
                    if (framechangecounter == 15)
                    {
                        mPlayer.creamFrame += 1;
                        framechangecounter = 0;
                    }
                }
            }
            if (mPlayer.standAutoMode && !mPlayer.creamVoidMode && !mPlayer.creamExposedMode && !mPlayer.creamExposedToVoid && !mPlayer.creamNormalToExposed && !mPlayer.creamDash)
            {
                BasicPunchAI();
            }
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

            mPlayer.creamExposedMode = reader.ReadBoolean();
            mPlayer.creamVoidMode = reader.ReadBoolean();
            mPlayer.creamDash = reader.ReadBoolean();
            dashprojspawn = reader.ReadBoolean();
            dashproj = reader.ReadInt32();
        }

        public override void SelectAnimation()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (attackFrames && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid && !mPlayer.creamExposedMode && !mPlayer.creamDash)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid && !mPlayer.creamExposedMode && !mPlayer.creamDash)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().posing && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid && !mPlayer.creamExposedMode && !mPlayer.creamDash)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
            if (mPlayer.creamExposedMode && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid && !mPlayer.creamDash || mPlayer.creamDash)
            {
                PlayAnimation("Idle2");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/Cream/Cream_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime / 2, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
            if (animationName == "Transform2")
            {
                AnimateStand(animationName, 8, 99999, true);
            }
            if (animationName == "Transform")
            {
                AnimateStand(animationName, 6, 99999, true);
            }
            if (animationName == "Idle2")
            {
                AnimateStand(animationName, 4, 30, true);
            }
        }

        public override void Kill(int timeLeft)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            mPlayer.creamDash = false;
        }
    }
}