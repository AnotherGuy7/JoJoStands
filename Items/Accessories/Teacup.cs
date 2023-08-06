using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class Teacup : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Teacup");
            // Tooltip.SetDefault("Next 10 ranged stand attacks will be buffed\nRecover one attack per second while at rest.\nStand Damage is increased by 3% for every enemy around you and damage gains are multiplied by 12% when a boss is around you.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 6);
            Item.maxStack = 1;
        }

        private int dustSpawnTimer = 0;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            MyPlayer myPlayer = player.GetModPlayer<MyPlayer>();
            float totalMultiplier = 1f;
            bool bossExists = false;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active && npc.lifeMax > 5 && !npc.townNPC && !npc.friendly && !npc.SpawnedFromStatue && npc.Distance(player.Center) <= 48 * 16)
                {
                    if (npc.boss)
                        bossExists = true;

                    totalMultiplier += 0.03f;
                }
            }
            myPlayer.standDamageBoosts += bossExists ? totalMultiplier * 1.12f : totalMultiplier;
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

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SoothingSpiritDisc>())
                .AddIngredient(ModContent.ItemType<HerbalTeaBag>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}