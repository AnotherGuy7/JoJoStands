using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.TheWorld
{
    public class TheWorldStandFinal : StandClass
    {
        public override int punchDamage => 82;
        public override int altDamage => 65;
        public override int punchTime => 8;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 1f;
        public override string punchSoundName => "Muda";
        public override string poseSoundName => "ComeAsCloseAsYouLike";
        public override string spawnSoundName => "The World";
        public override int standType => 1;

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
                if (JoJoStands.JoJoStandsSounds == null)
                    timestopStartDelay = 120;
                else
                {
                    SoundStyle zawarudo = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/TheWorld");
                    zawarudo.Volume = MyPlayer.ModSoundsVolume;
                    SoundEngine.PlaySound(zawarudo, Projectile.position);
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
                idleFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                abilityPose = true;
                Main.mouseLeft = false;
                Main.mouseRight = false;
                if (timestopPoseTimer <= 1)
                {
                    abilityPose = false;
                }
            }

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !secondaryAbilityFrames)
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
                    if (!secondaryAbilityFrames)
                    {
                        StayBehind();
                        Projectile.direction = Projectile.spriteDirection = player.direction;
                    }
                    else
                    {
                        GoInFront();
                        Projectile.direction = 1;
                        if (Main.MouseWorld.X < Projectile.position.X)
                            Projectile.direction = -1;

                        Projectile.spriteDirection = Projectile.direction;
                    }
                    secondaryAbilityFrames = false;
                }
                if (Main.mouseRight && player.HasItem(ModContent.ItemType<Knife>()) && Projectile.owner == Main.myPlayer)
                {
                    idleFrames = false;
                    attackFrames = false;
                    secondaryAbilityFrames = true;
                    if (shootCount <= 0 && Projectile.frame == 1)
                    {
                        shootCount += 13;       // has to be half if the framecounter + 1 (2 if shootCount goes to -1)
                        float numberOfKnives = 4;
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
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), shootPosition, perturbedSpeed, ModContent.ProjectileType<KnifeProjectile>(), (int)(altDamage * mPlayer.standDamageBoosts), 2f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                            player.ConsumeItem(ModContent.ItemType<Knife>());
                            Projectile.netUpdate = true;
                        }
                    }
                }
                if (SpecialKeyPressed() && player.HasBuff(ModContent.BuffType<TheWorldBuff>()) && timestopPoseTimer <= 0 && player.ownedProjectileCounts[ModContent.ProjectileType<RoadRoller>()] == 0)
                {
                    if (JoJoStands.SoundsLoaded)
                        SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/RoadRollerDa"));

                    shootCount += 12;
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);

                    shootVel.Normalize();
                    shootVel *= shootSpeed + 4f;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<RoadRoller>(), 512, 12f, player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
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
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<KnifeProjectile>(), (int)(altDamage * mPlayer.standDamageBoosts), 2f, player.whoAmI);
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
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<KnifeProjectile>(), (int)(altDamage * mPlayer.standDamageBoosts), 2f, player.whoAmI);
                    }

                    for (int i = 0; i < firstRingKnives + secondRingKnives; i++)
                    {
                        player.ConsumeItem(ModContent.ItemType<Knife>());
                    }

                    mPlayer.poseMode = true;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(15));
                }
            }
            if (mPlayer.standAutoMode)
            {
                PunchAndShootAI(ModContent.ProjectileType<KnifeProjectile>(), ModContent.ItemType<Knife>(), true);
            }
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
            }
            if (abilityPose)
            {
                idleFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("AbilityPose");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                idleFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/TheWorld/TheWorld_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 2, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 2, 24, true);
            }
            if (animationName == "AbilityPose")
            {
                AnimateStand(animationName, 1, 10, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 10, true);
            }
        }
    }
}