using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.SilverChariot
{
    public class SilverChariotStandT2 : StandClass
    {
        public override float MaxDistance => 98f;
        public override int PunchDamage => 34;
        public override int PunchTime => 7;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 10;
        public override int TierNumber => 2;
        public override string SpawnSoundName => "Silver Chariot";
        public override StandAttackType StandType => StandAttackType.Melee;

        private bool parryFrames = false;

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
                if (Main.mouseLeft && !secondaryAbilityFrames && Projectile.owner == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        idleFrames = true;
                        attackFrames = false;
                    }
                }
                if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && !attackFrames && Projectile.owner == Main.myPlayer)
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
                            if (parryRectangle.Intersects(otherProj.Hitbox) && otherProj.type != Projectile.type && !otherProj.friendly)
                            {
                                parryFrames = true;
                                secondaryAbilityFrames = false;
                                otherProj.owner = Projectile.owner;
                                otherProj.damage *= 2;
                                otherProj.velocity *= -1;
                                otherProj.hostile = false;
                                otherProj.friendly = true;
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(10));
                            }
                        }
                    }
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active)
                        {
                            if (!npc.townNPC && !npc.friendly && !npc.immortal && !npc.hide && parryRectangle.Intersects(npc.Hitbox))
                            {
                                npc.StrikeNPC(npc.damage * 2, 6f, player.direction);
                                secondaryAbilityFrames = false;
                                parryFrames = true;
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(10));
                            }
                        }
                    }
                }
                if (!Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    secondaryAbilityFrames = false;
                }
                if (!attackFrames && !parryFrames)
                {
                    if (!secondaryAbilityFrames)
                        StayBehind();
                    else
                        GoInFront();
                }
            }
            if (mPlayer.standAutoMode)
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
                Main.NewText("G");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SilverChariot/SilverChariot_" + animationName);

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
    }
}