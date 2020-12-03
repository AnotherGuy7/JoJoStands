using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{
    public class SheerHeartAttack : StandClass
    {
        public override string Texture => mod.Name + "/Projectiles/Minions/SheerHeartAttack";

        private bool saidKocchiwomiro = false;
        private bool[] targettedEnemy = new bool[Main.maxNPCs];
        private const float detectionRange = 20f * 16f;
        private NPC npcTarget = null;
        private int enemiesHit = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 1800;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }


        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (SpecialKeyPressedNoCooldown())
            {
                projectile.position = player.position;
                projectile.netUpdate = true;
            }
            if (projectile.velocity.Y < 6f)
            {
                projectile.velocity.Y += 0.3f;
            }

            if (npcTarget == null)
            {
                saidKocchiwomiro = false;
                projectile.rotation = 0f;
                projectile.velocity.X = 0f;
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !targettedEnemy[npc.whoAmI] && projectile.Distance(npc.Center) < detectionRange)
                    {
                        npcTarget = npc;
                        targettedEnemy[npc.whoAmI] = true;
                        break;
                    }
                }
            }
            else
            {
                if (npcTarget.life < 0 || !npcTarget.active)      //If the NPC somehow dies before SHA gets to it
                {
                    npcTarget = null;
                    return;
                }

                if (!saidKocchiwomiro)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Kocchiwomiro"));
                    saidKocchiwomiro = true;
                }
                if (npcTarget.position.X > projectile.position.X)
                {
                    projectile.direction = 1;
                }
                if (npcTarget.position.X <= projectile.position.X)
                {
                    projectile.direction = -1;
                }

                if (!npcTarget.noGravity)
                {
                    projectile.rotation = 0f;
                    projectile.spriteDirection = projectile.direction;
                    projectile.velocity.X = 3f * projectile.direction;
                    if (WorldGen.SolidTile((int)(projectile.Center.X / 16f) + projectile.direction, (int)(projectile.Center.Y / 16f)))
                    {
                        projectile.velocity.Y = -6f;
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    projectile.spriteDirection = 1;
                    Vector2 velocity = npcTarget.position - projectile.position;
                    velocity.Normalize();
                    projectile.rotation = velocity.ToRotation();
                    projectile.velocity = velocity * 3f;
                }
            }
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            target.immune[projectile.owner] = 0;
            if (projectile.ai[0] == 0f)
            {
                int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ProjectileID.GrenadeIII, (int)(350 * mPlayer.standDamageBoosts), 7f, projectile.owner);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].timeLeft = 2;
                Main.projectile[proj].netUpdate = true;
                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(10));
            }
            if (projectile.ai[0] == 1f)
            {
                int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ProjectileID.GrenadeIII, (int)(724 * mPlayer.standDamageBoosts), 7f, projectile.owner);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].timeLeft = 2;
                Main.projectile[proj].netUpdate = true;
                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(4));
            }
            enemiesHit++;
            if (enemiesHit >= 5)
            {
                projectile.Kill();
            }
            npcTarget = null;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}