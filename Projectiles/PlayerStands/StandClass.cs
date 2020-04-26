using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Networking;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Audio;

namespace JoJoStands.Projectiles.PlayerStands
{
    public abstract class StandClass : ModProjectile        //to simplify stand creation
    {
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 38;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public virtual float shootSpeed { get; } = 16f;       //how fast the projectile the minion shoots goes
        public virtual int punchDamage { get; }     //virtual is what allows that to be overridden
        public virtual int projectileDamage { get; }
        public virtual int halfStandHeight { get; }
        public virtual int punchTime { get; }
        public virtual int shootTime { get; }
        public virtual float maxDistance { get; } = 98f;
        public virtual float maxAltDistance { get; } = 0f;
        public virtual int altDamage { get; }
        public virtual float fistWhoAmI { get; }
        public virtual int drawOffsetLeft { get; } = -60;
        public virtual int drawOffsetRight { get; } = -10;
        public virtual float tierNumber { get; }
        public virtual float punchKnockback { get; }
        public virtual string punchSoundName { get; } = "";


        public bool normalFrames = false;       //all other animations handled in the stands themselves
        public bool attackFrames = false;
        public bool secondaryAbilityFrames = false;

        public int shootCount = 0;
        private Vector2 velocityAddition;
        private float mouseDistance;
        public bool secondaryAbility = false;
        private bool playedBeginning = false;
        private SoundEffectInstance beginningSoundInstance = null;
        private SoundEffectInstance punchingSoundInstance = null;

        public void Timestop(int seconds)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            modPlayer.TimestopEffectDurationTimer = 60;
            modPlayer.TheWorldEffect = true;
            if (JoJoStands.JoJoStandsSounds == null)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/timestop_start"));
            }
            player.AddBuff(mod.BuffType("TheWorldBuff"), seconds * 60, true);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModNetHandler.effectSync.SendTimestop(256, player.whoAmI, true, player.whoAmI);
            }
        }

        public void Punch(float movementSpeed = 5f)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            HandleDrawOffsets();
            normalFrames = false;
            attackFrames = true;
            projectile.netUpdate = true;
            float rotaY = Main.MouseWorld.Y - projectile.Center.Y;
            projectile.rotation = MathHelper.ToRadians((rotaY * projectile.spriteDirection) / 6f);
            if (Main.MouseWorld.X > projectile.position.X)
            {
                projectile.spriteDirection = 1;
                projectile.direction = 1;
            }
            if (Main.MouseWorld.X < projectile.position.X)
            {
                projectile.spriteDirection = -1;
                projectile.direction = -1;
            }
            velocityAddition = Main.MouseWorld - projectile.position;
            velocityAddition.Normalize();
            velocityAddition *= movementSpeed;
            /*if (projectile.position.X > Main.MouseWorld.X - 5f && projectile.position.X < Main.MouseWorld.X + 5f)
            {
                velocityAddition.X = 0f;
            }
            if (projectile.position.Y < Main.MouseWorld.Y + 5f && projectile.position.Y > Main.MouseWorld.Y - 5f)
            {
                velocityAddition.Y = 0f;
            }*/
            mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
            if (mouseDistance > 40f)
            {
                projectile.velocity = player.velocity + velocityAddition;
            }
            if (mouseDistance <= 40f)
            {
                projectile.velocity = Vector2.Zero;
            }
            if (JoJoStands.JoJoStandsSounds != null)
            {
                PlayPunchSound();
            }
            if (shootCount <= 0)
            {
                shootCount += punchTime - modPlayer.standSpeedBoosts;
                Vector2 shootVel = Main.MouseWorld - projectile.Center;
                if (shootVel == Vector2.Zero)
                {
                    shootVel = new Vector2(0f, 1f);
                }
                shootVel.Normalize();
                shootVel *= shootSpeed;
                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), (int)(punchDamage * modPlayer.standDamageBoosts), 2f, Main.myPlayer, shootCount);
                Main.projectile[proj].netUpdate = true;
                projectile.netUpdate = true;
            }
            LimitDistance();
        }

        public void StayBehind()
        {
            Player player = Main.player[projectile.owner];

            HandleDrawOffsets();
            Vector2 vector131 = player.Center;
            vector131.X -= (float)((12 + player.width / 2) * player.direction);
            vector131.Y -= -35f + halfStandHeight;
            projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
            projectile.velocity *= 0.8f;
            projectile.direction = projectile.spriteDirection = player.direction;
            projectile.rotation = 0;
            normalFrames = true;
            attackFrames = false;
            LimitDistance();
            if (playedBeginning)
            {
                if (beginningSoundInstance != null)
                {
                    beginningSoundInstance.Stop();
                }
                if (punchingSoundInstance != null)
                {
                    punchingSoundInstance.Stop();
                }
                playedBeginning = false;
            }
        }

        public void StayBehindWithAbility()
        {
            Player player = Main.player[projectile.owner];

            HandleDrawOffsets();
            Vector2 vector131 = player.Center;
            vector131.X -= (float)((12 + player.width / 2) * player.direction);
            vector131.Y -= -35f + halfStandHeight;
            projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
            projectile.velocity *= 0.8f;
            if (!secondaryAbilityFrames)
            {
                projectile.direction = projectile.spriteDirection = player.direction;
            }
            projectile.rotation = 0;
            normalFrames = true;
            attackFrames = false;
            LimitDistance();
            if (playedBeginning)
            {
                if (beginningSoundInstance != null)
                {
                    beginningSoundInstance.Stop();
                }
                if (punchingSoundInstance != null)
                {
                    punchingSoundInstance.Stop();
                }
                playedBeginning = false;
            }
        }

        public void GoInFront()
        {
            Player player = Main.player[projectile.owner];

            HandleDrawOffsets();
            Vector2 vector131 = player.Center;
            vector131.X += (float)((12 + player.width / 2) * player.direction);
            vector131.Y -= -35f + halfStandHeight;
            projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
            projectile.velocity *= 0.8f;
            projectile.direction = (projectile.spriteDirection = player.direction);
            projectile.rotation = 0;
            normalFrames = true;
            attackFrames = false;
            LimitDistance();
        }

        public void BasicPunchAI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            HandleDrawOffsets();
            NPC target = null;
            Vector2 targetPos = projectile.position;
            float targetDist = maxDistance * 3f;
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
                if (targetDist < (maxDistance * 1.5f))
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
                            if (JoJoStands.JoJoStandsSounds != null)
                            {
                                PlayPunchSound();
                            }
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
            }
            else
            {
                normalFrames = true;
                attackFrames = false;
            }
            LimitDistance();
        }

        public void PunchAndShootAI(int projToShoot)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            HandleDrawOffsets();
            NPC target = null;
            Vector2 targetPos = projectile.position;
            float targetDist = maxDistance * 3f;
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
            if (targetDist > (maxDistance * 1.5f) || secondaryAbility || target == null)
            {
                Vector2 vector131 = player.Center;
                normalFrames = true;
                attackFrames = false;
                if (secondaryAbility)
                {
                    vector131.X += (float)((12 + player.width / 2) * player.direction);
                }
                else
                {
                    vector131.X -= (float)((12 + player.width / 2) * player.direction);
                    projectile.spriteDirection = projectile.direction = player.direction;
                }
                vector131.Y -= -35f + halfStandHeight;
                projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                projectile.velocity *= 0.8f;
                projectile.rotation = 0;
            }
            if (target != null)
            {
                if (targetDist < (maxDistance * 1.5f))
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
                            if (JoJoStands.JoJoStandsSounds != null)
                            {
                                PlayPunchSound();
                            }
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
                else if (Main.rand.Next(0, 101) <= 1 && targetDist > (maxDistance * 1.5f))
                {
                    secondaryAbility = true;
                }
            }
            if (secondaryAbility)
            {
                secondaryAbilityFrames = true;
                secondaryAbility = true;
                attackFrames = false;
                normalFrames = false;
                if ((targetPos - projectile.Center).X > 0f)
                {
                    projectile.spriteDirection = projectile.direction = 1;
                }
                else if ((targetPos - projectile.Center).X < 0f)
                {
                    projectile.spriteDirection = projectile.direction = -1;
                }
                if (shootCount <= 0)
                {
                    if (Main.myPlayer == projectile.owner)
                    {
                        if (shootCount <= 0)
                        {
                            shootCount += 28;
                            Vector2 shootVel = targetPos - projectile.Center - new Vector2(0f, 2f);
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.position.X + 5f, projectile.position.Y - 3f, shootVel.X, shootVel.Y, projToShoot, (int)((altDamage * modPlayer.standDamageBoosts) * 0.9f), 2f, Main.myPlayer, projectile.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                }
            }
            if (secondaryAbility && player.ownedProjectileCounts[projToShoot] == 0)
            {
                secondaryAbility = false;
            }
            LimitDistance();
        }

        public void PlayPunchSound()
        {
            if (punchSoundName != "" && punchingSoundInstance == null)
            {
                InitializeSounds();
            }
            if (punchSoundName != "")
            {
                if (beginningSoundInstance != null)
                {
                    if (!playedBeginning)
                    {
                        beginningSoundInstance.Play();     //is this not just beginningSoundInstance.Play()?
                        if (beginningSoundInstance.State == SoundState.Stopped)
                        {
                            playedBeginning = true;
                        }
                    }
                    if (playedBeginning)
                    {
                        punchingSoundInstance.Play();     //is this not just beginningSoundInstance.Play()?
                    }
                }
                else
                {
                    punchingSoundInstance.Play();
                }
            }
        }

        public void InitializeSounds()
        {
            if (beginningSoundInstance == null)
            {
                SoundEffect sound = JoJoStands.JoJoStandsSounds.GetSound("Sounds/BattleCries/" + punchSoundName + "_Beginning");
                if (sound != null)
                {
                    beginningSoundInstance = sound.CreateInstance();
                    beginningSoundInstance.Volume = MyPlayer.soundVolume;
                }
            }
            if (punchingSoundInstance == null)
            {
                SoundEffect sound = JoJoStands.JoJoStandsSounds.GetSound("Sounds/BattleCries/" + punchSoundName + "_Beginning");
                punchingSoundInstance = sound.CreateInstance();
                punchingSoundInstance.Volume = MyPlayer.soundVolume;
            }
        }

        public void HandleDrawOffsets()
        {
            if (projectile.spriteDirection == 1)
            {
                drawOffsetX = drawOffsetRight;
            }
            if (projectile.spriteDirection == -1)
            {
                drawOffsetX = drawOffsetLeft;
            }
            drawOriginOffsetY = -halfStandHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)      //from ExampleMod ExampleDeathShader
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            // Retrieve reference to shader
            //var shader = GameShaders.Armor.GetSecondaryShader(Main.player[projectile.owner].cLight, Main.player[projectile.owner]);
            if (mPlayer.StandDyeSlot.Item.dye != 0)
            {
                var shader = GameShaders.Armor.GetShaderFromItemId(mPlayer.StandDyeSlot.Item.type);
                shader.Apply(null);
            }
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            Main.spriteBatch.End();     //ending the spriteBatch that started in PreDraw (it's a guess)
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);     //starting a new one for Post


            if (MyPlayer.RangeIndicators)
            {
                Texture2D texture = mod.GetTexture("Extras/RangeIndicator");        //the initial tile amount the indicator covers is 20 tiles, 320 pixels, border is included in the measurements
                if (maxDistance != 0f)
                {
                    spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), (maxDistance + mPlayer.standRangeBoosts) / 122.5f, SpriteEffects.None, 0);
                }
                if (maxAltDistance != 0f)
                {
                    spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.Orange * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), (maxAltDistance + mPlayer.standRangeBoosts) / 160f, SpriteEffects.None, 0);
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(normalFrames);
            writer.Write(attackFrames);
            writer.Write(secondaryAbilityFrames);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            normalFrames = reader.ReadBoolean();
            attackFrames = reader.ReadBoolean();
            secondaryAbilityFrames = reader.ReadBoolean();
        }

        public void LimitDistance()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            Vector2 direction = player.Center - projectile.Center;
            float distanceTo = direction.Length();
            float maxDist = maxDistance + modPlayer.standRangeBoosts;
            if (distanceTo > maxDist)
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
            if (distanceTo >= maxDist + 22f)
            {
                if (!modPlayer.StandAutoMode)
                {
                    Main.mouseLeft = false;
                    Main.mouseRight = false;
                }
                projectile.Center = player.Center;
            }
        }
    }
}