using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.TheWorld
{
    public class TheWorldStandT3 : StandClass
    {
        public override int punchDamage => 68;
        public override int altDamage => 47;
        public override int punchTime => 9;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 1f;
        public override string punchSoundName => "Muda";
        public override string poseSoundName => "ComeAsCloseAsYouLike";
        public override string spawnSoundName => "The World";
        public override int standType => 1;

        private bool abilityPose = false;
        private int timestopPoseTimer = 0;
        private int updateTimer = 0;
        private int timestopStartDelay = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }
            if (SpecialKeyPressed() && !player.HasBuff(mod.BuffType("TheWorldBuff")))
            {
                if (JoJoStands.JoJoStandsSounds == null)
                    timestopStartDelay = 120;
                else
                {
                    Terraria.Audio.LegacySoundStyle zawarudo = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/TheWorld");
                    zawarudo.WithVolume(MyPlayer.soundVolume);
                    Main.PlaySound(zawarudo, projectile.position);
                    timestopStartDelay = 1;
                }
            }
            if (timestopStartDelay != 0)
            {
                timestopStartDelay++;
                if (timestopStartDelay >= 120)
                {
                    Timestop(5);
                    timestopPoseTimer = 60;
                    timestopStartDelay = 0;
                }
            }
            if (timestopPoseTimer > 0)
            {
                timestopPoseTimer--;
                normalFrames = false;
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

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
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
                        projectile.direction = projectile.spriteDirection = player.direction;
                    }
                    else
                    {
                        GoInFront();
                        if (Main.MouseWorld.X > projectile.position.X)
                        {
                            projectile.spriteDirection = 1;
                            projectile.direction = 1;
                        }
                        if (Main.MouseWorld.X < projectile.position.X)
                        {
                            projectile.spriteDirection = -1;
                            projectile.direction = -1;
                        }
                        secondaryAbilityFrames = false;
                    }
                }
                if (Main.mouseRight && player.HasItem(mod.ItemType("Knife")) && projectile.owner == Main.myPlayer)
                {
                    Main.mouseLeft = false;
                    secondaryAbilityFrames = true;
                    normalFrames = false;
                    attackFrames = false;
                    if (shootCount <= 0 && projectile.frame == 1)
                    {
                        shootCount += 28;
                        float rotationk = MathHelper.ToRadians(15);
                        float numberKnives = 3;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 100f;
                        for (int i = 0; i < numberKnives; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotationk, rotationk, i / (numberKnives - 1))) * .2f;
                            int proj = Projectile.NewProjectile(projectile.position.X + 5f, projectile.position.Y - 3f, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Knife"), (int)(altDamage * modPlayer.standDamageBoosts), 2f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                            player.ConsumeItem(mod.ItemType("Knife"));
                            projectile.netUpdate = true;
                        }
                    }
                }
            }
            if (modPlayer.StandAutoMode)
            {
                PunchAndShootAI(mod.ProjectileType("Knife"), mod.ItemType("Knife"), true);
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
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
            if (abilityPose)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("AbilityPose");
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/TheWorld/TheWorld_" + animationName);

            if (animationName == "Idle")
            {
                AnimationStates(animationName, 2, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 2, 28, true);
            }
            if (animationName == "AbilityPose")
            {
                AnimationStates(animationName, 1, 10, true);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 10, true);
            }
        }
    }
}