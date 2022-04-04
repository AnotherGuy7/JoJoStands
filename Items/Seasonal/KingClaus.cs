using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Seasonal
{
    public class KingClaus : StandItemClass
    {
        public override int standSpeed => 20;
        public override int standType => 1;
        public override int standTier => 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Agent Crimson");
            Tooltip.SetDefault("Donut enemies with a powerful punch and hold right-click to block off enemies and reposition!\nConsecutive Donuts deal greater damage.\nSpecial: Skip 10 seconds of time!\nSecond Special: Use Epitaph for 9 seconds!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 186;
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("KingClausStand"), 0, 0f, Main.myPlayer);
            return true;
        }
    }
}
