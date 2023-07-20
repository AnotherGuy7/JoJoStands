using JoJoStands.NPCs;
using JoJoStands.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.KillerQueen
{
    public class KillerQueenStandT3 : StandClass
    {
        public override int PunchDamage => 59;
        public override int AltDamage => 278;
        public override int PunchTime => 12;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 5;
        public override int TierNumber => 3;
        public override float MaxAltDistance => 14 * 16;
        public override string PoseSoundName => "IWouldntLose";
        public override string SpawnSoundName => "Killer Queen";
        public override bool CanUseSaladDye => true;
        public override StandAttackType StandType => StandAttackType.Melee;
        private readonly SoundStyle kqClickSound = new SoundStyle("JoJoStands/Sounds/GameSounds/KQButtonClick");

        private Vector2 savedPosition = Vector2.Zero;
        private bool touchedNPC = false;
        private bool touchedTile = false;
        private int explosionTimer = 0;

        public static NPC savedTarget = null;

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
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        currentAnimationState = AnimationState.Idle;
                }
                if (!attacking)
                {
                    StayBehind();
                }
                if (Main.mouseRight && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                {
                    shootCount += 10;
                    float mouseToPlayerDistance = Vector2.Distance(Main.MouseWorld, player.Center);

                    if (!touchedNPC && !touchedTile)
                    {
                        if (mouseToPlayerDistance < MaxAltDistance)
                        {
                            bool foundNPCTarget = false;
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
                                        SoundEngine.PlaySound(kqClickSound);
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
                                    SoundEngine.PlaySound(kqClickSound);
                                }
                            }
                        }
                    }
                    else
                    {
                        shootCount += 20;
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
                        if (touchedTile)
                        {
                            touchedTile = false;
                            secondaryAbility = true;
                            currentAnimationState = AnimationState.SecondaryAbility;
                            int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), savedPosition, Vector2.Zero, ModContent.ProjectileType<KillerQueenBomb>(), 0, 9f, player.whoAmI, (int)(AltDamage * mPlayer.standDamageBoosts));
                            Main.projectile[projectile].timeLeft = 2;
                            Main.projectile[projectile].netUpdate = true;
                            savedPosition = Vector2.Zero;
                        }
                    }
                }
                else
                    secondaryAbility = false;

                if (SpecialKeyPressed() && player.ownedProjectileCounts[ModContent.ProjectileType<SheerHeartAttack>()] == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 10f * Projectile.direction, Projectile.position.Y, 0f, 0f, ModContent.ProjectileType<SheerHeartAttack>(), 1, 0f, Projectile.owner, 0f);
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                NPC target = FindNearestTarget(newMaxDistance * 1.5f);
                if (!attacking)
                    StayBehind();
                float touchedTargetDistance = 0f;
                if (savedTarget != null)
                {
                    touchedTargetDistance = Vector2.Distance(player.Center, savedTarget.Center);
                    if (!savedTarget.active)
                    {
                        savedTarget = null;
                        explosionTimer = 0;
                    }
                }
                if (savedTarget != null && touchedTargetDistance > newMaxDistance + 8f)       //if the target leaves and the bomb won't damage you, detonate the enemy
                {
                    secondaryAbility = true;
                    currentAnimationState = AnimationState.SecondaryAbility;
                    explosionTimer++;
                    if (explosionTimer == 5)
                        SoundEngine.PlaySound(kqClickSound);

                    if (explosionTimer >= 90)
                    {
                        int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), savedTarget.position, Vector2.Zero, ModContent.ProjectileType<KillerQueenBomb>(), 0, 9f, player.whoAmI, (int)(AltDamage * mPlayer.standDamageBoosts), savedTarget.whoAmI);
                        Main.projectile[projectile].timeLeft = 2;
                        Main.projectile[projectile].netUpdate = true;
                        explosionTimer = 0;
                        savedTarget = null;
                    }
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
                            Vector2 shootVel = target.position - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            if (Projectile.direction == 1)
                                shootVel *= ProjectileSpeed;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newPunchDamage * 0.9f), 3f, Projectile.owner, FistWhoAmI, TierNumber);
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                }
                else
                    currentAnimationState = AnimationState.Idle;
            }

            if (touchedTile && JoJoStands.AutomaticActivations)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    float npcDistance = Vector2.Distance(npc.Center, savedPosition);
                    if (npc.active && !npc.friendly && npcDistance < 50f && touchedTile)       //or youd need to go from its center, add half its width to the direction its facing, and then add 16 (also with direction) -- Direwolf
                    {
                        int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), savedPosition, Vector2.Zero, ModContent.ProjectileType<KillerQueenBomb>(), 0, 9f, player.whoAmI, (int)(AltDamage * mPlayer.standDamageBoosts));
                        Main.projectile[projectile].timeLeft = 2;
                        Main.projectile[projectile].netUpdate = true;
                        touchedTile = false;
                        savedPosition = Vector2.Zero;
                    }
                }
            }
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

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/KillerQueen", "KillerQueen_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 20, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Secondary")
                AnimateStand(animationName, 6, 18, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 2, true);
        }
    }
}