using JoJoStands.Buffs.Debuffs;
using JoJoStands.NPCs;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    // This class handles everything for our custom damage class
    // Any class that we wish to be using our custom damage class will derive from this class, instead of ModItem
    public abstract class HamonDamageClass : ModItem        //the main reason this class was made was so that things like the aja stone and hamon increasing items can affect all of them at once.
    {
        public virtual bool affectedByHamonScaling { get; } = false;
        // Custom items should override this to set their defaults
        public virtual void SafeSetDefaults()
        { }

        // By making the override sealed, we prevent derived classes from further overriding the method and enforcing the use of SafeSetDefaults()
        // We do this to ensure that the vanilla damage types are always set to false, which makes the custom damage type work
        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            Item.DamageType = DamageClass.Generic;
        }

        private int increaseCounter = 0;

        public void ChargeHamon()
        {
            Player player = Main.LocalPlayer;
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            bool specialPressed = false;
            if (!Main.dedServ)
                specialPressed = JoJoStands.SpecialHotKey.Current;

            if (player.breath <= 1)
                return;

            if (specialPressed)
            {

                increaseCounter++;
                player.velocity.X /= 3f;
                hamonPlayer.hamonIncreaseCounter = 0;
                hamonPlayer.chargingHamon = true;
                Dust.NewDust(player.position, player.width, player.height, 169, player.velocity.X * -0.5f, player.velocity.Y * -0.5f);
                if (increaseCounter % 10 == 0 && player.breath != player.breathMax)
                    player.breath -= 4;
            }
            if (increaseCounter >= 30)
            {
                hamonPlayer.amountOfHamon += 1;
                increaseCounter = 0;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Get the vanilla damage tooltip
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (tt != null)
            {
                // We want to grab the last word of the tooltip, which is the translated word for 'damage' (depending on what langauge the player is using)
                // So we split the string by whitespace, and grab the last word from the returned arrays to get the damage word, and the first to get the damage shown in the tooltip
                string[] splitText = tt.Text.Split(' ');
                // Change the tooltip text
                tt.Text = splitText.First() + " Hamon " + splitText.Last();
            }
        }

        public override void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit)
        {
            if (target.HasBuff(ModContent.BuffType<Buffs.AccessoryBuff.Vampire>()))
            {
                damage = (int)(damage * 1.3f);
                target.AddBuff(ModContent.BuffType<Sunburn>(), 90);
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            damage *= hamonPlayer.hamonDamageBoosts;

            if (!affectedByHamonScaling)
                return;

            if (NPC.downedBoss1)    //eye of cthulu
            {
                damage += 1.15f;     //this is 14%
            }
            if (NPC.downedBoss2)      //the crimson/corruption bosses
            {
                damage += 1.17f;
            }
            if (NPC.downedBoss3)       //skeletron
            {
                damage += 1.13f;
            }
            if (Main.hardMode)      //wall of flesh
            {
                damage *= 1.3f;
            }
            if (NPC.downedMechBoss1)        //any mechboss orders
            {
                damage += 1.18f;
            }
            if (NPC.downedMechBoss2)
            {
                damage += 1.16f;
            }
            if (NPC.downedMechBoss3)
            {
                damage += 1.18f;
            }
            if (NPC.downedPlantBoss)       //plantera
            {
                damage += 1.19f;
            }
            if (NPC.downedGolemBoss)        //golem
            {
                damage += 1.19f;
            }
            if (NPC.downedMoonlord)     //moon lord
            {
                damage += 1.29f;
            }
            if (hamonPlayer.amountOfHamon >= hamonPlayer.maxHamon / 2)     //more than half of maxHamon
            {
                damage *= 1.5f;
            }
        }

        public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            knockback *= hamonPlayer.hamonKnockbackBoosts;
        }
    }
}