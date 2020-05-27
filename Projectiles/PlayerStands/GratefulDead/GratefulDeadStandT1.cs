using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Networking;
using System.IO;

namespace JoJoStands.Projectiles.PlayerStands.GratefulDead
{
    public class GratefulDeadStandT1 : StandClass
    {
        public override void SetDefaults()      //Already defined in Stand Class
        {
            projectile.netImportant = true;
            projectile.width = 38;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            //projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 2;
        }

        public Texture2D standTexture;      //override all of these, they're already variables in Stand Class, not much sense in making new ones when they already exist

        public Vector2 velocityAddition = Vector2.Zero;
        public float mouseDistance = 0f;
        protected float shootSpeed = 16f;
        public bool normalFrames = false;
        public bool attackFrames = false;
        public bool grabFrames = false;
        public bool secondaryFrames = false;
        public float maxDistance = 0f;
        public int punchDamage = 16;
        public int shootCount = 0;
        public int punchTime = 12;
        public int halfStandHeight = 34;
        public float fistWhoAmI = 8f;
        public float tierNumber = 1f;
        public bool saidAbility = true;
        public int updateTimer = 0;


        public override void AI()
        {
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !grabFrames)     //simplify this using Punch()
                {
                    attackFrames = true;
                    normalFrames = false;
                    secondaryFrames = false;
                    grabFrames = false;
                    Main.mouseRight = false;
                    projectile.netUpdate = true;
                    PlayPunchSound();
                    float rotaY = Main.MouseWorld.Y - projectile.Center.Y;
                    projectile.rotation = MathHelper.ToRadians((rotaY * projectile.spriteDirection) / 6f);
                    if (Main.MouseWorld.X > projectile.position.X)
                    {
                        projectile.spriteDirection = 1;
                        projectile.direction = 1;
                        drawOffsetX = 20;       //these are handled with standOffset and drawOffsetY, just override those
                    }
                    if (Main.MouseWorld.X < projectile.position.X)
                    {
                        projectile.spriteDirection = -1;
                        projectile.direction = -1;
                        drawOffsetX = -20;
                    }
                    velocityAddition = Main.MouseWorld - projectile.position;
                    velocityAddition.Normalize();
                    velocityAddition *= 5f;
                    mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        projectile.velocity = player.velocity + velocityAddition;
                    }
                    if (mouseDistance <= 40f)
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                    if (shootCount <= 0)
                    {
                        shootCount += punchTime - modPlayer.standSpeedBoosts - modPlayer.standSpeedBoosts;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), (int)(punchDamage * modPlayer.standDamageBoosts), 2f, Main.myPlayer, fistWhoAmI);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames && !secondaryFrames)
                {
                    if (!secondaryFrames)
                    {
                        normalFrames = true;
                        grabFrames = false;
                        attackFrames = false;
                    }
                    Vector2 vector131 = player.Center;      //can be simplified with StayBehind()
                    vector131.X -= (float)((12 + player.width / 2) * player.direction);
                    vector131.Y -= -35f + halfStandHeight;
                    projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                    projectile.velocity *= 0.8f;
                    projectile.direction = (projectile.spriteDirection = player.direction);
                    projectile.rotation = 0;
                    drawOffsetX = 0;
                    StopSounds();
                }
                if (modPlayer.StandAutoMode)        //can be simplified with BasicPunchAI()
                {
                    NPC target = null;
                    Vector2 targetPos = projectile.position;
                    if (!attackFrames)
                    {
                        Vector2 vector131 = player.Center;
                        vector131.X -= (float)((12 + player.width / 2) * player.direction);
                        projectile.direction = (projectile.spriteDirection = player.direction);
                        vector131.Y -= -35f + halfStandHeight;
                        projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                        projectile.velocity *= 0.8f;
                        projectile.rotation = 0;
                    }
                    float targetDist = maxDistance * 1.5f;
                    if (target == null)
                    {
                        for (int k = 0; k < 200; k++)       //the targeting system
                        {
                            NPC npc = Main.npc[k];
                            if (npc.CanBeChasedBy(this, false))
                            {
                                float distance = Vector2.Distance(npc.Center, player.Center);
                                if (distance < targetDist && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                                {
                                    if (npc.boss)       //is gonna try to detect bosses over anything
                                    {
                                        targetDist = distance;
                                        targetPos = npc.Center;
                                        target = npc;
                                    }
                                    else        //if it fails to detect a boss, it'll detect the next best thing
                                    {
                                        targetDist = distance;
                                        targetPos = npc.Center;
                                        target = npc;
                                    }
                                }
                            }
                        }
                    }
                    if (target != null)
                    {
                        attackFrames = true;
                        normalFrames = false;
                        if ((targetPos - projectile.Center).X > 0f)
                        {
                            projectile.spriteDirection = projectile.direction = 1;
                        }
                        else if ((targetPos - projectile.Center).X < 0f)
                        {
                            projectile.spriteDirection = projectile.direction = -1;
                        }
                        if (targetPos.X > projectile.position.X)
                        {
                            projectile.velocity.X = 4f;
                        }
                        if (targetPos.X < projectile.position.X)
                        {
                            projectile.velocity.X = -4f;
                        }
                        if (targetPos.Y > projectile.position.Y)
                        {
                            projectile.velocity.Y = 4f;
                        }
                        if (targetPos.Y < projectile.position.Y)
                        {
                            projectile.velocity.Y = -4f;
                        }
                        if (shootCount <= 0)
                        {
                            if (Main.myPlayer == projectile.owner)
                            {
                                shootCount += punchTime - modPlayer.standSpeedBoosts;
                                Vector2 shootVel = targetPos - projectile.Center;
                                if (shootVel == Vector2.Zero)
                                {
                                    shootVel = new Vector2(0f, 1f);
                                }
                                shootVel.Normalize();
                                if (projectile.direction == 1)
                                {
                                    shootVel *= shootSpeed;
                                }
                                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), (int)((punchDamage * modPlayer.standDamageBoosts) * 0.9f), 3f, Main.myPlayer, fistWhoAmI);
                                Main.projectile[proj].netUpdate = true;
                                projectile.netUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        normalFrames = true;
                        secondaryFrames = false;
                        grabFrames = false;
                        attackFrames = false;
                    }
                }
                Vector2 direction = player.Center - projectile.Center;      //no need for this as the Stand Class covers these UNLESS you're using an alt attack, in which case you call LimitDistance()
                float distanceTo = direction.Length();
                maxDistance = 98f + modPlayer.standRangeBoosts;
                if (distanceTo > maxDistance)
                {
                    if (projectile.position.X <= player.position.X - 15f)
                    {
                        projectile.velocity = player.velocity + new Vector2(0.8f, 0f);
                    }
                    if (projectile.position.X >= player.position.X + 15f)
                    {
                        projectile.velocity = player.velocity + new Vector2(-0.8f, 0f);
                    }
                    if (projectile.position.Y >= player.position.Y + 15f)
                    {
                        projectile.velocity = player.velocity + new Vector2(0f, -0.8f);
                    }
                    if (projectile.position.Y <= player.position.Y - 15f)
                    {
                        projectile.velocity = player.velocity + new Vector2(0f, 0.8f);
                    }
                }
                if (distanceTo >= maxDistance + 22f)
                {
                    if (!modPlayer.StandAutoMode)
                    {
                        Main.mouseLeft = false;
                        Main.mouseRight = false;
                    }
                    projectile.Center = player.Center;
                }
                if (attackFrames)       //handle these in SelectAnimation
                {
                    normalFrames = false;
                    grabFrames = false;
                    secondaryFrames = false;
                    SwitchStatesTo("Attack");
                }
                if (normalFrames)
                {
                    attackFrames = false;
                    grabFrames = false;
                    secondaryFrames = false;
                    SwitchStatesTo("Idle");
                }
                if (grabFrames)
                {
                    normalFrames = false;
                    secondaryFrames = false;
                    attackFrames = false;
                    SwitchStatesTo("Grab");
                }
                if (secondaryFrames)
                {
                    normalFrames = false;
                    attackFrames = false;
                    grabFrames = false;
                    SwitchStatesTo("Secondary");
                }
                if (modPlayer.poseMode)
                {
                    if (Main.mouseLeft || Main.mouseRight)
                    {
                        modPlayer.poseMode = false;
                    }
                    attackFrames = false;
                    grabFrames = false;
                    normalFrames = false;
                    secondaryFrames = false;
                    SwitchStatesTo("Pose");
                }
            }
        }

        public SpriteEffects effects = SpriteEffects.None;      //Already exists

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)        //already handled in Stand Class
        {
            Player player = Main.player[projectile.owner];
            if (projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            if (projectile.spriteDirection == 1)
            {
                effects = SpriteEffects.None;
            }
            if (MyPlayer.RangeIndicators)
            {
                Texture2D texture = mod.GetTexture("Extras/RangeIndicator");        //the initial tile amount the indicator covers is 20 tiles, 320 pixels, border is included in the measurements
                spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), maxDistance / 122.5f, SpriteEffects.None, 0);
            }
            int frameHeight = standTexture.Height / Main.projFrames[projectile.whoAmI];
            spriteBatch.Draw(standTexture, projectile.Center - Main.screenPosition + new Vector2(drawOffsetX, drawOriginOffsetY), new Rectangle(0, frameHeight * projectile.frame, standTexture.Width, frameHeight), lightColor, 0f, new Vector2(standTexture.Width / 2f, frameHeight / 2f), 1f, effects, 0);
        }

        public override void SendExtraAI(BinaryWriter writer)       //already synced in Stand Class (not grabFrames)
        {
            writer.Write(attackFrames);
            writer.Write(normalFrames);
            writer.Write(grabFrames);
            writer.Write(secondaryFrames);
        }

        public override void ReceiveExtraAI(BinaryReader reader)    //already synced in Stand Class (not grabFrames)
        {
            attackFrames = reader.ReadBoolean();
            normalFrames = reader.ReadBoolean();
            grabFrames = reader.ReadBoolean();
            secondaryFrames = reader.ReadBoolean();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)     //already done in SC
        {
            return false;
        }

        public virtual void SwitchStatesTo(string animationName)        //this should be managed in SelectAnimations
        {
            if (animationName == "Idle")
            {
                AnimationStates(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 4, 10, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 4, 11, true);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 200, true, true);     //will loop that frame
            }
            if (animationName == "Grab")
            {
                AnimationStates(animationName, 2, 120, false);
            }
        }

        public virtual void AnimationStates(string stateName, int frameAmount, int frameCounterLimit, bool loop, bool loopCertainFrames = false, int loopFrameStart = 0, int loopFrameEnd = 0)      //why is this here
        {
            Main.projFrames[projectile.whoAmI] = frameAmount;
            projectile.frameCounter++;
            standTexture = mod.GetTexture("Projectiles/PlayerStands/GratefulDead/GratefulDead_" + stateName);
            if (projectile.frameCounter >= frameCounterLimit)
            {
                projectile.frameCounter = 0;
                projectile.frame += 1;
            }
            if (loopCertainFrames)
            {
                if (projectile.frame >= loopFrameEnd)
                {
                    projectile.frame = loopFrameStart;
                }
            }
            if (projectile.frame >= frameAmount && loop)
            {
                projectile.frame = 0;
            }
            if (projectile.frame >= frameAmount && !loop)
            {
                SwitchStatesTo("Idle");
                secondaryFrames = false;
            }
        }
    }
}