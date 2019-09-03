using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class BacktoZero : ModBuff
    {
        public int savedLife = 0;

        public override void SetDefaults()
        {
			DisplayName.SetDefault("Back to Zero");
            Description.SetDefault("Enemies that hurt you never touched you at all...");
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)       //it should only affect the user with this buff on
        {
            if (player.HasBuff(mod.BuffType(Name)))
            {
                player.GetModPlayer<MyPlayer>().BackToZero = true;
                if (savedLife == 0)
                {
                    savedLife = player.statLife;
                }
                if (player.statLife > savedLife)
                {
                    player.statLife = savedLife;
                }
                player.statDefense += 900;
                player.lifeRegen += 2;
            }
            else
            {
                player.AddBuff(mod.BuffType("GERAbilityCooldown"), 2100);
                player.GetModPlayer<MyPlayer>().BackToZero = false;
            }
        }
    }
}