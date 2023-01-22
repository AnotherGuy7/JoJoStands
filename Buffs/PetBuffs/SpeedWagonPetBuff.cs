using JoJoStands.Projectiles.Pets.Part1;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.PetBuffs
{
    public class SpeedWagonPetBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Speedwagon Pet");
            Description.SetDefault("He's rooting for you.");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SpeedWagonPet>()] != 0)
                player.buffTime[buffIndex] = 2;
        }
    }
}