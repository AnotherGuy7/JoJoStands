using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class YoAngelo : ModBuff
    {

        public override string Texture { get { return "Terraria/Images/Buff_" + BuffID.Stoned; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yo, Angelo");
            Description.SetDefault("You are historical landmark!");
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
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
        public override void Update(NPC npc, ref int buffIndex)
        {
            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Stone, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f);
            npc.velocity = new Vector2(Vector2.Zero.X, npc.position.Y - 100f);
            npc.velocity.Normalize();
            if (!npc.noTileCollide)
                npc.velocity.Y *= 12f;
        }
    }
}