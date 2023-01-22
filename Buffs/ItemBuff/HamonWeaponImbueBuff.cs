using Terraria;

namespace JoJoStands.Buffs.ItemBuff
{
    public class HamonWeaponImbueBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hamon Weapon Imbue");
            Description.SetDefault("You're injecting Hamon into your weapons.");
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}