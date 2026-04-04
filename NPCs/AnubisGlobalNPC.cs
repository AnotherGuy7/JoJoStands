using JoJoStands.Projectiles.PlayerStands.Anubis;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.NPCs
{
    public class AnubisGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => false;

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            TryGrantStack(npc, player, item.DamageType);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers) return;
            Player player = Main.player[projectile.owner];
            if (!player.active || player.dead) return;

            if (projectile.DamageType != DamageClass.Melee &&
                projectile.DamageType != DamageClass.MeleeNoSpeed)
                return;

            TryGrantStack(npc, player, DamageClass.Melee);
        }

        private static void TryGrantStack(NPC npc, Player player, DamageClass damageClass)
        {
            if (damageClass != DamageClass.Melee &&
                damageClass != DamageClass.MeleeNoSpeed)
                return;

            if (!npc.active || npc.friendly) return;

            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (!mPlayer.standOut) return;

            AnubisStand anubis = FindAnubisStand(player.whoAmI);
            if (anubis == null) return;

            anubis.RegisterMeleeHit(npc.whoAmI);

            AnubisStand.SpawnStackGainDust(npc.Center);
        }

        private static AnubisStand FindAnubisStand(int playerWhoAmI)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (!p.active || p.owner != playerWhoAmI) continue;
                if (p.ModProjectile is AnubisStand anubis)
                    return anubis;
            }
            return null;
        }
    }
}