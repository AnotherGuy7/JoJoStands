using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class TheFirstNapkin : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The First Napkin");
            /* Tooltip.SetDefault("Every kill reduces the Stand Ability Cooldown by 1s." +
                "\nReduces Ability Cooldown times by 8%."); */
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(gold: 8);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().firstNapkinEquipped = true;
            player.GetModPlayer<MyPlayer>().standCooldownReduction += 0.08f;
        }
    }
}