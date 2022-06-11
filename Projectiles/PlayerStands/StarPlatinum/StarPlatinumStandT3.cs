using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.StarPlatinum
{
    public class StarPlatinumStandT3 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 10;
        }

        public override int punchDamage => 92;
        public override int punchTime => 7;
        public override int altDamage => 139;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 0f;
        public override string punchSoundName => "Ora";
        public override string poseSoundName => "YareYareDaze";
        public override string spawnSoundName => "Star Platinum";
        public override StandType standType => StandType.Melee;

        private bool flickFrames = false;
        private bool resetFrame = false;

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
                                shootCount += 120;
                                Main.mouseLeft = false;
                                SoundStyle item41 = SoundID.Item41;
                                item41.Pitch = 2.8f;
                                SoundEngine.PlaySound(item41, player.Center);
                                Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                                if (shootVel == Vector2.Zero)
                                    shootVel = new Vector2(0f, 1f);

                                shootVel.Normalize();
                                shootVel *= 12f;
                                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, bulletItem.shoot, (int)(altDamage * mPlayer.standDamageBoosts), bulletItem.knockBack, Projectile.owner, Projectile.whoAmI);
                                Main.projectile[proj].netUpdate = true;
                                Projectile.netUpdate = true;
                                if (bulletItem.Name.Contains("Bullet"))
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
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StarFinger>(), (int)(altDamage * mPlayer.standDamageBoosts) - 39, 2f, Projectile.owner, Projectile.whoAmI);
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
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (flickFrames)
            {
                if (!resetFrame)
                {
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                    resetFrame = true;
                }
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Flick");
            }
            if (secondaryAbilityFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
                Projectile.frame = 0;
                if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<StarFinger>()] == 0)
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

        public override void AnimationCompleted(string animationName)
        {
            if (resetFrame && animationName == "Flick")
            {
                idleFrames = true;
                flickFrames = false;
                resetFrame = false;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/StarPlatinum/StarPlatinum_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Flick")
            {
                AnimateStand(animationName, 4, 10, false);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 2, 12, true);
            }
        }
    }
}