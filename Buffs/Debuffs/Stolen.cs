using JoJoStands.NPCs;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.Debuffs
{
    public class Stolen : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stolen");
            Description.SetDefault("Your stand disc has been stolen!");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.GetModPlayer<MyPlayer>().standOut = false;
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            if (!npc.boss)
                npc.velocity.X = 0f;
            int damage = (int)(npc.lifeMax * 0.015f);
            if (npc.GetGlobalNPC<JoJoGlobalNPC>().whitesnakeDISCImmune > 1)
                damage /= npc.GetGlobalNPC<JoJoGlobalNPC>().whitesnakeDISCImmune;
            if (damage < (int)(npc.lifeMax * 0.0015f))
                damage = (int)(npc.lifeMax * 0.0015f);
            if (damage < 10)
                damage = 10;
            npc.lifeRegen = -damage;
            npc.lifeRegenExpectedLossPerSecond = damage;
        }
    }
}