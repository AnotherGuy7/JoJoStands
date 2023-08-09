using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Items.CraftingMaterials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.SoftAndWet
{
    public class SoftAndWetStandT3 : StandClass
    {
        public override int PunchDamage => 63;
        public override int PunchTime => 11;
        public override int HalfStandHeight => 38;
        public override int AltDamage => TierNumber * 15;
        public override Vector2 StandOffset => new Vector2(27, 0);
        public override int FistWhoAmI => 0;
        public override int TierNumber => 3;
        public override string PunchSoundName => "SoftAndWet_Ora";
        public override string PoseSoundName => "SoftAndWet";
        public override string SpawnSoundName => "Soft and Wet";
        public override int AmountOfPunchVariants => 2;
        public override string PunchTexturePath => "JoJoStands/Projectiles/PlayerStands/SoftAndWet/SoftAndWet_Punch_";
        public override Vector2 PunchSize => new Vector2(36, 8);
        public override StandAttackType StandType => StandAttackType.Melee;

        private bool bubbleMode = false;
        private int bubbleSpawnTimer = 0;
        private int explosionClickTimer = 0;
        private const float BubbleSpawnRadius = 24 * 16f;
        private readonly SoundStyle BubbleFieldBubbleSpawnSound = new SoundStyle(SoundID.SplashWeak.SoundPath)
        {
            Volume = 0.4f,
            PitchVariance = 1f
        };

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;
            if (explosionClickTimer > 0)
            {
                explosionClickTimer--;
                if (explosionClickTimer <= 0)
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(15));
            }

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !bubbleMode)
                    {
                        Punch();
                        if (Main.rand.NextBool(14))
                        {
                            SoundEngine.PlaySound(SoundID.Drip, Projectile.Center);
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= 3f;
                            Vector2 bubbleSpawnPosition = Projectile.Center + new Vector2(Main.rand.Next(0, 18 + 1) * Projectile.direction, -Main.rand.Next(0, HalfStandHeight - 2 + 1));
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), bubbleSpawnPosition, shootVel, ModContent.ProjectileType<TinyBubble>(), 21, 2f, Projectile.owner, Projectile.whoAmI);
                        }
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
                if (!attacking)
                    StayBehindWithAbility();

                if (Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    if (!bubbleMode)
                    {
                        GoInFront();
                        if (shootCount <= 0)
                        {
                            shootCount += 36;
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= 3f;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<PlunderBubble>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, Projectile.owner, GetPlunderBubbleType());
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                            SoundEngine.PlaySound(SoundID.Item85, Projectile.Center);
                        }
                    }
                    else
                    {
                        explosionClickTimer = 5;
                    }
                }
                if (SpecialKeyPressed(false))
                {
                    bubbleSpawnTimer = 0;
                    bubbleMode = !bubbleMode;
                    if (bubbleMode)
                        Main.NewText("Bubble Mode: Active", Color.LightBlue);
                    else
                        Main.NewText("Bubble Mode: Inactive", Color.LightBlue);
                }
                if (bubbleMode)
                {
                    bubbleSpawnTimer++;
                    if (bubbleSpawnTimer >= 60)
                    {
                        bubbleSpawnTimer = 0;
                        if (Projectile.owner == Main.myPlayer)
                        {
                            Vector2 bubbleSpawnPosition = Projectile.Center + new Vector2(Main.rand.Next(-(int)BubbleSpawnRadius, (int)BubbleSpawnRadius), Main.rand.Next(-(int)BubbleSpawnRadius, (int)BubbleSpawnRadius));
                            if (Vector2.Distance(bubbleSpawnPosition, player.Center) > BubbleSpawnRadius)
                                bubbleSpawnPosition = Projectile.Center + new Vector2(Main.rand.Next(-(int)BubbleSpawnRadius, (int)BubbleSpawnRadius) / 2, Main.rand.Next(-(int)BubbleSpawnRadius, (int)BubbleSpawnRadius) / 2);

                            Point bubbleSpawnPoint = (bubbleSpawnPosition / 16f).ToPoint();
                            bubbleSpawnPoint.X = Math.Clamp(bubbleSpawnPoint.X, 0, Main.maxTilesX - 1);
                            bubbleSpawnPoint.Y = Math.Clamp(bubbleSpawnPoint.Y, 0, Main.maxTilesY - 1);
                            if (Main.tile[bubbleSpawnPoint.X, bubbleSpawnPoint.Y].HasTile)
                            {
                                int attempts = 0;
                                while (Main.tile[bubbleSpawnPoint.X, bubbleSpawnPoint.Y].HasTile && attempts < 5)
                                {
                                    attempts++;
                                    bubbleSpawnPoint.Y -= 2;
                                    bubbleSpawnPosition.Y -= 2 * 16;
                                }
                            }

                            Vector2 bubbleVelocity = new Vector2(0f, -Main.rand.Next(12, 24) / 10f);
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), bubbleSpawnPosition, bubbleVelocity, ModContent.ProjectileType<ControllableBubble>(), (int)(AltDamage * mPlayer.standDamageBoosts * 0.9f), 2f, Projectile.owner);
                            Main.projectile[projIndex].netUpdate = true;
                        }
                        SoundEngine.PlaySound(BubbleFieldBubbleSpawnSound, Projectile.Center);
                    }
                }
                if (SecondSpecialKeyPressed() && !player.HasBuff<BubbleBarrierBuff>() && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                {
                    player.AddBuff(ModContent.BuffType<BubbleBarrierBuff>(), 15 * 60);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<BubbleBarrier>(), 0, 0f, Projectile.owner, Projectile.whoAmI);
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
                BasicPunchAI();

            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        public int GetPlunderBubbleType()
        {
            Player player = Main.player[Projectile.owner];
            if (player.HeldItem.type == ItemID.Torch)
                return PlunderBubble.Plunder_Fire;
            if (player.HeldItem.type == ItemID.IchorTorch)
                return PlunderBubble.Plunder_Ichor;
            if (player.HeldItem.type == ItemID.CursedTorch)
                return PlunderBubble.Plunder_Cursed;
            if (player.HeldItem.type == ItemID.IceTorch)
                return PlunderBubble.Plunder_Ice;
            if (player.HeldItem.type == ModContent.ItemType<ViralPowder>())
                return PlunderBubble.Plunder_Viral;

            return PlunderBubble.Plunder_None;
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
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SoftAndWet/SoftAndWet_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 2, true);
        }
    }
}