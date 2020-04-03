using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands
{
    public class TestStand : ModProjectile      //has 2 poses
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 6;
        }

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
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public Vector2 velocityAddition = Vector2.Zero;
        public float mouseDistance = 0f;
        protected float shootSpeed = 16f;       //how fast the projectile the minion shoots goes
        public bool normalFrames = false;
        public bool attackFrames = false;
        public int timestopPoseTimer = 0;
        public float maxDistance = 0f;
        public int punchDamage = 70;
        //public int rippleEffectTimer = 0;
        int shootCount = 0;
        public int halfStandHeight = 37;

        /*ripple effect info
        private int rippleCount = 3;
        private int rippleSize = 5;
        private int rippleSpeed = 15;
        private float distortStrength = 100f;*/

        public override void AI()
        {
            SelectFrame();
            shootCount--;
            //rippleEffectTimer--;
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            //Main.NewText("A: " + attackFrames + "; N: " + normalFrames, Color.DarkGreen);
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (projectile.spriteDirection == 1)
            {
                drawOffsetX = -10;
            }
            if (projectile.spriteDirection == -1)
            {
                drawOffsetX = -60;
            }
            drawOriginOffsetY = -halfStandHeight;
            if (Main.mouseLeft && player.whoAmI == Main.myPlayer)
            {
                attackFrames = true;
                normalFrames = false;
                //Main.mouseRight = false;        //so that the player can't just stop time while punching
                float rotaY = Main.MouseWorld.Y - projectile.Center.Y;
                projectile.rotation = MathHelper.ToRadians((rotaY * projectile.spriteDirection) / 6f);
                projectile.netUpdate = true;
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
                if (projectile.position.X < Main.MouseWorld.X - 5f)
                {
                    velocityAddition.X = 5f;
                }
                if (projectile.position.X > Main.MouseWorld.X + 5f)
                {
                    velocityAddition.X = -5f;
                }
                if (projectile.position.X > Main.MouseWorld.X - 5f && projectile.position.X < Main.MouseWorld.X + 5f)
                {
                    velocityAddition.X = 0f;
                }
                if (projectile.position.Y > Main.MouseWorld.Y + 5f)
                {
                    velocityAddition.Y = -5f;
                }
                if (projectile.position.Y < Main.MouseWorld.Y - 5f)
                {
                    velocityAddition.Y = 5f;
                }
                if (projectile.position.Y < Main.MouseWorld.Y + 5f && projectile.position.Y > Main.MouseWorld.Y - 5f)
                {
                    velocityAddition.Y = 0f;
                }
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
                    shootCount += 12;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), (int)(punchDamage * modPlayer.standDamageBoosts), 2f, Main.myPlayer, 1f);
                    Main.projectile[proj].netUpdate = true;
                    projectile.netUpdate = true;
                }
                if (shootCount <= 0)
                {
                    shootCount = 0;
                }
            }
            else
            {
                if (player.whoAmI == Main.myPlayer)
                    attackFrames = false;
            }
            if (!attackFrames)
            {
                Vector2 vector131 = player.Center;
                vector131.X -= (float)((12 + player.width / 2) * player.direction);
                vector131.Y -= -35f + halfStandHeight;       //40 is half of TW's height
                projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                projectile.velocity *= 0.8f;
                projectile.direction = (projectile.spriteDirection = player.direction);
                projectile.rotation = 0;
                normalFrames = true;
                attackFrames = false;
            }
            if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("TheWorldBuff")))
            {
                //rippleEffectTimer = 80;
                timestopPoseTimer = 30;
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/timestop_start"));
                player.AddBuff(mod.BuffType("TheWorldBuff"), 540, true);
                //float progress = (180f - rippleEffectTimer) / 60f; // Will range from -3 to 3, 0 being the point where the bomb explodes.
                //Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));*/
            }
            if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(mod.BuffType("AbilityCooldown")) && player.HasBuff(mod.BuffType("TheWorldBuff")) && timestopPoseTimer <= 0 && player.ownedProjectileCounts[mod.ProjectileType("RoadRoller")] == 0)
            {
                shootCount += 12;
                Vector2 shootVel = Main.MouseWorld - projectile.Center;
                if (shootVel == Vector2.Zero)
                {
                    shootVel = new Vector2(0f, 1f);
                }
                shootVel.Normalize();
                shootVel *= shootSpeed + 4f;
                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("RoadRoller"), 120, 5f, Main.myPlayer);
                Main.projectile[proj].netUpdate = true;
                projectile.netUpdate = true;
            }
            if (timestopPoseTimer > 0)
            {
                timestopPoseTimer--;
                normalFrames = false;
                attackFrames = false;
                projectile.frame = 6;
                Main.mouseLeft = false;
                Main.mouseRight = false;
            }
            /*if (rippleEffectTimer <= 0)
            {
                Filters.Scene["Shockwave"].Deactivate();
                rippleEffectTimer = 0;
            }*/
            Vector2 direction = player.Center - projectile.Center;      //needs to be down here so velocity changes get applied when the stand enters the rim
            float distanceTo = direction.Length();
            maxDistance = 98f + modPlayer.standRangeBoosts;
            if (distanceTo > maxDistance)
            {
                if (projectile.position.X <= player.position.X - 15f)
                {
                    ////projectile.position = new Vector2(projectile.position.X + 0.2f, projectile.position.Y);
                    projectile.velocity = player.velocity + new Vector2(0.8f, 0f);
                }
                if (projectile.position.X >= player.position.X + 15f)
                {
                    //projectile.position = new Vector2(projectile.position.X - 0.2f, projectile.position.Y);
                    projectile.velocity = player.velocity + new Vector2(-0.8f, 0f);
                }
                if (projectile.position.Y >= player.position.Y + 15f)
                {
                    // projectile.position = new Vector2(projectile.position.X, projectile.position.Y - 0.2f);
                    projectile.velocity = player.velocity + new Vector2(0f, -0.8f);
                }
                if (projectile.position.Y <= player.position.Y - 15f)
                {
                    ///projectile.position = new Vector2(projectile.position.X, projectile.position.Y + 0.2f);
                    projectile.velocity = player.velocity + new Vector2(0f, 0.8f);
                }
            }
            if (distanceTo >= maxDistance + 22f)
            {
                Main.mouseLeft = false;
                Main.mouseRight = false;
                projectile.Center = player.Center;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(normalFrames);
            writer.Write(attackFrames);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            normalFrames = reader.ReadBoolean();
            attackFrames = reader.ReadBoolean();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            if (MyPlayer.RangeIndicators)
            {
                Texture2D texture = mod.GetTexture("Extras/RangeIndicator");        //the initial tile amount the indicator covers is 20 tiles, 320 pixels, border is included in the measurements
                spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), maxDistance / 122.5f, SpriteEffects.None, 0);
            }
        }

        public virtual void SelectFrame()
        {
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (projectile.frameCounter >= 12)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 1)
                {
                    projectile.frame = 2;
                }
                if (projectile.frame >= 6)
                {
                    projectile.frame = 2;
                }
            }
            if (normalFrames)
            {
                if (projectile.frameCounter >= 30)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}