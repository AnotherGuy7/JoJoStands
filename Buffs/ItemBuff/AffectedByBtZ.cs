using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class AffectedByBtZ : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Zero");
            Description.SetDefault("You are stuck at zero.");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }

        public Vector2 zeroPosition;
        public bool savedZeroPos = false;

        public override void Update(Player player, ref int buffIndex)
        {
            if (!savedZeroPos)
            {
                zeroPosition = player.position;
                savedZeroPos = true;
            }
            else
            {
                player.position = zeroPosition;
            }
            if (player.GetModPlayer<MyPlayer>().BackToZero)
            {
                player.AddBuff(mod.BuffType("AffectedByBtZ"), 2);
            }
            player.controlUseItem = false;
            player.dash *= 0;
            player.bodyVelocity = Vector2.Zero;
            player.controlLeft = false;
            player.controlJump = false;
            player.controlRight = false;
            player.controlDown = false;
            player.controlQuickHeal = false;
            player.controlQuickMana = false;
            player.controlRight = false;
            player.controlUseTile = false;
            player.controlUp = false;
            player.maxRunSpeed *= 0;
            player.moveSpeed *= 0;
            player.gravity = 0f;

            int newDust = Dust.NewDust(player.position, player.width, player.height, 220);
            Main.dust[newDust].noGravity = true;
            Main.dust[newDust].velocity = Vector2.Zero;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            int newDust = Dust.NewDust(npc.position, npc.width, npc.height, 220);
            Main.dust[newDust].noGravity = true;
            Main.dust[newDust].velocity = Vector2.Zero;
            npc.GetGlobalNPC<NPCs.JoJoGlobalNPC>().affectedbyBtz = true;
            npc.AddBuff(mod.BuffType(Name), 1);
        }
    }
}