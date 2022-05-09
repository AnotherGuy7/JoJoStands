using JoJoStands.Items.CraftingMaterials;
using JoJoStands.NPCs;
using JoJoStands.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Phantom
{
    [AutoloadEquip(EquipType.Head)]
    public class PhantomHoodNeutral : ModItem
    {
        private int detectionTimer = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Hood (Neutral)");
            Tooltip.SetDefault("A helmet that is made with Ectoplasm infused with an otherworldly virus.");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 8;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PhantomChestplate>() && legs.type == ModContent.ItemType<PhantomLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Spirits around you are marked and upon death they violently explode.";

            //Giving NPCs the markers
            detectionTimer++;
            if (detectionTimer >= 360)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        JoJoGlobalNPC jojoNPC = npc.GetGlobalNPC<JoJoGlobalNPC>();
                        if (npc.lifeMax > 5 && !npc.friendly && !jojoNPC.taggedWithPhantomMarker)
                        {
                            Projectile.NewProjectile(player.GetSource_FromThis(), npc.position, npc.velocity, ModContent.ProjectileType<PhantomMarker>(), 0, 0f, Main.myPlayer, npc.whoAmI);
                            jojoNPC.taggedWithPhantomMarker = true;
                        }
                    }
                }
                detectionTimer = 0;
            }
            Lighting.AddLight(player.Center, 1.73f / 2f, 2.24f / 2f, 2.3f / 2f);
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.phantomHoodNeutralEquipped = true;

            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<PhantomHoodLong>();
                Item.SetDefaults(ModContent.ItemType<PhantomHoodLong>());
            }
            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<PhantomHoodShort>();
                Item.SetDefaults(ModContent.ItemType<PhantomHoodShort>());
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Ectoplasm, 10)
                .AddIngredient(ModContent.ItemType<CraftingMaterials.ViralPearl>())
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}