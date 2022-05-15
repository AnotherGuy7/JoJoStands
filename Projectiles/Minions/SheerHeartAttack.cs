using JoJoStands.Buffs.Debuffs;
using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{
    public class SheerHeartAttack : StandClass
    {
        public override string Texture => Mod.Name + "/Projectiles/Minions/SheerHeartAttack";

        private bool saidKocchiwomiro = false;
        private bool[] targettedEnemy = new bool[Main.maxNPCs];
        private const float detectionRange = 20f * 16f;
        private NPC npcTarget = null;
        private int enemiesHit = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1800;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }


        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (SpecialKeyPressedNoCooldown())
            {
                Projectile.position = player.position;
                Projectile.netUpdate = true;
            }
            if (Projectile.velocity.Y < 6f)
            {
                Projectile.velocity.Y += 0.3f;
            }

            if (npcTarget == null)
            {
                saidKocchiwomiro = false;
                Projectile.rotation = 0f;
                Projectile.velocity.X = 0f;
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !targettedEnemy[npc.whoAmI] && Projectile.Distance(npc.Center) < detectionRange)
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
                    SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/GameSounds/Kocchiwomiro"));
                    saidKocchiwomiro = true;
                }
                if (npcTarget.position.X > Projectile.position.X)
                {
                    Projectile.direction = 1;
                }
                if (npcTarget.position.X <= Projectile.position.X)
                {
                    Projectile.direction = -1;
                }

                if (!npcTarget.noGravity)
                {
                    Projectile.rotation = 0f;
                    Projectile.spriteDirection = Projectile.direction;
                    Projectile.velocity.X = 3f * Projectile.direction;
                    if (WorldGen.SolidTile((int)(Projectile.Center.X / 16f) + Projectile.direction, (int)(Projectile.Center.Y / 16f)))
                    {
                        Projectile.velocity.Y = -6f;
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    Projectile.spriteDirection = 1;
                    Vector2 velocity = npcTarget.position - Projectile.position;
                    velocity.Normalize();
                    Projectile.rotation = velocity.ToRotation();
                    Projectile.velocity = velocity * 3f;
                }
            }
        }

        public override bool PreDraw(ref Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            SyncAndApplyDyeSlot();
            return true;
        }

        public override void PostDraw(Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);        //starting a draw with dyes that work
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            target.immune[Projectile.owner] = 0;
            if (Projectile.ai[0] == 0f)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.GrenadeIII, (int)(350 * mPlayer.standDamageBoosts), 7f, Projectile.owner);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].timeLeft = 2;
                Main.projectile[proj].netUpdate = true;
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(10));
            }
            if (Projectile.ai[0] == 1f)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.GrenadeIII, (int)(724 * mPlayer.standDamageBoosts), 7f, Projectile.owner);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].timeLeft = 2;
                Main.projectile[proj].netUpdate = true;
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(4));
            }
            enemiesHit++;
            if (enemiesHit >= 5)
            {
                Projectile.Kill();
            }
            npcTarget = null;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}