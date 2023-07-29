using JoJoStands.Buffs.Debuffs;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.GratefulDead
{
    public class GratefulDeadStandFinal : StandClass
    {
        public override float ProjectileSpeed => 16f;
        public override float MaxDistance => 98f;
        public override int PunchDamage => 90;
        public override int PunchTime => 11;
        public override int HalfStandHeight => 34;
        public override int FistWhoAmI => 8;
        public override int TierNumber => 4;
        public override Vector2 StandOffset => new Vector2(17, 0);
        public override StandAttackType StandType => StandAttackType.Melee;
        public override string PoseSoundName => "OnceWeDecideToKillItsDone";
        public override string SpawnSoundName => "The Grateful Dead";
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private const float GasDetectionDist = 30 * 16f;

        private bool grabFrames = false;
        private bool gasActive = false;
        private float gasRange = GasDetectionDist;

        public new enum AnimationState
        {
            Idle,
            Attack,
            SecondaryAbility,
            Grab,
            Pose
        }

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            mPlayer.gratefulDeadGasActive = gasActive;
            if (gasActive)
            {
                gasRange = GasDetectionDist + mPlayer.standRangeBoosts;
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        float distance = Vector2.Distance(player.Center, npc.Center);
                        if (distance <= gasRange && !npc.immortal && !npc.hide)
                        {
                            npc.GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner = player.whoAmI;
                            npc.AddBuff(ModContent.BuffType<Aging>(), 2);
                        }
                    }
                }
                if (JoJoStands.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayer = Main.player[i];
                        if (otherPlayer.active && otherPlayer.InOpposingTeam(player) && otherPlayer.whoAmI != player.whoAmI)
                        {
                            float distance = Vector2.Distance(player.Center, otherPlayer.Center);
                            if (distance <= gasRange)
                                otherPlayer.AddBuff(ModContent.BuffType<Aging>(), 2);
                        }
                    }
                }
                if (Main.rand.Next(0, 12 + 1) == 0)
                    Dust.NewDust(Projectile.Center - new Vector2(gasRange / 2f, 0f), (int)gasRange, Projectile.height, ModContent.DustType<Dusts.GratefulDeadCloud>());

                player.AddBuff(ModContent.BuffType<Aging>(), 2);
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !secondaryAbility && !grabFrames)
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch();
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }

                    if (Main.mouseRight && !grabFrames && shootCount <= 0)
                    {
                        Projectile.velocity = Main.MouseWorld - Projectile.position;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 5f;

                        float mouseDistance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
                        if (mouseDistance > 40f)
                            Projectile.velocity = player.velocity + Projectile.velocity;
                        else
                            Projectile.velocity = Vector2.Zero;
                        Projectile.direction = 1;
                        if (mouseX < Projectile.Center.X)
                            Projectile.direction = -1;
                        Projectile.spriteDirection = Projectile.direction;

                        secondaryAbility = true;
                        currentAnimationState = AnimationState.SecondaryAbility;
                        Rectangle grabRect = new Rectangle((int)Projectile.Center.X + (40 * Projectile.direction), (int)Projectile.Center.Y - HalfStandHeight, 40, HalfStandHeight * 2);
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active)
                            {
                                if (grabRect.Intersects(npc.Hitbox) && !npc.boss && !npc.immortal && !npc.hide)
                                {
                                    grabFrames = true;
                                    Projectile.ai[0] = npc.whoAmI;
                                    break;
                                }
                            }
                        }
                        LimitDistance();
                    }
                    if (Main.mouseRight && grabFrames && Projectile.ai[0] != -1f)
                    {
                        Projectile.velocity = Vector2.Zero;
                        NPC npc = Main.npc[(int)Projectile.ai[0]];
                        npc.direction = -Projectile.direction;
                        npc.position = Projectile.position + new Vector2(5f * Projectile.direction, -(2f * npc.height) / 3f);
                        npc.velocity = Vector2.Zero;
                        npc.GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner = Projectile.owner;
                        npc.AddBuff(ModContent.BuffType<RapidAging>(), 2);
                        currentAnimationState = AnimationState.Grab;
                        if (!npc.active || Vector2.Distance(player.Center, Projectile.Center) > newMaxDistance * 1.5f)
                        {
                            shootCount += 30;
                            grabFrames = false;
                            secondaryAbility = false;
                            Projectile.ai[0] = -1f;
                        }
                        Projectile.netUpdate = true;
                    }
                    if (!Main.mouseRight && (grabFrames || secondaryAbility))
                    {
                        shootCount += 30;
                        grabFrames = false;
                        secondaryAbility = false;
                        Projectile.ai[0] = -1f;
                        Projectile.netUpdate = true;
                    }
                }
                if (!attacking && !secondaryAbility && !grabFrames)
                    StayBehind();
            }
            if (SpecialKeyPressed() && shootCount <= 0)
            {
                shootCount += 30;
                gasActive = !gasActive;
                if (gasActive)
                    Main.NewText("Gas Spread: On");
                else
                    Main.NewText("Gas Spread: Off");
                Projectile.netUpdate = true;
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        private float gasTextureSize;
        private Vector2 gasTextureOrigin;
        private Rectangle gasTextureSourceRect;
        private Texture2D gasRangeIndicatorTexture;

        public override bool PreDrawExtras()
        {
            Player player = Main.player[Projectile.owner];
            if (JoJoStands.RangeIndicators && gasActive)
            {
                if (gasRange != gasTextureSize)
                {
                    gasRangeIndicatorTexture = GenerateRangeIndicatorTexture(gasRange, 0);
                    gasTextureOrigin = new Vector2(gasRangeIndicatorTexture.Width / 2f, gasRangeIndicatorTexture.Height / 2f);
                    gasTextureSourceRect = new Rectangle(0, 0, gasRangeIndicatorTexture.Width, gasRangeIndicatorTexture.Height);
                    gasTextureSize = gasRange;
                }
                if (gasRangeIndicatorTexture != null)
                    Main.EntitySpriteDraw(gasRangeIndicatorTexture, player.Center - Main.screenPosition, gasTextureSourceRect, Color.Red * JoJoStands.RangeIndicatorAlpha, 0f, gasTextureOrigin, 2f, SpriteEffects.None, 0);
            }
            return true;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(grabFrames);
            writer.Write(gasActive);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            grabFrames = reader.ReadBoolean();
            gasActive = reader.ReadBoolean();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
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

            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Attack)
                PlayAnimation("Attack");
            else if (currentAnimationState == AnimationState.SecondaryAbility)
                PlayAnimation("Secondary");
            else if (currentAnimationState == AnimationState.Grab)
                PlayAnimation("Grab");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/GratefulDead/GratefulDead_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Secondary")
                AnimateStand(animationName, 1, 1, true);
            else if (animationName == "Grab")
                AnimateStand(animationName, 3, 6, true, 2, 2);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 12, true);
        }
    }
}