using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class EchoesACT1 : StandItemClass
    {
        public override int standSpeed => 16;
        public override int standType => 1;
        public override string standProjectileName => "Echoes";
        public override int standTier => 2;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echoes (ACT 1)");
            Tooltip.SetDefault("WIP");
        }

        public override void SetDefaults()
        {
            Item.damage = 4;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}
