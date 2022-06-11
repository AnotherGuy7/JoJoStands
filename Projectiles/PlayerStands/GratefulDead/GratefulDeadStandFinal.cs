using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.GratefulDead
{
    public class GratefulDeadStandFinal : StandClass
    {
        public override float shootSpeed => 16f;
        public override float maxDistance => 98f;
        public override int punchDamage => 90;
        public override int punchTime => 10;
        public override int halfStandHeight => 34;
        public override float fistWhoAmI => 8f;
        public override float tierNumber => 1f;
        public override int standOffset => 32;
        public override StandType standType => StandType.Melee;
        public override string poseSoundName => "OnceWeDecideToKillItsDone";
        public override string spawnSoundName => "The Grateful Dead";

        private const float GasDetectionDist = 30 * 16f;

        private bool grabFrames = false;
        private bool secondaryFrames = false;
        private bool gasActive = false;
        public float gasRange = GasDetectionDist;

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

            mPlayer.gratefulDeadGasActive = gasActive;
            if (gasActive)
            {
                gasRange = GasDetectionDist + mPlayer.standRangeBoosts;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active)
                    {
                        float distance = Vector2.Distance(player.Center, npc.Center);
                        if (distance < gasRange && !npc.immortal && !npc.hide)
                            npc.AddBuff(ModContent.BuffType<Aging>(), 2);
                    }
                }
                if (MyPlayer.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayer = Main.player[i];
                        if (otherPlayer.active && otherPlayer.InOpposingTeam(player) && otherPlayer.whoAmI != player.whoAmI)
                        {
                            float distance = Vector2.Distance(player.Center, otherPlayer.Center);
                            if (distance < gasRange)
                                otherPlayer.AddBuff(ModContent.BuffType<Aging>(), 2);
                        }
                    }
                }
                if (Main.rand.Next(0, 12 + 1) == 0)
                    Dust.NewDust(Projectile.Center - new Vector2(gasRange / 2f, 0f), (int)gasRange, Projectile.height, ModContent.DustType<Dusts.GratefulDeadCloud>());

                player.AddBuff(ModContent.BuffType<Aging>(), 2);
            }

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !secondaryFrames && !grabFrames)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames && !secondaryFrames && !grabFrames)
                {
                    StayBehind();
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer && shootCount <= 0 && !grabFrames)
                {
                    Projectile.velocity = Main.MouseWorld - Projectile.position;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 5f;

                    float mouseDistance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        Projectile.velocity = player.velocity + Projectile.velocity;
                    }
                    if (mouseDistance <= 40f)
                    {
                        Projectile.velocity = Vector2.Zero;
                    }

                    secondaryFrames = true;
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active)
                        {
                            if (Projectile.Distance(npc.Center) <= 30f && !npc.boss && !npc.immortal && !npc.hide)
                            {
                                Projectile.ai[0] = npc.whoAmI;
                                grabFrames = true;
                            }
                        }
                    }
                    LimitDistance();
                }
                if (grabFrames && Projectile.ai[0] != -1f)
                {
                    Projectile.velocity = Vector2.Zero;
                    NPC npc = Main.npc[(int)Projectile.ai[0]];
                    npc.direction = -Projectile.direction;
                    npc.position = Projectile.position + new Vector2(5f * Projectile.direction, -2f - npc.height / 3f);
                    npc.velocity = Vector2.Zero;
                    npc.AddBuff(ModContent.BuffType<RapidAging>(), 2);
                    if (!npc.active)
                    {
                        grabFrames = false;
                        Projectile.ai[0] = -1f;
                        shootCount += 30;
                    }
                    Projectile.netUpdate = true;
                    LimitDistance();
                }
                if (!Main.mouseRight && (grabFrames || secondaryFrames))
                {
                    grabFrames = false;
                    Projectile.ai[0] = -1f;
                    shootCount += 30;
                    secondaryFrames = false;
                    Projectile.netUpdate = true;
                }
            }
            if (SpecialKeyPressed() && shootCount <= 0)
            {
                shootCount += 30;
                gasActive = !gasActive;
                if (gasActive)
                    Main.NewText("Gas Spread: On");
                else
                    Main.NewText("Gas Spread: Off");
            }
            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
            }
        }

        private float gasTextureSize;
        private Vector2 gasTextureOrigin;
        private Rectangle gasTextureSourceRect;
        private Texture2D gasRangeIndicatorTexture;

        public override bool PreDrawExtras()
        {
            Player player = Main.player[Projectile.owner];
            if (MyPlayer.RangeIndicators && gasActive)
            {
                if (gasRange != gasTextureSize)
                {
                    gasRangeIndicatorTexture = GenerateRangeIndicatorTexture(gasRange, 0);
                    gasTextureOrigin = new Vector2(gasRangeIndicatorTexture.Width / 2f, gasRangeIndicatorTexture.Height / 2f);
                    gasTextureSourceRect = new Rectangle(0, 0, gasRangeIndicatorTexture.Width, gasRangeIndicatorTexture.Height);
                    gasTextureSize = gasRange;
                }
                if (gasRangeIndicatorTexture != null)
                    Main.EntitySpriteDraw(gasRangeIndicatorTexture, player.Center - Main.screenPosition, gasTextureSourceRect, Color.Red * MyPlayer.RangeIndicatorAlpha, 0f, gasTextureOrigin, 2f, SpriteEffects.None, 0);
            }
            return true;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(grabFrames);
            writer.Write(gasActive);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            grabFrames = reader.ReadBoolean();
            gasActive = reader.ReadBoolean();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
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
            if (secondaryFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
            if (grabFrames)
            {
                idleFrames = false;
                attackFrames = false;
                secondaryFrames = false;
                PlayAnimation("Grab");

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
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/GratefulDead/GratefulDead_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 1, 1, true);
            }
            if (animationName == "Grab")
            {
                AnimateStand(animationName, 3, 12, true, 2, 2);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 12, true);
            }
        }

    }
}