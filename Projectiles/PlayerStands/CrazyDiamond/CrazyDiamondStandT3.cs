using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI;
using JoJoStands.NPCs;

namespace JoJoStands.Projectiles.PlayerStands.CrazyDiamond
{
    public class CrazyDiamondStandT3 : StandClass
    {
        public override int punchDamage => 88;
        public override int punchTime => 8;
        public override int altDamage => 132;
        public override int halfStandHeight => 51;
        public override float fistWhoAmI => 12f;
        public override float tierNumber => 3f;
        public override StandType standType => StandType.Melee;

        private bool healingFrames = false;
        private bool flickFrames = false;
        private bool resetFrame = false;
        private bool blindRage = false;
        private bool healingFramesRepeatTimerOnlyOnce = false;
        private bool returnToOwner = false;
        private bool offsetDirection = false;
        private bool restore = false;

        private int healingFramesRepeatTimer = 0;
        private int onlyOneTarget = 0;
        private int healingTargetNPC = -1;
        private int healingTargetPlayer = -1;
        private int rightClickCooldown = 0;
        private int heal = 0;
        private int standTier = 3;

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

            if (player.HasBuff(ModContent.BuffType<BlindRage>()))
                blindRage = true;
            if(!player.HasBuff(ModContent.BuffType<BlindRage>()))
                blindRage = false;

            if (blindRage) 
            {
                Punch();
                player.direction = Projectile.spriteDirection;
                Projectile.Center = new Vector2(player.Center.X, player.Center.Y);
            }

            if (!mPlayer.standAutoMode && !blindRage)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !flickFrames && !healingFrames && onlyOneTarget == 0)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames && !healingFrames && onlyOneTarget == 0)
                    StayBehind();
                if (flickFrames)
                    StayBehindWithAbility();
                if (SpecialKeyPressedNoCooldown() && !healingFrames && !flickFrames && onlyOneTarget == 0 && Projectile.owner == Main.myPlayer)
                {
                    restore = !restore;
                    if (restore)
                        Main.NewText("Restoration Mode: Active");
                    else
                        Main.NewText("Restoration Mode: Disabled");
                }
                if (!restore)
                {
                    if (Main.mouseRight && shootCount <= 0 && Projectile.owner == Main.myPlayer && !healingFrames && onlyOneTarget == 0)
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

                                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2 (0, 18f), shootVel, bulletItem.shoot, (int)(altDamage * mPlayer.standDamageBoosts), bulletItem.knockBack, Projectile.owner, Projectile.whoAmI);
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
                    if (SecondSpecialKeyPressed() && onlyOneTarget == 0 && Projectile.owner == Main.myPlayer) 
                    {
                        player.AddBuff(ModContent.BuffType<BlindRage>(), 600);
                        EmoteBubble.NewBubble(1, new WorldUIAnchor(player), 600);
                        SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/PoseSound")); 
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
                    if (SecondSpecialKeyPressed() && onlyOneTarget == 0 && Projectile.owner == Main.myPlayer)
                    {
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active && !npc.hide && !npc.immortal)
                            {
                                if (Vector2.Distance(Main.MouseWorld, npc.Center) <= 20f && Vector2.Distance(player.Center, npc.Center) > 200f)
                                    Main.NewText("Target too far");
                                if (Vector2.Distance(Main.MouseWorld, npc.Center) <= 20f && Vector2.Distance(player.Center, npc.Center) <= 200f && !healingFrames && onlyOneTarget < 1)
                                {
                                    onlyOneTarget += 1;
                                    healingTargetNPC = npc.whoAmI;
                                }
                            }
                        }
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            for (int p = 0; p < Main.maxPlayers; p++)
                            {
                                Player otherPlayer = Main.player[p];
                                if (otherPlayer.active)
                                {
                                    if (Vector2.Distance(Main.MouseWorld, otherPlayer.Center) <= 20f && Vector2.Distance(player.Center, otherPlayer.Center) > 200f)
                                        Main.NewText("Target too far");
                                    if (Vector2.Distance(Main.MouseWorld, otherPlayer.Center) <= 20f && Vector2.Distance(player.Center, otherPlayer.Center) <= 200f && !healingFrames && onlyOneTarget < 1 && otherPlayer.whoAmI != player.whoAmI)
                                    {
                                        onlyOneTarget += 1;
                                        healingTargetPlayer = otherPlayer.whoAmI;
                                    }
                                }
                            }
                        }
                    }
                    if (onlyOneTarget > 0)
                    {
                        float offset = 0f;
                        if (offsetDirection)
                            offset = 20f;
                        if (healingTargetNPC != -1)
                        {
                            NPC npc = Main.npc[healingTargetNPC];
                            if (!healingFrames && !returnToOwner)
                            {
                                Projectile.velocity = npc.Center - Projectile.Center;
                                Projectile.velocity.Normalize();
                                Projectile.velocity *= 6f;
                                if (Projectile.position.X >= npc.position.X)
                                    offsetDirection = true;
                                if (Vector2.Distance(Projectile.Center, npc.Center) <= 20f)
                                {
                                    Projectile.frame = 0;
                                    healingFrames = true;
                                }
                                Projectile.netUpdate = true;
                            }
                            if (healingFrames && !returnToOwner)
                            {
                                Projectile.position = new Vector2(npc.Center.X - 10f - offset, npc.Center.Y - 20f);
                                if (Projectile.frame == 0 && !healingFramesRepeatTimerOnlyOnce)
                                {
                                    healingFramesRepeatTimer += 1;
                                    healingFramesRepeatTimerOnlyOnce = true;
                                }
                                if (Projectile.frame != 0)
                                    healingFramesRepeatTimerOnlyOnce = false;
                                if (healingFramesRepeatTimer >= 4)
                                {
                                    offsetDirection = false;
                                    healingFrames = false;
                                    healingFramesRepeatTimerOnlyOnce = false;
                                    onlyOneTarget = 0;
                                    healingFramesRepeatTimer = 0;
                                    int heal = npc.lifeMax - npc.life;
                                    if (npc.HasBuff(ModContent.BuffType<MissingOrgans>()))
                                        heal = 0;
                                    npc.AddBuff(ModContent.BuffType<Restoration>(), 360);
                                    if (npc.townNPC && heal > 0)
                                        npc.GetGlobalNPC<JoJoGlobalNPC>().crazyDiamondFullHealth = true;
                                    else
                                        npc.lifeMax = npc.life;
                                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(45));
                                }
                                Projectile.netUpdate = true;
                            }
                            if (Vector2.Distance(npc.Center, player.Center) > 200f)
                            {
                                returnToOwner = true;
                                healingFrames = false;
                            }
                        }
                        if (healingTargetPlayer != -1)
                        {
                            Player otherPlayer = Main.player[healingTargetPlayer];
                            if (!healingFrames && !returnToOwner)
                            {
                                Projectile.velocity = otherPlayer.Center - Projectile.Center;
                                Projectile.velocity.Normalize();
                                Projectile.velocity *= 6f;
                                if (Projectile.position.X >= otherPlayer.position.X)
                                    offsetDirection = true;
                                if (Vector2.Distance(Projectile.Center, otherPlayer.Center) <= 20f)
                                {
                                    Projectile.frame = 0;
                                    healingFrames = true;
                                }
                                Projectile.netUpdate = true;
                            }
                            if (healingFrames && !returnToOwner)
                            {
                                Projectile.position = new Vector2(otherPlayer.Center.X - 10f - offset, otherPlayer.Center.Y - 20f);
                                if (Projectile.frame == 0 && !healingFramesRepeatTimerOnlyOnce)
                                {
                                    healingFramesRepeatTimer += 1;
                                    healingFramesRepeatTimerOnlyOnce = true;
                                }
                                if (Projectile.frame != 0)
                                    healingFramesRepeatTimerOnlyOnce = false;
                                if (healingFramesRepeatTimer >= 4)
                                {
                                    offsetDirection = false;
                                    healingFrames = false;
                                    healingFramesRepeatTimerOnlyOnce = false;
                                    onlyOneTarget = 0;
                                    healingFramesRepeatTimer = 0;
                                    heal = otherPlayer.statLifeMax - otherPlayer.statLife;
                                    if (otherPlayer.HasBuff(ModContent.BuffType<MissingOrgans>()))
                                        heal = 0;
                                    if (otherPlayer.whoAmI != player.whoAmI)
                                    {
                                        if (heal > 0)
                                            otherPlayer.Heal(heal);
                                        otherPlayer.AddBuff(ModContent.BuffType<Restoration>(), 360);
                                    }
                                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(45));
                                }
                                Projectile.netUpdate = true;
                            }
                            if (Vector2.Distance(otherPlayer.Center, player.Center) > 200f)
                            {
                                returnToOwner = true;
                                healingFrames = false;
                            }
                        }
                        if (returnToOwner)
                        {
                            if (Projectile.velocity.X < 0)
                                Projectile.spriteDirection = -1;
                            else
                                Projectile.spriteDirection = 1;
                            Projectile.velocity = player.Center - Projectile.Center;
                            Projectile.velocity.Normalize();
                            Projectile.velocity *= 6f;
                            idleFrames = true;
                            if (Vector2.Distance(Projectile.Center, player.Center) <= 20f)
                            {
                                returnToOwner = false;
                                offsetDirection = false;
                                healingFrames = false;
                                healingFramesRepeatTimerOnlyOnce = false;
                                onlyOneTarget = 0;
                                healingFramesRepeatTimer = 0;
                            }
                            Projectile.netUpdate = true;
                        }
                        if (onlyOneTarget == 0)
                        {
                            healingTargetNPC = -1;
                            healingTargetPlayer = -1;
                        }
                    }
                }
            }
            if (restore)
                Lighting.AddLight(Projectile.position, 11);
            if (mPlayer.standAutoMode && onlyOneTarget == 0)
            {
                returnToOwner = false;
                healingFrames = false;
                onlyOneTarget = 0;
                healingFramesRepeatTimer = 0;
                healingFramesRepeatTimerOnlyOnce = false;
                BasicPunchAI();
            }

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
            if (healingFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Heal");
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
            if (animationName == "Heal")
            {
                AnimateStand(animationName, 4, 12, true);
            }
        }
        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(healingFrames);
            writer.Write(flickFrames);
            writer.Write(blindRage);
            writer.Write(returnToOwner);
            writer.Write(restore);
            writer.Write(onlyOneTarget);
            writer.Write(healingTargetNPC);
            writer.Write(healingTargetPlayer);
            writer.Write(rightClickCooldown);
            writer.Write(healingFramesRepeatTimer);
            writer.Write(standTier);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            healingFrames = reader.ReadBoolean();
            flickFrames = reader.ReadBoolean();
            blindRage = reader.ReadBoolean();
            returnToOwner = reader.ReadBoolean();
            restore = reader.ReadBoolean();
            onlyOneTarget = reader.ReadInt32();
            healingTargetNPC = reader.ReadInt32();
            healingTargetPlayer = reader.ReadInt32();
            rightClickCooldown = reader.ReadInt32();
            healingFramesRepeatTimer = reader.ReadInt32();
            standTier = reader.ReadInt32();
        }
    }
}