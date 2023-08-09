using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Items;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.TheWorld
{
    public class TheWorldStandFinal : StandClass
    {
        public override int PunchDamage => 82;
        public override int AltDamage => 65;
        public override int PunchTime => 10;
        public override int HalfStandHeight => 44;
        public override int FistWhoAmI => 1;
        public override int TierNumber => 4;
        public override string PunchSoundName => "Muda";
        public override string PoseSoundName => "ComeAsCloseAsYouLike";
        public override string SpawnSoundName => "The World";
        public override int AmountOfPunchVariants => 3;
        public override string PunchTexturePath => "JoJoStands/Projectiles/PlayerStands/TheWorld/TheWorld_Punch_";
        public override Vector2 PunchSize => new Vector2(28, 12);
        public override PunchSpawnData PunchData => new PunchSpawnData()
        {
            standardPunchOffset = new Vector2(12f, 6f),
            minimumLifeTime = 5,
            maximumLifeTime = 12,
            minimumTravelDistance = 16,
            maximumTravelDistance = 32,
            bonusAfterimageAmount = 0
        };

        public override bool CanUseSaladDye => true;
        public override StandAttackType StandType => StandAttackType.Melee;
        public static readonly SoundStyle TheWorldTimestopSound = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/TheWorld")
        {
            Volume = JoJoStands.ModSoundsVolume
        };
        public static readonly SoundStyle RoadRollerSound = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/RoadRollerDa")
        {
            Volume = JoJoStands.ModSoundsVolume
        };
        private bool abilityPose = false;
        private int timestopPoseTimer = 0;
        private int timestopStartDelay = 0;

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

            if (SpecialKeyPressed() && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()) && timestopStartDelay <= 0)
            {
                if (!JoJoStands.SoundsLoaded || !JoJoStands.SoundsModAbilityVoicelines)
                    timestopStartDelay = 120;
                else
                {
                    SoundEngine.PlaySound(TheWorldTimestopSound, Projectile.position);
                    timestopStartDelay = 1;
                }
            }
            if (timestopStartDelay != 0)
            {
                timestopStartDelay++;
                if (timestopStartDelay >= 120)
                {
                    Timestop(9);
                    timestopPoseTimer = 60;
                    timestopStartDelay = 0;
                }
            }
            if (timestopPoseTimer > 0)
            {
                timestopPoseTimer--;
                abilityPose = true;
                Main.mouseLeft = false;
                Main.mouseRight = false;
                if (timestopPoseTimer <= 1)
                    abilityPose = false;
            }
            if (mPlayer.timestopActive && !mPlayer.timestopOwner)
                return;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !secondaryAbility)
                        Punch();
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }

                    if (Main.mouseRight && player.HasItem(ModContent.ItemType<Knife>()))
                    {
                        secondaryAbility = true;
                        currentAnimationState = AnimationState.SecondaryAbility;
                        Projectile.netUpdate = true;
                        if (shootCount <= 0 && Projectile.frame == 1)
                        {
                            shootCount += 16;       // has to be half if the framecounter + 1 (2 if shootCount goes to -1)
                            float numberOfKnives = 5;
                            float knivesAngleSpread = MathHelper.ToRadians(15f);
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= 100f;
                            for (int i = 0; i < numberOfKnives; i++)
                            {
                                Vector2 shootPosition = Projectile.position + new Vector2(5f, -3f);
                                Vector2 perturbedSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-knivesAngleSpread, knivesAngleSpread, i / (numberOfKnives - 1))) * .2f;
                                int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), shootPosition, perturbedSpeed, ModContent.ProjectileType<KnifeProjectile>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, player.whoAmI);
                                Main.projectile[projIndex].netUpdate = true;
                                player.ConsumeItem(ModContent.ItemType<Knife>());
                            }
                            SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                        }
                    }
                    else
                        secondaryAbility = false;
                }

                if (!attacking)
                {
                    if (!secondaryAbility)
                    {
                        StayBehind();
                        Projectile.direction = Projectile.spriteDirection = player.direction;
                    }
                    else
                    {
                        GoInFront();
                        Projectile.direction = 1;
                        if (Projectile.owner == Main.myPlayer)
                        {
                            if (Main.MouseWorld.X < Projectile.position.X)
                                Projectile.direction = -1;

                            Projectile.spriteDirection = Projectile.direction;
                        }
                    }
                }
                if (SpecialKeyPressed() && player.HasBuff(ModContent.BuffType<TheWorldBuff>()) && timestopPoseTimer <= 0 && player.ownedProjectileCounts[ModContent.ProjectileType<RoadRoller>()] == 0)
                {
                    if (JoJoStands.SoundsLoaded)
                        SoundEngine.PlaySound(RoadRollerSound, Projectile.Center);

                    shootCount += 12;
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);

                    shootVel.Normalize();
                    shootVel *= ProjectileSpeed + 4f;
                    int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<RoadRoller>(), 512, 12f, player.whoAmI);
                    Main.projectile[projIndex].netUpdate = true;
                    Projectile.netUpdate = true;
                }
                if (SecondSpecialKeyPressed() && player.HasItem(ModContent.ItemType<Knife>()) && player.CountItem(ModContent.ItemType<Knife>()) >= 75 && Projectile.owner == Main.myPlayer)
                {
                    NPC target = null;

                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.lifeMax > 5 && !npc.townNPC && !npc.immortal && !npc.hide && Vector2.Distance(npc.Center, Main.MouseWorld) <= npc.width + 20f)
                        {
                            target = npc;
                            break;
                        }
                    }

                    if (target == null)
                        return;

                    int firstRingKnives = 25;
                    for (int k = 0; k < firstRingKnives; k++)
                    {
                        float radius = target.height;
                        float radians = (360 / firstRingKnives) * k;
                        Vector2 position = target.position + (MathHelper.ToRadians(radians).ToRotationVector2() * radius);
                        Vector2 velocity = target.position - position;
                        velocity.Normalize();
                        velocity *= 8f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<KnifeProjectile>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, player.whoAmI);
                    }

                    int secondRingKnives = 50;
                    for (int k = 0; k < secondRingKnives; k++)
                    {
                        float radius = target.height * 1.8f;
                        float radians = (360 / secondRingKnives) * k;
                        Vector2 position = target.position + (MathHelper.ToRadians(radians).ToRotationVector2() * radius);
                        Vector2 velocity = target.position - position;
                        velocity.Normalize();
                        velocity *= 8f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<KnifeProjectile>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, player.whoAmI);
                    }

                    for (int i = 0; i < firstRingKnives + secondRingKnives; i++)
                    {
                        player.ConsumeItem(ModContent.ItemType<Knife>());
                    }

                    mPlayer.posing = true;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(15));
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                PunchAndShootAI(ModContent.ProjectileType<KnifeProjectile>(), ModContent.ItemType<Knife>(), true);
            }
            if (abilityPose)
                currentAnimationState = AnimationState.Special;
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        public override void SendExtraStates(BinaryWriter writer)       //since this is overriden you have to sync the normal stuff
        {
            writer.Write(abilityPose);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            abilityPose = reader.ReadBoolean();
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
            else if (currentAnimationState == AnimationState.Special)
                PlayAnimation("AbilityPose");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/TheWorld", "TheWorld_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 15, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Secondary")
                AnimateStand(animationName, 2, 14, true);
            else if (animationName == "AbilityPose")
                AnimateStand(animationName, 1, 10, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 10, true);
        }
    }
}