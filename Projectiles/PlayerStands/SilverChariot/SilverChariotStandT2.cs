﻿using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System.IO;
using JoJoStands.Networking;

namespace JoJoStands.Projectiles.PlayerStands.SilverChariot
{
    public class SilverChariotStandT2 : StandClass
    {
        public override float MaxDistance => 98f;
        public override int PunchDamage => 34;
        public override int PunchTime => 9;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 10;
        public override int TierNumber => 2;
        public override string PoseSoundName => "SilverChariot";
        public override string SpawnSoundName => "Silver Chariot";
        public override int AmountOfPunchVariants => 3;
        public override string PunchTexturePath => "JoJoStands/Projectiles/PlayerStands/SilverChariot/SilverChariot_Stab_";
        public override Vector2 PunchSize => new Vector2(20, 10);
        public override PunchSpawnData PunchData => new PunchSpawnData()
        {
            standardPunchOffset = new Vector2(6f, 0f),
            minimumLifeTime = 6,
            maximumLifeTime = 12,
            minimumTravelDistance = 20,
            maximumTravelDistance = 48,
            bonusAfterimageAmount = 0
        };
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private bool parryFrames = false;

        public new enum AnimationState
        {
            Idle,
            Attack,
            Secondary,
            Parry,
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
            if (secondaryAbility || parryFrames)
            {
                if (mouseX > player.position.X)
                    player.direction = 1;
                else
                    player.direction = -1;
            }
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !secondaryAbility)
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch();
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                    if (Main.mouseRight && !attacking)
                    {
                        secondaryAbility = true;
                        Projectile.netUpdate = true;
                        currentAnimationState = AnimationState.Secondary;
                        Rectangle parryRectangle = new Rectangle((int)Projectile.Center.X + (4 * Projectile.direction), (int)Projectile.Center.Y - 29, 16, 54);
                        for (int p = 0; p < Main.maxProjectiles; p++)
                        {
                            Projectile otherProj = Main.projectile[p];
                            if (otherProj.active)
                            {
                                if (parryRectangle.Intersects(otherProj.Hitbox) && otherProj.type != Projectile.type && !otherProj.friendly && !otherProj.GetGlobalProjectile<JoJoGlobalProjectile>().exceptionForSCParry)
                                {
                                    parryFrames = true;
                                    secondaryAbility = false;
                                    otherProj.owner = Projectile.owner;
                                    otherProj.damage += (int)(otherProj.damage * mPlayer.standDamageBoosts) - otherProj.damage;
                                    otherProj.damage *= 2;
                                    otherProj.velocity *= -1;
                                    otherProj.hostile = false;
                                    otherProj.friendly = true;
                                    SyncCall.SyncStandEffectInfo(player.whoAmI, otherProj.whoAmI, 10, 1);
                                }
                            }
                        }
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                            {
                                if (!npc.townNPC && !npc.friendly && !npc.immortal && !npc.hide && parryRectangle.Intersects(npc.Hitbox))
                                {
                                    int damage = (int)(npc.damage * 2 * mPlayer.standDamageBoosts);
                                    NPC.HitInfo hitInfo = new NPC.HitInfo()
                                    {
                                        Damage = damage,
                                        Knockback = 6f,
                                        HitDirection = player.direction
                                    };
                                    npc.StrikeNPC(hitInfo);
                                    SyncCall.SyncStandEffectInfo(player.whoAmI, npc.whoAmI, 10, 2, damage, player.direction);
                                    secondaryAbility = false;
                                    parryFrames = true;
                                    SoundStyle npcHit4 = SoundID.NPCHit4;
                                    npcHit4.Pitch = Main.rand.Next(4, 6 + 1) / 10f;
                                    SoundEngine.PlaySound(npcHit4, Projectile.Center);
                                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(9));
                                }
                            }
                        }
                    }
                    else
                        secondaryAbility = false;
                }
                if (!attacking && !parryFrames)
                {
                    if (!secondaryAbility)
                        StayBehind();
                    else
                        GoInFront();
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
                if (!attacking)
                    currentAnimationState = AnimationState.Idle;
                else
                    currentAnimationState = AnimationState.Attack;
            }
            if (parryFrames)
                currentAnimationState = AnimationState.Parry;
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
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
            else if (currentAnimationState == AnimationState.Secondary)
                PlayAnimation("Secondary");
            else if (currentAnimationState == AnimationState.Parry)
                PlayAnimation("Parry");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void AnimationCompleted(string animationName)
        {
            if (animationName == "Parry")
            {
                currentAnimationState = AnimationState.Idle;
                parryFrames = false;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SilverChariot/SilverChariot_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 30, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 5, newPunchTime / 2, true);
            else if (animationName == "Secondary")
                AnimateStand(animationName, 1, 1, true);
            else if (animationName == "Parry")
                AnimateStand(animationName, 6, 3, false);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 10, true);
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(parryFrames);
        }
        public override void ReceiveExtraStates(BinaryReader reader)
        {
            parryFrames = reader.ReadBoolean();
        }
    }
}