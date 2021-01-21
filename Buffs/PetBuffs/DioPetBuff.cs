using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.PetBuffs
{
    public class DioPetBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Dio Pet");
            Description.SetDefault("He abhors you.");
            Main.buffNoTimeDisplay[Type] = false;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("DioPet")] != 0)
            {
                player.buffTime[buffIndex] = 2;
            }
        }
    }
}