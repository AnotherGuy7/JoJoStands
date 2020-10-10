using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class RedBindDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Red Bind");
            Description.SetDefault("You are bound by fire...");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.velocity /= 1.5f;
            player.statDefense -= 15;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.velocity.X = 0f;
            npc.AddBuff(BuffID.OnFire, 2);
            npc.AddBuff(BuffID.Suffocation, 2);
        }
    }
}