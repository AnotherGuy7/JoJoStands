using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace JoJoStands.DropConditions
{
    public class HerbalTeaBagCondition : IItemDropRuleCondition
    {
        public string GetConditionDescription()
        {
            return "Drops while in the Jungle";
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
                return info.player.ZoneJungle;

            return false;
        }
    }
}
