using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class CrackedPearl : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cracked Pearl");
            // Tooltip.SetDefault("A pearl that cracked during the removal process from the ring. It seems to leak a peculiar virus constantly.\nWhen worn: Punching enemies with Stands can infect enemies with the virus.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(gold: 45);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().crackedPearlEquipped = true;
        }
    }
}
