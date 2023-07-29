using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.StarPlatinum
{
    public class StarPlatinumStandT3 : StandClass
    {
        public override int PunchDamage => 92;
        public override int PunchTime => 9;
        public override int AltDamage => 139;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 0;
        public override int TierNumber => 3;
        public override string PunchSoundName => "Ora";
        public override string PoseSoundName => "YareYareDaze";
        public override string SpawnSoundName => "Star Platinum";
        public override bool CanUsePart4Dye => true;
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private bool flickFrames = false;

        public new enum AnimationState
        {
            Idle,
            Attack,
            Secondary,
            Flick,
            Pose
        }

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

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                secondaryAbility = player.ownedProjectileCounts[ModContent.ProjectileType<StarFinger>()] != 0;
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !flickFrames && player.ownedProjectileCounts[ModContent.ProjectileType<StarFinger>()] == 0)
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch();
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                    if (Main.mouseRight && shootCount <= 0)
                    {
                        int bulletIndex = GetPlayerAmmo(player);
                        if (bulletIndex != -1)
                        {
                            Item bulletItem = player.inventory[bulletIndex];
                            if (bulletItem.shoot != -1)
                            {
                                flickFrames = true;
                                currentAnimationState = AnimationState.Flick;
                                if (Projectile.frame == 1)
                                {
                                    shootCount += 60;
                                    Main.mouseLeft = false;
                                    SoundStyle item41 = SoundID.Item41;
                                    item41.Pitch = 2.8f;
                                    SoundEngine.PlaySound(item41, player.Center);
                                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                                    if (shootVel == Vector2.Zero)
                                        shootVel = new Vector2(0f, 1f);

                                    shootVel.Normalize();
                                    shootVel *= 12f;
                                    int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, bulletItem.shoot, (int)(AltDamage * mPlayer.standDamageBoosts), bulletItem.knockBack, Projectile.owner, Projectile.whoAmI);
                                    Main.projectile[projIndex].GetGlobalProjectile<JoJoGlobalProjectile>().kickedByStarPlatinum = true;
                                    Main.projectile[projIndex].netUpdate = true;
                                    if (bulletItem.type != ItemID.EndlessMusketPouch)
                                    {
                                        player.ConsumeItem(bulletItem.type);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (player.ownedProjectileCounts[ModContent.ProjectileType<StarFinger>()] == 0)
                            {
                                shootCount += 60;
                                Main.mouseLeft = false;
                                Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                                if (shootVel == Vector2.Zero)
                                    shootVel = new Vector2(0f, 1f);

                                shootVel.Normalize();
                                shootVel *= ProjectileSpeed;
                                int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StarFinger>(), (int)(AltDamage * mPlayer.standDamageBoosts) - 39, 2f, Projectile.owner, Projectile.whoAmI);
                                Main.projectile[projIndex].netUpdate = true;
                                Projectile.netUpdate = true;
                            }
                        }
                    }
                }
                if (!attacking)
                    StayBehindWithAbility();
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                PunchAndShootAI(ModContent.ProjectileType<StarFinger>(), shootMax: 1);
            }

            if (secondaryAbility)
                currentAnimationState = AnimationState.Secondary;
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

        public override byte SendAnimationState() => (byte)currentAnimationState;
        public override void ReceiveAnimationState(byte state) => currentAnimationState = (AnimationState)state;

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
            else if (currentAnimationState == AnimationState.Flick)
                PlayAnimation("Flick");
            else if (currentAnimationState == AnimationState.Secondary || currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void AnimationCompleted(string animationName)
        {
            if (animationName == "Flick")
            {
                currentAnimationState = AnimationState.Idle;
                flickFrames = false;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/StarPlatinum", "StarPlatinum_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Flick")
                AnimateStand(animationName, 4, 10, false);
            else if (animationName == "Pose")
                AnimateStand(animationName, 2, 12, true);
        }
    }
}