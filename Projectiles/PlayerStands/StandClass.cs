using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Networking;
using JoJoStands.Projectiles.PlayerStands.Echoes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
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
        public virtual int StandOffset { get; } = 60;            //from an idle frame, get the first pixel from the left and standOffset = distance from that pixel you got to the right edge of the spritesheet - 38
        public virtual int standYOffset { get; } = 0;
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

        public static int StandNetworkUpdateTime = 90;

        public enum StandAttackType
        {
            None,
            Melee,
            Ranged
        }

        public struct AnimationData
        {
            public int frames;
            public int frameCounterLimit;
            public bool loopedAnimation;
        }

        public bool idleFrames = false;       //Much easier to sync animations this way rather than syncing everything about it
        public bool attackFrames = false;
        public bool secondaryAbilityFrames = false;
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
        private Vector2 velocityAddition;
        private float mouseDistance;
        public bool secondaryAbility = false;
        private bool playedBeginning = false;
        private bool playedSpawnSound = false;
        private bool sentDyePacket = false;
        private SoundEffectInstance beginningSoundInstance = null;
        private SoundEffectInstance punchingSoundInstance = null;
        private Vector2 rangeIndicatorSize;
        private Vector2 secondaryRangeIndicatorSize;
        private int netUpdateTimer = 0;
        public float mouseX = 0f;
        public float mouseY = 0f;
        //private int rangeIndicatorSize = 0;
        //private int secondaryRangeIndicatorSize = 0;

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
            if (!JoJoStands.SoundsLoaded)
                SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/timestop_start"));

            player.AddBuff(ModContent.BuffType<TheWorldBuff>(), seconds * 60, true);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.EffectSync.SendTimestop(256, player.whoAmI, true, player.whoAmI);
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
            return specialPressed && !Main.player[Projectile.owner].HasBuff(ModContent.BuffType<AbilityCooldown>()) && Projectile.owner == Main.myPlayer;
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
            return specialPressed && Projectile.owner == Main.myPlayer;
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
        /// <returns>True if the Second Special bind has just been pressed and the player has no Ability Cooldown. False if otherwise.</returns>
        public bool SecondSpecialKeyPressed()     //checks for if this isn't the server, if the key is pressed, and if the player has no cooldown (is the owner check even needed?)
        {
            bool specialPressed = false;
            if (!Main.dedServ)      //if it's the clinet, as hotkeys don't exist on the server
                specialPressed = JoJoStands.SecondSpecialHotKey.JustPressed;
            return specialPressed && !Main.player[Projectile.owner].HasBuff(ModContent.BuffType<AbilityCooldown>()) && Projectile.owner == Main.myPlayer;
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
            return specialPressed && Projectile.owner == Main.myPlayer;
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
                idleFrames = true;
                attackFrames = false;
                return;
            }

            movementSpeed += mPlayer.standTier;
            HandleDrawOffsets();
            idleFrames = false;
            attackFrames = true;
            float rotaY = mouseY - Projectile.Center.Y;
            Projectile.rotation = MathHelper.ToRadians((rotaY * Projectile.spriteDirection) / 6f);
            velocityAddition = new Vector2(mouseX, mouseY) - Projectile.position;
            velocityAddition.Normalize();
            velocityAddition *= movementSpeed;

            if (mouseX < Projectile.position.X)
            {
                Projectile.spriteDirection = -1;
                Projectile.direction = -1;
            }
            if (mouseX > Projectile.position.X)
            {
                Projectile.spriteDirection = 1;
                Projectile.direction = 1;
            }

            mouseDistance = Vector2.Distance(new Vector2(mouseX, mouseY), Projectile.Center);
            if (mouseDistance > 40f)
                Projectile.velocity = player.velocity + velocityAddition;
            if (mouseDistance <= 40f)
                Projectile.velocity = Vector2.Zero;

            PlayPunchSound();
            if (shootCount <= 0)
            {
                shootCount += newPunchTime;
                Vector2 shootVel = new Vector2(mouseX, mouseY) - Projectile.Center;
                if (shootVel == Vector2.Zero)
                    shootVel = new Vector2(0f, 1f);

                shootVel.Normalize();
                shootVel *= ProjectileSpeed;
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), newPunchDamage, PunchKnockback, Projectile.owner, FistWhoAmI, TierNumber);
                Main.projectile[proj].timeLeft = (int)(Main.projectile[proj].timeLeft * punchLifeTimeMultiplier);
                Main.projectile[proj].netUpdate = true;
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
            idleFrames = true;
            attackFrames = false;
            Vector2 areaBehindPlayer = player.Center;
            areaBehindPlayer.X -= (float)((12 + player.width / 2) * player.direction);
            areaBehindPlayer.Y -= -35f + HalfStandHeight + standYOffset;
            Projectile.Center = Vector2.Lerp(Projectile.Center, areaBehindPlayer, 0.2f);
            Projectile.velocity *= 0.8f;
            Projectile.direction = Projectile.spriteDirection = player.direction;
            Projectile.rotation = 0;
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
            Player player = Main.player[Projectile.owner];

            idleFrames = true;
            attackFrames = false;
            Vector2 areaBehindPlayer = player.Center;
            areaBehindPlayer.X -= (float)((12 + player.width / 2) * player.direction);
            areaBehindPlayer.Y -= -35f + HalfStandHeight + standYOffset;
            Projectile.Center = Vector2.Lerp(Projectile.Center, areaBehindPlayer, 0.2f);
            Projectile.velocity *= 0.8f;
            if (!secondaryAbilityFrames)
            {
                Projectile.direction = Projectile.spriteDirection = player.direction;
            }
            Projectile.rotation = 0;
            HandleDrawOffsets();
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

            Vector2 areaBehindPlayer = player.Center;
            float offsetX = 50f;
            areaBehindPlayer.X += (float)((12 + offsetX + player.width / 2) * direction);
            areaBehindPlayer.Y -= -35f + HalfStandHeight;
            Projectile.Center = Vector2.Lerp(Projectile.Center, areaBehindPlayer, 0.2f);
            Projectile.velocity *= 0.8f;
            Projectile.direction = (Projectile.spriteDirection = direction);
            Projectile.rotation = 0;
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
            Player player = Main.player[Projectile.owner];

            HandleDrawOffsets();
            if (!attackFrames)
            {
                Vector2 vector131 = player.Center;
                vector131.X -= (float)((12 + player.width / 2) * player.direction);
                Projectile.direction = (Projectile.spriteDirection = player.direction);
                vector131.Y -= -35f + HalfStandHeight +standYOffset;
                Projectile.Center = Vector2.Lerp(Projectile.Center, vector131, 0.2f);
                Projectile.velocity *= 0.8f;
                Projectile.rotation = 0;
            }
            float detectionDist = newMaxDistance * 1.2f;
            NPC target = FindNearestTarget(detectionDist);
            if (target != null)
            {
                attackFrames = true;
                idleFrames = false;

                Projectile.direction = 1;
                if (target.position.X - Projectile.Center.X <= 0f)
                {
                    Projectile.direction = -1;
                }
                Projectile.spriteDirection = Projectile.direction;

                Vector2 velocity = target.position - Projectile.position;
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
                        {
                            shootVel *= ProjectileSpeed;
                        }
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newPunchDamage * 0.9f), PunchKnockback, Projectile.owner, FistWhoAmI, TierNumber);
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

            HandleDrawOffsets();
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
                idleFrames = true;
                attackFrames = false;
                if (secondaryAbility)
                {
                    areaBehindPlayer.X += (float)((12 + player.width / 2) * player.direction);
                }
                else
                {
                    areaBehindPlayer.X -= (float)((12 + player.width / 2) * player.direction);
                    Projectile.spriteDirection = Projectile.direction = player.direction;
                }
                areaBehindPlayer.Y -= -35f + HalfStandHeight;
                Projectile.Center = Vector2.Lerp(Projectile.Center, areaBehindPlayer, 0.2f);
                Projectile.velocity *= 0.8f;
                Projectile.rotation = 0;
            }
            if (target != null)
            {
                if (targetDist < punchDetectionDist && !secondaryAbility)
                {
                    attackFrames = true;
                    idleFrames = false;
                    secondaryAbilityFrames = false;

                    Projectile.direction = 1;
                    if (target.position.X - Projectile.Center.X < 0f)
                    {
                        Projectile.spriteDirection = Projectile.direction = -1;
                    }
                    Projectile.spriteDirection = Projectile.direction;

                    Vector2 velocity = target.position - Projectile.position;
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
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            if (Projectile.direction == 1)
                            {
                                shootVel *= ProjectileSpeed;
                            }
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newPunchDamage * 0.9f), PunchKnockback, Projectile.owner, FistWhoAmI, TierNumber);
                            Main.projectile[proj].netUpdate = true;
                            Projectile.netUpdate = true;
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
                    idleFrames = false;
                    secondaryAbilityFrames = true;
                    Projectile.direction = 1;
                    if (target.position.X - Projectile.Center.X < 0f)
                    {
                        Projectile.spriteDirection = Projectile.direction = -1;
                    }
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
                                {
                                    shootVel = new Vector2(0f, 1f);
                                }
                                shootVel.Normalize();
                                shootVel *= ProjectileSpeed;
                                if (gravityAccounting)
                                {
                                    shootVel.Y -= Projectile.Distance(target.position) / 110f;        //Adding force with the distance of the enemy / 110 (Dividing by 110 cause if not it's gonna fly straight up)
                                }
                                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 5f, Projectile.position.Y - 3f, shootVel.X, shootVel.Y, projToShoot, (int)((AltDamage * mPlayer.standDamageBoosts) * 0.9f), 2f, Projectile.owner, Projectile.whoAmI, TierNumber);
                                Main.projectile[proj].netUpdate = true;
                                Projectile.netUpdate = true;
                            }
                            if (itemToConsumeType != -1)
                            {
                                player.ConsumeItem(itemToConsumeType);
                            }
                        }
                    }
                    if ((!gravityAccounting && player.ownedProjectileCounts[projToShoot] == 0) || targetDist > rangedDetectionDist)     //!gravityAccounting and 0 of that Projectile cause it's usually no gravity projectiles that are just 1 shot (star finger, zipper punch), while things like knives have gravity (meant to be thrown in succession)
                    {
                        secondaryAbility = false;
                        secondaryAbilityFrames = false;
                        idleFrames = true;
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
                    beginningSoundInstance.Volume = MyPlayer.ModSoundsVolume;
                }
            }
            if (punchingSoundInstance == null)
            {
                SoundEffect sound = ModContent.Request<SoundEffect>("JoJoStandsSounds/Sounds/BattleCries/" + PunchSoundName, AssetRequestMode.ImmediateLoad).Value;
                punchingSoundInstance = sound.CreateInstance();
                punchingSoundInstance.Volume = MyPlayer.ModSoundsVolume;
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
                        beginningSoundInstance.Volume = MyPlayer.ModSoundsVolume;
                        beginningSoundInstance.Play();                 //if there is no other way to have this play for everyone, send a packet with that sound type so that it plays for everyone
                        SoundInstanceGarbageCollector.Track(beginningSoundInstance);
                        playedBeginning = true;
                    }
                    if (playedBeginning && beginningSoundInstance.State == SoundState.Stopped)
                    {
                        //punchingSoundInstance.Play();     //is this not just beginningSoundInstance.Play()?
                        punchingSoundInstance.Volume = MyPlayer.ModSoundsVolume;
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

        /// <summary>
        /// Adjusts the draw offsets of the Stand based on its Projectile.spriteDirection field.
        /// </summary>
        public void HandleDrawOffsets()     //this method kind of lost its usage when we found a better way to do offsets but whatever; Future AG: No it did not.
        {
            DrawOffsetX = StandOffset * Projectile.spriteDirection;
        }

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

            if (MyPlayer.RangeIndicators && CanUseRangeIndicators && newMaxDistance > 0)
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
                    spawnSound.Volume = MyPlayer.ModSoundsVolume;
                    SoundEngine.PlaySound(spawnSound, Projectile.position);
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
        }

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
                int frameHeight = standTexture.Height / Main.projFrames[Projectile.whoAmI];
                Vector2 drawPosition = Projectile.Center - Main.screenPosition + new Vector2(DrawOffsetX / 2f, 0f);
                Rectangle animRect = new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight);
                Vector2 standOrigin = new Vector2(standTexture.Width / 2f, frameHeight / 2f);
                Main.EntitySpriteDraw(standTexture, drawPosition, animRect, drawColor, Projectile.rotation, standOrigin, 1f, effects, 0);
            }
        }

        /// <summary>
        /// Draws the Stands range indicators.
        /// Only draws if the MyPlayer.RangeIndicators field is set to true.
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DrawRangeIndicators()
        {
            Player player = Main.player[Projectile.owner];
            if (!MyPlayer.RangeIndicators || Main.netMode == NetmodeID.Server || !CanUseRangeIndicators || rangeIndicatorSize == Vector2.Zero)
                return;

            //Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/RangeIndicator>().Value;        //the initial tile amount the indicator covers is 20 tiles, 320 pixels, border is included in the measurements
            Vector2 rangeIndicatorDrawPosition = player.Center - Main.screenPosition;
            Vector2 rangeIndicatorOrigin = rangeIndicatorSize / 2f;
            float rangeIndicatorAlpha = MyPlayer.RangeIndicatorAlpha;

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
            if (mPlayer.StandDyeSlot.SlotItem.dye == 0)
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
            StandKillEffects();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(idleFrames);
            writer.Write(attackFrames);
            writer.Write(secondaryAbilityFrames);
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
            idleFrames = reader.ReadBoolean();
            attackFrames = reader.ReadBoolean();
            secondaryAbilityFrames = reader.ReadBoolean();
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
        /// Criteria for the search is set by the MyPlayer.standSearchType field.
        /// </summary>
        /// <param name="maxDetectionRange">The max distance (in pixels) to search</param>
        /// <returns>The NPC that is closest to the player and follows the given criteria.</returns>
        public NPC FindNearestTarget(float maxDetectionRange)
        {
            NPC target = null;
            Player player = Main.player[Projectile.owner];
            switch (MyPlayer.standSearchType)
            {
                case MyPlayer.StandSearchType.Bosses:
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < maxDetectionRange && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
                            if (distance < closestDistance && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
                            if (distance > farthestDistance && distance < maxDetectionRange && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
                            if (distance < maxDetectionRange && npc.life < leasthealth && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
                            if (distance < maxDetectionRange && npc.life >= mosthealth && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
        public NPC FindNearestTarget(MyPlayer.StandSearchType searchType, float maxDetectionRange)
        {
            NPC target = null;
            Player player = Main.player[Projectile.owner];
            switch (searchType)
            {
                case MyPlayer.StandSearchType.Bosses:
                    for (int n = 0; n < Main.maxNPCs; n++)       //the targeting system
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < maxDetectionRange && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
                            if (distance < closestDistance && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
                            if (distance > farthestDistance && distance < maxDetectionRange && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
                            if (distance < maxDetectionRange && npc.life < leasthealth && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
                            if (distance < maxDetectionRange && npc.life >= mosthealth && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
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
            }

            newTexturePath += standAnimationName;
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
        public void AnimateStand(string stateName, int frameAmount, int frameCounterLimit, bool loop)
        {
            Projectile.frameCounter++;
            Main.projFrames[Projectile.whoAmI] = frameAmount;
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
            Main.projFrames[Projectile.whoAmI] = frameAmount;
            if (Projectile.frameCounter >= frameCounterLimit)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
            if (loopCertainFrames)
            {
                if (Projectile.frame >= loopFrameEnd)
                {
                    Projectile.frame = loopFrameStart;
                }
            }
        }
    }
}