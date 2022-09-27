using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class EchoesACT3 : StandItemClass
    {
        public override int standSpeed => 8;
        public override int standType => 1;
        public override string standProjectileName => "Echoes";
        public override int standTier => 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echoes (ACT 3)");
            Tooltip.SetDefault("WIP");
        }

        public override void SetDefaults()
        {
            Item.damage = 64;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}
