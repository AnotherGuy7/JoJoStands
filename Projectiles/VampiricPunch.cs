using JoJoStands.Buffs.Debuffs;
using JoJoStands.Items.Vampire;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class VampiricPunch : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.friendly = true;
            DrawOriginOffsetY = 15;
            Projectile.scale = (int)1.5;
            Projectile.timeLeft = 30;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 centerOffset = new Vector2((player.width / 2f) * player.direction, -24f);
            if (player.direction == -1)
                centerOffset.X -= 24f;

            Projectile.position = player.Center + centerOffset;
            Projectile.spriteDirection = Projectile.direction = player.direction;

            Projectile.frameCounter++;
            if (Projectile.frame < 4 && Projectile.frameCounter >= 5)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();

            vPlayer.StealHealthFrom(target, damage);
            target.GetGlobalNPC<JoJoGlobalNPC>().vampireUserLastHitIndex = player.whoAmI;
            if (vPlayer.HasSkill(player, VampirePlayer.SavageInstincts))
                if (Main.rand.Next(0, 100) <= vPlayer.lacerationChance)
                    target.AddBuff(ModContent.BuffType<Lacerated>(), (vPlayer.GetSkillLevel(player, VampirePlayer.SavageInstincts) * 4) * 60);
        }
    }
}