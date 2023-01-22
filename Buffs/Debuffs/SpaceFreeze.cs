using Terraria;

namespace JoJoStands.Buffs.Debuffs
{
    public class SpaceFreeze : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Space Freeze!");
            Description.SetDefault("You went too high and are now going to stay in space for the rest of eternity... Have a good time!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.lifeRegen > 0)
                player.lifeRegen = 0;

            player.velocity.Y += -0.2f;
            player.velocity.X += -0.2f;
            player.lifeRegenTime = 0;
            player.lifeRegen -= 120;
            player.moveSpeed *= 0.5f;
        }
    }
}