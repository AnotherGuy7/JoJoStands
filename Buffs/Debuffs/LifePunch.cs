using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class LifePunch : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Life Punched");
            Description.SetDefault("Your senses are accellarated and your body can't keep up with you!");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        private bool appliedChange = false;

        public override void Update(Player player, ref int buffIndex)
        {
            player.velocity.X *= 0.5f;
            player.statDefense -= 5;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.velocity.X *= 0.5f;
            if (!appliedChange)
            {
                npc.defense -= 5;
                appliedChange = true;
            }
        }
    }
}