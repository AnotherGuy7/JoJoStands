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
            // Tooltip.SetDefault("Next 10 ranged stand attacks will be buffed\nRecover one attack per second while at rest.\nStand Damage is increased by 2% for every enemy around you.");
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

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            MyPlayer myPlayer = player.GetModPlayer<MyPlayer>();
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active && npc.lifeMax > 5 && !npc.townNPC && !npc.friendly && !npc.SpawnedFromStatue && npc.Distance(player.Center) <= 48 * 16)
                    myPlayer.standDamageBoosts += 0.02f;
            }
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
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}