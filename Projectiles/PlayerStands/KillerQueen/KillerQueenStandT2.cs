using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.KillerQueen
{
    public class KillerQueenStandT2 : StandClass
    {
        public override int punchDamage => 35;
        public override int altDamage => 134;
        public override int punchTime => 11;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 5f;
        public override float maxAltDistance => 201f;     //about 10 tiles
        public override string poseSoundName => "IWouldntLose";
        public override string spawnSoundName => "Killer Queen";
        public override bool CanUseSaladDye => true;
        public override StandType standType => StandType.Melee;


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

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    Punch();
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
                if (Main.mouseRight && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                {
                    shootCount += 10;
                    attackFrames = false;
                    idleFrames = false;
                    float mouseToPlayerDistance = Vector2.Distance(Main.MouseWorld, player.Center);

                    if (!touchedNPC && !touchedTile)
                    {
                        if (mouseToPlayerDistance < maxAltDistance)
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
                                        SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/KQButtonClick"));
                                        break;
                                    }
                                }
                            }
                            if (!foundNPCTarget)
                            {
                                if (Collision.SolidCollision(Main.MouseWorld, 1, 1) && !touchedTile)
                                {
                                    shootCount += 20;
                                    touchedTile = true;
                                    savedPosition = Main.MouseWorld;
                                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/KQButtonClick"));
                                }
                            }
                        }
                    }
                    else
                    {
                        shootCount += 20;
                        if (touchedNPC)
                        {
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                NPC npc = Main.npc[n];
                                if (npc.active)
                                {
                                    JoJoGlobalNPC jojoNPC = npc.GetGlobalNPC<JoJoGlobalNPC>();
                                    if (jojoNPC.taggedByKillerQueen)
                                    {
                                        touchedNPC = false;
                                        int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), npc.position, Vector2.Zero, ModContent.ProjectileType<KillerQueenBomb>(), 0, 9f, player.whoAmI, (int)(altDamage * mPlayer.standDamageBoosts), npc.whoAmI);
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
                            secondaryAbilityFrames = true;
                            int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), savedPosition, Vector2.Zero, ModContent.ProjectileType<KillerQueenBomb>(), 0, 9f, player.whoAmI, (int)(altDamage * mPlayer.standDamageBoosts));
                            Main.projectile[projectile].timeLeft = 2;
                            Main.projectile[projectile].netUpdate = true;
                            savedPosition = Vector2.Zero;
                        }
                    }
                }
                else
                {
                    secondaryAbilityFrames = false;
                }
            }
            if (mPlayer.standAutoMode)
            {
                NPC target = FindNearestTarget(maxDistance * 1.5f);
                if (!attackFrames)
                {
                    StayBehind();
                }
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
                    attackFrames = false;
                    idleFrames = false;
                    secondaryAbilityFrames = true;

                    explosionTimer++;
                    if (explosionTimer == 5)
                        SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/KQButtonClick"));

                    if (explosionTimer >= 90)
                    {
                        int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), savedTarget.position, Vector2.Zero, ModContent.ProjectileType<KillerQueenBomb>(), 0, 9f, player.whoAmI, (int)(altDamage * mPlayer.standDamageBoosts), savedTarget.whoAmI);
                        Main.projectile[projectile].timeLeft = 2;
                        Main.projectile[projectile].netUpdate = true;
                        explosionTimer = 0;
                        savedTarget = null;
                    }
                }
                if (target != null)
                {
                    attackFrames = true;
                    idleFrames = false;

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
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            if (Projectile.direction == 1)
                            {
                                shootVel *= shootSpeed;
                            }
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newPunchDamage * 0.9f), 3f, Projectile.owner, fistWhoAmI, tierNumber);
                            Main.projectile[proj].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    idleFrames = true;
                    attackFrames = false;
                }
            }

            if (touchedTile && MyPlayer.AutomaticActivations)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    float npcDistance = Vector2.Distance(npc.Center, savedPosition);
                    if (npc.active && !npc.friendly && npcDistance < 50f && touchedTile)       //or youd need to go from its center, add half its width to the direction its facing, and then add 16 (also with direction) -- Direwolf
                    {
                        touchedTile = false;
                        int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), savedPosition, Vector2.Zero, ModContent.ProjectileType<KillerQueenBomb>(), 0, 9f, player.whoAmI, (int)(altDamage * mPlayer.standDamageBoosts));
                        Main.projectile[projectile].timeLeft = 2;
                        Main.projectile[projectile].netUpdate = true;
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
            if (attackFrames)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
                if (Projectile.frame >= 4)      //cause it should only click once
                {
                    secondaryAbilityFrames = false;
                }
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/KillerQueen", "/KillerQueen_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 20, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 6, 18, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}