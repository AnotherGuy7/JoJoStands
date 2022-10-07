using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Buffs.EffectBuff
{
    public class ThreeFreezeBarrage : ModBuff
    {
        public override string Texture { get { return "Terraria/Images/Buff_" + BuffID.Rage; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("3 Freeze Barrage");
            Description.SetDefault("Each of your punches is much harder than it might seem!");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }
    }
}