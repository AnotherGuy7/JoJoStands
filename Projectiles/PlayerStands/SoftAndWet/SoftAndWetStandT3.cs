using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Items.CraftingMaterials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.SoftAndWet
{
    public class SoftAndWetStandT3 : StandClass
    {
        public override int PunchDamage => 63;
        public override int PunchTime => 10;
        public override int HalfStandHeight => 38;
        public override int AltDamage => ((int)(TierNumber * 15));
        public override int StandOffset => 54;
        public override int FistWhoAmI => 0;
        public override int TierNumber => 3;
        public override StandAttackType StandType => StandAttackType.Melee;

        public bool bubbleBarrier = false;

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
                secondaryAbilityFrames = player.ownedProjectileCounts[ModContent.ProjectileType<PlunderBubble>()] != 0;
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    StayBehindWithAbility();
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    GoInFront();
                    if (shootCount <= 0)
                    {
                        shootCount += 36;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        SoundEngine.PlaySound(SoundID.Item85);
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);
                        shootVel.Normalize();
                        shootVel *= 3f;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<PlunderBubble>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, Projectile.owner, GetPlunderBubbleType());
                        shootCount += 1;
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                }
                if (SecondSpecialKeyPressed() && Projectile.owner == Main.myPlayer && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                    {
                    player.AddBuff(ModContent.BuffType<BarrierBuff>(), 600);
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(22));
                    Vector2 playerFollow = Vector2.Zero;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, playerFollow,ModContent.ProjectileType<BubbleBarrier>(), 0, 0f, Projectile.owner, Projectile.whoAmI);
                }
                if (attackFrames == true && Main.rand.NextBool(37))
                {
                    SoundEngine.PlaySound(SoundID.Drip);
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);
                    shootVel.Normalize();
                    shootVel *= 3f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<TinyBubble>(), 35, 2f, Projectile.owner, Projectile.whoAmI); 
                }

                if (attackFrames == true && Main.rand.NextBool(38))
                {
                    Vector2 shootVel = Main.MouseWorld - Projectile.position;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);
                    shootVel.Normalize();
                    shootVel *= 3f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, shootVel, ModContent.ProjectileType<TinyBubble>(), 35, 2f, Projectile.owner, Projectile.whoAmI);
                }

            }
            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
            }
        }

        public int GetPlunderBubbleType()
        {
            Player player = Main.player[Projectile.owner];
            if (player.HeldItem.type == ItemID.Torch)
                return PlunderBubble.Plunder_Fire;
            if (player.HeldItem.type == ItemID.IchorTorch)
                return PlunderBubble.Plunder_Ichor;
            if (player.HeldItem.type == ItemID.CursedTorch)
                return PlunderBubble.Plunder_Cursed;
            if (player.HeldItem.type == ItemID.IceTorch)
                return PlunderBubble.Plunder_Ice;
            if (player.HeldItem.type == ModContent.ItemType<ViralPowder>())
                return PlunderBubble.Plunder_Viral;

            return PlunderBubble.Plunder_None;
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
                PlayAnimation("Idle");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().posing)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SoftAndWet/SoftAndWet_" + animationName);

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
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}