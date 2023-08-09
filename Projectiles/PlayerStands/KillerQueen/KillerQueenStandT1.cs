using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.KillerQueen
{
    public class KillerQueenStandT1 : StandClass
    {
        public override int PunchDamage => 14;
        public override int AltDamage => 92;
        public override int PunchTime => 14;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 5;
        public override int TierNumber => 1;
        public override float MaxAltDistance => 10 * 16;
        public override string PoseSoundName => "IWouldntLose";
        public override string SpawnSoundName => "Killer Queen";
        public override int AmountOfPunchVariants => 3;
        public override string PunchTexturePath => "JoJoStands/Projectiles/PlayerStands/KillerQueen/KillerQueen_Punch_";
        public override Vector2 PunchSize => new Vector2(32, 10);
        public override StandAttackType StandType => StandAttackType.Melee;
        public override bool CanUseSaladDye => true;
        private const float AutomaticExplosionDetectionDistance = 3 * 16f;
        private const int AnimationStallFrameAmount = 3;

        private int autoModeTriggerTimer = 0;
        private Vector2 savedPosition = Vector2.Zero;
        private bool touchedNPC = false;
        private bool touchedTile = false;
        private int animationStallFrameCount = 0;
        public int autoModeTaggedTargetIndex = -1;

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
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft)
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch();
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
                if (!attacking)
                    StayBehind();

                if (Main.mouseRight && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                {
                    shootCount += 10;
                    float mouseToPlayerDistance = Vector2.Distance(Main.MouseWorld, player.Center);

                    if (!touchedNPC && !touchedTile)
                    {
                        if (mouseToPlayerDistance < MaxAltDistance)
                        {
                            bool foundNPCTarget = false;        //This first so you can get targets in tiles, like worms
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                NPC npc = Main.npc[n];
                                if (npc.active)
                                {
                                    if (npc.Distance(Main.MouseWorld) <= (npc.width / 2f) + 20f)
                                    {
                                        shootCount += 20;
                                        touchedNPC = true;
                                        foundNPCTarget = true;
                                        npc.GetGlobalNPC<JoJoGlobalNPC>().taggedByKillerQueen = true;
                                        SoundEngine.PlaySound(KillerQueenStandFinal.KillerQueenClickSound, Projectile.Center);
                                        break;
                                    }
                                }
                            }
                            if (!foundNPCTarget)
                            {
                                bool mouseOnPlatform = TileID.Sets.Platforms[Main.tile[(int)(Main.MouseWorld.X / 16f), (int)(Main.MouseWorld.Y / 16f)].TileType];
                                if ((Collision.SolidCollision(Main.MouseWorld, 1, 1) || mouseOnPlatform) && !touchedTile)
                                {
                                    shootCount += 20;
                                    touchedTile = true;
                                    savedPosition = Main.MouseWorld;
                                    SoundEngine.PlaySound(KillerQueenStandFinal.KillerQueenClickSound, Projectile.Center);
                                }
                            }
                        }
                    }
                    else
                    {
                        shootCount += 20;
                        secondaryAbility = true;
                    }
                }

                if (touchedTile && JoJoStands.AutomaticActivations)
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        float npcDistance = Vector2.Distance(npc.Center, savedPosition);
                        if (npc.active && !npc.friendly && npcDistance < AutomaticExplosionDetectionDistance && touchedTile)
                        {
                            int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), savedPosition, Vector2.Zero, ModContent.ProjectileType<KillerQueenBomb>(), 0, 9f, player.whoAmI, (int)(AltDamage * mPlayer.standDamageBoosts));
                            Main.projectile[projectile].timeLeft = 2;
                            Main.projectile[projectile].netUpdate = true;
                            touchedTile = false;
                            savedPosition = Vector2.Zero;
                        }
                    }
                }

                if (secondaryAbility)
                {
                    currentAnimationState = AnimationState.SecondaryAbility;
                    if (Projectile.frame == 2)
                    {
                        if (touchedNPC)
                        {
                            touchedNPC = false;
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                NPC npc = Main.npc[n];
                                if (npc.active)
                                {
                                    JoJoGlobalNPC jojoNPC = npc.GetGlobalNPC<JoJoGlobalNPC>();
                                    if (jojoNPC.taggedByKillerQueen)
                                    {
                                        int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), npc.position, Vector2.Zero, ModContent.ProjectileType<KillerQueenBomb>(), 0, 9f, player.whoAmI, (int)(AltDamage * mPlayer.standDamageBoosts), npc.whoAmI);
                                        Main.projectile[projectile].timeLeft = 2;
                                        Main.projectile[projectile].netUpdate = true;
                                        jojoNPC.taggedByKillerQueen = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (touchedTile)
                        {
                            touchedTile = false;
                            currentAnimationState = AnimationState.SecondaryAbility;
                            int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), savedPosition, Vector2.Zero, ModContent.ProjectileType<KillerQueenBomb>(), 0, 9f, player.whoAmI, (int)(AltDamage * mPlayer.standDamageBoosts));
                            Main.projectile[projectile].timeLeft = 2;
                            Main.projectile[projectile].netUpdate = true;
                            savedPosition = Vector2.Zero;
                        }
                    }
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                NPC target = FindNearestTarget(newMaxDistance * 1.5f);
                if (!attacking)
                    StayBehind();

                if (autoModeTaggedTargetIndex != -1)
                {
                    NPC taggedTarget = Main.npc[autoModeTaggedTargetIndex];
                    if (taggedTarget.active)
                    {
                        float touchedTargetDistance = Vector2.Distance(player.Center, taggedTarget.Center);
                        if (touchedTargetDistance > newMaxDistance + 8f)       //if the target leaves and the bomb won't damage you, detonate the enemy
                        {
                            currentAnimationState = AnimationState.SecondaryAbility;
                            autoModeTriggerTimer++;
                            if (autoModeTriggerTimer == 5)
                                SoundEngine.PlaySound(KillerQueenStandFinal.KillerQueenClickSound, Projectile.Center);

                            if (autoModeTriggerTimer >= 90)
                            {
                                autoModeTriggerTimer = 0;
                                autoModeTaggedTargetIndex = -1;
                                int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), taggedTarget.Center, Vector2.Zero, ModContent.ProjectileType<KillerQueenBomb>(), 0, 9f, player.whoAmI, (int)(AltDamage * mPlayer.standDamageBoosts), taggedTarget.whoAmI);
                                Main.projectile[projectile].timeLeft = 2;
                                Main.projectile[projectile].netUpdate = true;
                            }
                        }
                    }
                    else
                        autoModeTaggedTargetIndex = -1;
                }

                if (target != null)
                {
                    currentAnimationState = AnimationState.Attack;
                    Projectile.direction = 1;
                    if (target.position.X - Projectile.Center.X < 0)
                        Projectile.direction = -1;
                    Projectile.spriteDirection = Projectile.direction;

                    Vector2 velocity = target.position - Projectile.position;
                    velocity.Normalize();
                    Projectile.velocity = velocity * 4f;

                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            shootCount += newPunchTime;
                            Vector2 shootVel = target.Center - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            if (Projectile.direction == 1)
                                shootVel *= ProjectileSpeed;

                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newPunchDamage * 0.9f), 3f, Projectile.owner, FistWhoAmI, TierNumber);
                            (Main.projectile[projIndex].ModProjectile as Fists).extraInfo1 = Projectile.whoAmI;
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                }
                else
                    currentAnimationState = AnimationState.Idle;
            }
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        public override bool PreDrawExtras()
        {
            if (touchedTile)
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Extras/Bomb");
                Main.EntitySpriteDraw(texture, savedPosition - Main.screenPosition, new Rectangle(0, 0, 16, 16), Color.White, 0f, new Vector2(16f / 2f), 1f, SpriteEffects.None, 0);
            }
            return true;
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
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void AnimationCompleted(string animationName)
        {
            if (animationName == "Secondary")
            {
                animationStallFrameCount++;
                if (animationStallFrameCount >= AnimationStallFrameAmount)
                {
                    secondaryAbility = false;
                    currentAnimationState = AnimationState.Idle;
                    animationStallFrameCount = 0;
                }
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/KillerQueen", "KillerQueen_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 20, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Secondary")
                AnimateStand(animationName, 5, 6, false);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 2, true);
        }
    }
}