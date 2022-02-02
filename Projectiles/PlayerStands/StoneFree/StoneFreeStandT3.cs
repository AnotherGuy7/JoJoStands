using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.StoneFree
{
    public class StoneFreeStandT3 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
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

        private const int ExtendedBarrageAbility = 1;
        private const int TiedTogetherAbility = 4;
        private const float MaxTrapDistance = 35f * 16f;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }
            

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
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
                if (Main.mouseRight && projectile.owner == Main.myPlayer && shootCount <= 0)
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
                            int stringPointIndex = Projectile.NewProjectile(firstStringPos, Vector2.Zero, mod.ProjectileType("StoneFreeStringPoint"), 0, 0f, player.whoAmI);
                            Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, mod.ProjectileType("StoneFreeStringConnector"), 0, 0f, player.whoAmI, stringPointIndex, punchDamage + 29 * (int)mPlayer.standDamageBoosts);
                        }
                    }
                }
                if (SpecialKeyPressed() && mPlayer.chosenAbility == ExtendedBarrageAbility)
                    extendedBarrage = !extendedBarrage;
                if (SpecialKeyPressed() && mPlayer.chosenAbility == TiedTogetherAbility)
                {
                    
                }

                if (SecondSpecialKeyPressedNoCooldown())
                {
                    if (!StoneFreeAbilityWheel.visible)
                        StoneFreeAbilityWheel.OpenAbilityWheel(mPlayer);
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
                normalFrames = false;
                if (!extendedBarrage)
                    PlayAnimation("Attack");
                else
                    PlayAnimation("ExtendedAttack");
            }
            if (normalFrames)
            {
                PlayAnimation("Idle");
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/StoneFree/StoneFree_" + animationName);

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
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 2, 12, true);
            }
        }
    }
}