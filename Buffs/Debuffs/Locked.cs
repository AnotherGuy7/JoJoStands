using JoJoStands.NPCs;
using Terraria;

namespace JoJoStands.Buffs.Debuffs
{
    public class Locked : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Locked");
            // Description.SetDefault("Your guilt is increasing and it hurts.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void OnApply(NPC npc)
        {
            npc.defense = (int)(npc.defense * 0.95);
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            JoJoGlobalNPC jojoNPC = npc.GetGlobalNPC<JoJoGlobalNPC>();
            npc.lifeRegen = -4;
            npc.velocity *= 0.95f;
            jojoNPC.lockRegenCounter++;
            if (jojoNPC.lockRegenCounter >= 60)    //increases lifeRegen damage every second
            {
                jojoNPC.lockRegenCounter = 0;
                bool lockCrit = Main.rand.NextFloat(1, 100 + 1) <= jojoNPC.theLockCrit;
                int defence = lockCrit ? 4 : 2;
                int lockDamage = !npc.boss ? 4 : 2;
                jojoNPC.lifeRegenIncrement += lockDamage;
                npc.StrikeNPC((int)(jojoNPC.lifeRegenIncrement * jojoNPC.theLockDamageBoost) + npc.defense / defence, 0f, 0, lockCrit);
            }
        }

        public override void OnBuffEnd(NPC npc)
        {
            npc.GetGlobalNPC<JoJoGlobalNPC>().lockFrame = 0;
            npc.GetGlobalNPC<JoJoGlobalNPC>().lockFrameCounter = 0;
        }
    }
}