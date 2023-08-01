using JoJoStands.Buffs.Debuffs;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.StoneFree
{
    public class StoneFreeStandFinal : StandClass
    {
        public override float MaxDistance => 98f;
        public override int PunchDamage => 65;
        public override int PunchTime => 10;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 0;
        public override int TierNumber => 4;
        public override string PunchSoundName => "StoneFree_Ora";
        public override string PoseSoundName => "StoneFree";
        public override string SpawnSoundName => "Stone Free";
        public override int AmountOfPunchVariants => 2;
        public override string PunchTexturePath => "JoJoStands/Projectiles/PlayerStands/StoneFree/StoneFree_Punch_";
        public override Vector2 PunchSize => new Vector2(32, 8);
        public override PunchSpawnData PunchData => new PunchSpawnData()
        {
            standardPunchOffset = new Vector2(6f, 0f),
            minimumLifeTime = 5,
            maximumLifeTime = 12,
            minimumTravelDistance = 20,
            maximumTravelDistance = 48,
            bonusAfterimageAmount = 0
        };
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private bool stringConnectorPlaced = false;
        private Vector2 firstStringPos;
        private bool extendedBarrage = false;
        private bool holdingStringNPC = false;      //Holding an NPC via string
        private int heldStringProjectileIndex;

        private const int ExtendedBarrageAbility = 0;
        private const int StringTraps = 1;
        private const int Bind = 2;
        private const int TiedTogetherAbility = 3;
        private const int WeaveShield = 4;
        private const float MaxTrapDistance = 35f * 16f;

        public new enum AnimationState
        {
            Idle,
            Attack,
            ExtendedBarrage,
            StringHold,
            Pose
        }

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
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !holdingStringNPC)
                    {
                        currentAnimationState = AnimationState.Attack;
                        float lifeTimeMultiplier = 1f;
                        if (extendedBarrage)
                        {
                            currentAnimationState = AnimationState.ExtendedBarrage;
                            newPunchDamage = (int)(newPunchDamage * 0.92f);
                            lifeTimeMultiplier = 1.8f;
                        }
                        Punch(punchLifeTimeMultiplier: lifeTimeMultiplier);
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }

                    if (Main.mouseRight && !mPlayer.stoneFreeWeaveAbilityActive && shootCount <= 0 && !playerHasAbilityCooldown)
                    {
                        if (mPlayer.chosenAbility == StringTraps)
                        {
                            shootCount += 30;
                            if (!stringConnectorPlaced)
                            {
                                if (Collision.SolidTiles((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, (int)Main.MouseWorld.Y / 16))
                                {
                                    stringConnectorPlaced = true;
                                    firstStringPos = Main.MouseWorld;
                                }
                            }
                            else
                            {
                                if (Collision.SolidTiles((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, (int)Main.MouseWorld.Y / 16))
                                {
                                    if (Vector2.Distance(firstStringPos, Main.MouseWorld) >= MaxTrapDistance)
                                    {
                                        stringConnectorPlaced = false;
                                        Main.NewText("Your strings do not extend that far.");
                                        return;
                                    }

                                    stringConnectorPlaced = false;
                                    int stringPointIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), firstStringPos, Vector2.Zero, ModContent.ProjectileType<StoneFreeStringPoint>(), 0, 0f, player.whoAmI);
                                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<StoneFreeStringConnector>(), 0, 0f, player.whoAmI, stringPointIndex, PunchDamage + 29 * (int)mPlayer.standDamageBoosts);
                                }
                            }
                        }
                        else if (mPlayer.chosenAbility == TiedTogetherAbility && !holdingStringNPC)
                        {
                            holdingStringNPC = true;
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            shootVel.Normalize();
                            shootVel *= 12f;
                            heldStringProjectileIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StoneFreeTiedTogetherString>(), 4, 0f, player.whoAmI, Projectile.whoAmI);
                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(4));
                        }
                    }
                }
                holdingStringNPC = player.ownedProjectileCounts[ModContent.ProjectileType<StoneFreeTiedTogetherString>()] > 0;
                if (holdingStringNPC)
                {
                    currentAnimationState = AnimationState.StringHold;
                    int direction = 1;
                    if (Main.projectile[heldStringProjectileIndex].active)
                    {
                        float xDifference = player.Center.X - Main.projectile[heldStringProjectileIndex].Center.X;
                        if (xDifference > 0)
                            direction = -1;
                        else
                            direction = 1;
                    }
                    GoInFront(direction);
                    Projectile.spriteDirection = Projectile.direction = direction;
                }

                if (SpecialKeyPressed())
                {
                    if (mPlayer.chosenAbility == ExtendedBarrageAbility && !mPlayer.stoneFreeWeaveAbilityActive)
                    {
                        extendedBarrage = !extendedBarrage;
                        Projectile.netUpdate = true;
                    }
                    else if (mPlayer.chosenAbility == Bind && !mPlayer.stoneFreeWeaveAbilityActive)
                    {
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        shootVel.Normalize();
                        shootVel *= 12f;
                        int projectileIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StoneFreeBindString>(), 4, 0f, player.whoAmI, Projectile.whoAmI, 18);
                        Main.projectile[projectileIndex].netUpdate = true;
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
                    }

                    if (mPlayer.chosenAbility == WeaveShield)
                    {
                        mPlayer.stoneFreeWeaveAbilityActive = !mPlayer.stoneFreeWeaveAbilityActive;
                        if (mPlayer.stoneFreeWeaveAbilityActive)
                            player.AddBuff(ModContent.BuffType<Buffs.ItemBuff.Weave>(), 2);
                        Projectile.netUpdate = true;
                    }
                }

                if (SecondSpecialKeyPressed(false))
                {
                    if (!StoneFreeAbilityWheel.Visible)
                        StoneFreeAbilityWheel.OpenAbilityWheel(mPlayer, 5);
                    else
                        StoneFreeAbilityWheel.CloseAbilityWheel();
                }

                if (!attacking && !holdingStringNPC)
                    StayBehind();
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(extendedBarrage);
            writer.Write(Main.player[Projectile.owner].GetModPlayer<MyPlayer>().stoneFreeWeaveAbilityActive);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            extendedBarrage = reader.ReadBoolean();
            Main.player[Projectile.owner].GetModPlayer<MyPlayer>().stoneFreeWeaveAbilityActive = reader.ReadBoolean();
        }

        public override bool PreKill(int timeLeft)
        {
            Main.player[Projectile.owner].GetModPlayer<MyPlayer>().stoneFreeWeaveAbilityActive = false;
            StoneFreeAbilityWheel.CloseAbilityWheel();
            return true;
        }

        public override byte SendAnimationState() => (byte)currentAnimationState;
        public override void ReceiveAnimationState(byte state) => currentAnimationState = (AnimationState)state;

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
            else if (currentAnimationState == AnimationState.ExtendedBarrage)
                PlayAnimation("ExtendedAttack");
            else if (currentAnimationState == AnimationState.StringHold)
                PlayAnimation("StringHold");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/StoneFree/StoneFree_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "ExtendedAttack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "StringHold")
                AnimateStand(animationName, 1, 40, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 60, true);
        }
    }
}