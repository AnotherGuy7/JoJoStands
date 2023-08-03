using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.StoneFree
{
    public class StoneFreeStandT2 : StandClass
    {
        public override float MaxDistance => 98f;
        public override int PunchDamage => 39;
        public override int PunchTime => 12;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 0;
        public override int TierNumber => 2;
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

        private const float MaxTrapDistance = 30f * 16f;

        public new enum AnimationState
        {
            Idle,
            Attack,
            ExtendedBarrage,
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
                    if (Main.mouseLeft)
                    {
                        currentAnimationState = AnimationState.Attack;
                        float lifeTimeMultiplier = 1f;
                        if (extendedBarrage)
                        {
                            newPunchDamage = (int)(newPunchDamage * 0.92f);
                            lifeTimeMultiplier = 1.8f;
                            currentAnimationState = AnimationState.ExtendedBarrage;
                        }
                        Punch(punchLifeTimeMultiplier: lifeTimeMultiplier);
                        if (extendedBarrage)
                            currentAnimationState = AnimationState.ExtendedBarrage;
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }

                    if (Main.mouseRight && shootCount <= 0)
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
                }
                if (SpecialKeyPressed())
                {
                    extendedBarrage = !extendedBarrage;
                    Projectile.netUpdate = true;
                }
                if (!attacking)
                    StayBehind();
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
                if (!attacking)
                    currentAnimationState = AnimationState.Idle;
                else
                    currentAnimationState = AnimationState.Attack;
            }
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(extendedBarrage);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            extendedBarrage = reader.ReadBoolean();
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
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 60, true);
        }
    }
}