using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class EchoesACT2 : StandItemClass
    {
        public override int standSpeed => 12;
        public override int standType => 1;
        public override string standProjectileName => "Echoes";
        public override int standTier => 3;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echoes (ACT 2)");
            Tooltip.SetDefault("WIP");
        }

        public override void SetDefaults()
        {
            Item.damage = 44;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}
