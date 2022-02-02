using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands
{
    public abstract class StandClass : ModProjectile        //to simplify stand creation
    {
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 38;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override string Texture => mod.Name + "/Projectiles/PlayerStands/StandPlaceholder";
        public virtual float shootSpeed { get; } = 16f;       //how fast the projectile the minion shoots goes
        public virtual int punchDamage { get; }     //virtual is what allows that to be overridden
        public virtual int projectileDamage { get; }
        public virtual int halfStandHeight { get; }     //used to correctly set the Y position the stand is at during idle
        public virtual int punchTime { get; }
        public virtual int shootTime { get; }
        public virtual float maxDistance { get; } = 98f;
        public virtual float maxAltDistance { get; } = 0f;
        public virtual int altDamage { get; }
        public virtual float fistWhoAmI { get; }        //this is used in Fists.cs for effects
        public virtual int standOffset { get; } = 60;            //from an idle frame, get the first pixel from the left and standOffset = distance from that pixel you got to the right edge of the spritesheet - 38
        //public virtual int standYOffset { get; } = 0;
        public virtual float tierNumber { get; }
        public virtual float punchKnockback { get; } = 3f;
        public virtual string punchSoundName { get; } = "";
        public virtual string poseSoundName { get; } = "";
        public virtual string spawnSoundName { get; } = "";
        public virtual int standType { get; } = 0;
        public virtual Texture2D standTexture { get; set; }
        public virtual bool useProjectileAlpha { get; } = false;


        public bool normalFrames = false;       //Much easier to sync animations this way rather than syncing everything about it
        public bool attackFrames = false;
        public bool secondaryAbilityFrames = false;
        public int newPunchTime = 0;       //so we don't have to type newPunchTime all the time
        public int newShootTime = 0;
        public float newMaxDistance = 0f;
        public float newAltMaxDistance = 0f;
        public int newPunchDamage = 0;
        public int newProjectileDamage = 0;
        public bool playerHasAbilityCooldown = false;

        public int shootCount = 0;
        private Vector2 velocityAddition;
        private float mouseDistance;
        public bool secondaryAbility = false;
        private bool playedBeginning = false;
        private bool playedSpawnSound = false;
        private bool sentDyePacket = false;
        private SoundEffectInstance beginningSoundInstance = null;
        private SoundEffectInstance punchingSoundInstance = null;

        private const int RangeIndicatorSize = 320;

        /// <summary>
        /// Starts a timestop that lasts x amount of seconds.
        /// </summary>
        public void Timestop(int seconds)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.timestopEffectDurationTimer = 60;
            mPlayer.timestopActive = true;
            if (JoJoStands.JoJoStandsSounds == null)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/timestop_start"));
            }
            player.AddBuff(mod.BuffType("TheWorldBuff"), seconds * 60, true);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModNetHandler.effectSync.SendTimestop(256, player.whoAmI, true, player.whoAmI);
            }
        }

        /// <summary>
        /// Checks if the Special bind has just been pressed and there is no cooldown.
        /// </summary>
        /// <returns>True if the bind has just been pressed and the player has no ability cooldown on them. Returns false otherwise.</returns>
        public bool SpecialKeyPressed()     //checks for if this isn't the server, if the key is pressed, and if the player has no cooldown (is the owner check even needed?)
        {
            bool specialPressed = false;
            if (!Main.dedServ)      //if it's the clinet, as hotkeys don't exist on the server
                specialPressed = JoJoStands.SpecialHotKey.JustPressed;
            return specialPressed && !Main.player[projectile.owner].HasBuff(mod.BuffType("AbilityCooldown")) && projectile.owner == Main.myPlayer;
        }

        /// <summary>
        /// Checks if the Special bind has just been pressed.
        /// </summary>
        /// <returns>True if the bind has just been pressed. Returns false otherwise.</returns>
        public bool SpecialKeyPressedNoCooldown()
        {
            bool specialPressed = false;
            if (!Main.dedServ)
                specialPressed = JoJoStands.SpecialHotKey.JustPressed;
            return specialPressed && projectile.owner == Main.myPlayer;
        }

        /// <summary>
        /// Checks if the Special bind is currently being held down.
        /// </summary>
        /// <returns>True if the Special bind is being held down.</returns>
        public bool SpecialKeyCurrent()
        {
            bool specialPressed = false;
            if (!Main.dedServ)
                specialPressed = JoJoStands.SpecialHotKey.Current;
            return specialPressed && projectile.owner == Main.myPlayer;
        }

        /// <summary>
        /// Checks if the Second Special bind has just been pressed and the player has no Ability Cooldown.
        /// </summary>
        /// <returns>True if the Second Special bind has just been pressed and the player has no Ability Cooldown. False if otherwise.</returns>
        public bool SecondSpecialKeyPressed()     //checks for if this isn't the server, if the key is pressed, and if the player has no cooldown (is the owner check even needed?)
        {
            bool specialPressed = false;
            if (!Main.dedServ)      //if it's the clinet, as hotkeys don't exist on the server
                specialPressed = JoJoStands.SecondSpecialHotKey.JustPressed;
            return specialPressed && !Main.player[projectile.owner].HasBuff(mod.BuffType("AbilityCooldown")) && projectile.owner == Main.myPlayer;
        }

        /// <summary>
        /// Checks if the Second Special bind has just been pressed.
        /// </summary>
        /// <returns>True if the Second Special bind has just been pressed. False if otherwise.</returns>
        public bool SecondSpecialKeyPressedNoCooldown()
        {
            bool specialPressed = false;
            if (!Main.dedServ)
                specialPressed = JoJoStands.SecondSpecialHotKey.JustPressed;
            return specialPressed && projectile.owner == Main.myPlayer;
        }

        /// <summary>
        /// Checks if the Second Special bind is currently being held down.
        /// </summary>
        /// <returns>True if the Second Special bind is currently being held down. False if otherwise.</returns>
        public bool SecondSpecialKeyCurrent()
        {
            bool specialPressed = false;
            if (!Main.dedServ)
                specialPressed = JoJoStands.SecondSpecialHotKey.Current;
            return specialPressed && projectile.owner == Main.myPlayer;
        }

        /// <summary>
        /// Has the Stand switch to its attack state and punch.
        /// Damage, knockback, and punch speed depend on the punchDamage, punchKnockback, and punchTime fields respectively.
        /// Stand ID and Stand Tier Number are passed into the fist projectile.
        /// </summary>
        /// <param name="movementSpeed">How fast the Stand moves while it's punching</param>
        ///  /// <param name="punchLifeTimeMultiplier">A multiplier for the punch projectiles' lifetime.</param>
        public void Punch(float movementSpeed = 5f, float punchLifeTimeMultiplier = 1f)
        {
            Player player = Main.player[projectile.owner];

            HandleDrawOffsets();
            normalFrames = false;
            attackFrames = true;
            projectile.netUpdate = true;
            float rotaY = Main.MouseWorld.Y - projectile.Center.Y;
            projectile.rotation = MathHelper.ToRadians((rotaY * projectile.spriteDirection) / 6f);
            velocityAddition = Main.MouseWorld - projectile.position;
            velocityAddition.Normalize();
            velocityAddition *= movementSpeed;

            projectile.spriteDirection = 1;
            projectile.direction = 1;
            if (Main.MouseWorld.X < projectile.position.X)
            {
                projectile.spriteDirection = -1;
                projectile.direction = -1;
            }

            mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
            if (mouseDistance > 40f)
            {
                projectile.velocity = player.velocity + velocityAddition;
            }
            if (mouseDistance <= 40f)
            {
                projectile.velocity = Vector2.Zero;
            }
            PlayPunchSound();
            if (shootCount <= 0)
            {
                shootCount += newPunchTime;
                Vector2 shootVel = Main.MouseWorld - projectile.Center;
                if (shootVel == Vector2.Zero)
                {
                    shootVel = new Vector2(0f, 1f);
                }
                shootVel.Normalize();
                shootVel *= shootSpeed;
                int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("Fists"), newPunchDamage, punchKnockback, projectile.owner, fistWhoAmI, tierNumber);
                Main.projectile[proj].timeLeft = (int)(Main.projectile[proj].timeLeft * punchLifeTimeMultiplier);
                Main.projectile[proj].netUpdate = true;
                projectile.netUpdate = true;
            }
            LimitDistance();
        }

        /// <summary>
        /// Has the Stand switch to its idle state and stay behind the player.
        /// </summary>
        public void StayBehind()
        {
            Player player = Main.player[projectile.owner];

            normalFrames = true;
            attackFrames = false;
            Vector2 areaBehindPlayer = player.Center;
            areaBehindPlayer.X -= (float)((12 + player.width / 2) * player.direction);
            areaBehindPlayer.Y -= -35f + halfStandHeight;
            projectile.Center = Vector2.Lerp(projectile.Center, areaBehindPlayer, 0.2f);
            projectile.velocity *= 0.8f;
            projectile.direction = projectile.spriteDirection = player.direction;
            projectile.rotation = 0;
            HandleDrawOffsets();
            LimitDistance();
            StopSounds();
        }

        /// <summary>
        /// Has the Stand switch to its idle state and stay behind the player.
        /// During a secondary ability, the stand is able to face any direction.
        /// </summary>
        public void StayBehindWithAbility()
        {
            Player player = Main.player[projectile.owner];

            normalFrames = true;
            attackFrames = false;
            Vector2 areaBehindPlayer = player.Center;
            areaBehindPlayer.X -= (float)((12 + player.width / 2) * player.direction);
            areaBehindPlayer.Y -= -35f + halfStandHeight;
            projectile.Center = Vector2.Lerp(projectile.Center, areaBehindPlayer, 0.2f);
            projectile.velocity *= 0.8f;
            if (!secondaryAbilityFrames)
            {
                projectile.direction = projectile.spriteDirection = player.direction;
            }
            projectile.rotation = 0;
            HandleDrawOffsets();
            LimitDistance();
            StopSounds();
        }

        /// <summary>
        /// Has the Stand switch to its idle state and go in front of the player.
        /// </summary>
        public void GoInFront()
        {
            Player player = Main.player[projectile.owner];

            normalFrames = true;
            attackFrames = false;
            Vector2 areaBehindPlayer = player.Center;
            areaBehindPlayer.X += (float)((12 + player.width / 2) * player.direction);
            areaBehindPlayer.Y -= -35f + halfStandHeight;
            projectile.Center = Vector2.Lerp(projectile.Center, areaBehindPlayer, 0.2f);
            projectile.velocity *= 0.8f;
            projectile.direction = (projectile.spriteDirection = player.direction);
            projectile.rotation = 0;
            HandleDrawOffsets();
            LimitDistance();
        }

        /// <summary>
        /// A basic punch AI for Stands.
        /// Detects enemies as far as what the maxDistance field is set to then multiplied by 1.2f (in pixels).
        /// For use in Auto Mode.
        /// </summary>
        public void BasicPunchAI()
        {
            Player player = Main.player[projectile.owner];

            HandleDrawOffsets();
            if (!attackFrames)
            {
                Vector2 vector131 = player.Center;
                vector131.X -= (float)((12 + player.width / 2) * player.direction);
                projectile.direction = (projectile.spriteDirection = player.direction);
                vector131.Y -= -35f + halfStandHeight;
                projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                projectile.velocity *= 0.8f;
                projectile.rotation = 0;
            }
            float detectionDist = newMaxDistance * 1.2f;
            NPC target = FindNearestTarget(detectionDist);
            if (target != null)
            {
                attackFrames = true;
                normalFrames = false;

                projectile.direction = 1;
                if (target.position.X - projectile.Center.X <= 0f)
                {
                    projectile.direction = -1;
                }
                projectile.spriteDirection = projectile.direction;

                Vector2 velocity = target.position - projectile.position;
                velocity.Normalize();
                projectile.velocity = velocity * 4f;
                if (shootCount <= 0)
                {
                    if (Main.myPlayer == projectile.owner)
                    {
                        PlayPunchSound();
                        shootCount += newPunchTime;
                        Vector2 shootVel = target.position - projectile.Center;
                        shootVel.Normalize();
                        if (projectile.direction == 1)
                        {
                            shootVel *= shootSpeed;
                        }
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("Fists"), (int)(newPunchDamage * 0.9f), punchKnockback, projectile.owner, fistWhoAmI, tierNumber);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
            }
            else
            {
                normalFrames = true;
                attackFrames = false;
            }
            LimitDistance();
        }

        /// <summary>
        /// A basic punch AI for Stands.
        /// Contains additional functionality for secondary abilities, which allows the stand to use ranged abilities while in Auto Mode.
        /// For use in Auto Mode.
        /// </summary>
        /// <param name="projToShoot">The type of projectile that the sSand will shoot.</param>
        /// <param name="itemToConsumeType">The type of item that will be consumed whenever the Stand uses its secondary ability.</param>
        /// <param name="gravityAccounting">Whether or not the Stand should account for gravity when using its secondary ability.</param>
        /// <param name="shootMax">The limit on the amount of the same projectile that the Stand can use with its secondary ability.</param>
        public void PunchAndShootAI(int projToShoot, int itemToConsumeType = -1, bool gravityAccounting = false, int shootMax = 999)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            HandleDrawOffsets();
            float rangedDetectionDist = newMaxDistance * 3f;        //Distance for ranged attacks to work
            float punchDetectionDist = newMaxDistance * 1.5f;       //Distance for melee attacks to work
            float maxDetectionDist = newMaxDistance * 3.1f;
            float targetDist = maxDetectionDist;
            NPC target = FindNearestTarget(targetDist);
            if (target != null)
            {
                targetDist = Vector2.Distance(target.Center, player.Center);
            }
            if (targetDist > punchDetectionDist || secondaryAbility || target == null)
            {
                Vector2 areaBehindPlayer = player.Center;
                normalFrames = true;
                attackFrames = false;
                if (secondaryAbility)
                {
                    areaBehindPlayer.X += (float)((12 + player.width / 2) * player.direction);
                }
                else
                {
                    areaBehindPlayer.X -= (float)((12 + player.width / 2) * player.direction);
                    projectile.spriteDirection = projectile.direction = player.direction;
                }
                areaBehindPlayer.Y -= -35f + halfStandHeight;
                projectile.Center = Vector2.Lerp(projectile.Center, areaBehindPlayer, 0.2f);
                projectile.velocity *= 0.8f;
                projectile.rotation = 0;
            }
            if (target != null)
            {
                if (targetDist < punchDetectionDist && !secondaryAbility)
                {
                    attackFrames = true;
                    normalFrames = false;
                    secondaryAbilityFrames = false;

                    projectile.direction = 1;
                    if (target.position.X - projectile.Center.X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    Vector2 velocity = target.position - projectile.position;
                    velocity.Normalize();
                    projectile.velocity = velocity * 4f;
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            PlayPunchSound();
                            shootCount += newPunchTime;
                            Vector2 shootVel = target.position - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            if (projectile.direction == 1)
                            {
                                shootVel *= shootSpeed;
                            }
                            int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("Fists"), (int)(newPunchDamage * 0.9f), punchKnockback, projectile.owner, fistWhoAmI, tierNumber);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                }
                else if (Main.rand.Next(0, 101) <= 1 && targetDist > punchDetectionDist)
                {
                    if (itemToConsumeType != -1 && MyPlayer.AutomaticActivations && player.HasItem(itemToConsumeType))
                    {
                        secondaryAbility = true;
                    }
                    else if (itemToConsumeType == -1)
                    {
                        secondaryAbility = true;
                    }
                }

                if (secondaryAbility)
                {
                    attackFrames = false;
                    normalFrames = false;
                    secondaryAbilityFrames = true;
                    projectile.direction = 1;
                    if (target.position.X - projectile.Center.X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    if (shootCount <= 0 && player.ownedProjectileCounts[projToShoot] < shootMax)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            if (shootCount <= 0)
                            {
                                shootCount += 28;
                                Vector2 shootVel = target.position - projectile.Center - new Vector2(0f, 3f);
                                if (shootVel == Vector2.Zero)
                                {
                                    shootVel = new Vector2(0f, 1f);
                                }
                                shootVel.Normalize();
                                shootVel *= shootSpeed;
                                if (gravityAccounting)
                                {
                                    shootVel.Y -= projectile.Distance(target.position) / 110f;        //Adding force with the distance of the enemy / 110 (Dividing by 110 cause if not it's gonna fly straight up)
                                }
                                int proj = Projectile.NewProjectile(projectile.position.X + 5f, projectile.position.Y - 3f, shootVel.X, shootVel.Y, projToShoot, (int)((altDamage * mPlayer.standDamageBoosts) * 0.9f), 2f, projectile.owner, projectile.whoAmI, tierNumber);
                                Main.projectile[proj].netUpdate = true;
                                projectile.netUpdate = true;
                            }
                            if (itemToConsumeType != -1)
                            {
                                player.ConsumeItem(itemToConsumeType);
                            }
                        }
                    }
                    if ((!gravityAccounting && player.ownedProjectileCounts[projToShoot] == 0) || targetDist > rangedDetectionDist)     //!gravityAccounting and 0 of that projectile cause it's usually no gravity projectiles that are just 1 shot (star finger, zipper punch), while things like knives have gravity (meant to be thrown in succession)
                    {
                        secondaryAbility = false;
                        secondaryAbilityFrames = false;
                        normalFrames = true;
                    }
                }
            }
            LimitDistance();
        }

        /// <summary>
        /// Initializes the sounds the Stand would use. 
        /// Sounds are checked for in the JoJoStands Sounds directory.
        /// This method checks in Sounds/BattleCries/ for the sound.
        /// </summary>
        public void InitializeSounds()
        {
            if (!JoJoStands.SoundsLoaded)
                return;

            if (beginningSoundInstance == null)
            {
                SoundEffect sound = JoJoStands.JoJoStandsSounds.GetSound("Sounds/BattleCries/" + punchSoundName + "_Beginning");
                if (sound != null)
                {
                    beginningSoundInstance = sound.CreateInstance();
                    beginningSoundInstance.Volume = MyPlayer.ModSoundsVolume;
                }
            }
            if (punchingSoundInstance == null)
            {
                SoundEffect sound = JoJoStands.JoJoStandsSounds.GetSound("Sounds/BattleCries/" + punchSoundName);
                punchingSoundInstance = sound.CreateInstance();
                punchingSoundInstance.Volume = MyPlayer.ModSoundsVolume;
            }
        }

        /// <summary>
        /// Call this method to play the punch sounds.
        /// </summary>
        public void PlayPunchSound()
        {
            if (!JoJoStands.SoundsLoaded)
                return;

            if (punchSoundName != "" && punchingSoundInstance == null)
            {
                InitializeSounds();
            }
            if (punchSoundName != "")
            {
                if (beginningSoundInstance != null)
                {
                    if (!playedBeginning)
                    {
                        //beginningSoundInstance.Play();     //is this not just beginningSoundInstance.Play()?
                        beginningSoundInstance.Volume = MyPlayer.ModSoundsVolume;
                        Main.PlaySoundInstance(beginningSoundInstance);                 //if there is no other way to have this play for everyone, send a packet with that sound type so that it plays for everyone
                        playedBeginning = true;
                    }
                    if (playedBeginning && beginningSoundInstance.State == SoundState.Stopped)
                    {
                        //punchingSoundInstance.Play();     //is this not just beginningSoundInstance.Play()?
                        punchingSoundInstance.Volume = MyPlayer.ModSoundsVolume;
                        Main.PlaySoundInstance(punchingSoundInstance);
                        SyncSounds();
                    }
                }
                else
                {
                    //punchingSoundInstance.Play();
                    Main.PlaySoundInstance(punchingSoundInstance);
                    SyncSounds();
                    playedBeginning = true;
                }
            }
        }

        /// <summary>
        /// Syncs the sounds with other clients in the server.
        /// </summary>
        public void SyncSounds()
        {
            if (!JoJoStands.SoundsLoaded)
                return;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (punchingSoundInstance != null)
                {
                    Player player = Main.player[projectile.owner];
                    string path = "Sounds/BattleCries/" + punchSoundName;
                    SoundState state = punchingSoundInstance.State;
                    //SendSoundInstance(int sender, string soundPath, SoundState state, Vector2 pos (this one has to be sent as individual float values), float soundTravelDistance = 10f), which is the method called here
                    JoJoStands.JoJoStandsSounds.Call("SendSoundInstance", player.whoAmI, path, state, projectile.Center.X, projectile.Center.Y, 40);
                }
            }
        }

        /// <summary>
        /// Stops all the sounds this Stand is currently playing.
        /// </summary>
        public void StopSounds()
        {
            if (!JoJoStands.SoundsLoaded)
                return;

            if (playedBeginning)
            {
                if (beginningSoundInstance != null)
                {
                    beginningSoundInstance.Stop();
                    SyncSounds();
                }
                if (punchingSoundInstance != null)
                {
                    punchingSoundInstance.Stop();
                    SyncSounds();
                }
                playedBeginning = false;
            }
            if (projectile.netUpdate)       //We put it here cause we don't want to sync this all the time, but specifically whenever this method is called (Idles)
            {
                SyncSounds();
            }
        }

        /// <summary>
        /// Adjusts the draw offsets of the Stand based on its projectile.spriteDirection field.
        /// </summary>
        public void HandleDrawOffsets()     //this method kind of lost its usage when we found a better way to do offsets but whatever
        {
            drawOffsetX = standOffset * projectile.spriteDirection;
        }

        /// <summary>
        /// Updates all client-side stand info.
        /// Updates newPunchTime, newShootTime, newMaxDistance, newAltMaxDistance, newPunchDamage, newProjectileDamage, and mPlayer.standType.
        /// </summary>
        public void UpdateStandInfo()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            newPunchTime = punchTime - mPlayer.standSpeedBoosts;
            newShootTime = shootTime - mPlayer.standSpeedBoosts;
            newMaxDistance = maxDistance + mPlayer.standRangeBoosts;
            newAltMaxDistance = maxAltDistance + mPlayer.standRangeBoosts;
            newPunchDamage = (int)(punchDamage * mPlayer.standDamageBoosts);
            newProjectileDamage = (int)(projectileDamage * mPlayer.standDamageBoosts);
            playerHasAbilityCooldown = player.HasBuff(mod.BuffType("AbilityCooldown"));
            mPlayer.poseSoundName = poseSoundName;
            if (newPunchTime <= 2)
            {
                newPunchTime = 2;
            }
            if (newShootTime <= 5)
            {
                newShootTime = 5;
            }
            if (mPlayer.standType != standType)
            {
                mPlayer.standType = standType;
            }
            if (JoJoStands.SoundsLoaded)
            {
                if (spawnSoundName != "" && !playedSpawnSound)
                {
                    Terraria.Audio.LegacySoundStyle spawnSound = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SummonCries/" + spawnSoundName);
                    spawnSound.WithVolume(MyPlayer.ModSoundsVolume);
                    Main.PlaySound(spawnSound, projectile.position);
                    playedSpawnSound = true;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)      //from ExampleMod ExampleDeathShader
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);        //starting a draw with dyes that work

            DrawRangeIndicators(spriteBatch);       //not affected by dyes since it's starting a new batch with no effect
            SyncAndApplyDyeSlot();
            DrawStand(spriteBatch, drawColor);

            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)     //manually drawing stands cause sometimes stands had too many frames, it's easier to manage this way, and dye effects didn't work for stands that were overriding PostDraw
        {
            spriteBatch.End();     //ending the spriteBatch that started in PreDraw
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
        }

        public SpriteEffects effects = SpriteEffects.None;

        /// <summary>
        /// Draws the Stand.
        /// </summary>
        private void DrawStand(SpriteBatch spriteBatch, Color drawColor)
        {
            if (useProjectileAlpha)
                drawColor *= projectile.alpha / 255f;

            effects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
                effects = SpriteEffects.FlipHorizontally;

            if (standTexture != null && Main.netMode != NetmodeID.Server)
            {
                int frameHeight = standTexture.Height / Main.projFrames[projectile.whoAmI];
                Vector2 drawPosition = projectile.Center - Main.screenPosition + new Vector2(drawOffsetX / 2f, 0f);
                Rectangle animRect = new Rectangle(0, frameHeight * projectile.frame, standTexture.Width, frameHeight);
                Vector2 standOrigin = new Vector2(standTexture.Width / 2f, frameHeight / 2f);
                spriteBatch.Draw(standTexture, drawPosition, animRect, drawColor, projectile.rotation, standOrigin, 1f, effects, 0);
            }
        }

        /// <summary>
        /// Draws the Stands range indicators.
        /// Only draws if the MyPlayer.RangeIndicators field is set to true.
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawRangeIndicators(SpriteBatch spriteBatch)
        {
            Player player = Main.player[projectile.owner];

            if (MyPlayer.RangeIndicators && Main.netMode != NetmodeID.Server)
            {
                Texture2D texture = mod.GetTexture("Extras/RangeIndicator");        //the initial tile amount the indicator covers is 20 tiles, 320 pixels, border is included in the measurements
                Vector2 rangeIndicatorDrawPosition = player.Center - Main.screenPosition;
                float rangeIndicatorAlpha = (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f);
                Vector2 rangeIndicatorOrigin = new Vector2(RangeIndicatorSize / 2f);
                if (maxDistance > 0f)
                    spriteBatch.Draw(texture, rangeIndicatorDrawPosition, null, Color.White * rangeIndicatorAlpha, 0f, rangeIndicatorOrigin, newMaxDistance / (RangeIndicatorSize / 2f), SpriteEffects.None, 0);

                if (maxAltDistance > 0f)
                    spriteBatch.Draw(texture, rangeIndicatorDrawPosition, null, Color.Orange * rangeIndicatorAlpha, 0f, rangeIndicatorOrigin, newAltMaxDistance / (RangeIndicatorSize / 2f), SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Syncs and applies the current dye in the Dye Slot to the Stand.
        /// </summary>
        public void SyncAndApplyDyeSlot()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.StandDyeSlot.Item.dye != 0)
            {
                ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(mPlayer.StandDyeSlot.Item.type);
                shader.Apply(null);     //has to be on top of whatever it's applying to (I spent hours and hours trying to find a solution and the only problem was that this was under it)
                if (!sentDyePacket)       //we sync dye slot item here
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModNetHandler.playerSync.SendDyeItem(256, player.whoAmI, mPlayer.StandDyeSlot.Item.type, player.whoAmI);
                    }
                    sentDyePacket = true;
                }
            }
            if (mPlayer.StandDyeSlot.Item.dye == 0)
            {
                if (sentDyePacket)
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModNetHandler.playerSync.SendDyeItem(256, player.whoAmI, mPlayer.StandDyeSlot.Item.type, player.whoAmI);
                    }
                    sentDyePacket = false;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            MyPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
            mPlayer.standType = 0;
            mPlayer.poseSoundName = "";
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(normalFrames);
            writer.Write(attackFrames);
            writer.Write(secondaryAbilityFrames);
            SendExtraStates(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            normalFrames = reader.ReadBoolean();
            attackFrames = reader.ReadBoolean();
            secondaryAbilityFrames = reader.ReadBoolean();
            ReceiveExtraStates(reader);
        }

        /// <summary>
        /// Use this method to write any extra information that needs to be communicated to other clients about this Stand.
        /// Called when projectile.netUpdate is True. Sends the extra info to other clients.
        /// </summary>
        /// <param name="writer"></param>
        public virtual void SendExtraStates(BinaryWriter writer)        //for those extra special 4th states (TH Charge, TW Pose, GER Rock Flick)
        { }

        /// <summary>
        /// Use this method to read any extra information that has been communicated from other clients about that Stand.
        /// Called when a client receives extra Stand information from other clients.
        /// </summary>
        /// <param name="reader"></param>
        public virtual void ReceiveExtraStates(BinaryReader reader)
        { }

        /// <summary>
        /// Limits the distance the Stand can travel.
        /// </summary>
        public void LimitDistance()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            Vector2 direction = player.Center - projectile.Center;
            float distanceFromPlayer = direction.Length();
            if (distanceFromPlayer > newMaxDistance)
            {
                direction.Normalize();
                direction *= 0.8f;
                projectile.velocity = player.velocity + direction;
            }

            if (distanceFromPlayer >= newMaxDistance + 22f)
            {
                if (!mPlayer.standAutoMode)
                {
                    Main.mouseLeft = false;
                    Main.mouseRight = false;
                }
                projectile.Center = player.Center;
            }
        }

        /// <summary>
        /// Limits the distance the Stand can travel.
        /// </summary>
        public void LimitDistance(float maxDistance, bool affectedByRangeModifiers = false)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (affectedByRangeModifiers)
                maxDistance += mPlayer.standRangeBoosts;

            Vector2 direction = player.Center - projectile.Center;
            float distanceFromPlayer = direction.Length();
            if (distanceFromPlayer > maxDistance)
            {
                direction.Normalize();
                direction *= 0.8f;
                projectile.velocity = player.velocity + direction;
            }

            if (distanceFromPlayer >= maxDistance)
            {
                if (!mPlayer.standAutoMode)
                {
                    Main.mouseLeft = false;
                    Main.mouseRight = false;
                }
                projectile.Center = player.Center;
            }
        }

        /// <summary>
        /// Find the closest NPC to the player.
        /// Criteria for the search is set by the MyPlayer.standSearchType field.
        /// </summary>
        /// <param name="maxDetectionRange"></param>
        /// <returns>The NPC that is closest to the player and follows the given criteria.</returns>
        public NPC FindNearestTarget(float maxDetectionRange)
        {
            NPC target = null;
            Player player = Main.player[projectile.owner];
            switch (MyPlayer.standSearchType)
            {
                case MyPlayer.StandSearchType.Bosses:
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < maxDetectionRange && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                            {
                                if (npc.boss)       //is gonna try to detect bosses over anything
                                {
                                    target = npc;
                                    break;
                                }
                                else        //if it fails to detect a boss, it'll detect the next best thing
                                {
                                    target = npc;
                                }
                            }
                        }
                    }
                    break;
                case MyPlayer.StandSearchType.Closest:
                    float closestDistance = maxDetectionRange;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < closestDistance && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                            {
                                target = npc;
                                closestDistance = distance;
                            }
                        }
                    }
                    break;
                case MyPlayer.StandSearchType.Farthest:
                    float farthestDistance = 0f;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance > farthestDistance && distance < maxDetectionRange && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                            {
                                target = npc;
                                farthestDistance = distance;
                            }
                        }
                    }
                    break;
                case MyPlayer.StandSearchType.LeastHealth:
                    int leasthealth = int.MaxValue;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < maxDetectionRange && npc.life < leasthealth && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                            {
                                target = npc;
                                leasthealth = npc.life;
                            }
                        }
                    }
                    break;
                case MyPlayer.StandSearchType.MostHealth:
                    int mosthealth = 0;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < maxDetectionRange && npc.life >= mosthealth && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                            {
                                target = npc;
                                mosthealth = npc.life;
                            }
                        }
                    }
                    break;
            }
            return target;
        }

        /// <summary>
        /// Determines the animation of the Stand based on its state.
        /// </summary>
        public virtual void SelectAnimation()       //what you override to use normalFrames, attackFrames, etc. and make the animations play
        { }

        /// <summary>
        /// Plays an animation.
        /// </summary>
        /// <param name="animationName">The name of the animation that has to be played. Animation Name determines which Stand spritesheet to animate.</param>
        public virtual void PlayAnimation(string animationName)     //What you override to set each animations information
        { }

        /// <summary>
        /// Gets called when an animation that isn't set to loop is completed.
        /// </summary>
        /// <param name="animationName">The name of the animation that has finished playing.</param>
        public virtual void AnimationCompleted(string animationName)
        { }

        /// <summary>
        /// Plays a set animation.
        /// </summary>
        /// <param name="stateName">The Stands state name.</param>
        /// <param name="frameAmount">The amount of frames the spritesheet contains.</param>
        /// <param name="frameCounterLimit">The amoutn of updates that pass before the frame changes.</param>
        /// <param name="loop">Whether or not the animation should loop. If false, currentAnimationDone becomes true at animation completion.</param>
        public void AnimateStand(string stateName, int frameAmount, int frameCounterLimit, bool loop)
        {
            projectile.frameCounter++;
            Main.projFrames[projectile.whoAmI] = frameAmount;
            if (projectile.frameCounter >= frameCounterLimit)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= frameAmount)
            {
                if (loop)
                    projectile.frame = 0;
                else
                {
                    projectile.frame = frameAmount - 1;
                    AnimationCompleted(stateName);
                }
            }
        }


        /// <summary>
        /// Plays a set animation with looping in certain frames.
        /// </summary>
        /// <param name="stateName">The Stands state name.</param>
        /// <param name="frameAmount">The amount of frames the spritesheet contains.</param>
        /// <param name="frameCounterLimit">The amoutn of updates that pass before the frame changes.</param>
        /// <param name="loop">Whether or not the animation should loop. If false, currentAnimationDone becomes true at animation completion.</param>
        /// <param name="loopCertainFrames">Determines whether or not the Stand will loop in certain frames.</param>
        /// <param name="loopFrameStart">The frame where the loop will start at.</param>
        /// <param name="loopFrameEnd">The frame which will cause the loop to restart.</param>
        public void AnimateStand(string stateName, int frameAmount, int frameCounterLimit, bool loopCertainFrames, int loopFrameStart, int loopFrameEnd)
        {
            projectile.frameCounter++;
            Main.projFrames[projectile.whoAmI] = frameAmount;
            if (projectile.frameCounter >= frameCounterLimit)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
            }
            if (loopCertainFrames)
            {
                if (projectile.frame >= loopFrameEnd)
                {
                    projectile.frame = loopFrameStart;
                }
            }
        }
    }
}