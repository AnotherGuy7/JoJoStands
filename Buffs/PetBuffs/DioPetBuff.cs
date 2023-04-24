using JoJoStands.Projectiles.Pets.Part1;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.PetBuffs
{
    public class DioPetBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dio Pet");
            // Description.SetDefault("He abhors you.");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<DioPet>()] != 0)
                player.buffTime[buffIndex] = 2;
        }
    }
}