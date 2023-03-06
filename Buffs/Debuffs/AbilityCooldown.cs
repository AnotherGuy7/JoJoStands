using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.Debuffs
{
    public class AbilityCooldown : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ability Cooldown");
            Description.SetDefault("You can no longer use any stand abilities...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }
}