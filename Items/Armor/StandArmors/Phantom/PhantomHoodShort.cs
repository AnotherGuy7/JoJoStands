using JoJoStands.NPCs;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Phantom
{
    [AutoloadEquip(EquipType.Head)]
    public class PhantomHoodShort : ModItem
    {
        private int detectionTimer = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Hood (Short-Ranged)");
            Tooltip.SetDefault("A helmet that is made with Ectoplasm infused with an otherworldly virus.\n+18% Stand Damage\n+1 Stand Speed");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 24;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PhantomChestplate>() && legs.type == ModContent.ItemType<PhantomLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Enemies around you are marked periodically.\nMarked enemies violently explode upon death and damage surrounding enemies!";

            //Giving NPCs the markers
            detectionTimer++;
            if (detectionTimer >= 10 * 60)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active)
                        {
                            JoJoGlobalNPC jojoNPC = npc.GetGlobalNPC<JoJoGlobalNPC>();
                            if (Main.rand.Next(1, 100 + 1) <= 30 && npc.lifeMax > 5 && !npc.friendly && !jojoNPC.taggedWithPhantomMarker)
                            {
                                int projectile = Projectile.NewProjectile(player.GetSource_FromThis(), npc.position, npc.velocity, ModContent.ProjectileType<PhantomMarker>(), 0, 0f, Main.myPlayer, npc.whoAmI);
                                Main.projectile[projectile].netUpdate = true;
                                jojoNPC.taggedWithPhantomMarker = true;
                            }
                        }
                    }
                }
                for (int d = 0; d < 15; d++)
                {
                    int dustIndex = Dust.NewDust(player.position + new Vector2(0f, player.height / 2f), player.width, player.height / 2, DustID.Electric, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-1f, 1f));
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].noLight = true;
                }
                detectionTimer = 0;
            }
            Lighting.AddLight(player.Center, 1.73f / 2f, 2.24f / 2f, 2.3f / 2f);
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.phantomHoodShortEquipped = true;
            mPlayer.standDamageBoosts += 0.18f;
            mPlayer.standSpeedBoosts += 1;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<PhantomHoodNeutral>();
                Item.SetDefaults(ModContent.ItemType<PhantomHoodNeutral>());
            }
            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<PhantomHoodLong>();
                Item.SetDefaults(ModContent.ItemType<PhantomHoodLong>());
            }
        }
    }
}