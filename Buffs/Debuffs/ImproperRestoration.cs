using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.Debuffs
{
    public class ImproperRestoration : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Improper Restoration");
            // Description.SetDefault("Things didn't go as planned while healing.");
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            Dust.NewDust(player.position + player.velocity, player.width, player.height, DustID.Stone, player.velocity.X * -0.5f, player.velocity.Y * -0.5f);
            player.moveSpeed = 0;
            player.stoned = true;
            player.noFallDmg = false;
            player.slowFall = false;
            player.noKnockback = true;
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.controlLeft = false;
            player.controlRight = false;
            player.controlUp = false;
            player.controlDown = false;
            player.controlJump = false;
            player.controlQuickHeal = false;
            player.controlQuickMana = false;
            player.controlHook = false;
            player.controlThrow = false;
            player.controlMount = false;
            player.gravControl = false;
            player.gravControl2 = false;
            player.controlTorch = false;
            player.preventAllItemPickups = true;
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Stone, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f);
            npc.velocity = new Vector2(0f, 1f);
            if (!npc.noTileCollide)
                npc.velocity.Y *= 12f;
        }
    }
}