using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.ItemBuff;
using System;
using JoJoStands.NPCs;

namespace JoJoStands.Projectiles.PlayerStands.SoftAndWetGoBeyond
{
    public class SoftAndWetGoBeyondStand : StandClass
    {
        public override int PunchDamage => 109;
        public override int PunchTime => 9;
        public override int HalfStandHeight => 38;
        public override int AltDamage => 85;
        public override Vector2 StandOffset => new Vector2(27, 0);
        public override int FistWhoAmI => 0;
        public override int TierNumber => 5;
        public override string PunchSoundName => "SoftAndWet_Ora";
        public override string PoseSoundName => "SoftAndWet";
        public override string SpawnSoundName => "Soft and Wet";
        public override StandAttackType StandType => StandAttackType.Melee;

        private const float BubbleSpawnRadius = 24 * 16f;

        private int bubbleSpawnTimer = 0;
        private int highVelocityBubbleChargeUpTimer = 0;
        private int bubbleTargetIndex = -1;
        private readonly SoundStyle BubbleFieldBubbleSpawnSound = new SoundStyle(SoundID.SplashWeak.SoundPath);

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
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !secondaryAbilityFrames)
                {
                    Punch();
                    if (Main.rand.NextBool(7))
                    {
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= 3f;
                        Vector2 bubbleSpawnPosition = Projectile.Center + new Vector2(Main.rand.Next(0, 18 + 1) * Projectile.direction, -Main.rand.Next(0, HalfStandHeight - 2 + 1));
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), bubbleSpawnPosition, shootVel, ModContent.ProjectileType<TinyBubble>(), 44, 2f, Projectile.owner, Projectile.whoAmI);
                        SoundEngine.PlaySound(SoundID.Drip);
                    }
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    if (!secondaryAbilityFrames)
                    {
                        StayBehind();
                        Projectile.direction = Projectile.spriteDirection = player.direction;
                    }
                    else
                    {
                        GoInFront();
                        if (Projectile.owner == Main.myPlayer)
                        {
                            Projectile.direction = 1;
                            if (Main.MouseWorld.X < Projectile.position.X)
                                Projectile.direction = -1;
                            Projectile.spriteDirection = Projectile.direction;
                        }
                        secondaryAbilityFrames = false;
                    }
                }
                if (bubbleTargetIndex != -1 && (Main.npc[bubbleTargetIndex] == null || !Main.npc[bubbleTargetIndex].active || SpecialKeyPressed(false)))
                    bubbleTargetIndex = -1;

                if (Main.mouseRight && !playerHasAbilityCooldown && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                {
                    idleFrames = false;
                    attackFrames = false;
                    secondaryAbilityFrames = true;
                    highVelocityBubbleChargeUpTimer++;
                    if (highVelocityBubbleChargeUpTimer >= 60)
                    {
                        shootCount += 45;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= 9f;
                        Vector2 shootPosition = Projectile.position + new Vector2(40f * Projectile.direction, -10f);
                        int hvb = Projectile.NewProjectile(Projectile.GetSource_FromThis(), shootPosition, shootVel, ModContent.ProjectileType<HighVelocityBubble>(), (int)(5 * AltDamage * mPlayer.standDamageBoosts), 8f, Projectile.owner, bubbleTargetIndex);
                        Main.projectile[hvb].netUpdate = true;
                        Projectile.netUpdate = true;
                        highVelocityBubbleChargeUpTimer = 0;
                        SoundEngine.PlaySound(SoundID.Item130);
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(8));
                    }
                }
                if (highVelocityBubbleChargeUpTimer > 0 && !Main.mouseRight && Projectile.owner == Main.myPlayer)
                    highVelocityBubbleChargeUpTimer = 0;
                bubbleSpawnTimer++;
                if (bubbleSpawnTimer >= 45)
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
                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), bubbleSpawnPosition, bubbleVelocity, ModContent.ProjectileType<TrackerBubble>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, Projectile.owner, 0, bubbleTargetIndex);
                        Main.projectile[projIndex].netUpdate = true;
                    }
                    SoundEngine.PlaySound(BubbleFieldBubbleSpawnSound);
                }
                if (SpecialKeyPressed(false) && Projectile.owner == Main.myPlayer)
                {
                    int oldTarget = bubbleTargetIndex;
                    bubbleTargetIndex = -1;
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active)
                        {
                            if (npc.lifeMax > 5 && !npc.friendly && !npc.townNPC && Vector2.Distance(Main.MouseWorld, npc.Center) <= npc.height + 16f)
                            {
                                bubbleTargetIndex = npc.whoAmI;
                                break;
                            }
                        }
                    }
                    if (oldTarget != bubbleTargetIndex)
                    {
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active)
                                npc.GetGlobalNPC<JoJoGlobalNPC>().targetedBySoftAndWet = bubbleTargetIndex == npc.whoAmI ? true : false;
                        }
                        for (int p = 0; p < Main.maxProjectiles; p++)
                        {
                            Projectile otherProj = Main.projectile[p];
                            if (otherProj.active && otherProj.owner == Projectile.whoAmI && otherProj.type == ModContent.ProjectileType<TrackerBubble>())
                            {
                                otherProj.ai[1] = bubbleTargetIndex;
                                otherProj.netUpdate = true;
                            }
                        }
                    }
                }
                if (SecondSpecialKeyPressed() && Projectile.owner == Main.myPlayer && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                {
                    player.AddBuff(ModContent.BuffType<BubbleBarrierBuff>(), 10 * 60);
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                    Vector2 playerFollow = Vector2.Zero;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, playerFollow, ModContent.ProjectileType<BubbleBarrier>(), 0, 0f, Projectile.owner, Projectile.whoAmI);
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                PlayAnimation("Idle");
            }

            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().posing)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
            if (secondaryAbilityFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SoftAndWetGoBeyond/SoftAndWetGoBeyond_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 5, 6, true, 0, 3);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}