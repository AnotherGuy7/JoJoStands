using JoJoStands.DataStructures;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.CrazyDiamond
{
    public class CrazyDiamondStandT2 : StandClass
    {
        public override int PunchDamage => 52;
        public override int PunchTime => 11;
        public override int AltDamage => 81;
        public override int HalfStandHeight => 51;
        public override int FistWhoAmI => 12;
        public override int TierNumber => 2;
        public override StandAttackType StandType => StandAttackType.Melee;

        private bool flickFrames = false;
        private bool resetFrame = false;
        private bool restorationMode = false;

        private int rightClickCooldown = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;
            if (rightClickCooldown > 0)
                rightClickCooldown--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            mPlayer.crazyDiamondRestorationMode = restorationMode;
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
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
                    restorationMode = !restorationMode;
                    if (restorationMode)
                        Main.NewText("Restoration Mode: Active");
                    else
                        Main.NewText("Restoration Mode: Disabled");
                }
                if (!restorationMode)
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
                                if (Projectile.frame == 2)
                                {
                                    shootCount += 40;
                                    Main.mouseLeft = false;
                                    Vector2 shootVel = Main.MouseWorld - (Projectile.Center - new Vector2(0, 18f));
                                    if (shootVel == Vector2.Zero)
                                        shootVel = new Vector2(0f, 1f);

                                    shootVel.Normalize();
                                    shootVel *= 12f;

                                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 18f), shootVel, bulletItem.shoot, (int)(AltDamage * mPlayer.standDamageBoosts), bulletItem.knockBack, Projectile.owner, Projectile.whoAmI);
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
                if (restorationMode)
                {
                    if (Main.mouseRight && rightClickCooldown == 0 && mPlayer.crazyDiamondDestroyedTileData.Count > 0 && Projectile.owner == Main.myPlayer)
                    {
                        SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/CrazyDiamondRestore"));
                        rightClickCooldown += 180;
                    }
                    if (rightClickCooldown == 10)
                    {
                        rightClickCooldown -= 1;
                        mPlayer.crazyDiamondDestroyedTileData.ForEach(DestroyedTileData.Restore);
                        mPlayer.crazyDiamondMessageCooldown = 0;
                        mPlayer.crazyDiamondDestroyedTileData.Clear();
                    }
                }
            }
            if (restorationMode)
                Lighting.AddLight(Projectile.position, 11);
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
                BasicPunchAI();
            if (player.teleporting)
                Projectile.position = player.position;
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
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().posing)
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
            if (restorationMode)
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
            writer.Write(restorationMode);
            writer.Write(rightClickCooldown);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            flickFrames = reader.ReadBoolean();
            restorationMode = reader.ReadBoolean();
            rightClickCooldown = reader.ReadInt32();
        }
    }
}