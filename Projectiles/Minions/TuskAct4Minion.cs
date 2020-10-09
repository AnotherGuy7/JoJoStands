using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using JoJoStands.Projectiles.PlayerStands;
 
namespace JoJoStands.Projectiles.Minions
{  
    public class TuskAct4Minion : StandClass
    {
        public override string Texture => mod.Name + "/Projectiles/Minions/TuskAct4Minion";

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 92;
            projectile.height = 74;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.timeLeft = 1;
            projectile.tileCollide = false; ;
            projectile.ignoreWater = true;
        }

        public override int standType => 2;
        private readonly float shootCool = 6f;
        private int goldenRectangleEffectTimer = 256;

        public override void AI()
        {
            UpdateStandInfo();
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            Vector2 targetPos = projectile.position;
            Vector2 direction2 = targetPos - player.position;
            bool target = false;
            float targetDist = 98f;
            projectile.frameCounter++;
            if (player.whoAmI == Main.myPlayer && modPlayer.TuskActNumber == 4)         //Making an owner check cause tuskActNumber isn't in sync with other players, causing TA4 to die for everyone else
            {
                projectile.timeLeft = 10;
            }
            if (goldenRectangleEffectTimer >= 215)
            {
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

            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy(this, false))
                {
                    float distance = Vector2.Distance(npc.Center, projectile.Center);
                    if (distance < targetDist && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                    {
                        targetDist = distance;
                        targetPos = npc.Center;
                        target = true;
                    }
                }
            }
            if (projectile.ai[1] > 0f)
            {
                projectile.ai[1] += 1f;
                if (Main.rand.NextBool(3))
                {
                    projectile.ai[1] += 1f;
                }
            }
            if (projectile.ai[1] > shootCool)
            {
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }
            SelectFrame();
            normalFrames = true;
            attackFrames = false;
            if (target && direction2.Length() < 98f)
            {
                attackFrames = true;
                normalFrames = false;
                if (projectile.ai[1] == 0f)
                {
                    projectile.ai[1] = 1f;
                    if ((targetPos - projectile.Center).X > 0f)     //the turn around stuff
                    {
                        projectile.spriteDirection = projectile.direction = 1;
                        projectile.velocity.X = 2f;
                    }
                    else if ((targetPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                        projectile.velocity.X = -2f;
                    }
                    if (Main.myPlayer == projectile.owner)
                    {
                        Vector2 shootVel = targetPos - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), 61, 2f, Main.myPlayer, 0f, 0f);
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
            if (!target || (!attackFrames && normalFrames))
            {
                Vector2 vector131 = player.Center;
                vector131.X -= (float)((12 + player.width / 2) * player.direction);
                vector131.Y -= 25f;
                projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                projectile.velocity *= 0.8f;
                projectile.direction = projectile.spriteDirection = player.direction;
                projectile.netUpdate = true;
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

        public virtual void SelectFrame()   //too lazy to change, not like it has many states anyway
        {
            projectile.frameCounter++;
            if (attackFrames)
            {
                if (projectile.frameCounter >= 6)
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