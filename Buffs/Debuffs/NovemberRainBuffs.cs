using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class RainBarrierCooldown : JoJoBuff
    {
        public override string Texture => "JoJoStands/Buffs/Debuffs/AbilityCooldown";

        public override void SetStaticDefaults()
        {
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }

    public class MaelstromCooldown : JoJoBuff
    {
        public override string Texture => "JoJoStands/Buffs/Debuffs/AbilityCooldown";

        public override void SetStaticDefaults()
        {
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }

    public class MaelstromDrain : JoJoBuff
    {
        public override string Texture => "JoJoStands/Buffs/Debuffs/AbilityCooldown";

        public override void SetStaticDefaults()
        {
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }
}
