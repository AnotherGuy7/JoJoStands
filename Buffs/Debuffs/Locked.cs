using Terraria;

namespace JoJoStands.Buffs.Debuffs
{
    public class Locked : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Locked");
            Description.SetDefault("Your guilt is increasing and it hurts.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}