using JoJoStands.Buffs.Debuffs;
using JoJoStands.Projectiles.PlayerStands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Networking;


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

        private bool crit = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (SpecialKeyPressed(false))
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
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/Kocchiwomiro"));
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

        private void Explode()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                crit = true;
            int bombDamage = (int)(350 * mPlayer.standDamageBoosts);
            if (Projectile.ai[0] == 1f)
                bombDamage = (int)(724 * mPlayer.standDamageBoosts);

            //Normal grenade explosion effects
            for (int i = 0; i < 30; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Alpha: 100, Scale: 1.5f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Alpha: 100, Scale: 3.5f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 7f;
                dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Alpha: 100, Scale: 1.5f);
                Main.dust[dustIndex].velocity *= 3f;
            }

            for (int i = 0; i < 25; i++)        //Extra explosion effects
            {
                float angle = (360f / 25f) * i;
                Vector2 dustPosition = Projectile.Center + (angle.ToRotationVector2() * 7f);
                Vector2 dustVelocity = dustPosition - Projectile.Center;
                dustVelocity.Normalize();
                dustVelocity *= 7f;
                int dustIndex = Dust.NewDust(dustPosition, Projectile.width, Projectile.height, DustID.Torch, dustVelocity.X, dustVelocity.Y, 100, Scale: 3.5f);
                Main.dust[dustIndex].noGravity = true;
            }

            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.lifeMax > 5 && !npc.friendly && !npc.hide && !npc.immortal && Vector2.Distance(Projectile.Center, npc.Center) <= 5f * 16f)
                    {
                        int hitDirection = -1;
                        if (npc.position.X - Projectile.position.X > 0)
                            hitDirection = 1;
                        int critMultiplayer = 0;
                        if (crit)
                            critMultiplayer = 1;
                        npc.StrikeNPC(bombDamage, 7f, hitDirection, crit);
                        SyncCall.SyncStandEffectInfo(player.whoAmI, npc.whoAmI, 5, critMultiplayer, bombDamage, hitDirection);
                    }
                }
            }
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player otherPlayer = Main.player[p];
                if (otherPlayer.active && !otherPlayer.dead)
                {
                    if (Vector2.Distance(Projectile.Center, otherPlayer.Center) > 5f * 16f)
                        continue;

                    if (p == otherPlayer.whoAmI || Main.player[Projectile.owner].team == otherPlayer.team)
                        bombDamage = (int)(bombDamage * 0.25f);

                    int hitDirection = -1;
                    if (otherPlayer.position.X - Projectile.position.X > 0)
                        hitDirection = 1;

                    otherPlayer.Hurt(PlayerDeathReason.ByCustomReason(otherPlayer.name + " had too high a heat signurature."), bombDamage, hitDirection);
                    SyncCall.SyncOtherPlayerExtraEffect(player.whoAmI, otherPlayer.whoAmI, 3, bombDamage, hitDirection, 0f, 0f);
                }
            }
            SoundEngine.PlaySound(SoundID.Item62);
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
            target.immune[Projectile.owner] = 0;
            Explode();

            enemiesHit++;
            if (enemiesHit >= 5)
                Projectile.Kill();

            npcTarget = null;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Projectile.ai[0] == 0f)
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(10));

            if (Projectile.ai[0] == 1f)
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(4));
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}