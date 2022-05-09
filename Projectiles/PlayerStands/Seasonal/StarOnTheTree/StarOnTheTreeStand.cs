using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.StarPlatinum
{
    public class StarOnTheTreeStand : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 10;
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

        private int updateTimer = 0;
        private int timestopStartDelay = 0;
        private bool flickFrames = false;
        private bool resetFrame = false;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>());
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (Main.rand.Next(0, 4 + 1) == 0)
            {
                int dust = Dust.NewDust(Projectile.position - new Vector2(0f, halfStandHeight), 58, 64, 71);
                Main.dust[dust].noGravity = true;
            }
            Lighting.AddLight(Projectile.Center + new Vector2(0f, -halfStandHeight + 2f), 1f / 2f, 0.88f / 2f, 0.9f / 2f);

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                Projectile.netUpdate = true;
            }
            if (SpecialKeyPressed() && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()) && timestopStartDelay <= 0)
            {
                if (JoJoStands.JoJoStandsSounds == null)
                    timestopStartDelay = 240;
                else
                {
                    Terraria.Audio.LegacySoundStyle zawarudo = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/StarPlatinumTheWorld>();
                    zawarudo.WithVolume(MyPlayer.ModSoundsVolume);
                    SoundEngine.PlaySound(zawarudo, Projectile.position);
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

            if (!mPlayer.standAutoMode)
            {
                secondaryAbilityFrames = player.ownedProjectileCounts[ModContent.ProjectileType<StarFinger>()] != 0;

                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !flickFrames && player.ownedProjectileCounts[ModContent.ProjectileType<StarFinger>()] == 0)
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
                if (Main.mouseRight && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                {
                    int bulletIndex = GetPlayerAmmo(player);
                    if (bulletIndex != -1)
                    {
                        Item bulletItem = player.inventory[bulletIndex];
                        if (bulletItem.shoot != -1)
                        {
                            flickFrames = true;
                            if (Projectile.frame == 1)
                            {
                                shootCount += 80;
                                Main.mouseLeft = false;
                                SoundEngine.PlaySound(2, (int)player.position.X, (int)player.position.Y, 41, 1f, 2.8f);
                                Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                                if (shootVel == Vector2.Zero)
                                {
                                    shootVel = new Vector2(0f, 1f);
                                }
                                shootVel.Normalize();
                                shootVel *= 12f;
                                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, bulletItem.shoot, (int)(altDamage * mPlayer.standDamageBoosts), bulletItem.knockBack, Projectile.owner, Projectile.whoAmI);
                                Main.projectile[proj].netUpdate = true;
                                Projectile.netUpdate = true;
                                if (bulletItem.Name.Contains("Bullet>())
                                    player.ConsumeItem(bulletItem.type);
                            }
                        }
                    }
                    else
                    {
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<StarFinger>()] == 0)
                        {
                            shootCount += 120;
                            Main.mouseLeft = false;
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StarFinger>(), (int)(altDamage * mPlayer.standDamageBoosts), 4f, Projectile.owner, Projectile.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                }
            }
            if (mPlayer.standAutoMode)
            {
                PunchAndShootAI(ModContent.ProjectileType<StarFinger>(), shootMax: 1);
            }
        }

        private int GetPlayerAmmo(Player player)
        {
            int ammoType = -1;
            for (int i = 54; i < 58; i++)       //These are the 4 ammo slots
            {
                Item Item = player.inventory[i];

                if (Item.ammo == AmmoID.Bullet && Item.stack > 0)
                {
                    ammoType = i;
                    break;
                }
            }
            if (ammoType == -1)
            {
                for (int i = 0; i < 54; i++)       //The rest of the inventory
                {
                    Item Item = player.inventory[i];
                    if (Item.ammo == AmmoID.Bullet && Item.stack > 0)
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
                PlayAnimation("Attack>();
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle>();
            }
            if (flickFrames)
            {
                if (!resetFrame)
                {
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                    resetFrame = true;
                }
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Flick>();
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose>();
                Projectile.frame = 0;
                if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<StarFinger>()] == 0)
                {
                    secondaryAbilityFrames = false;
                }
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>()).poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose>();
            }
        }

        public override void AnimationCompleted(string animationName)
        {
            if (resetFrame && animationName == "Flick>()
            {
                normalFrames = true;
                flickFrames = false;
                resetFrame = false;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = Mod.GetTexture("Projectiles/PlayerStands/Seasonal/StarOnTheTree/StarOnTheTree_" + animationName);

            if (animationName == "Idle>()
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack>()
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Flick>()
            {
                AnimateStand(animationName, 4, 10, false);
            }
            if (animationName == "Pose>()
            {
                AnimateStand(animationName, 2, 12, true);
            }
        }
    }
}