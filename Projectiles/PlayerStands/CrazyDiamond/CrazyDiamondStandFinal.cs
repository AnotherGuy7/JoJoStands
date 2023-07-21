using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.DataStructures;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.CrazyDiamond
{
    public class CrazyDiamondStandFinal : StandClass
    {
        public override int PunchDamage => 105;
        public override int PunchTime => 9;
        public override int AltDamage => 162;
        public override int HalfStandHeight => 51;
        public override int FistWhoAmI => 12;
        public override int TierNumber => 4;
        public override string PunchSoundName => "Dora";
        public override string PoseSoundName => "CrazyDiamond";
        public override string SpawnSoundName => "Crazy Diamond";
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;
        public static readonly SoundStyle RestorationSound = new SoundStyle("JoJoStands/Sounds/GameSounds/CrazyDiamondRestore");
        public new enum AnimationState
        {
            Idle,
            Attack,
            Flick,
            Healing,
            Pose
        }

        private bool healingFrames = false;
        private bool flickFrames = false;
        private bool blindRage = false;
        private bool healingFramesRepeatTimerOnlyOnce = false;
        private bool returnToOwner = false;
        private bool playerStandDirectionMismatch = false;
        private bool restorationMode = false;
        private bool restoringObjects = false;
        private bool restoredEnemies = false;
        private bool restorationTargetSelected = false;

        private int healingFramesRepeatTimer = 0;
        private int healingTargetNPC = -1;
        private int healingTargetPlayer = -1;
        private int restorationEffectStartTimer = 0;
        private int tileRestorationTimer = 0;
        private Rectangle mouseClickRect;

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
            if (tileRestorationTimer > 0)
                tileRestorationTimer--;
            if (restorationEffectStartTimer > 0)
                restorationEffectStartTimer--;

            mPlayer.crazyDiamondRestorationMode = restorationMode;
            if (blindRage)
            {
                newMaxDistance *= 0.5f;
                player.AddBuff(ModContent.BuffType<Rampage>(), 2);
            }
            if (restorationMode)
                newPunchDamage = (int)(newPunchDamage * 0.5f);

            if (SecondSpecialKeyPressed() && !restorationTargetSelected && Projectile.owner == Main.myPlayer)
            {
                blindRage = !blindRage;
                if (blindRage)
                {
                    player.AddBuff(ModContent.BuffType<Rampage>(), 2);
                    EmoteBubble.NewBubble(1, new WorldUIAnchor(player), 5 * 60);
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/PoseSound"));
                }
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !flickFrames && !healingFrames && !restorationTargetSelected)
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch();
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }

                if (!attacking && !healingFrames && !restorationTargetSelected)
                    StayBehind();
                if (flickFrames)
                    StayBehindWithAbility();
                if (SpecialKeyPressed(false) && !healingFrames && !flickFrames && !restorationTargetSelected && Projectile.owner == Main.myPlayer)
                {
                    restorationMode = !restorationMode;
                    if (restorationMode)
                        Main.NewText("Restoration Mode: Active");
                    else
                        Main.NewText("Restoration Mode: Disabled");
                }
                if (!restorationMode)
                {
                    if (Main.mouseRight && !playerHasAbilityCooldown && !flickFrames && !healingFrames && !restorationTargetSelected && shootCount <= 0 && Projectile.owner == Main.myPlayer)
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
                    if (flickFrames)
                    {
                        currentAnimationState = AnimationState.Flick;
                        if (Projectile.frame == 2 && shootCount <= 0)
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
                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(4));
                        }
                    }
                }
                else
                {
                    int amountOfDusts = Main.rand.Next(1, 4 + 1);
                    for (int i = 0; i < amountOfDusts; i++)
                    {
                        int index = Dust.NewDust(Projectile.position - new Vector2(0f, HalfStandHeight), Projectile.width, HalfStandHeight * 2, DustID.IchorTorch, Scale: Main.rand.Next(8, 12) / 10f);
                        Main.dust[index].noGravity = true;
                        Main.dust[index].velocity = new Vector2(Main.rand.Next(-2, 2 + 1) / 10f, Main.rand.Next(-5, -2 + 1) / 10f);
                    }
                    Lighting.AddLight(Projectile.position, TorchID.Ichor);

                    if (Main.mouseRight && restorationEffectStartTimer <= 0 && !playerHasAbilityCooldown && Projectile.owner == Main.myPlayer)
                    {
                        if (Projectile.owner == player.whoAmI)
                            mouseClickRect = new Rectangle((int)(Main.MouseWorld.X - 10), (int)(Main.MouseWorld.Y - 10), 20, 20);

                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active && !npc.hide && !npc.immortal && npc.Hitbox.Intersects(mouseClickRect))
                            {
                                if (Vector2.Distance(Projectile.Center, npc.Center) > 200f)
                                {
                                    Main.NewText("That target is too far.");
                                    break;
                                }

                                if (Vector2.Distance(Projectile.Center, npc.Center) <= 200f && !healingFrames && !restorationTargetSelected)
                                {
                                    restorationTargetSelected = true;
                                    healingTargetNPC = npc.whoAmI;
                                    break;
                                }
                            }
                        }
                        if (!restorationTargetSelected)
                        {
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                if (mPlayer.overHeaven && player.active && player.Hitbox.Intersects(mouseClickRect))
                                {
                                    if (!restorationTargetSelected && !healingFrames)
                                    {
                                        restorationTargetSelected = true;
                                        healingTargetPlayer = player.whoAmI;
                                    }
                                }
                            }
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                for (int p = 0; p < Main.maxPlayers; p++)
                                {
                                    Player otherPlayer = Main.player[p];
                                    if (otherPlayer.active && otherPlayer.Hitbox.Intersects(mouseClickRect))
                                    {
                                        if (Vector2.Distance(Projectile.Center, otherPlayer.Center) > 200f)
                                        {
                                            Main.NewText("That target is too far.");
                                            break;
                                        }

                                        if (Vector2.Distance(Projectile.Center, otherPlayer.Center) <= 200f && !restorationTargetSelected && !healingFrames)
                                        {
                                            if (!mPlayer.overHeaven && otherPlayer.whoAmI != player.whoAmI || mPlayer.overHeaven)
                                            {
                                                restorationTargetSelected = true;
                                                healingTargetPlayer = otherPlayer.whoAmI;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (!restorationTargetSelected)
                        {
                            SoundEngine.PlaySound(RestorationSound);
                            restorationEffectStartTimer += 180;
                            restoringObjects = true;
                            restoredEnemies = false;
                        }
                    }
                    if (restoringObjects)
                    {
                        if (restorationEffectStartTimer > 0)
                        {
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                NPC npc = Main.npc[n];
                                if (npc.active && !npc.hide && !npc.immortal && npc.GetGlobalNPC<JoJoGlobalNPC>().taggedByCrazyDiamondRestoration && npc.GetGlobalNPC<JoJoGlobalNPC>().crazyDiamondPunchCount >= 7)
                                {
                                    int amountOfNPCDusts = Main.rand.Next(1, 4 + 1);
                                    for (int i = 0; i < amountOfNPCDusts; i++)
                                    {
                                        int index = Dust.NewDust(npc.position, npc.width, npc.height, DustID.IchorTorch, Scale: Main.rand.Next(8, 12) / 10f);
                                        Main.dust[index].noGravity = true;
                                        Main.dust[index].velocity = new Vector2(Main.rand.Next(-2, 2 + 1) / 10f, Main.rand.Next(-5, -2 + 1) / 10f);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!restoredEnemies)
                            {
                                restoredEnemies = true;
                                for (int n = 0; n < Main.maxNPCs; n++)
                                {
                                    NPC npc = Main.npc[n];
                                    if (npc.active && !npc.hide && !npc.immortal && npc.GetGlobalNPC<JoJoGlobalNPC>().taggedByCrazyDiamondRestoration && npc.GetGlobalNPC<JoJoGlobalNPC>().crazyDiamondPunchCount >= 7)
                                    {
                                        npc.defense = (int)(npc.defense * 0.9f);
                                        if (!npc.boss)
                                            npc.lifeMax = (int)(npc.lifeMax * 0.8f);
                                        else
                                            npc.lifeMax = (int)(npc.lifeMax * 0.95f);
                                        npc.life = npc.lifeMax;
                                        if (blindRage)
                                            npc.AddBuff(ModContent.BuffType<ImproperRestoration>(), 8 * 60);
                                    }
                                }
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(20));
                            }

                            if (tileRestorationTimer <= 0)
                            {
                                if (mPlayer.crazyDiamondDestroyedTileData.Count <= 0)
                                {
                                    restoringObjects = false;
                                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(10));
                                }
                                else
                                {
                                    tileRestorationTimer += 1;
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
            }
            if (restorationTargetSelected)
            {
                float offset = 0f;
                float offset2 = 0f;
                if (playerStandDirectionMismatch)
                    offset = -60f * Projectile.spriteDirection;
                if (Projectile.spriteDirection == -1)
                    offset2 = 24f;

                if (healingTargetNPC != -1)
                {
                    NPC npc = Main.npc[healingTargetNPC];
                    if (!returnToOwner)
                    {
                        if (!healingFrames)
                        {
                            if (npc.Center.X > Projectile.Center.X)
                                Projectile.spriteDirection = 1;
                            if (npc.Center.X < Projectile.Center.X)
                                Projectile.spriteDirection = -1;
                            Projectile.velocity = npc.Center - Projectile.Center;
                            Projectile.velocity.Normalize();
                            Projectile.velocity *= 6f;
                            if (Vector2.Distance(Projectile.Center, npc.Center) <= 20f)
                            {
                                if (Projectile.spriteDirection != player.direction)
                                    playerStandDirectionMismatch = true;
                                Projectile.frame = 0;
                                healingFrames = true;
                            }
                            Projectile.netUpdate = true;
                        }
                        else
                        {
                            currentAnimationState = AnimationState.Healing;
                            Projectile.position = new Vector2(npc.Center.X - 10f - offset - offset2, npc.Center.Y - 20f);
                            if (Projectile.frame == 0 && !healingFramesRepeatTimerOnlyOnce)
                            {
                                healingFramesRepeatTimer += 1;
                                healingFramesRepeatTimerOnlyOnce = true;
                            }
                            if (Projectile.frame != 0)
                                healingFramesRepeatTimerOnlyOnce = false;
                            if (healingFramesRepeatTimer >= 4)
                            {
                                playerStandDirectionMismatch = false;
                                healingFrames = false;
                                healingFramesRepeatTimerOnlyOnce = false;
                                restorationTargetSelected = false;
                                healingFramesRepeatTimer = 0;
                                int heal = npc.lifeMax - npc.life;
                                if (npc.HasBuff(ModContent.BuffType<MissingOrgans>()))
                                    heal = 0;
                                if (!blindRage)
                                {
                                    npc.AddBuff(ModContent.BuffType<Restoration>(), 360);
                                    if (npc.townNPC && heal > 0)
                                        npc.GetGlobalNPC<JoJoGlobalNPC>().crazyDiamondFullHealth = true;
                                    else
                                        npc.lifeMax = npc.life;
                                }
                                else
                                {
                                    npc.AddBuff(ModContent.BuffType<ImproperRestoration>(), 15 * 60);
                                }
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(20));
                            }
                            Projectile.netUpdate = true;
                        }
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
                    if (!returnToOwner)
                    {
                        if (!healingFrames)
                        {
                            if (otherPlayer.Center.X > Projectile.Center.X)
                                Projectile.spriteDirection = 1;
                            else
                                Projectile.spriteDirection = -1;
                            Projectile.velocity = otherPlayer.Center - Projectile.Center;
                            Projectile.velocity.Normalize();
                            Projectile.velocity *= 6f;
                            if (Vector2.Distance(Projectile.Center, otherPlayer.Center) <= 20f)
                            {
                                if (Projectile.spriteDirection != player.direction)
                                    playerStandDirectionMismatch = true;
                                Projectile.frame = 0;
                                healingFrames = true;
                            }
                            Projectile.netUpdate = true;
                        }
                        else
                        {
                            currentAnimationState = AnimationState.Healing;
                            Projectile.position = new Vector2(otherPlayer.Center.X - 10f - offset - offset2, otherPlayer.Center.Y - 20f);
                            if (Projectile.frame == 0)
                            {
                                if (healingFramesRepeatTimerOnlyOnce)
                                {
                                    healingFramesRepeatTimer += 1;
                                    healingFramesRepeatTimerOnlyOnce = true;
                                }
                            }
                            else
                                healingFramesRepeatTimerOnlyOnce = false;

                            if (healingFramesRepeatTimer >= 4)
                            {
                                playerStandDirectionMismatch = false;
                                healingFrames = false;
                                healingFramesRepeatTimerOnlyOnce = false;
                                restorationTargetSelected = false;
                                healingFramesRepeatTimer = 0;
                                int healthValue = otherPlayer.statLifeMax - otherPlayer.statLife;
                                if (otherPlayer.HasBuff(ModContent.BuffType<MissingOrgans>()))
                                    healthValue = 0;
                                if (blindRage)
                                {
                                    if (healthValue > 0)
                                        otherPlayer.Heal(healthValue);
                                    otherPlayer.AddBuff(ModContent.BuffType<Restoration>(), 360);
                                }
                                else
                                    player.AddBuff(ModContent.BuffType<ImproperRestoration>(), 10 * 60);

                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                            }
                            Projectile.netUpdate = true;
                        }
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
                    Projectile.velocity *= 6f + player.moveSpeed;
                    currentAnimationState = AnimationState.Idle;
                    if (Vector2.Distance(Projectile.Center, player.Center) <= 20f)
                    {
                        returnToOwner = false;
                        playerStandDirectionMismatch = false;
                        healingFrames = false;
                        healingFramesRepeatTimerOnlyOnce = false;
                        restorationTargetSelected = false;
                        healingFramesRepeatTimer = 0;
                    }
                    Projectile.netUpdate = true;
                }
                if (!restorationTargetSelected)
                {
                    healingTargetNPC = -1;
                    healingTargetPlayer = -1;
                }
            }
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto && !restorationTargetSelected)
            {
                returnToOwner = false;
                healingFrames = false;
                restorationTargetSelected = false;
                healingFramesRepeatTimer = 0;
                healingFramesRepeatTimerOnlyOnce = false;
                BasicPunchAI();
            }
            if (player.teleporting)
                Projectile.position = player.position;
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
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
            else if (currentAnimationState == AnimationState.Healing)
                PlayAnimation("Heal");
            else if (currentAnimationState == AnimationState.Pose)
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
            string pathAddition = "";
            if (restorationMode)
                pathAddition = "Restoration_";

            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/CrazyDiamond", "CrazyDiamond_" + pathAddition + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Flick")
                AnimateStand(animationName, 4, 10, false);
            else if (animationName == "Pose")
                AnimateStand(animationName, 4, 10, true);
            else if (animationName == "Heal")
                AnimateStand(animationName, 4, 12, true);
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(healingFrames);
            writer.Write(flickFrames);
            writer.Write(blindRage);
            writer.Write(returnToOwner);
            writer.Write(restorationMode);
            writer.Write(restorationTargetSelected);
            writer.Write(healingTargetNPC);
            writer.Write(healingTargetPlayer);
            writer.Write(restorationEffectStartTimer);
            writer.Write(healingFramesRepeatTimer);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            healingFrames = reader.ReadBoolean();
            flickFrames = reader.ReadBoolean();
            blindRage = reader.ReadBoolean();
            returnToOwner = reader.ReadBoolean();
            restorationMode = reader.ReadBoolean();
            restorationTargetSelected = reader.ReadBoolean();
            healingTargetNPC = reader.ReadInt32();
            healingTargetPlayer = reader.ReadInt32();
            restorationEffectStartTimer = reader.ReadInt32();
            healingFramesRepeatTimer = reader.ReadInt32();
        }
    }
}