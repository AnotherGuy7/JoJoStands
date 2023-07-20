using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.GratefulDead
{
    public class GratefulDeadStandT2 : StandClass
    {
        public override float ProjectileSpeed => 16f;
        public override float MaxDistance => 98f;
        public override int PunchDamage => 41;
        public override int PunchTime => 13;
        public override int HalfStandHeight => 34;
        public override int FistWhoAmI => 8;
        public override int TierNumber => 2;
        public override Vector2 StandOffset => new Vector2(17, 0);
        public override StandAttackType StandType => StandAttackType.Melee;
        public override string PoseSoundName => "OnceWeDecideToKillItsDone";
        public override string SpawnSoundName => "The Grateful Dead";
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private bool grabFrames = false;

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

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Main.mouseLeft && !grabFrames && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        currentAnimationState = AnimationState.Idle;
                }
                if (!attacking && !secondaryAbility && !grabFrames)
                    StayBehind();

                if (Main.mouseRight && Projectile.owner == Main.myPlayer && shootCount <= 0 && !grabFrames)
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
                if (Main.mouseRight && grabFrames && Projectile.ai[0] != -1f && Projectile.owner == Main.myPlayer)
                {
                    Projectile.velocity = Vector2.Zero;
                    NPC npc = Main.npc[(int)Projectile.ai[0]];
                    npc.direction = -Projectile.direction;
                    npc.position = Projectile.position + new Vector2(5f * Projectile.direction, -(2f * npc.height) / 3f);
                    npc.velocity = Vector2.Zero;
                    npc.AddBuff(ModContent.BuffType<RapidAging>(), 2);
                    currentAnimationState = AnimationState.Grab;
                    if (!npc.active || Vector2.Distance(player.Center, Projectile.Center) > newMaxDistance * 1.5f)
                    {
                        shootCount += 30;
                        grabFrames = false;
                        Projectile.ai[0] = -1f;
                    }
                    Projectile.netUpdate = true;
                    LimitDistance();
                }
                if (!Main.mouseRight && (grabFrames || secondaryAbility) && Projectile.owner == Main.myPlayer)
                {
                    shootCount += 30;
                    grabFrames = false;
                    Projectile.ai[0] = -1f;
                    secondaryAbility = false;
                    Projectile.netUpdate = true;
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(grabFrames);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            grabFrames = reader.ReadBoolean();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

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