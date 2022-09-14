using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.CrazyDiamond
{
    public class CrazyDiamondStandT2 : StandClass
    {
        public override int PunchDamage => 54;
        public override int PunchTime => 9;
        public override int AltDamage => 81;
        public override int HalfStandHeight => 51;
        public override int FistWhoAmI => 12;
        public override int TierNumber => 2;
        public override StandAttackType StandType => StandAttackType.Melee;

        private bool flickFrames = false;
        private bool resetFrame = false;
        private bool restore = false;

        private int rightClickCooldown = 0;
        private int standTier = 2;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (rightClickCooldown > 0)
                rightClickCooldown--;
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            mPlayer.standTier = standTier;
            mPlayer.crazyDiamondRestorationMode = restore;


            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !flickFrames)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                    StayBehind();
                if (flickFrames)
                    StayBehindWithAbility();
                if (SpecialKeyPressedNoCooldown() && !flickFrames)
                {
                    restore = !restore;
                    if (restore)
                        Main.NewText("Restoration Mode: Active");
                    else
                        Main.NewText("Restoration Mode: Disabled");
                }
                if (!restore)
                {
                    if (Main.mouseRight && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                    {
                        int bulletIndex = GetPlayerAmmo(player);
                        if (bulletIndex != -1)
                        {
                            Item bulletItem = player.inventory[bulletIndex];
                            if (bulletItem.shoot != -1)
                            {
                                flickFrames = true;
                                Projectile.frame = 1;
                                if (Projectile.frame == 1)
                                {
                                    shootCount += 40;
                                    Main.mouseLeft = false;
                                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                                    if (shootVel == Vector2.Zero)
                                        shootVel = new Vector2(0f, 1f);

                                    shootVel.Normalize();
                                    shootVel *= 12f;

                                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2 (0, 18f), shootVel, bulletItem.shoot, (int)(AltDamage * mPlayer.standDamageBoosts), bulletItem.knockBack, Projectile.owner, Projectile.whoAmI);
                                    Main.projectile[proj].GetGlobalProjectile<JoJoGlobalProjectile>().kickedByStarPlatinum = true;
                                    Main.projectile[proj].netUpdate = true;
                                    Projectile.netUpdate = true;
                                    SoundStyle item41 = SoundID.Item41;
                                    item41.Pitch = 2.8f;
                                    SoundEngine.PlaySound(item41, player.Center);
                                    if (bulletItem.type != ItemID.EndlessMusketPouch)
                                        player.ConsumeItem(bulletItem.type);
                                }
                            }
                        }
                    }
                }
                if (restore)
                {
                    if (Main.mouseRight && rightClickCooldown == 0 && mPlayer.ExtraTileCheck.Count > 0 && Projectile.owner == Main.myPlayer)
                    {
                        SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/RestoreSound"));
                        rightClickCooldown += 180;
                    }
                    if (rightClickCooldown == 10)
                    {
                        rightClickCooldown -= 1;
                        mPlayer.ExtraTileCheck.ForEach(mPlayer.Restore);
                        mPlayer.crazyDiamondMessageCooldown = 0;
                        mPlayer.ExtraTileCheck.Clear();
                    }
                }
            }
            if (restore)
                Lighting.AddLight(Projectile.position, 11);
            if (mPlayer.standAutoMode)
                BasicPunchAI();
        }

        private int GetPlayerAmmo(Player player)
        {
            int ammoType = -1;
            for (int i = 54; i < 58; i++)
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
                for (int i = 0; i < 54; i++)
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
                    resetFrame = true;
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                }
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Flick");
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
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            string pathAddition = "";
            if (restore)
                pathAddition = "Restoration_";

            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/CrazyDiamond", "/CrazyDiamond_" + pathAddition + animationName);

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
                AnimateStand(animationName, 4, 12, true);
            }
        }
        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(flickFrames);
            writer.Write(restore);
            writer.Write(rightClickCooldown);
            writer.Write(standTier);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            flickFrames = reader.ReadBoolean();
            restore = reader.ReadBoolean();
            rightClickCooldown = reader.ReadInt32();
            standTier = reader.ReadInt32();
        }
    }
}