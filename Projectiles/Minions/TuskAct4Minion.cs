using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using JoJoStands.Projectiles.PlayerStands;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{  
    public class TuskAct4Minion : StandClass
    {
        public override string Texture => mod.Name + "/Projectiles/Minions/TuskAct4Minion";
		public override string poseSoundName => "ItsBeenARoundaboutPath";
        public override string punchSoundName => "Tusk_Ora";

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 64;
            projectile.height = 74;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.timeLeft = 2;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override int standType => 2;
        public override int punchDamage => 82;
        public override int punchTime => 12;

        private int goldenRectangleEffectTimer = 256;
        private bool playedSound = false;

        public override void AI()
        {
            PlayAnimations();
            UpdateStandInfo();
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;

            if (shootCount > 0)
            {
                shootCount--;
            }
            if (player.whoAmI == Main.myPlayer && modPlayer.TuskActNumber == 4)         //Making an owner check cause tuskActNumber isn't in sync with other players, causing TA4 to die for everyone else
            {
                projectile.timeLeft = 10;
            }
            if (goldenRectangleEffectTimer >= 215)
            {
                if (JoJoStands.SoundsLoaded && !playedSound)
                {
                    Terraria.Audio.LegacySoundStyle chumimiiin = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Chumimiiin");
                    chumimiiin.WithVolume(MyPlayer.soundVolume);
                    Main.PlaySound(chumimiiin, projectile.position);
                    playedSound = true;
                }
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 169, projectile.velocity.X, projectile.velocity.Y);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 169, projectile.velocity.X - 5f, projectile.velocity.Y + 5f);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 169, projectile.velocity.X + 5f, projectile.velocity.Y - 5f);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 169, projectile.velocity.X + 5f, projectile.velocity.Y + 5f);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 169, projectile.velocity.X - 5f, projectile.velocity.Y - 5f);
                //Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 169, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            }
            if (goldenRectangleEffectTimer > 0)
            {
                goldenRectangleEffectTimer -= 2;
            }

            NPC target = null;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    float distance = Vector2.Distance(npc.Center, player.Center);
                    if (npc.CanBeChasedBy(this) && distance < 9f * 16f && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                    {
                        target = npc;
                    }
                }
            }
            if (target != null)
            {
                attackFrames = true;
                normalFrames = false;
                PlayPunchSound();

                Vector2 velocity = (target.position + new Vector2(0f, -4f)) - projectile.position;
                velocity.Normalize();
                projectile.velocity = velocity * 4f;
                if ((target.position - projectile.Center).X > 0f)     //the turn around stuff
                {
                    projectile.spriteDirection = projectile.direction = 1;
                }
                else if ((target.position - projectile.Center).X < 0f)
                {
                    projectile.spriteDirection = projectile.direction = -1;
                }
                if (Main.myPlayer == projectile.owner)
                {
                    if (shootCount <= 0)
                    {
                        shootCount += newPunchTime;
                        Vector2 shootVel = target.position - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("Fists"), newPunchDamage, punchKnockback, projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                        Main.projectile[proj].timeLeft = 3;
                        projectile.netUpdate = true;
                    }
                }
            }
            else
            {
                attackFrames = false;
                normalFrames = true;
            }
            if (target == null || (!attackFrames && normalFrames))
            {
                Vector2 vector131 = player.Center;
                vector131.X -= (float)((12 + player.width / 2) * player.direction);
                vector131.Y -= 25f;
                projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                projectile.velocity *= 0.8f;
                projectile.direction = projectile.spriteDirection = player.direction;
                projectile.netUpdate = true;
                StopSounds();
            }
        }

        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            if (goldenRectangleEffectTimer > 0)
            {
                Vector2 rectangleOffset = Vector2.Zero;
                if (projectile.spriteDirection == 1)
                {
                    rectangleOffset = new Vector2(-30f, 0f);
                }
                spriteBatch.Draw(mod.GetTexture("Extras/GoldenSpinComplete"), (projectile.position + rectangleOffset) - Main.screenPosition, Color.White * (((float)goldenRectangleEffectTimer * 3.9215f) / 1000f));
            }
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            SyncAndApplyDyeSlot();
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);        //starting a draw with dyes that work
        }

        public virtual void PlayAnimations()   //too lazy to change, not like it has many states anyway
        {
            projectile.frameCounter++;
            if (attackFrames)
            {
                if (projectile.frameCounter >= newPunchTime)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 8)
                {
                    projectile.frame = 4;
                }
                if (projectile.frame <= 3)
                {
                    projectile.frame = 4;
                }
            }
            if (normalFrames)
            {
                if (projectile.frameCounter >= 20)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}