using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.StarPlatinum
{
    public class StarPlatinumStandFinal : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }

        public override int punchDamage => 106;
        public override int punchTime => 6;
        public override int altDamage => 84;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 0f;
        public override string punchSoundName => "Ora";
        public override string poseSoundName => "YareYareDaze";
        public override string spawnSoundName => "Star Platinum";
        public override int standType => 1;

        public int updateTimer = 0;
        private int timestopStartDelay = 0;
        private bool flickFrames = false;
        private bool resetFrame = false;

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
            if (SpecialKeyPressed() && !player.HasBuff(mod.BuffType("TheWorldBuff")) && timestopStartDelay <= 0)
            {
                if (JoJoStands.JoJoStandsSounds == null)
                    timestopStartDelay = 240;
                else
                {
                    Terraria.Audio.LegacySoundStyle zawarudo = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/StarPlatinumTheWorld");
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
                    Timestop(4);
                    timestopStartDelay = 0;
                }
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && player.ownedProjectileCounts[mod.ProjectileType("StarFinger")] == 0)
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
                    StayBehindWithAbility();
                }
                if (Main.mouseRight && shootCount <= 0 && projectile.owner == Main.myPlayer)
                {
                    int bulletIndex = GetPlayerAmmo(player);
                    if (bulletIndex != -1)
                    {
                        Item bulletItem = player.inventory[bulletIndex];
                        if (bulletItem.shoot != -1)
                        {
                            flickFrames = true;
                            if (projectile.frame == 1)
                            {
                                shootCount += 80;
                                Main.mouseLeft = false;
                                Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 41, 1f, 2.8f);
                                Vector2 shootVel = Main.MouseWorld - projectile.Center;
                                if (shootVel == Vector2.Zero)
                                {
                                    shootVel = new Vector2(0f, 1f);
                                }
                                shootVel.Normalize();
                                shootVel *= 12f;
                                int proj = Projectile.NewProjectile(projectile.Center, shootVel, bulletItem.shoot, (int)(altDamage * modPlayer.standDamageBoosts), bulletItem.knockBack, projectile.owner, projectile.whoAmI);
                                Main.projectile[proj].netUpdate = true;
                                projectile.netUpdate = true;
                                if (bulletItem.Name.Contains("Bullet"))
                                    player.ConsumeItem(bulletItem.type);
                            }
                        }
                    }
                    else
                    {
                        if (player.ownedProjectileCounts[mod.ProjectileType("StarFinger")] == 0)
                        {
                            shootCount += 120;
                            Main.mouseLeft = false;
                            Vector2 shootVel = Main.MouseWorld - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("StarFinger"), (int)(altDamage * modPlayer.standDamageBoosts), 4f, projectile.owner, projectile.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                }
                if (player.ownedProjectileCounts[mod.ProjectileType("StarFinger")] != 0)
                {
                    secondaryAbilityFrames = true;
                    Main.mouseLeft = false;
                    projectile.netUpdate = true;
                }
            }
            if (modPlayer.StandAutoMode)
            {
                PunchAndShootAI(mod.ProjectileType("StarFinger"), shootMax: 1);
            }
        }

        private int GetPlayerAmmo(Player player)
        {
            int ammoType = -1;
            for (int i = 54; i < 58; i++)       //These are the 4 ammo slots
            {
                Item item = player.inventory[i];

                if (item.ammo == AmmoID.Bullet && item.stack > 0)
                {
                    ammoType = i;
                    break;
                }
            }
            if (ammoType == -1)
            {
                for (int i = 0; i < 54; i++)       //The rest of the inventory
                {
                    Item item = player.inventory[i];
                    if (item.ammo == AmmoID.Bullet && item.stack > 0)
                    {
                        ammoType = i;
                        break;
                    }
                }
            }
            return ammoType;
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
            if (flickFrames)
            {
                if (!resetFrame)
                {
                    projectile.frame = 0;
                    projectile.frameCounter = 0;
                    resetFrame = true;
                }
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Flick");
                if (resetFrame && currentAnimationDone)
                {
                    normalFrames = true;
                    flickFrames = false;
                    resetFrame = false;
                }
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
                projectile.frame = 0;
                if (Main.player[projectile.owner].ownedProjectileCounts[mod.ProjectileType("StarFinger")] == 0)
                {
                    secondaryAbilityFrames = false;
                }
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/StarPlatinum/StarPlatinum_" + animationName);

            if (animationName == "Idle")
            {
                AnimationStates(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Flick")
            {
                AnimationStates(animationName, 4, 10, false);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 2, 12, true);
            }
        }
    }
}