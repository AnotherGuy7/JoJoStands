using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.ItemBuff
{
    public class SwanSong : JoJoBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Rage;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swan Song");
            Description.SetDefault("You're not dead just yet...");
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.GetModPlayer<MyPlayer>().zippedHandDeath = true;
            player.GetModPlayer<MyPlayer>().standDamageBoosts *= 2f;
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts *= 2f;
        }
    }
}