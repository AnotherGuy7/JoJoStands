using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class RainBarrierActive : JoJoBuff
    {
        public override string Texture => "JoJoStands/Buffs/EffectBuff/RainBarrierActive";

        public override void SetStaticDefaults()
        {
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }
}
