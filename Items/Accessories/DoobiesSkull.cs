using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Items.Accessories
{
    public class DoobiesSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Doobies Skull");
            // Tooltip.SetDefault("Summons vampiric snakes when hit");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 44;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<Vampire.VampirePlayer>().doobiesskullEquipped = true;
        }
    }
}
