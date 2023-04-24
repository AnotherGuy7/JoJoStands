using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class HerbalTeaBag : ModItem
    {
        private int dustSpawnTimer = 0;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Herbal Tea Bag");
            // Tooltip.SetDefault("Next 10 ranged stand attacks will be buffed\nRecover one attack per second while at rest");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            dustSpawnTimer++;
            player.GetModPlayer<MyPlayer>().herbalTeaBag = true;
            if (dustSpawnTimer >= 2)
            {
                dustSpawnTimer = 0;
                for (int i = 0; i < player.GetModPlayer<MyPlayer>().herbalTeaBagCount; i++)
                {
                    int dustIndex = Dust.NewDust(player.position, player.width, player.height, DustID.IceTorch);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].noLight = true;
                }
            }
        }
    }
}