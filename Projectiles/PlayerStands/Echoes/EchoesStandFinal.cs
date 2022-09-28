using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using JoJoStands.NPCs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.Debuffs;

namespace JoJoStands.Projectiles.PlayerStands.Echoes
{
    public class EchoesStandFinal : StandClass
    {
        public override int PunchDamage => 64;
        public override int PunchTime => 8;
        public override int HalfStandHeight => 28;
        public override int FistWhoAmI => 15;
        public override int TierNumber => 4;
        public override int StandOffset => 20;
        public override StandAttackType StandType => StandAttackType.Melee;

        private int ACT = 3;
        private bool threeFreeze = false;
        private int onlyOneTarget = 0;
        private int targetPlayer = -1;
        private int targetNPC = -1;
        private int crutch = 90;
        private float x1 = 0f;

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

            mPlayer.echoesACT = ACT;
            Rectangle rectangle = Rectangle.Empty;

            if (Projectile.owner == player.whoAmI)
                rectangle = new Rectangle((int)(Main.MouseWorld.X), (int)(Main.MouseWorld.Y), 20, 20);

            if (Projectile.owner == Main.myPlayer)
                x1 = Main.MouseWorld.X;

            if (threeFreeze)
            {
                if (x1 > player.position.X)
                    player.direction = 1;
                if (x1 < player.position.X)
                    player.direction = -1;
            }
            if (!Main.mouseRight && Projectile.owner == Main.myPlayer)
                threeFreeze = false;


            if (!mPlayer.standAutoMode)
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
                if (Main.mouseRight && Projectile.owner == Main.myPlayer && !mPlayer.posing && !attackFrames) //3freeze activation
                {
                    Projectile.frame = 0;
                    threeFreeze = true;
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !npc.hide && !npc.immortal)
                        {
                            if (npc.Hitbox.Intersects(rectangle) && Vector2.Distance(Projectile.Center, npc.Center) <= 200f && npc.GetGlobalNPC<JoJoGlobalNPC>().echoesFreeze <= 15 && onlyOneTarget < 1)
                            {
                                onlyOneTarget += 1;
                                targetNPC = npc.whoAmI;
                            }
                        }
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player otherPlayer = Main.player[p];
                            if (otherPlayer.active && otherPlayer.hostile && player.hostile && player.InOpposingTeam(Main.player[otherPlayer.whoAmI]) && onlyOneTarget < 1)
                            {
                                if (otherPlayer.Hitbox.Intersects(rectangle) && Vector2.Distance(Projectile.Center, otherPlayer.Center) <= 200f && otherPlayer.whoAmI != player.whoAmI)
                                {
                                    onlyOneTarget += 1;
                                    targetPlayer = otherPlayer.whoAmI;
                                }
                            }
                        }
                    }
                }
                if (SpecialKeyPressed() && Projectile.owner == Main.myPlayer) //3freeze barrgage activation
                {
                    int secretReference = 0;
                    if (MyPlayer.SecretReferences)
                        secretReference = 5;
                    if (Main.rand.NextFloat(1, 100 + 1) <= secretReference)
                        Main.NewText("My God... ACT 3  is such a downgrade. The ability to just pin someone to the ground and swear... I added this ability just to use the special bind! (C) Prooooooos21", Color.Magenta);
                    else
                        Main.NewText("Okay, master! Let's kill da ho! Beeetch!", Color.LightGreen);
                    player.AddBuff(ModContent.BuffType<ThreeFreezeBarrage>(), 600);
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/PoseSound"));
                }
            }
            if (onlyOneTarget > 0) //3freeze
            {
                if (targetNPC != -1)
                {
                    NPC npc = Main.npc[targetNPC];
                    npc.GetGlobalNPC<JoJoGlobalNPC>().echoesCrit = mPlayer.standCritChangeBoosts;
                    npc.GetGlobalNPC<JoJoGlobalNPC>().echoesDamageBoost = mPlayer.standDamageBoosts;
                    npc.GetGlobalNPC<JoJoGlobalNPC>().echoesFreeze += 30;
                    onlyOneTarget = 0;
                }
                if (targetPlayer != -1)
                {
                    Player otherPlayer = Main.player[targetPlayer];
                    MyPlayer mOtherPlayer = otherPlayer.GetModPlayer<MyPlayer>();
                    crutch--;
                    if (crutch <= 0)
                    {
                        onlyOneTarget = 0;
                        crutch = 90;
                    }
                    if (mOtherPlayer.echoesFreeze <= 15)
                    {
                        mOtherPlayer.echoesCrit = mPlayer.standCritChangeBoosts;
                        mOtherPlayer.echoesDamageBoost = mPlayer.standDamageBoosts;
                        mOtherPlayer.echoesFreeze += 30;
                    }
                }
            }

            if (onlyOneTarget == 0)
            {
                targetPlayer = -1;
                targetNPC = -1;
            }

            if (player.HasBuff(ModContent.BuffType<ThreeFreezeBarrage>())) //bassboosted punches v2 (3freeze barrage)
            {
                for (int f = 0; f < Main.maxProjectiles; f++) 
                {
                    Projectile proj = Main.projectile[f];
                    if (proj.type == ModContent.ProjectileType<Fists>() && proj.owner == player.whoAmI)
                    {
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active && !npc.hide && !npc.immortal && !npc.friendly && npc.Hitbox.Intersects(proj.Hitbox) && !proj.GetGlobalProjectile<JoJoGlobalProjectile>().onlyOnceforFists)
                            {
                                npc.GetGlobalNPC<JoJoGlobalNPC>().echoesCrit = mPlayer.standCritChangeBoosts;
                                npc.GetGlobalNPC<JoJoGlobalNPC>().echoesDamageBoost = mPlayer.standDamageBoosts;
                                if (npc.GetGlobalNPC<JoJoGlobalNPC>().echoesFreeze <= 15)
                                    npc.GetGlobalNPC<JoJoGlobalNPC>().echoesFreeze += 30;
                                proj.GetGlobalProjectile<JoJoGlobalProjectile>().onlyOnceforFists = true;
                            }
                        }
                        if (MyPlayer.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
                        {
                            for (int p = 0; p < Main.maxPlayers; p++)
                            {
                                {
                                    Player otherPlayer = Main.player[p];
                                    MyPlayer mOtherPlayer = otherPlayer.GetModPlayer<MyPlayer>();
                                    if (otherPlayer.active && otherPlayer.whoAmI != player.whoAmI && otherPlayer.hostile && player.hostile && player.InOpposingTeam(Main.player[otherPlayer.whoAmI]) && otherPlayer.Hitbox.Intersects(proj.Hitbox) && !proj.GetGlobalProjectile<JoJoGlobalProjectile>().onlyOnceforFists)
                                    {
                                        mOtherPlayer.echoesCrit = mPlayer.standCritChangeBoosts;
                                        mOtherPlayer.echoesDamageBoost = mPlayer.standDamageBoosts;
                                        if (mOtherPlayer.echoesFreeze <= 15)
                                            mOtherPlayer.echoesFreeze += 30;
                                        proj.GetGlobalProjectile<JoJoGlobalProjectile>().onlyOnceforFists = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
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
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();

            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/Echoes", "/EchoesACT3_" + animationName);

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
            writer.Write(ACT);
            writer.Write(onlyOneTarget);
            writer.Write(targetNPC);
            writer.Write(targetPlayer);
            writer.Write(crutch);
            writer.Write(threeFreeze);
            writer.Write(x1);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            ACT = reader.ReadInt32();
            onlyOneTarget = reader.ReadInt32();
            targetNPC = reader.ReadInt32();
            targetPlayer = reader.ReadInt32();
            crutch = reader.ReadInt32();
            threeFreeze = reader.ReadBoolean();
            x1 = reader.ReadSingle();
        }
    }
}