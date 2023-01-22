using Terraria;

namespace JoJoStands.Buffs.ItemBuff
{
    public class CooledOut : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cooled Out");
            Description.SetDefault("You lowered your body temperature.");
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}