using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class DiosScarf : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dio's Scarf");
            Tooltip.SetDefault("A scarf that's been through many fights, betrayals, and murders...\nGrants 15% Damage Resistance to undead enemies.");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(gold: 2, silver: 25);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<VampirePlayer>().wearingDiosScarf = true;
        }
    }
}
