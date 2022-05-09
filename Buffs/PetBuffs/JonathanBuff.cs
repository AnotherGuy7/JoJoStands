using JoJoStands.Projectiles.Pets.Part1;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.PetBuffs
{
    public class JonathanPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jonathan Pet");
            Description.SetDefault("He believes in you.");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<JonathanPet>()] != 0)
                player.buffTime[buffIndex] = 2;
        }
    }
}