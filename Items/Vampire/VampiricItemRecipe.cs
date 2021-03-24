using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public class VampiricItemRecipe : ModRecipe
    {
        private bool itemCraftRequirementMet = false;

        public VampiricItemRecipe(Mod mod, bool requirement) : base(mod)
        {
            itemCraftRequirementMet = requirement;
        }

        public override bool RecipeAvailable()
        {
            return itemCraftRequirementMet;
        }
    }
}
