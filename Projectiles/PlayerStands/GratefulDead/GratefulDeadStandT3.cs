using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.GratefulDead
{
    public class GratefulDeadStandT3 : StandClass
    {
        public override float shootSpeed => 16f;
        public override float maxDistance => 98f;
        public override int punchDamage => 67;
        public override int punchTime => 11;
        public override int halfStandHeight => 34;
        public override float fistWhoAmI => 8f;
        public override float tierNumber => 1f;
        public override int standOffset => 32;
        public override int standType => 1;
        public override string poseSoundName => "OnceWeDecideToKillItsDone";
        public override string spawnSoundName => "The Grateful Dead";

        private const float GasDetectionDist = 20 * 16f;

        public int updateTimer = 0;
        public bool grabFrames = false;
        public bool secondaryFrames = false;
        public bool gasActive = false;
        public float gasRange = GasDetectionDist;

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

            mPlayer.gratefulDeadGasActive = gasActive;
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
                    LimitDistance();
                }
                if (!Main.mouseRight && (grabFrames || secondaryFrames))
                {
                    grabFrames = false;
                    Projectile.ai[0] = -1f;
                    shootCount += 30;
                    secondaryFrames = false;
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
            if (gasActive)
            {
                gasRange = GasDetectionDist + mPlayer.standRangeBoosts;
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        float distance = Vector2.Distance(player.Center, npc.Center);
                        if (distance < gasRange && !npc.immortal && !npc.hide)
                        {
                            npc.AddBuff(ModContent.BuffType<Aging>(), 2);
                        }
                    }
                }
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                    {
                        float distance = Vector2.Distance(player.Center, otherPlayer.Center);
                        if (distance < gasRange && otherPlayer.whoAmI != player.whoAmI)
                        {
                            otherPlayer.AddBuff(ModContent.BuffType<Aging>(), 2);
                        }
                    }
                }
                if (Main.rand.Next(0, 12 + 1) == 0)
                    Dust.NewDust(Projectile.Center - new Vector2(gasRange / 2f, 0f), (int)gasRange, Projectile.height, ModContent.DustType<Dusts.GratefulDeadCloud>());

                player.AddBuff(ModContent.BuffType<Aging>(), 2);
            }
            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
            }
        }

        private Texture2D gasRangeIndicatorTexture;
        private float gasTextureSize;
        private Vector2 gasTextureOrigin;
        private Rectangle gasTextureSourceRect;

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
                    Main.EntitySpriteDraw(gasRangeIndicatorTexture, player.Center - Main.screenPosition, gasTextureSourceRect, Color.Red * ((MyPlayer.RangeIndicatorAlpha / 100f) * 255f), 0f, gasTextureOrigin, 2f, SpriteEffects.None, 0);
            }
            return true;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(grabFrames);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            grabFrames = reader.ReadBoolean();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                PlayAnimation("Idle");
            }
            if (secondaryFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
            if (grabFrames)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryFrames = false;
                PlayAnimation("Grab");

            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
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