using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Networking;

namespace JoJoStands.Projectiles.PlayerStands
{
    public class TheWorldStandFinal : StandClass
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/PlayerStands/TheWorldStand"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }

        public override int punchDamage => 82;
        public override int altDamage => 65;
        public override int punchTime => 8;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 1f;
        public override string punchSoundName => "Muda";

        private bool abilityPose = false;
        private int timestopPoseTimer = 0;
        private int updateTimer = 0;
        private int timestopStartDelay = 0;

        public override void AI()
        {
            SelectFrame();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }
            if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("TheWorldBuff")) && projectile.owner == Main.myPlayer)
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
                    Timestop(9);
                    timestopStartDelay = 0;
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
                        projectile.direction = (projectile.spriteDirection = player.direction);
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
                    }
                    secondaryAbilityFrames = false;
                }
                if (Main.mouseRight && player.HasItem(mod.ItemType("Knife")) && projectile.owner == Main.myPlayer)
                {
                    Main.mouseLeft = false;
                    secondaryAbilityFrames = true;
                    normalFrames = false;
                    attackFrames = false;
                    if (shootCount <= 0 && projectile.frame == 9)
                    {
                        shootCount += 13;       // has to be half if the framecounter + 1 (2 if shootCount goes to -1)
                        float rotationk = MathHelper.ToRadians(15);
                        float numberKnives = 4;
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
                            projectile.netUpdate = true;
                        }
                        player.ConsumeItem(mod.ItemType("Knife"));
                        player.ConsumeItem(mod.ItemType("Knife"));
                        player.ConsumeItem(mod.ItemType("Knife"));
                        player.ConsumeItem(mod.ItemType("Knife"));
                    }
                }
                if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(mod.BuffType("AbilityCooldown")) && player.HasBuff(mod.BuffType("TheWorldBuff")) && timestopPoseTimer <= 0 && player.ownedProjectileCounts[mod.ProjectileType("RoadRoller")] == 0 && projectile.owner == Main.myPlayer)
                {
                    shootCount += 12;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed + 4f;
                    int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("RoadRoller"), 120, 5f, Main.myPlayer);
                    Main.projectile[proj].netUpdate = true;
                    projectile.netUpdate = true;
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
            }
            if (modPlayer.StandAutoMode)
            {
                PunchAndShootAI(mod.ProjectileType("Knife"));
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(abilityPose);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            abilityPose = reader.ReadBoolean();
        }

        public virtual void SelectFrame()
        {
            Player player = Main.player[projectile.owner];
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (projectile.frameCounter >= punchTime - player.GetModPlayer<MyPlayer>().standSpeedBoosts)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 1)
                {
                    projectile.frame = 2;
                }
                if (projectile.frame >= 6)
                {
                    projectile.frame = 2;
                }
            }
            if (normalFrames)
            {
                if (projectile.frameCounter >= 30)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
            if (secondaryAbilityFrames)
            {
                if (projectile.frameCounter >= 24)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 10)
                {
                    projectile.frame = 8;
                }
                if (projectile.frame <= 7)
                {
                    projectile.frame = 8;
                }
            }
            if (abilityPose)
            {
                projectile.frame = 6;
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                projectile.frame = 7;
            }
        }
    }
}