using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using JoJoStands.NPCs;

namespace JoJoStands.Items.Armor
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
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.defense = 14;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("PhantomChestplate") && legs.type == mod.ItemType("PhantomLeggings");
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
                            Projectile.NewProjectile(npc.position, npc.velocity, mod.ProjectileType("PhantomMarker"), 0, 0f, Main.myPlayer, npc.whoAmI);
                            jojoNPC.taggedWithPhantomMarker = true;
                        }
                    }
                }
                detectionTimer = 0;
            }
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("PhantomHoodLong");
                item.SetDefaults(mod.ItemType("PhantomHoodLong"));
            }
            if (mPlayer.standType == 1)
            {
                item.type = mod.ItemType("PhantomHoodShort");
                item.SetDefaults(mod.ItemType("PhantomHoodShort"));
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Ectoplasm, 10);
            recipe.AddIngredient(mod.ItemType("ViralPearl"));
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}