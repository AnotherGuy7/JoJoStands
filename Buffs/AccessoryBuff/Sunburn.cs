using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.AccessoryBuff
{
    public class Sunburn : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Sunburn");
            Description.SetDefault("You're burning in the sunlight!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0;
            }
            player.lifeRegenTime = 0;
            player.lifeRegen -= 60;     //losing 30 health
            player.moveSpeed *= 0.5f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            npc.lifeRegenExpectedLossPerSecond = 30;
            npc.lifeRegen -= 60;     //losing 30 health
        }
    }
}