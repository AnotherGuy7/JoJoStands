using JoJoStands.Projectiles.PlayerStands.KingCrimson;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Seasonal
{
    public class KingClaus : StandItemClass
    {
        public override int standSpeed => 22;
        public override int standType => 1;
        public override int standTier => 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Agent Crimson");
            Tooltip.SetDefault("Donut enemies with a powerful punch and hold right-click to block off enemies and reposition!\nConsecutive Donuts deal greater damage.\nSpecial: Skip 10 seconds of time!\nSecond Special: Use Epitaph for 9 seconds!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 186;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            Projectile.NewProjectile(player.GetSource_FromThis(), player.position, player.velocity, ModContent.ProjectileType<KingClausStand>(), 0, 0f, Main.myPlayer);
            return true;
        }
    }
}
