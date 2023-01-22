using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class AffectedByBtZ : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zero");
            Description.SetDefault("You are stuck at zero.");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }

        private Vector2 zeroPosition;

        public override void OnApply(Player player)
        {
            zeroPosition = player.position;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.position = zeroPosition;
            if (player.GetModPlayer<MyPlayer>().backToZeroActive)
                player.buffTime[buffIndex] = 2;

            player.controlUseItem = false;
            player.dashType *= 0;
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

        public override void UpdateBuffOnNPC(NPC npc)
        {
            int newDust = Dust.NewDust(npc.position, npc.width, npc.height, 220);
            Main.dust[newDust].noGravity = true;
            Main.dust[newDust].velocity = Vector2.Zero;
            npc.GetGlobalNPC<NPCs.JoJoGlobalNPC>().affectedbyBtz = true;
            npc.AddBuff(Type, 1);
        }
    }
}