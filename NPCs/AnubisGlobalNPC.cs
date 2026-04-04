using JoJoStands.Projectiles.PlayerStands.Anubis;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.NPCs
{
    public class AnubisGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => false;

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (!IsMelee(item.DamageType)) return;
            TryApplyParryBonus(player, ref modifiers);
            TryApplyArmorShred(npc, player, ref modifiers);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers) return;
            if (!IsMelee(projectile.DamageType)) return;
            Player player = Main.player[projectile.owner];
            if (!player.active || player.dead) return;
            TryApplyParryBonus(player, ref modifiers);
            TryApplyArmorShred(npc, player, ref modifiers);
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            AnubisStand anubis = FindAnubisStand(target.whoAmI);
            if (anubis == null) return;

            if (anubis.IsInParryPerfectWindow())
                modifiers.FinalDamage *= 0f;
            else if (anubis.IsInParryGoodWindow())
                modifiers.FinalDamage *= 0.5f;
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (!IsMelee(item.DamageType)) return;
            TryGrantStack(npc, player);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers) return;
            if (!IsMelee(projectile.DamageType)) return;
            Player player = Main.player[projectile.owner];
            if (!player.active || player.dead) return;
            TryGrantStack(npc, player);
        }

        public override void OnKill(NPC npc)
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                AnubisStand anubis = FindAnubisStand(i);
                anubis?.OnNPCKilled(npc);
            }
        }

        private static void TryApplyParryBonus(Player player, ref NPC.HitModifiers modifiers)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!mPlayer.standOut) return;
            AnubisStand anubis = FindAnubisStand(player.whoAmI);
            if (anubis == null) return;

            if (anubis.IsInParryPerfectWindow())
                modifiers.SetCrit();
        }

        private static void TryApplyArmorShred(NPC npc, Player player, ref NPC.HitModifiers modifiers)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!mPlayer.standOut) return;
            AnubisStand anubis = FindAnubisStand(player.whoAmI);
            if (anubis == null) return;

            int shred = anubis.GetAndApplyArmorShred(npc);
            if (shred > 0)
                modifiers.Defense.Flat -= shred;
        }

        private static void TryGrantStack(NPC npc, Player player)
        {
            if (!npc.active || npc.friendly) return;
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!mPlayer.standOut) return;
            AnubisStand anubis = FindAnubisStand(player.whoAmI);
            if (anubis == null) return;

            anubis.RegisterMeleeHit(npc.whoAmI);
            AnubisStand.SpawnStackGainDust(npc.Center);
        }

        private static bool IsMelee(DamageClass dc)
            => dc == DamageClass.Melee || dc == DamageClass.MeleeNoSpeed;

        public static AnubisStand FindAnubisStand(int playerWhoAmI)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (!p.active || p.owner != playerWhoAmI) continue;
                if (p.ModProjectile is AnubisStand anubis) return anubis;
            }
            return null;
        }
    }
}