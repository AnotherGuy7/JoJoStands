using Terraria.GameContent.ItemDropRules;

namespace JoJoStands.DropConditions
{
    public class WillToDestroyCondition : IItemDropRuleCondition
    {
        public string GetConditionDescription()
        {
            return "Drops while in the Underworld";
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
                return info.player.ZoneUnderworldHeight;

            return false;
        }
    }
}
