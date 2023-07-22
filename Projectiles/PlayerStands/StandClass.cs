using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Dusts;
using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands
{
    public abstract class StandClass : ModProjectile        //to simplify stand creation
    {
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 38;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.shouldFallThrough = true;
        }

        /// <summary>
        /// Gets called when SetDefaults is called.
        /// </summary>
        public virtual void ExtraSetDefaults()
        { }

        public override string Texture => Mod.Name + "/Extras/EmptyTexture";
        /// <summary>
        /// How fast the projectile the Stand shoots goes. Usually at 16f.
        /// </summary>
        public virtual float ProjectileSpeed { get; } = 16f;
        /// <summary>
        /// The damage the punch deals to an enemy. Don't use this field directly, use newPunchDamage instead.
        /// </summary>
        public virtual int PunchDamage { get; }
        /// <summary>
        /// The damage the projectile deals to an enemy. Don't use this field directly, use newProjectileDamage instead.
        /// </summary>
        public virtual int ProjectileDamage { get; }
        /// <summary>
        /// Half the Stand's height. Used for offset purposes.
        /// </summary>
        public virtual int HalfStandHeight { get; }     //used to correctly set the Y position the stand is at during idle
        /// <summary>
        /// The time (in frames) it takes before the next punch is thrown.
        /// </summary>
        public virtual int PunchTime { get; }
        /// <summary>
        /// The time (in frames) it takes before the next projectile is shot.
        /// </summary>
        public virtual int ShootTime { get; }
        /// <summary>
        /// The max distance the Stand can go from the player. Usually 104f, or 6.5 tiles. Use newMaxDistance when attempting to use.
        /// </summary>
        public virtual float MaxDistance { get; } = 6.5f * 16f;
        /// <summary>
        /// The max distance the Stand's ability can go from the player. Can have many different use cases, and is not limited to only the named use case. Please use newMaxAltDistance when using.
        /// </summary>
        public virtual float MaxAltDistance { get; } = 0f;
        /// <summary>
        /// The damage of the main alternate damage source the Stand can use. Use newAltDamage.
        /// </summary>
        public virtual int AltDamage { get; }
        /// <summary>
        /// This Stand's fist whoAmI. The whoAmI is passed in through the fists' ai[0] array.
        /// </summary>
        public virtual int FistWhoAmI { get; }        //this is used in Fists.cs for effects
        /// <summary>
        /// The Stand's X offset while drawn. This value is halved, so multiply by 2 if trying to apply a direct and known constant offset.
        /// </summary>
        public virtual Vector2 StandOffset { get; } = new Vector2(15, 0);            //from an idle frame, get the first pixel from the left and standOffset = distance from that pixel you got to the right edge of the spritesheet - 38
        public virtual Vector2 ManualIdleHoverOffset { get; } = Vector2.Zero;
        public virtual int TierNumber { get; }
        public virtual float PunchKnockback { get; } = 3f;
        public virtual string PunchSoundName { get; } = "";
        public virtual string PoseSoundName { get; } = "";
        public virtual string SpawnSoundName { get; } = "";
        public virtual StandAttackType StandType { get; } = StandAttackType.None;
        /// <summary>
        /// Whether or not the Stand will use the projectile.alpha field while being drawn. False by default.
        /// </summary>
        public virtual bool UseProjectileAlpha { get; } = false;
        public virtual bool CanUseRangeIndicators { get; } = true;
        public virtual bool CanUseSaladDye { get; } = false;
        public virtual bool CanUsePart4Dye { get; } = false;

        public static int StandNetworkUpdateTime = 90;

        public enum StandAttackType
        {
            None,
            Melee,
            Ranged
        }

        public enum AnimationState
        {
            None,
            Idle,
            Attack,
            SecondaryAbility,
            Special,
            Pose
        }

        public struct AnimationData
        {
            public int frames;
            public string textureName;
            public int frameCounterLimit;
            public bool loopedAnimation;
            public int loopStartFrame;
            public int loopEndFrame;

            public void UpdateFrameCounterLimit(int newLimit)
            {

            }
        }

        public int newPunchTime = 0;       //so we don't have to type newPunchTime all the time
        public int newShootTime = 0;
        public float newMaxDistance = 0f;
        public float newAltMaxDistance = 0f;
        public int newPunchDamage = 0;
        public int newProjectileDamage = 0;
        public bool playerHasAbilityCooldown = false;
        public Texture2D standTexture;
        public Texture2D standRangeIndicatorTexture;
        public Texture2D secondaryStandRangeIndicatorTexture;

        public int shootCount = 0;
        public bool attacking = false;
        public bool secondaryAbility = false;
        private bool playedBeginning = false;
        private bool playedSpawnSound = false;
        private bool sentDyePacket = false;
        private SoundEffectInstance beginningSoundInstance = null;
        private SoundEffectInstance punchingSoundInstance = null;
        private Vector2 rangeIndicatorSize;
        private Vector2 secondaryRangeIndicatorSize;
        private int netUpdateTimer = 0;
        private int summonParticleTimer = 15;
        public float mouseX = 0f;
        public float mouseY = 0f;
        public AnimationState currentAnimationState;
        public AnimationState oldAnimationState;
        //private int rangeIndicatorSize = 0;
        //private int secondaryRangeIndicatorSize = 0;

        /// <summary>
        /// Checks if the Special bind has just been pressed and there is no cooldown.
        /// </summary>
        /// <param name="cooldownCheck">Whether or not to check if the player has a cooldown.</param>
        /// <returns>True if the bind has just been pressed and the player has no ability cooldown on them. Returns false otherwise.</returns>
        public bool SpecialKeyPressed(bool cooldownCheck = true)     //checks for if this isn't the server, if the key is pressed, and if the player has no cooldown (is the owner check even needed?)
        {
            bool specialPressed = false;
            if (!Main.dedServ)      //if it's the clinet, as hotkeys don't exist on the server
                specialPressed = JoJoStands.SpecialHotKey.JustPressed;
            bool hasCooldown = cooldownCheck ? Main.player[Projectile.owner].HasBuff(ModContent.BuffType<AbilityCooldown>()) : false;
            return specialPressed && !hasCooldown && Projectile.owner == Main.myPlayer;
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
            return specialPressed && Projectile.owner == Main.myPlayer;
        }

        /// <summary>
        /// Checks if the Second Special bind has just been pressed and the player has no Ability Cooldown.
        /// </summary>
        /// <param name="cooldownCheck">Whether or not to check if the player has a cooldown.</param>
        /// <returns>True if the Second Special bind has just been pressed and the player has no Ability Cooldown. False if otherwise.</returns>
        public bool SecondSpecialKeyPressed(bool cooldownCheck = true)     //checks for if this isn't the server, if the key is pressed, and if the player has no cooldown (is the owner check even needed?)
        {
            bool specialPressed = false;
            if (!Main.dedServ)      //if it's the clinet, as hotkeys don't exist on the server
                specialPressed = JoJoStands.SecondSpecialHotKey.JustPressed;
            bool hasCooldown = cooldownCheck ? Main.player[Projectile.owner].HasBuff(ModContent.BuffType<AbilityCooldown>()) : false;
            return specialPressed && !hasCooldown && Projectile.owner == Main.myPlayer;
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
            return specialPressed && Projectile.owner == Main.myPlayer;
        }

        /// <summary>
        /// Starts a timestop that lasts x amount of seconds.
        /// </summary>
        public void Timestop(int seconds)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.overHeaven)
                seconds *= 2;
            mPlayer.timestopActive = true;
            mPlayer.timestopEffectDurationTimer = 60;
            if (!JoJoStands.SoundsLoaded || (JoJoStands.SoundsLoaded && !JoJoStands.SoundsModAbilityVoicelines))
                SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/timestop_start"));

            player.AddBuff(ModContent.BuffType<TheWorldBuff>(), seconds * 60, true);
            SyncCall.SyncTimestop(player.whoAmI, true);
        }

        /// <summary>
        /// Has the Stand switch to its attack state and punch.
        /// Damage, knockback, and punch speed depend on the punchDamage, punchKnockback, and punchTime fields respectively.
        /// Stand ID and Stand Tier Number are passed into the fist Projectile.
        /// </summary>
        /// <param name="movementSpeed">How fast the Stand moves while it's punching</param>
        /// <param name="punchLifeTimeMultiplier">A multiplier for the punch projectiles' lifetime.</param>
        public void Punch(float movementSpeed = 5f, float punchLifeTimeMultiplier = 1f)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!mPlayer.canStandBasicAttack)
            {
                currentAnimationState = AnimationState.Idle;
                return;
            }

            attacking = true;
            currentAnimationState = AnimationState.Attack;
            float rotaY = mouseY - Projectile.Center.Y;
            Projectile.rotation = MathHelper.ToRadians((rotaY * Projectile.spriteDirection) / 6f);
            Vector2 velocityAddition = Main.MouseWorld - Projectile.Center;
            velocityAddition.Normalize();
            velocityAddition *= movementSpeed + mPlayer.standTier;

            Projectile.spriteDirection = Projectile.direction = mouseX > Projectile.Center.X ? 1 : -1;
            float mouseDistance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
            if (mouseDistance > 2.5f * 16f)
                Projectile.velocity = player.velocity + velocityAddition;
            else
                Projectile.velocity = Vector2.Zero;

            PlayPunchSound();
            if (shootCount <= 0)
            {
                shootCount += newPunchTime;
                Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                if (shootVel == Vector2.Zero)
                    shootVel = new Vector2(0f, 1f);

                shootVel.Normalize();
                shootVel *= ProjectileSpeed;
                int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), newPunchDamage, PunchKnockback, Projectile.owner, FistWhoAmI, TierNumber);
                Main.projectile[projIndex].timeLeft = (int)(Main.projectile[projIndex].timeLeft * punchLifeTimeMultiplier);
                Main.projectile[projIndex].netUpdate = true;
            }
            LimitDistance();
            Projectile.netUpdate = true;
        }

        /// <summary>
        /// Has the Stand switch to its idle state and stay behind the player.
        /// </summary>
        public void StayBehind()
        {
            Player player = Main.player[Projectile.owner];
            currentAnimationState = AnimationState.Idle;
            Vector2 position = player.Center;
            position.X -= (12 + ManualIdleHoverOffset.X + player.width / 2) * player.direction;
            position.Y -= -35f + HalfStandHeight - ManualIdleHoverOffset.Y;
            Projectile.Center = Vector2.Lerp(Projectile.Center, position, 0.2f);
            Projectile.velocity *= 0.8f;
            Projectile.spriteDirection = Projectile.direction = player.direction;
            Projectile.rotation = 0;
            LimitDistance();
            StopSounds();
        }

        /// <summary>
        /// Has the Stand switch to its idle state and stay behind the player.
        /// During a secondary ability, the stand is able to face any direction.
        /// </summary>
        public void StayBehindWithAbility()
        {
            Player player = Main.player[Projectile.owner];

            currentAnimationState = AnimationState.Idle;
            Vector2 position = player.Center;
            position.X -= (12 + ManualIdleHoverOffset.X + player.width / 2) * player.direction;
            position.Y -= -35f + HalfStandHeight - ManualIdleHoverOffset.Y;
            Projectile.Center = Vector2.Lerp(Projectile.Center, position, 0.2f);
            Projectile.velocity *= 0.8f;
            if (!secondaryAbility)
                Projectile.spriteDirection = Projectile.direction = player.direction;
            Projectile.rotation = 0;
            LimitDistance();
            StopSounds();
        }

        /// <summary>
        /// Has the Stand go in front of the player.
        /// <param name="forcedDirection">The direction the Stand will be forced to face.</param>
        /// </summary>
        public void GoInFront(int forcedDirection = 0)
        {
            Player player = Main.player[Projectile.owner];
            int direction = player.direction;
            if (forcedDirection != 0)
                direction = forcedDirection;

            Vector2 position = player.Center;
            float offsetX = 50f;
            position.X += (12 + offsetX - ManualIdleHoverOffset.X + player.width / 2) * direction;
            position.Y -= -35f + HalfStandHeight - ManualIdleHoverOffset.Y;
            Projectile.Center = Vector2.Lerp(Projectile.Center, position, 0.2f);
            Projectile.velocity *= 0.8f;
            Projectile.spriteDirection = Projectile.direction = player.direction;
            Projectile.rotation = 0;
            LimitDistance();
        }

        /// <summary>
        /// A basic punch AI for Stands.
        /// Detects enemies as far as what the maxDistance field is set to then multiplied by 1.2f (in pixels).
        /// For use in Auto Mode.
        /// </summary>
        public void BasicPunchAI()
        {
            Player player = Main.player[Projectile.owner];
            if (!attacking)
            {
                StayBehind();
                Projectile.spriteDirection = Projectile.direction = player.direction;
            }

            float detectionDist = newMaxDistance * 1.2f;
            NPC target = FindNearestTarget(detectionDist);
            if (target != null)
            {
                currentAnimationState = AnimationState.Attack;
                Projectile.direction = 1;
                if (target.position.X - Projectile.Center.X <= 0f)
                    Projectile.direction = -1;
                Projectile.spriteDirection = Projectile.direction;

                Vector2 velocity = target.position - Projectile.Center;
                velocity.Normalize();
                Projectile.velocity = velocity * 4f;
                if (shootCount <= 0)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        PlayPunchSound();
                        shootCount += newPunchTime;
                        Vector2 shootVel = target.position - Projectile.Center;
                        shootVel.Normalize();
                        if (Projectile.direction == 1)
                            shootVel *= ProjectileSpeed;

                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newPunchDamage * 0.9f), PunchKnockback, Projectile.owner, FistWhoAmI, TierNumber);
                        Main.projectile[projIndex].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                }
            }
            else
                currentAnimationState = AnimationState.Idle;

            LimitDistance();
        }

        /// <summary>
        /// A basic punch AI for Stands.
        /// Contains additional functionality for secondary abilities, which allows the stand to use ranged abilities while in Auto Mode.
        /// For use in Auto Mode.
        /// </summary>
        /// <param name="projToShoot">The type of Projectile that the sSand will shoot.</param>
        /// <param name="itemToConsumeType">The type of Item that will be consumed whenever the Stand uses its secondary ability.</param>
        /// <param name="gravityAccounting">Whether or not the Stand should account for gravity when using its secondary ability.</param>
        /// <param name="shootMax">The limit on the amount of the same Projectile that the Stand can use with its secondary ability.</param>
        public void PunchAndShootAI(int projToShoot, int itemToConsumeType = -1, bool gravityAccounting = false, int shootMax = 999)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            float rangedDetectionDist = newMaxDistance * 3f;        //Distance for ranged attacks to work
            float punchDetectionDist = newMaxDistance * 1.5f;       //Distance for melee attacks to work
            float maxDetectionDist = newMaxDistance * 3.1f;
            float targetDist = maxDetectionDist;
            NPC target = FindNearestTarget(targetDist);
            if (target != null)
                targetDist = Vector2.Distance(target.Center, player.Center);

            if (targetDist > punchDetectionDist || secondaryAbility || target == null)
            {
                Vector2 areaBehindPlayer = player.Center;
                currentAnimationState = AnimationState.Idle;
                if (secondaryAbility)
                    areaBehindPlayer.X += (float)((12 + player.width / 2) * player.direction);
                else
                    areaBehindPlayer.X -= (float)((12 + player.width / 2) * player.direction);
                areaBehindPlayer.Y -= -35f + HalfStandHeight;
                Projectile.Center = Vector2.Lerp(Projectile.Center, areaBehindPlayer, 0.2f);
                Projectile.velocity *= 0.8f;
                Projectile.rotation = 0;
                Projectile.spriteDirection = Projectile.direction = player.direction;
            }
            if (target != null)
            {
                if (targetDist < punchDetectionDist && !secondaryAbility)
                {
                    currentAnimationState = AnimationState.Attack;
                    Projectile.direction = 1;
                    if (target.position.X - Projectile.Center.X < 0f)
                        Projectile.direction = -1;
                    Projectile.spriteDirection = Projectile.direction;

                    Vector2 velocity = target.position - Projectile.Center;
                    velocity.Normalize();
                    Projectile.velocity = velocity * 4f;
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            PlayPunchSound();
                            shootCount += newPunchTime;
                            Vector2 shootVel = target.position - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);
                            shootVel.Normalize();
                            if (Projectile.direction == 1)
                                shootVel *= ProjectileSpeed;

                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newPunchDamage * 0.9f), PunchKnockback, Projectile.owner, FistWhoAmI, TierNumber);
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                }
                else if (Main.rand.Next(0, 101) <= 1 && targetDist > punchDetectionDist)
                {
                    if (itemToConsumeType != -1 && JoJoStands.AutomaticActivations && player.HasItem(itemToConsumeType))
                        secondaryAbility = true;
                    else if (itemToConsumeType == -1)
                        secondaryAbility = true;
                }

                if (secondaryAbility)
                {
                    currentAnimationState = AnimationState.SecondaryAbility;
                    Projectile.direction = 1;
                    if (target.position.X - Projectile.Center.X < 0f)
                        Projectile.direction = -1;
                    Projectile.spriteDirection = Projectile.direction;

                    if (shootCount <= 0 && player.ownedProjectileCounts[projToShoot] < shootMax)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            if (shootCount <= 0)
                            {
                                shootCount += 28;
                                Vector2 shootVel = target.position - Projectile.Center - new Vector2(0f, 3f);
                                if (shootVel == Vector2.Zero)
                                    shootVel = new Vector2(0f, 1f);

                                shootVel.Normalize();
                                shootVel *= ProjectileSpeed;
                                if (gravityAccounting)
                                    shootVel.Y -= Projectile.Distance(target.position) / 110f;        //Adding force with the distance of the enemy / 110 (Dividing by 110 cause if not it's gonna fly straight up)

                                int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X + 5f, Projectile.Center.Y - 3f, shootVel.X, shootVel.Y, projToShoot, (int)((AltDamage * mPlayer.standDamageBoosts) * 0.9f), 2f, Projectile.owner, Projectile.whoAmI, TierNumber);
                                Main.projectile[projIndex].netUpdate = true;
                                Projectile.netUpdate = true;
                            }
                            if (itemToConsumeType != -1)
                                player.ConsumeItem(itemToConsumeType);
                        }
                    }
                    if ((!gravityAccounting && player.ownedProjectileCounts[projToShoot] == 0) || targetDist > rangedDetectionDist)     //!gravityAccounting and 0 of that Projectile cause it's usually no gravity projectiles that are just 1 shot (star finger, zipper punch), while things like knives have gravity (meant to be thrown in succession)
                    {
                        secondaryAbility = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
            }
            LimitDistance();
        }

        /// <summary>
        /// Generates a Stand Range Indicator Texture based on the needed distance.
        /// </summary>
        /// <param name="dist">The distance of the range indicator.</param>
        /// <param name="sizeUpdateID">The size to update. 1 updates the rangeIndicatorSize variable and 2 updates the secondaryRangeIndicatorSize variable.</param>
        /// <returns></returns>
        public Texture2D GenerateRangeIndicatorTexture(float dist, int sizeUpdateID = 1)
        {
            if (Main.dedServ)
                return null;

            dist /= 2f;     //For smaller texture sizes which we will scale up later. Also prevents mixels.
            int size = (int)dist * 2;
            Vector2 center = new Vector2(size) / 2f;
            Texture2D texture = new Texture2D(Main.graphics.GraphicsDevice, size, size);
            Color[] colorArray = new Color[size * size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Color pixelColor = Color.Transparent;
                    Vector2 pixelPos = new Vector2(x, y);
                    int pixelDist = (int)Vector2.Distance(pixelPos, center);

                    if (pixelDist == dist - 1)
                        pixelColor = Color.Black;
                    else if (pixelDist == dist - 2)
                        pixelColor = Color.White;

                    colorArray[x + y * size] = pixelColor;
                }
            }
            if (sizeUpdateID == 1)
                rangeIndicatorSize = new Vector2(size);
            else if (sizeUpdateID == 2)
                secondaryRangeIndicatorSize = new Vector2(size);

            texture.SetData(colorArray);
            return texture;
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
                SoundEffect sound = ModContent.Request<SoundEffect>("JoJoStandsSounds/Sounds/BattleCries/" + PunchSoundName + "_Beginning", AssetRequestMode.ImmediateLoad).Value;
                if (sound != null)
                {
                    beginningSoundInstance = sound.CreateInstance();
                    beginningSoundInstance.Volume = JoJoStands.ModSoundsVolume;
                }
            }
            if (punchingSoundInstance == null)
            {
                SoundEffect sound = ModContent.Request<SoundEffect>("JoJoStandsSounds/Sounds/BattleCries/" + PunchSoundName, AssetRequestMode.ImmediateLoad).Value;
                punchingSoundInstance = sound.CreateInstance();
                punchingSoundInstance.Volume = JoJoStands.ModSoundsVolume;
            }
        }

        /// <summary>
        /// Call this method to play the punch sounds if this Stand has one. PunchSoundName must be initialized for this method to have any effect!
        /// </summary>
        public void PlayPunchSound()
        {
            if (!JoJoStands.SoundsLoaded || Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standHitTime <= 0)
                return;

            if (PunchSoundName != "" && punchingSoundInstance == null)
                InitializeSounds();

            if (PunchSoundName != "")
            {
                if (beginningSoundInstance != null)
                {
                    if (!playedBeginning)
                    {
                        //beginningSoundInstance.Play();     //is this not just beginningSoundInstance.Play()?
                        beginningSoundInstance.Volume = JoJoStands.ModSoundsVolume;
                        beginningSoundInstance.Play();                 //if there is no other way to have this play for everyone, send a packet with that sound type so that it plays for everyone
                        SoundInstanceGarbageCollector.Track(beginningSoundInstance);
                        playedBeginning = true;
                    }
                    if (playedBeginning && beginningSoundInstance.State == SoundState.Stopped)
                    {
                        //punchingSoundInstance.Play();     //is this not just beginningSoundInstance.Play()?
                        punchingSoundInstance.Volume = JoJoStands.ModSoundsVolume;
                        punchingSoundInstance.Play();
                        SoundInstanceGarbageCollector.Track(punchingSoundInstance);
                        SyncSounds();
                    }
                }
                else
                {
                    //punchingSoundInstance.Play();
                    punchingSoundInstance.Play();
                    SoundInstanceGarbageCollector.Track(punchingSoundInstance);
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
                    Player player = Main.player[Projectile.owner];
                    string path = "JoJoStandsSounds/Sounds/BattleCries/" + PunchSoundName;
                    SoundState state = punchingSoundInstance.State;
                    //SendSoundInstance(int sender, string soundPath, SoundState state, Vector2 pos (this one has to be sent as individual float values), float soundTravelDistance = 10f), which is the method called here
                    JoJoStands.JoJoStandsSounds.Call("SendSoundInstance", player.whoAmI, path, state, Projectile.Center.X, Projectile.Center.Y, 40);
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
            if (Projectile.netUpdate)       //It's put here because we don't want to sync this all the time. Only whenever this method is called (Idles).
                SyncSounds();
        }

        public override sealed void OnSpawn(IEntitySource source)
        {
            summonParticleTimer = Main.rand.Next(6, 10 + 1);
            ExtraSpawnEffects();
        }

        /// <summary>
        /// Gets called when the Stand is spawned.
        /// </summary>
        public virtual void ExtraSpawnEffects()
        { }

        /// <summary>
        /// Updates all client-side stand info.
        /// Updates newPunchTime, newShootTime, newMaxDistance, newAltMaxDistance, newPunchDamage, newProjectileDamage, and mPlayer.standType.
        /// </summary>
        public void UpdateStandInfo()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            newPunchTime = PunchTime - mPlayer.standSpeedBoosts;
            newShootTime = ShootTime - mPlayer.standSpeedBoosts;
            newMaxDistance = MaxDistance + mPlayer.standRangeBoosts;
            newAltMaxDistance = MaxAltDistance + mPlayer.standRangeBoosts;
            newPunchDamage = (int)(PunchDamage * mPlayer.standDamageBoosts);
            newProjectileDamage = (int)(ProjectileDamage * mPlayer.standDamageBoosts);
            playerHasAbilityCooldown = player.HasBuff(ModContent.BuffType<AbilityCooldown>());
            if (Projectile.owner == Main.myPlayer)
            {
                mouseX = Main.MouseWorld.X;
                mouseY = Main.MouseWorld.Y;
            }
            mPlayer.standFistsType = FistWhoAmI;
            mPlayer.standTier = TierNumber;
            mPlayer.poseSoundName = PoseSoundName;
            if (newPunchTime <= 2)
                newPunchTime = 2;
            if (newShootTime <= 5)
                newShootTime = 5;
            if (PlayerInput.Triggers.Current.SmartSelect || player.dead)
                mPlayer.canStandBasicAttack = false;
            if (JoJoStands.SoundsLoaded && mPlayer.standHitTime > 0)
                mPlayer.standHitTime--;

            if (mPlayer.standType != (int)StandType)
                mPlayer.standType = (int)StandType;

            if (summonParticleTimer > 0)
            {
                summonParticleTimer--;
                int amountOfParticles = Main.rand.Next(1, 2);
                int[] dustTypes = new int[3] { ModContent.DustType<StandSummonParticles>(), ModContent.DustType<StandSummonShine1>(), ModContent.DustType<StandSummonShine2>() };
                Vector2 dustSpawnOffset = StandOffset;
                dustSpawnOffset.X *= Projectile.spriteDirection;
                for (int i = 0; i < amountOfParticles; i++)
                {
                    int dustType = dustTypes[Main.rand.Next(0, 3)];
                    Dust.NewDust(Projectile.Center - new Vector2(Projectile.width * Projectile.spriteDirection, HalfStandHeight) + dustSpawnOffset, Projectile.width, HalfStandHeight * 2, dustType, Scale: (float)Main.rand.Next(80, 120) / 100f);
                }
            }

            if (JoJoStands.RangeIndicators && CanUseRangeIndicators && newMaxDistance > 0)
            {
                if (Math.Abs((int)rangeIndicatorSize.X - (int)newMaxDistance) > 1)     //Comparing via subtraction to have a minimum error count of 1
                    standRangeIndicatorTexture = GenerateRangeIndicatorTexture((int)newMaxDistance);
                if (Math.Abs((int)secondaryRangeIndicatorSize.X - (int)newAltMaxDistance) > 1)
                    secondaryStandRangeIndicatorTexture = GenerateRangeIndicatorTexture((int)newAltMaxDistance, 2);
            }

            if (JoJoStands.SoundsLoaded)
            {
                if (SpawnSoundName != "" && !playedSpawnSound)
                {
                    SoundStyle spawnSound = new SoundStyle("JoJoStandsSounds/Sounds/SummonCries/" + SpawnSoundName);
                    spawnSound.Volume = JoJoStands.ModSoundsVolume;
                    SoundEngine.PlaySound(spawnSound, Projectile.Center);
                    playedSpawnSound = true;
                }
            }
        }

        public void UpdateStandSync()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;

            netUpdateTimer++;
            if (netUpdateTimer >= StandNetworkUpdateTime)
            {
                Projectile.netUpdate = true;
                netUpdateTimer = 0;
                SyncSounds();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool PreDraw(ref Color drawColor)      //from ExampleMod ExampleDeathShader
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);        //starting a draw with dyes that work

            DrawRangeIndicators();       //not affected by dyes since it's starting a new batch with no effect
            SyncAndApplyDyeSlot();
            DrawStand(drawColor);

            return true;
        }

        public override void PostDraw(Color drawColor)     //manually drawing stands cause sometimes stands had too many frames, it's easier to manage this way, and dye effects didn't work for stands that were overriding PostDraw
        {
            Main.spriteBatch.End();     //ending the spriteBatch that started in PreDraw
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            PostDrawExtras();
        }

        /// <summary>
        /// Allows you to draw things in front of this projectile. Use Main.EntitySpriteDraw() for drawing using this method.
        /// </summary>
        public virtual void PostDrawExtras() { }

        public SpriteEffects effects = SpriteEffects.None;

        /// <summary>
        /// Draws the Stand.
        /// </summary>
        private void DrawStand(Color drawColor)
        {
            if (UseProjectileAlpha)
                drawColor *= Projectile.alpha / 255f;

            effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                effects = SpriteEffects.FlipHorizontally;

            if (standTexture != null && Main.netMode != NetmodeID.Server)
            {
                int frameHeight = standTexture.Height / Main.projFrames[Projectile.type];
                Vector2 drawOffset = StandOffset;
                drawOffset.X *= Projectile.spriteDirection;
                Vector2 drawPosition = Projectile.Center - Main.screenPosition + drawOffset;
                Rectangle animRect = new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight);
                Vector2 standOrigin = new Vector2(standTexture.Width / 2f, frameHeight / 2f);
                Main.EntitySpriteDraw(standTexture, drawPosition, animRect, drawColor, Projectile.rotation, standOrigin, 1f, effects, 0);
            }
        }

        /// <summary>
        /// Draws the Stands range indicators.
        /// Only draws if the JoJoStands.RangeIndicators field is set to true.
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawRangeIndicators()
        {
            Player player = Main.player[Projectile.owner];
            if (!JoJoStands.RangeIndicators || Main.netMode == NetmodeID.Server || !CanUseRangeIndicators || rangeIndicatorSize == Vector2.Zero)
                return;

            //Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/RangeIndicator>().Value;        //the initial tile amount the indicator covers is 20 tiles, 320 pixels, border is included in the measurements
            Vector2 rangeIndicatorDrawPosition = player.Center - Main.screenPosition;
            Vector2 rangeIndicatorOrigin = rangeIndicatorSize / 2f;
            float rangeIndicatorAlpha = JoJoStands.RangeIndicatorAlpha;

            if (MaxDistance > 0f)
                Main.EntitySpriteDraw(standRangeIndicatorTexture, rangeIndicatorDrawPosition, null, Color.White * rangeIndicatorAlpha, 0f, rangeIndicatorOrigin, 2f, SpriteEffects.None, 0);

            if (MaxAltDistance > 0f)
            {
                rangeIndicatorOrigin = secondaryRangeIndicatorSize / 2f;
                Main.EntitySpriteDraw(secondaryStandRangeIndicatorTexture, rangeIndicatorDrawPosition, null, Color.Orange * rangeIndicatorAlpha, 0f, rangeIndicatorOrigin, 2f, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Syncs and applies the current dye in the Dye Slot to the Stand.
        /// </summary>
        public void SyncAndApplyDyeSlot()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.StandDyeSlot.SlotItem.dye != 0)
            {
                ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(mPlayer.StandDyeSlot.SlotItem.type);
                shader.Apply(null);     //has to be on top of whatever it's applying to (I spent hours and hours trying to find a solution and the only problem was that this was under it)
                if (!sentDyePacket)       //we sync dye slot Item here
                {
                    SyncCall.SyncCurrentDyeItem(player.whoAmI, mPlayer.StandDyeSlot.SlotItem.type);
                    sentDyePacket = true;
                }
            }
            else
            {
                if (sentDyePacket)
                {
                    SyncCall.SyncCurrentDyeItem(player.whoAmI, mPlayer.StandDyeSlot.SlotItem.type);
                    sentDyePacket = false;
                }
            }
        }

        /// <summary>
        /// A method that gets called along with Kill(). Useful for extra things that have to happen without having to manually reset stand type to 0 and other variables.
        /// </summary>
        public virtual void StandKillEffects()
        { }


        public override void Kill(int timeLeft)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            mPlayer.standType = 0;
            mPlayer.poseSoundName = "";
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote)
                mPlayer.standControlStyle = MyPlayer.StandControlStyle.Manual;

            mPlayer.standDesummonTimer = 15;
            if (Main.netMode != NetmodeID.Server)
                mPlayer.standDesummonTextureData = BuildStandDesummonDrawData();

            int amountOfParticles = Main.rand.Next(2, 5 + 1);
            int[] dustTypes = new int[3] { ModContent.DustType<StandSummonParticles>(), ModContent.DustType<StandSummonShine1>(), ModContent.DustType<StandSummonShine2>() };
            Vector2 dustSpawnOffset = StandOffset;
            dustSpawnOffset.X *= Projectile.spriteDirection;
            for (int i = 0; i < amountOfParticles; i++)
            {
                int dustType = dustTypes[Main.rand.Next(0, 3)];
                Dust.NewDust(Projectile.Center - new Vector2(Projectile.width * Projectile.spriteDirection, HalfStandHeight) + dustSpawnOffset, Projectile.width, HalfStandHeight * 2, dustType, Scale: (float)Main.rand.Next(80, 120) / 100f);
            }

            StandKillEffects();
        }

        public virtual DrawData BuildStandDesummonDrawData()
        {
            if (standTexture == null)
                return new DrawData();

            int frameHeight = standTexture.Height / Main.projFrames[Projectile.type];
            Vector2 drawOffset = StandOffset;
            drawOffset.X *= Projectile.spriteDirection;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + drawOffset;
            Rectangle animRect = new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight);
            Vector2 standOrigin = new Vector2(standTexture.Width / 2f, frameHeight / 2f);
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                effects = SpriteEffects.FlipHorizontally;

            return new DrawData(standTexture, drawPosition, animRect, Color.White, Projectile.rotation, standOrigin, 1f, effects, 0);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)currentAnimationState);
            writer.Write(playerHasAbilityCooldown);
            writer.Write(shootCount);
            writer.Write(Projectile.spriteDirection);
            writer.Write(Projectile.rotation);
            writer.Write(mouseX);
            writer.Write(mouseY);
            SendExtraStates(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            currentAnimationState = (AnimationState)reader.ReadByte();
            playerHasAbilityCooldown = reader.ReadBoolean();
            shootCount = reader.ReadInt32();
            Projectile.spriteDirection = reader.ReadInt32();
            Projectile.rotation = reader.ReadSingle();
            mouseX = reader.ReadSingle();
            mouseY = reader.ReadSingle();
            ReceiveExtraStates(reader);
        }

        /// <summary>
        /// Use this method to write any extra information that needs to be communicated to other clients about this Stand.
        /// Called when Projectile.netUpdate is True. Sends the extra info to other clients.
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
            Player player = Main.player[Projectile.owner];

            Vector2 direction = player.Center - Projectile.Center;
            float distanceFromPlayer = direction.Length();
            if (distanceFromPlayer >= newMaxDistance)
            {
                direction.Normalize();
                Projectile.Center = player.Center + (-direction * newMaxDistance);
                if (Math.Abs(Projectile.Center.X + Projectile.velocity.X - player.Center.X) > newMaxDistance - (Projectile.width / 2f))     //Controls the separate vector components
                    Projectile.velocity.X = player.velocity.X;
                if (Math.Abs(Projectile.Center.Y + Projectile.velocity.Y - player.Center.Y) > newMaxDistance - HalfStandHeight)
                    Projectile.velocity.Y = player.velocity.Y;

                if (distanceFromPlayer >= newMaxDistance + 16)
                    Projectile.Center = player.Center;
            }
        }

        /// <summary>
        /// Limits the distance the Stand can travel.
        /// </summary>
        public void LimitDistance(float maxDistance, bool affectedByRangeModifiers = false)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (affectedByRangeModifiers)
                maxDistance += mPlayer.standRangeBoosts;

            Vector2 direction = player.Center - Projectile.Center;
            float distanceFromPlayer = direction.Length();
            if (distanceFromPlayer >= maxDistance)
            {
                direction.Normalize();
                Projectile.Center = player.Center + (-direction * maxDistance);
                if (Math.Abs(Projectile.Center.X + Projectile.velocity.X - player.Center.X) > maxDistance - (Projectile.width / 2f))     //Controls the separate vector components
                    Projectile.velocity.X = player.velocity.X;
                if (Math.Abs(Projectile.Center.Y + Projectile.velocity.Y - player.Center.Y) > maxDistance - HalfStandHeight)
                    Projectile.velocity.Y = player.velocity.Y;

                if (distanceFromPlayer >= maxDistance + 16)
                    Projectile.Center = player.Center;
            }
        }

        /// <summary>
        /// Find the closest NPC to the player.
        /// Criteria for the search is set by the JoJoStands.standSearchType field.
        /// </summary>
        /// <param name="maxDetectionRange">The max distance (in pixels) to search</param>
        /// <returns>The NPC that is closest to the player and follows the given criteria.</returns>
        public NPC FindNearestTarget(float maxDetectionRange)
        {
            NPC target = null;
            Player player = Main.player[Projectile.owner];
            switch (JoJoStands.StandSearchTypeEnum)
            {
                case MyPlayer.StandSearchTypeEnum.Bosses:
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < maxDetectionRange && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
                case MyPlayer.StandSearchTypeEnum.Closest:
                    float closestDistance = maxDetectionRange;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < closestDistance && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                            {
                                target = npc;
                                closestDistance = distance;
                            }
                        }
                    }
                    break;
                case MyPlayer.StandSearchTypeEnum.Farthest:
                    float farthestDistance = 0f;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance > farthestDistance && distance < maxDetectionRange && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                            {
                                target = npc;
                                farthestDistance = distance;
                            }
                        }
                    }
                    break;
                case MyPlayer.StandSearchTypeEnum.LeastHealth:
                    int leasthealth = int.MaxValue;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < maxDetectionRange && npc.life < leasthealth && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                            {
                                target = npc;
                                leasthealth = npc.life;
                            }
                        }
                    }
                    break;
                case MyPlayer.StandSearchTypeEnum.MostHealth:
                    int mosthealth = 0;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < maxDetectionRange && npc.life >= mosthealth && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
        /// Finds the closest NPC to the player with the given search type.
        /// </summary>
        /// <param name="searchType">The search type criteria to search with</param>
        /// <param name="maxDetectionRange">The max distance (in pixels) to search</param>
        /// <returns>The NPC that is closest to the player and follows the given criteria.</returns>
        public NPC FindNearestTarget(MyPlayer.StandSearchTypeEnum searchType, float maxDetectionRange)
        {
            NPC target = null;
            Player player = Main.player[Projectile.owner];
            switch (searchType)
            {
                case MyPlayer.StandSearchTypeEnum.Bosses:
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < maxDetectionRange && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
                case MyPlayer.StandSearchTypeEnum.Closest:
                    float closestDistance = maxDetectionRange;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < closestDistance && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                            {
                                target = npc;
                                closestDistance = distance;
                            }
                        }
                    }
                    break;
                case MyPlayer.StandSearchTypeEnum.Farthest:
                    float farthestDistance = 0f;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance > farthestDistance && distance < maxDetectionRange && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                            {
                                target = npc;
                                farthestDistance = distance;
                            }
                        }
                    }
                    break;
                case MyPlayer.StandSearchTypeEnum.LeastHealth:
                    int leasthealth = int.MaxValue;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < maxDetectionRange && npc.life < leasthealth && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                            {
                                target = npc;
                                leasthealth = npc.life;
                            }
                        }
                    }
                    break;
                case MyPlayer.StandSearchTypeEnum.MostHealth:
                    int mosthealth = 0;
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < maxDetectionRange && npc.life >= mosthealth && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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

        public Texture2D GetStandTexture(string texturePath, string standAnimationName)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            string newTexturePath = texturePath;
            if (mPlayer.usingStandTextureDye)
            {
                if (mPlayer.currentTextureDye == MyPlayer.StandTextureDye.Salad && CanUseSaladDye)
                    newTexturePath += mPlayer.dyePathAddition;
                if (mPlayer.currentTextureDye == MyPlayer.StandTextureDye.Part4 && CanUsePart4Dye)
                    newTexturePath += mPlayer.dyePathAddition;
            }

            newTexturePath += "/" + standAnimationName;
            return (Texture2D)ModContent.Request<Texture2D>(newTexturePath);
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
        public void AnimateStand(string stateName, int frameAmount, int frameCounterLimit, bool loop)       //We pass in animation names instead of currentstate because AnimationState can vary wildly.
        {
            Projectile.frameCounter++;
            Main.projFrames[Projectile.type] = frameAmount;
            if (Projectile.frameCounter >= frameCounterLimit)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= frameAmount)
            {
                if (loop)
                    Projectile.frame = 0;
                else
                {
                    Projectile.frame = frameAmount - 1;
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
            Projectile.frameCounter++;
            Main.projFrames[Projectile.type] = frameAmount;
            if (Projectile.frameCounter >= frameCounterLimit)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
            if (loopCertainFrames)
            {
                if (Projectile.frame >= loopFrameEnd)
                    Projectile.frame = loopFrameStart;
            }
        }
    }
}