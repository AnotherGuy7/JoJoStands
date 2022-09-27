using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Buffs.EffectBuff
{
    public class ThreeFreezeBarrage : ModBuff
    {
        public override string Texture { get { return "Terraria/Images/Buff_" + BuffID.Wrath; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("3 Freeze Barrage");
            Description.SetDefault("WIP.");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }
    }
}