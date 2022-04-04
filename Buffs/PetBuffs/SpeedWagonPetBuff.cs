using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.PetBuffs
{
    public class SpeedWagonPetBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Speedwagon Pet");
            Description.SetDefault("He's rooting for you.");
            Main.buffNoTimeDisplay[Type] = false;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("SpeedWagonPet")] != 0)
            {
                player.buffTime[buffIndex] = 2;
            }
        }
    }
}