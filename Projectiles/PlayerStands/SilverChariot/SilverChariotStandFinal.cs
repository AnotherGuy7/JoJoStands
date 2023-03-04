using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using JoJoStands.Networking;

namespace JoJoStands.Projectiles.PlayerStands.SilverChariot
{
    public class SilverChariotStandFinal : StandClass
    {
        public override float MaxDistance => 98f;
        public override int PunchDamage => 76;
        public override int PunchTime => 7;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 10;
        public override int TierNumber => 4;
        public override string SpawnSoundName => "Silver Chariot";
        public override StandAttackType StandType => StandAttackType.Melee;
        private const int AfterImagesLimit = 5;

        private bool parryFrames = false;
        private bool Shirtless = false;
        private float punchMovementSpeed = 5f;

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

            mPlayer.silverChariotShirtless = Shirtless;

            if (secondaryAbilityFrames || parryFrames)
            {
                if (mouseX > player.position.X)
                    player.direction = 1;
                if (mouseX < player.position.X)
                    player.direction = -1;
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Main.mouseLeft && !secondaryAbilityFrames && Projectile.owner == Main.myPlayer)
                {
                    Punch(punchMovementSpeed);
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        idleFrames = true;
                        attackFrames = false;
                    }
                }
                if (Main.mouseRight && !attackFrames && Projectile.owner == Main.myPlayer)
                {
                    idleFrames = false;
                    attackFrames = false;
                    secondaryAbilityFrames = true;
                    Projectile.netUpdate = true;
                    Rectangle parryRectangle = new Rectangle((int)Projectile.Center.X + (4 * Projectile.direction), (int)Projectile.Center.Y - 29, 16, 54);
                    for (int p = 0; p < Main.maxProjectiles; p++)
                    {
                        Projectile otherProj = Main.projectile[p];
                        if (otherProj.active)
                        {
                            if (parryRectangle.Intersects(otherProj.Hitbox) && otherProj.type != Projectile.type && !otherProj.friendly && !otherProj.GetGlobalProjectile<JoJoGlobalProjectile>().exceptionForSCParry)
                            {
                                parryFrames = true;
                                secondaryAbilityFrames = false;
                                otherProj.owner = Projectile.owner;
                                otherProj.damage += (int)(otherProj.damage * mPlayer.standDamageBoosts) - otherProj.damage;
                                otherProj.damage *= 2;
                                otherProj.velocity *= -1;
                                otherProj.hostile = false;
                                otherProj.friendly = true;
                                SoundStyle npcHit4 = SoundID.NPCHit4;
                                npcHit4.Pitch = Main.rand.Next(4, 6 + 1) / 10f;
                                SoundEngine.PlaySound(npcHit4, Projectile.Center);
                                SyncCall.SyncStandEffectInfo(player.whoAmI, otherProj.whoAmI, 10, 1);
                            }
                        }
                    }
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                        {
                            if (!npc.townNPC && !npc.friendly && !npc.immortal && !npc.hide && parryRectangle.Intersects(npc.Hitbox))
                            {
                                int damage = (int)(npc.damage * 2 * mPlayer.standDamageBoosts);
                                npc.StrikeNPC(damage, 6f, player.direction);
                                SyncCall.SyncStandEffectInfo(player.whoAmI, npc.whoAmI, 10, 2, damage, player.direction);
                                secondaryAbilityFrames = false;
                                parryFrames = true;
                                SoundStyle npcHit4 = SoundID.NPCHit4;
                                npcHit4.Pitch = Main.rand.Next(4, 6 + 1) / 10f;
                                SoundEngine.PlaySound(npcHit4, Projectile.Center);
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(3));
                            }
                        }
                    }
                }
                if (!Main.mouseRight && Projectile.owner == Main.myPlayer)
                    secondaryAbilityFrames = false;
                if (!attackFrames && !parryFrames)
                {
                    if (!secondaryAbilityFrames)
                        StayBehind();
                    else
                        GoInFront();
                }

                if (SpecialKeyPressed())
                {
                    Shirtless = !Shirtless;
                    if (Shirtless)
                    {
                        punchMovementSpeed = 7.5f;
                        if (!player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && player.ownedProjectileCounts[ModContent.ProjectileType<SilverChariotAfterImage>()] == 0)
                        {
                            for (int i = 0; i < AfterImagesLimit; i++)
                            {
                                int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<SilverChariotAfterImage>(), 0, 0f, Projectile.owner, i, AfterImagesLimit);
                                Main.projectile[projIndex].netUpdate = true;
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
                            }
                        }
                    }
                    else
                    {
                        punchMovementSpeed = 5f;
                    }
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
        }

        public override void SelectAnimation()
        {
            if (parryFrames)
            {
                idleFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Parry");
            }
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
            if (secondaryAbilityFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
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
            if (animationName == "Parry")
            {
                idleFrames = true;
                parryFrames = false;
            }
            if (animationName == "ShirtlessParry")
            {
                idleFrames = true;
                parryFrames = false;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            string pathAddition = "";
            if (Shirtless)
                pathAddition = "Shirtless_";

            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SilverChariot/SilverChariot_" + pathAddition + animationName);

            if (!Shirtless)
            {
                if (animationName == "Idle")
                {
                    AnimateStand(animationName, 4, 30, true);
                }
                if (animationName == "Attack")
                {
                    AnimateStand(animationName, 5, newPunchTime, true);
                }
                if (animationName == "Secondary")
                {
                    AnimateStand(animationName, 1, 1, true);
                }
                if (animationName == "Parry")
                {
                    AnimateStand(animationName, 6, 3, false);
                }
                if (animationName == "Pose")
                {
                    AnimateStand(animationName, 1, 10, true);
                }
            }
            else
            {
                if (animationName == "Idle")
                {
                    AnimateStand("Shirtless" + animationName, 4, 30, true);
                }
                if (animationName == "Attack")
                {
                    AnimateStand("Shirtless" + animationName, 5, newPunchTime, true);
                }
                if (animationName == "Secondary")
                {
                    AnimateStand("Shirtless" + animationName, 1, 1, true);
                }
                if (animationName == "Parry")
                {
                    AnimateStand("Shirtless" + animationName, 6, 3, false);
                }
                if (animationName == "Pose")
                {
                    AnimateStand(animationName, 1, 10, true);
                }
            }
        }
        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(parryFrames);
            writer.Write(Shirtless);
        }
        public override void ReceiveExtraStates(BinaryReader reader)
        {
            parryFrames = reader.ReadBoolean();
            Shirtless = reader.ReadBoolean();
        }
    }
}