using System;
using JoJoStands.Items.Vampire;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class VampiricPunch : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.friendly = true;
            drawOriginOffsetY = 15;
            projectile.scale = (int)1.5;
            projectile.timeLeft = 30;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 centerOffset = new Vector2((player.width / 2f) * player.direction, -24f);
            if (player.direction == -1)
            {
                centerOffset.X -= 24f;
            }
            projectile.position = player.Center + centerOffset;
            projectile.spriteDirection = projectile.direction = player.direction;

            projectile.frameCounter++;
            if (projectile.frame < 4 && projectile.frameCounter >= 5)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();

            vPlayer.StealHealthFrom(target, damage);
            target.GetGlobalNPC<JoJoGlobalNPC>().vampireUserLastHitIndex = player.whoAmI;
        }
    }
}