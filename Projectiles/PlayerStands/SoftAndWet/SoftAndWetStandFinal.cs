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
    public class SoftAndWetStandFinal : StandClass
    {
        public override int PunchDamage => 79;
        public override int PunchTime => 10;
        public override int HalfStandHeight => 38;
        public override int AltDamage => ((int)(TierNumber * 15));
        public override Vector2 StandOffset => new Vector2(27, 0);
        public override int FistWhoAmI => 0;
        public override int TierNumber => 4;
        public override string PunchSoundName => "SoftAndWet_Ora";
        public override string PoseSoundName => "SoftAndWet";
        public override string SpawnSoundName => "Soft and Wet";
        public override StandAttackType StandType => StandAttackType.Melee;

        private const float BubbleSpawnRadius = 24 * 16f;
        private const int MaxBubbleBombs = 5;
        private const float MaxBombPlacementDistance = 8 * 16;
        private const int RightClickDustAmount = 15;
        private readonly SoundStyle BubbleFieldBubbleSpawnSound = new SoundStyle(SoundID.SplashWeak.SoundPath)
        {
            Volume = 0.4f,
            PitchVariance = 1f
        };

        private int amountOfBubbleBombs = 0;
        private Vector2[] bombPositions = new Vector2[MaxBubbleBombs];
        private bool bubbleMode = false;
        private int bubbleSpawnTimer = 0;
        private int explosionClickTimer = 0;
        private int rightClickTimer = 0;

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
                if (Main.mouseLeft && !bubbleMode && Projectile.owner == Main.myPlayer)
                {
                    Punch();
                    if (Main.rand.NextBool(9))
                    {
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= 3f;
                        Vector2 bubbleSpawnPosition = Projectile.Center + new Vector2(Main.rand.Next(0, 18 + 1) * Projectile.direction, -Main.rand.Next(0, HalfStandHeight - 2 + 1));
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), bubbleSpawnPosition, shootVel, ModContent.ProjectileType<TinyBubble>(), 32, 2f, Projectile.owner, Projectile.whoAmI);
                        SoundEngine.PlaySound(SoundID.Drip);
                    }
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        currentAnimationState = AnimationState.Idle;
                }
                if (!attacking)
                    StayBehind();

                bool rightClickReleasedPrematurely = false;
                if (Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    if (!bubbleMode)
                    {
                        Point clampedMousePosition = new Point(Math.Clamp((int)Main.MouseWorld.X / 16, 0, Main.maxTilesX), Math.Clamp((int)Main.MouseWorld.Y / 16, 0, Main.maxTilesY));
                        if (!playerHasAbilityCooldown && Vector2.Distance(player.Center, Main.MouseWorld) <= MaxBombPlacementDistance && Main.tile[clampedMousePosition.X, clampedMousePosition.Y].HasTile)
                        {
                            if (amountOfBubbleBombs < MaxBubbleBombs)
                            {
                                rightClickTimer++;
                                GenerateVisualClickRing(30);
                                if (rightClickTimer >= 30)
                                {
                                    currentAnimationState = AnimationState.Idle;
                                    bool mouseOnPlatform = TileID.Sets.Platforms[Main.tile[(int)(Main.MouseWorld.X / 16f), (int)(Main.MouseWorld.Y / 16f)].TileType];
                                    if (Collision.SolidCollision(Main.MouseWorld, 1, 1) || mouseOnPlatform)
                                    {
                                        bombPositions[amountOfBubbleBombs] = Main.MouseWorld;
                                        amountOfBubbleBombs++;
                                        SoundEngine.PlaySound(SoundID.SplashWeak);
                                    }
                                    rightClickTimer = 0;
                                }
                            }
                            else
                            {
                                rightClickTimer++;
                                GenerateVisualClickRing(45);
                                if (rightClickTimer >= 45)
                                {
                                    int numberProjectiles = 4;
                                    float rotation = MathHelper.ToRadians(55);
                                    float randomSpeedOffset = Main.rand.NextFloat(-3f, 3f);
                                    Vector2 shootVel = new(0f, 1f);
                                    for (int b = 0; b < amountOfBubbleBombs; b++)
                                    {
                                        for (int i = 0; i < numberProjectiles; i++)
                                        {
                                            Vector2 perturbedSpeed = new Vector2(shootVel.X + randomSpeedOffset, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / ((float)numberProjectiles - 1))) * .2f;
                                            perturbedSpeed.X += Main.rand.Next(-3, 3 + 1) / 10f;
                                            perturbedSpeed.Y = -3f * (Main.rand.Next(30, 100 + 1) / 100f);
                                            int timeLeft = Main.rand.Next(45, 75 + 1);
                                            int trapIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), bombPositions[b], perturbedSpeed, ModContent.ProjectileType<BombBubble>(), 0, 0f, Projectile.owner, MathHelper.ToRadians(Main.rand.Next(-2, 2 + 1)), timeLeft / 2);
                                            Main.projectile[trapIndex].timeLeft = timeLeft;
                                            Main.projectile[trapIndex].rotation = perturbedSpeed.ToRotation() + MathHelper.PiOver2;
                                            Main.projectile[trapIndex].netUpdate = true;
                                        }
                                    }
                                    amountOfBubbleBombs = 0;
                                    bombPositions = new Vector2[5];
                                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(15));
                                    rightClickTimer = 0;
                                }
                            }
                        }
                        else
                        {
                            if (amountOfBubbleBombs > 0 && !playerHasAbilityCooldown)
                            {
                                rightClickTimer++;
                                GenerateVisualClickRing(45);
                                if (rightClickTimer >= 45)
                                {
                                    int numberProjectiles = 4;
                                    float rotation = MathHelper.ToRadians(55);
                                    float randomSpeedOffset = Main.rand.NextFloat(-3f, 3f);
                                    Vector2 shootVel = new(0f, 1f);
                                    for (int b = 0; b < amountOfBubbleBombs; b++)
                                    {
                                        for (int i = 0; i < numberProjectiles; i++)
                                        {
                                            Vector2 perturbedSpeed = new Vector2(shootVel.X + randomSpeedOffset, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / ((float)numberProjectiles - 1))) * .2f;
                                            perturbedSpeed.X += Main.rand.Next(-3, 3 + 1) / 10f;
                                            perturbedSpeed.Y = -3f * (Main.rand.Next(30, 100 + 1) / 100f);
                                            int timeLeft = Main.rand.Next(45, 75 + 1);
                                            int trapIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), bombPositions[b], perturbedSpeed, ModContent.ProjectileType<BombBubble>(), 0, 0f, Projectile.owner, MathHelper.ToRadians(Main.rand.Next(-2, 2 + 1)), timeLeft / 2);
                                            Main.projectile[trapIndex].timeLeft = timeLeft;
                                            Main.projectile[trapIndex].rotation = perturbedSpeed.ToRotation() + MathHelper.PiOver2;
                                            Main.projectile[trapIndex].netUpdate = true;
                                        }
                                    }
                                    amountOfBubbleBombs = 0;
                                    bombPositions = new Vector2[5];
                                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(15));
                                    rightClickTimer = 0;
                                }
                            }
                            else
                            {
                                rightClickReleasedPrematurely = true;
                            }
                        }
                    }
                    else
                    {
                        explosionClickTimer = 5;
                    }
                }
                else
                {
                    if (rightClickTimer > 0)
                    {
                        if (rightClickTimer <= 30)
                            rightClickReleasedPrematurely = true;
                        rightClickTimer = 0;
                    }
                }

                if (rightClickReleasedPrematurely)
                {
                    GoInFront();
                    if (shootCount <= 0)
                    {
                        shootCount += 28;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= 3f;
                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<PlunderBubble>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, Projectile.owner, GetPlunderBubbleType());
                        Main.projectile[projIndex].netUpdate = true;
                        Projectile.netUpdate = true;
                        SoundEngine.PlaySound(SoundID.Item85);
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
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), bubbleSpawnPosition, bubbleVelocity, ModContent.ProjectileType<ControllableBubble>(), (int)(AltDamage * mPlayer.standDamageBoosts * 0.9f), 2f, Projectile.owner);
                            Main.projectile[projIndex].netUpdate = true;
                        }
                        SoundEngine.PlaySound(BubbleFieldBubbleSpawnSound);
                    }
                }

                if (SecondSpecialKeyPressed() && Projectile.owner == Main.myPlayer && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                {
                    player.AddBuff(ModContent.BuffType<BubbleBarrierBuff>(), 20 * 60);
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<BubbleBarrier>(), 0, 0f, Projectile.owner, Projectile.whoAmI);
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }

            if (amountOfBubbleBombs > 0 && JoJoStands.AutomaticActivations)
            {
                int numberProjectiles = 4;
                float rotation = MathHelper.ToRadians(55);
                float randomSpeedOffset = Main.rand.NextFloat(-3f, 3f);
                Vector2 shootVel = new Vector2(0f, 1f);
                for (int b = 0; b < amountOfBubbleBombs; b++)
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        float npcDistance = Vector2.Distance(npc.Center, bombPositions[b]);
                        if (npc.active && !npc.friendly && npcDistance < 24 + (npc.height / 2f))
                        {
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = new Vector2(shootVel.X + randomSpeedOffset, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / ((float)numberProjectiles - 1))) * .2f;
                                perturbedSpeed.X += Main.rand.Next(-3, 3 + 1) / 10f;
                                perturbedSpeed.Y = -3f * (Main.rand.Next(30, 100 + 1) / 100f);
                                int timeLeft = Main.rand.Next(45, 75 + 1);
                                int trapIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), bombPositions[b], perturbedSpeed, ModContent.ProjectileType<BombBubble>(), 0, 0f, Projectile.owner, MathHelper.ToRadians(Main.rand.Next(-2, 2 + 1)), timeLeft / 2);
                                Main.projectile[trapIndex].timeLeft =  timeLeft;
                                Main.projectile[trapIndex].rotation = perturbedSpeed.ToRotation() + MathHelper.PiOver2;
                                Main.projectile[trapIndex].netUpdate = true;
                            }
                            for (int j = b; j < amountOfBubbleBombs - 1; j++)
                            {
                                bombPositions[j] = bombPositions[j + 1];
                            }
                            bombPositions[amountOfBubbleBombs - 1] = Vector2.Zero;
                            amountOfBubbleBombs--;
                        }
                    }
                }
            }
        }

        private readonly Vector2 bubbleTrapOrigin = new Vector2(8f);

        public override bool PreDrawExtras()
        {
            if (amountOfBubbleBombs > 0)
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Extras/BubbleTrap");
                for (int b = 0; b < amountOfBubbleBombs; b++)
                {
                    Main.EntitySpriteDraw(texture, bombPositions[b] - Main.screenPosition, null, Color.White, 0f, bubbleTrapOrigin, 1f, SpriteEffects.None, 0);
                }
            }
            return true;
        }

        private int GetPlunderBubbleType()
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

        private void GenerateVisualClickRing(int waitTime)
        {
            float clickProgress = rightClickTimer / (float)waitTime;
            int amountOfDusts = (int)(RightClickDustAmount * clickProgress);
            for (int i = 0; i < amountOfDusts; i++)
            {
                float rotation = MathHelper.ToRadians(((360 / RightClickDustAmount) * i) - 90f);        //60 since it's the max amount of dusts that is supposed to circle it
                Vector2 dustPosition = Main.MouseWorld + (rotation.ToRotationVector2() * 16f);
                int dustIndex = Dust.NewDust(dustPosition, 1, 1, DustID.Cloud);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].noLight = true;
                Main.dust[dustIndex].velocity = Vector2.Zero;
            }
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