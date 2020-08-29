using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class ArtificialSoul : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Artificial Soul");
            Description.SetDefault("An artificial soul has been given to you!");
            canBeCleared = false;
            Main.debuff[Type] = true;
        }
    }
}