using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public class VampiricItemRecipe : ModRecipe
    {
        private int recipeItemType = -1;

        public VampiricItemRecipe(Mod mod, int itemType) : base(mod)
        {
            recipeItemType = itemType;
        }

        public override bool RecipeAvailable()
        {
            bool result = false;

            Player player = Main.LocalPlayer;
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (recipeItemType == mod.ItemType("ZombieAbilities") && vPlayer.zombie)
            {
                result = true;
            }
            else if (recipeItemType == mod.ItemType("VampireAbilities") && vPlayer.vampire)
            {
                result = true;
            }
            else if (recipeItemType == mod.ItemType("PerfectBeingAbilities") && vPlayer.perfectBeing)
            {
                result = true;
            }
            else if (recipeItemType == mod.ItemType("KnifeWielder") && vPlayer.zombie && vPlayer.HasSkill(player, VampirePlayer.KnifeWielder))
            {
                result = true;
            }
            else if (recipeItemType == mod.ItemType("EntrailAbilities") && vPlayer.zombie && vPlayer.HasSkill(player, VampirePlayer.EntrailAbilities))
            {
                result = true;
            }
            return result;
        }
    }
}
