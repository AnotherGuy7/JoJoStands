using JoJoStands.Buffs.Debuffs;
using JoJoStands.Projectiles.Minions;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.GoldExperience
{
    public class GoldExperienceStandT2 : StandClass
    {
        public override int PunchDamage => 41;
        public override int PunchTime => 13;
        public override int HalfStandHeight => 35;
        public override int FistID => 2;
        public override int TierNumber => 2;
        public override Vector2 StandOffset => new Vector2(17, 0);
        public override string PunchSoundName => "GER_Muda";
        public override string PoseSoundName => "TheresADreamInMyHeart";
        public override string SpawnSoundName => "Gold Experience";
        public override int AmountOfPunchVariants => 2;
        public override string PunchTexturePath => "JoJoStands/Projectiles/PlayerStands/GoldExperience/GoldExperience_Punch_";
        public override Vector2 PunchSize => new Vector2(18, 10);
        public override StandAttackType StandType => StandAttackType.Melee;

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

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {

                if (!attacking)
                    StayBehind();

                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft)
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch();
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }

                    if (SpecialKeyPressed(false))
                    {
                        if (!GoldExperienceAbilityWheel.Visible)
                            GoldExperienceAbilityWheel.OpenAbilityWheel(mPlayer, 2);
                        else
                            GoldExperienceAbilityWheel.CloseAbilityWheel();
                    }

                    float mouseDistance = Vector2.Distance(Main.MouseWorld, player.Center);
                    bool mouseOnPlatform = TileID.Sets.Platforms[Main.tile[(int)(Main.MouseWorld.X / 16f), (int)(Main.MouseWorld.Y / 16f)].TileType];
                    if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                    {
                        if (mPlayer.chosenAbility == 0)
                        {
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<GEFrog>(), 1, 0f, Projectile.owner, TierNumber, TierNumber - 1f);
                            Main.projectile[projIndex].netUpdate = true;
                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(6));
                        }
                        if (mPlayer.chosenAbility == 1 && mouseDistance < MaxDistance && (Collision.SolidCollision(Main.MouseWorld, 1, 1) || mouseOnPlatform) && !Collision.SolidCollision(Main.MouseWorld - new Vector2(0f, 16f), 1, 1) && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                        {
                            int yPos = (((int)Main.MouseWorld.Y / 16) - 3) * 16;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.MouseWorld.X, yPos, 0f, 0f, ModContent.ProjectileType<GETree>(), 1, 0f, Projectile.owner, TierNumber);
                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(12));
                        }
                    }
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        public override bool PreKill(int timeLeft)
        {
            GoldExperienceAbilityWheel.CloseAbilityWheel();
            return true;
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
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/GoldExperience/GoldExperience_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 30, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 12, true);
        }
    }
}