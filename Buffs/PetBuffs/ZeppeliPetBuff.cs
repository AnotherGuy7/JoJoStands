using JoJoStands.Projectiles.Pets.Part1;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.PetBuffs
{
    public class ZeppeliPetBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zeppeli Pet");
            Description.SetDefault("He says he will leave if you spill the wine.");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ZeppeliPet>()] != 0)
                player.buffTime[buffIndex] = 2;
        }
    }
}