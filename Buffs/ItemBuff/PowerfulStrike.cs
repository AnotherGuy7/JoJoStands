using Terraria;

namespace JoJoStands.Buffs.ItemBuff
{
    public class PowerfulStrike : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Powerful Strike");
            Description.SetDefault("Your next attack deals 6x more damage.");
            Main.persistentBuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)       //it should only affect the user with this buff on
        {
            player.buffTime[buffIndex] = 2;
        }
    }
}