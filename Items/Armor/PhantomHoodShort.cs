﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using JoJoStands.NPCs;

namespace JoJoStands.Items.Armor
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
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.defense = 24;
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
            Lighting.AddLight(player.Center, 1.73f / 2f, 2.24f / 2f, 2.3f / 2f);
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.phantomHoodShortEquipped = false;
            mPlayer.standCritChangeBoosts += 10f;
            mPlayer.standSpeedBoosts += 1;

            if (mPlayer.standType == 0)
            {
                item.type = mod.ItemType("PhantomHoodNeutral");
                item.SetDefaults(mod.ItemType("PhantomHoodNeutral"));
            }
            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("PhantomHoodLong");
                item.SetDefaults(mod.ItemType("PhantomHoodLong"));
            }
        }
    }
}