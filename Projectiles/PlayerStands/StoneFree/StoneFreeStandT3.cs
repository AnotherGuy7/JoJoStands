using JoJoStands.Buffs.Debuffs;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.StoneFree
{
    public class StoneFreeStandT3 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 10;
        }

        public override float maxDistance => 98f;
        public override int punchDamage => 65;
        public override int punchTime => 9;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 0f;
        /*public override string punchSoundName => "Ora";
        public override string poseSoundName => "YareYareDaze";
        public override string spawnSoundName => "Star Platinum";*/
        public override int standType => 1;

        private int updateTimer = 0;
        private bool stringConnectorPlaced = false;
        private Vector2 firstStringPos;
        private bool extendedBarrage = false;
        private bool holdingStringNPC = false;      //Holding an NPC via string

        private const int ExtendedBarrageAbility = 0;
        private const int StringTraps = 1;
        private const int Bind = 2;
        private const int TiedTogetherAbility = 3;
        private const float MaxTrapDistance = 35f * 16f;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                Projectile.netUpdate = true;
            }
            

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && !holdingStringNPC && Projectile.owner == Main.myPlayer)
                {
                    float lifeTimeMultiplier = 1f;
                    if (extendedBarrage)
                    {
                        newPunchDamage = (int)(newPunchDamage * 0.92f);
                        lifeTimeMultiplier = 1.8f;
                    }
                    Punch(punchLifeTimeMultiplier: lifeTimeMultiplier);
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer && shootCount <= 0 && !playerHasAbilityCooldown)
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
                                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<StoneFreeStringConnector>(), 0, 0f, player.whoAmI, stringPointIndex, punchDamage + 29 * (int)mPlayer.standDamageBoosts);
                            }
                        }
                    }
                    else if (mPlayer.chosenAbility == TiedTogetherAbility && !holdingStringNPC)
                    {
                        holdingStringNPC = true;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        shootVel.Normalize();
                        shootVel *= 12f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StoneFreeTiedTogetherString>(), 4, 0f, player.whoAmI, Projectile.whoAmI);
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(8));
                    }
                }
                holdingStringNPC = player.ownedProjectileCounts[ModContent.ProjectileType<StoneFreeTiedTogetherString>()] > 0;
                if (holdingStringNPC)
                    GoInFront();

                if (SpecialKeyPressed())
                {
                    if (mPlayer.chosenAbility == ExtendedBarrageAbility)
                        extendedBarrage = !extendedBarrage;
                    else if (mPlayer.chosenAbility == Bind)
                    {
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        shootVel.Normalize();
                        shootVel *= 12f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StoneFreeBindString>(), 4, 0f, player.whoAmI, Projectile.whoAmI, 12);
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(8));
                    }
                }

                if (SecondSpecialKeyPressedNoCooldown())
                {
                    if (!StoneFreeAbilityWheel.visible)
                        StoneFreeAbilityWheel.OpenAbilityWheel(mPlayer, 4);
                    else
                        StoneFreeAbilityWheel.CloseAbilityWheel();
                }

                if (!attackFrames)
                {
                    StayBehind();
                }
            }
            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
            }
        }

        public override bool PreKill(int timeLeft)
        {
            StoneFreeAbilityWheel.CloseAbilityWheel();
            return true;
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                idleFrames = false;
                if (!extendedBarrage)
                    PlayAnimation("Attack");
                else
                    PlayAnimation("ExtendedAttack");
            }
            if (idleFrames)
            {
                PlayAnimation("Idle");
            }
            if (holdingStringNPC)
            {
                PlayAnimation("StringHold");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/StoneFree/StoneFree_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "ExtendedAttack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "StringHold")
            {
                AnimateStand(animationName, 1, 40, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 2, 12, true);
            }
        }
    }
}