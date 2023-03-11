using JoJoStands.Buffs.Debuffs;
using JoJoStands.DataStructures;
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
        private bool restoringObjects = false;
        private int tileRestorationTimer = 0;
        private int restorationEffectStartTimer = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;
            if (tileRestorationTimer > 0)
                tileRestorationTimer--;
            if (restorationEffectStartTimer > 0)
                restorationEffectStartTimer--;

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
                if (SpecialKeyPressed(false) && !flickFrames)
                {
                    restorationMode = !restorationMode;
                    if (restorationMode)
                        Main.NewText("Restoration Mode: Active");
                    else
                        Main.NewText("Restoration Mode: Disabled");
                }
                if (!restorationMode)
                {
                    if (Main.mouseRight && !playerHasAbilityCooldown && !flickFrames && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                    {
                        int bulletIndex = GetPlayerAmmo(player);
                        if (bulletIndex != -1)
                        {
                            Item bulletItem = player.inventory[bulletIndex];
                            if (bulletItem.shoot != -1)
                            {
                                flickFrames = true;
                                Projectile.frame = 0;
                                Projectile.frameCounter = 0;
                            }
                        }
                    }
                    if (flickFrames && shootCount <= 0)
                    {
                        if (Projectile.frame == 2)
                        {
                            int bulletIndex = GetPlayerAmmo(player);
                            Item bulletItem = player.inventory[bulletIndex];

                            shootCount += 40;
                            Main.mouseLeft = false;
                            Vector2 shootVel = Main.MouseWorld - (Projectile.Center - new Vector2(0, 18f));
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= 12f;

                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 18f), shootVel, bulletItem.shoot, (int)(AltDamage * mPlayer.standDamageBoosts), bulletItem.knockBack, Projectile.owner, Projectile.whoAmI);
                            Main.projectile[projIndex].GetGlobalProjectile<JoJoGlobalProjectile>().kickedByStarPlatinum = true;
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                            SoundStyle item41 = SoundID.Item41;
                            item41.Pitch = 2.8f;
                            SoundEngine.PlaySound(item41, player.Center);
                            if (bulletItem.type != ItemID.EndlessMusketPouch)
                                player.ConsumeItem(bulletItem.type);
                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(8));
                        }
                    }
                }
                else
                {
                    int amountOfDusts = Main.rand.Next(0, 2 + 1);
                    for (int i = 0; i < amountOfDusts; i++)
                    {
                        int index = Dust.NewDust(Projectile.position - new Vector2(0f, HalfStandHeight), Projectile.width, HalfStandHeight * 2, DustID.IchorTorch, Scale: Main.rand.Next(8, 12) / 10f);
                        Main.dust[index].noGravity = true;
                        Main.dust[index].velocity = new Vector2(Main.rand.Next(-2, 2 + 1) / 10f, Main.rand.Next(-5, -2 + 1) / 10f);
                    }
                    Lighting.AddLight(Projectile.position, TorchID.Ichor);

                    if (Main.mouseRight && restorationEffectStartTimer <= 0 && mPlayer.crazyDiamondDestroyedTileData.Count > 0 && !playerHasAbilityCooldown && Projectile.owner == Main.myPlayer)
                    {
                        SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/CrazyDiamondRestore"));
                        restorationEffectStartTimer += 180;
                        restoringObjects = true;
                    }

                    if (restoringObjects && restorationEffectStartTimer <= 0)
                    {
                        if (tileRestorationTimer <= 0)
                        {
                            if (mPlayer.crazyDiamondDestroyedTileData.Count <= 0)
                            {
                                restoringObjects = false;
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(20));
                            }
                            else
                            {
                                tileRestorationTimer += 3;
                                mPlayer.crazyDiamondMessageCooldown = 0;
                                DestroyedTileData.Restore(mPlayer.crazyDiamondDestroyedTileData[mPlayer.crazyDiamondDestroyedTileData.Count - 1]);
                                mPlayer.crazyDiamondDestroyedTileData.RemoveAt(mPlayer.crazyDiamondDestroyedTileData.Count - 1);
                            }
                        }

                        int startingIndex = (int)MathHelper.Clamp(mPlayer.crazyDiamondDestroyedTileData.Count - 20, 0, mPlayer.crazyDiamondDestroyedTileData.Count);
                        for (int i = startingIndex; i < mPlayer.crazyDiamondDestroyedTileData.Count; i++)
                        {
                            int index = Dust.NewDust(mPlayer.crazyDiamondDestroyedTileData[i].TilePosition * 16f, 16, 16, DustID.IchorTorch, Scale: Main.rand.Next(8, 12) / 10f);
                            Main.dust[index].noGravity = true;
                        }
                    }
                }
            }
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
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            flickFrames = reader.ReadBoolean();
            restorationMode = reader.ReadBoolean();
        }
    }
}