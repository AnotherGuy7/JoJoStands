using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Networking;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Echoes
{
    public class EchoesStandFinal : StandClass
    {
        public override int PunchDamage => 68;
        public override int PunchTime => 10;
        public override int HalfStandHeight => 28;
        public override int FistWhoAmI => 15;
        public override int TierNumber => 4;
        public override string PoseSoundName => "EchoesAct3";
        public override string SpawnSoundName => "Echoes Act 3";
        public override Vector2 StandOffset => new Vector2(10, 0);
        public override Vector2 ManualIdleHoverOffset => new Vector2(0, -10);
        public override StandAttackType StandType => StandAttackType.Melee;

        private const int ActNumber = 3;
        private int targetPlayer = -1;
        private int targetNPC = -1;
        private int actChangeCooldown = 30;

        private bool threeFreeze = false;
        private bool returnToPlayer = false;
        private bool changeACT = false;
        private bool playedThreeFreezeSound = false;
        private bool playedThreeFreezeThudSound = false;
        private bool threeFreezeTargetFound = false;

        public override void ExtraSpawnEffects()
        {
            if (Projectile.ai[0] == 2f)
                returnToPlayer = true;
            idleFrames = true;
        }

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;
            if (actChangeCooldown > 0)
                actChangeCooldown--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            mPlayer.currentEchoesAct = ActNumber;
            Rectangle rectangle = Rectangle.Empty;
            if (Projectile.owner == player.whoAmI)
                rectangle = new Rectangle((int)(Main.MouseWorld.X - 10), (int)(Main.MouseWorld.Y - 10), 20, 20);

            if (threeFreeze)
            {
                if (mouseX > player.position.X)
                    player.direction = 1;
                else
                    player.direction = -1;
            }
            if (!Main.mouseRight && Projectile.owner == Main.myPlayer)
                threeFreeze = false;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual && !returnToPlayer)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !threeFreeze && !mPlayer.posing)
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
                if (threeFreeze)
                    GoInFront();
                else
                {
                    playedThreeFreezeSound = false;
                    playedThreeFreezeThudSound = false;
                }

                if (Main.mouseRight && Projectile.owner == Main.myPlayer && !mPlayer.posing && !attackFrames) //3freeze activation
                {
                    threeFreeze = true;
                    Projectile.frame = 0;
                    bool enemyAffectedByThreeFreeze = false;
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !npc.hide && !npc.immortal)
                        {
                            if (npc.Hitbox.Intersects(rectangle) && Vector2.Distance(Projectile.Center, npc.Center) <= 250f)
                            {
                                enemyAffectedByThreeFreeze = true;
                                if (npc.GetGlobalNPC<JoJoGlobalNPC>().echoesThreeFreezeTimer <= 15)
                                {
                                    threeFreezeTargetFound = true;
                                    targetNPC = npc.whoAmI;
                                }
                                break;
                            }
                        }
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient && !enemyAffectedByThreeFreeze)
                    {
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player otherPlayer = Main.player[p];
                            if (otherPlayer.active && otherPlayer.hostile && player.hostile && player.InOpposingTeam(Main.player[otherPlayer.whoAmI]))
                            {
                                if (otherPlayer.Hitbox.Intersects(rectangle) && Vector2.Distance(Projectile.Center, otherPlayer.Center) <= 250f && otherPlayer.whoAmI != player.whoAmI)
                                {
                                    threeFreezeTargetFound = true;
                                    targetPlayer = otherPlayer.whoAmI;
                                    enemyAffectedByThreeFreeze = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (JoJoStands.SoundsLoaded && !playedThreeFreezeSound)
                    {
                        playedThreeFreezeSound = true;
                        SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/EchoesActThreeFreeze").WithVolumeScale(JoJoStands.ModSoundsVolume));
                    }
                    if (JoJoStands.SoundsLoaded && !playedThreeFreezeThudSound && enemyAffectedByThreeFreeze)
                        SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/EchoesActThreeFreeze_Thud").WithVolumeScale(JoJoStands.ModSoundsVolume));
                    playedThreeFreezeThudSound = enemyAffectedByThreeFreeze;
                }
                if (SpecialKeyPressed() && Projectile.owner == Main.myPlayer)       //3freeze barrgage activation
                {
                    player.AddBuff(ModContent.BuffType<ThreeFreezeBarrage>(), 10 * 60);
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                }
                if (SecondSpecialKeyPressed(false) && actChangeCooldown <= 0 && Projectile.owner == Main.myPlayer)
                {
                    changeACT = true;
                    Projectile.Kill();
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto && !returnToPlayer)
            {
                BasicPunchAI();
            }

            if (threeFreezeTargetFound)      //3freeze
            {
                threeFreezeTargetFound = false;
                if (targetNPC != -1)
                {
                    NPC npc = Main.npc[targetNPC];
                    npc.GetGlobalNPC<JoJoGlobalNPC>().echoesFreezeTarget = true;
                    npc.GetGlobalNPC<JoJoGlobalNPC>().echoesCrit = mPlayer.standCritChangeBoosts;
                    npc.GetGlobalNPC<JoJoGlobalNPC>().echoesDamageBoost = mPlayer.standDamageBoosts;
                    if (npc.GetGlobalNPC<JoJoGlobalNPC>().echoesThreeFreezeTimer <= 15)
                        npc.GetGlobalNPC<JoJoGlobalNPC>().echoesThreeFreezeTimer += 30;
                    SyncCall.SyncStandEffectInfo(player.whoAmI, targetNPC, 15, 3, 0, 0, mPlayer.standCritChangeBoosts, mPlayer.standDamageBoosts);
                }
                else if (targetPlayer != -1)
                {
                    Player otherPlayer = Main.player[targetPlayer];
                    MyPlayer mOtherPlayer = otherPlayer.GetModPlayer<MyPlayer>();
                    if (mOtherPlayer.echoesFreeze <= 15)
                    {
                        mOtherPlayer.echoesDamageBoost = mPlayer.standDamageBoosts;
                        mOtherPlayer.echoesFreeze += 30;
                        SyncCall.SyncOtherPlayerExtraEffect(player.whoAmI, otherPlayer.whoAmI, 1, 0, 0, mPlayer.standDamageBoosts, 0f);
                    }
                }
                targetPlayer = -1;
                targetNPC = -1;
            }

            if (Projectile.Distance(player.Center) >= newMaxDistance + 10f && !returnToPlayer) //if suddenly stand is too far
                returnToPlayer = true;
            if (Projectile.Distance(player.Center) <= 20f)
                returnToPlayer = false;

            if (returnToPlayer)
            {
                Projectile.tileCollide = false;
                if (Projectile.velocity.X > 0)
                    Projectile.spriteDirection = 1;
                if (Projectile.velocity.X < 0)
                    Projectile.spriteDirection = -1;
                if (Projectile.owner == Main.myPlayer)
                {
                    if (player.Center == Projectile.Center)
                        return;

                    Projectile.velocity = player.Center - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 8f + player.moveSpeed * 2;
                }
                Projectile.netUpdate = true;
            }

            if (player.teleporting)
                Projectile.position = player.position;
        }

        public override void StandKillEffects()
        {
            Player player = Main.player[Projectile.owner];
            float remoteModeOnSpawn = 0f;
            if (Projectile.Distance(player.Center) >= newMaxDistance + 10f)
                remoteModeOnSpawn = 2f;
            if (changeACT)
            {
                player.maxMinions += 1;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EchoesStandT2>(), 0, 0f, Main.myPlayer, remoteModeOnSpawn);
            }
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
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().posing)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
            if (threeFreeze)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("ThreeFreeze");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/Echoes", "/EchoesAct3_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 10, true);
            }
            if (animationName == "ThreeFreeze")
            {
                AnimateStand(animationName, 1, 10, true);
            }
        }
        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(targetNPC);
            writer.Write(targetPlayer);

            writer.Write(threeFreeze);
            writer.Write(returnToPlayer);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            targetNPC = reader.ReadInt32();
            targetPlayer = reader.ReadInt32();
            threeFreezeTargetFound = targetNPC != -1 || targetPlayer != -1 ? true : false;

            threeFreeze = reader.ReadBoolean();
            returnToPlayer = reader.ReadBoolean();
        }
    }
}