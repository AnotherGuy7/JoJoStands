using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.PetBuffs
{
    public class ZeppeliPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zeppeli Pet>();
            Description.SetDefault("He says he will leave if you spill the wine.>();
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ZeppeliPet>()] != 0)
            {
                player.buffTime[buffIndex] = 2;
            }
        }
    }
}