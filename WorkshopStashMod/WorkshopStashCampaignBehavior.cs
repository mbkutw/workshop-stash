using Helpers;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace WorkshopStashMod
{
    public class WorkshopStashCampaignBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener((object)this, new Action<CampaignGameStarter>(this.OnAfterNewGameCreated));
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener((object)this, new Action<CampaignGameStarter>(this.OnAfterNewGameCreated));
        }

        private void OnAfterNewGameCreated(CampaignGameStarter starter)
        {
            starter.AddGameMenuOption("town", "town_workshop_stash", "Visit your workshops", new GameMenuOption.OnConditionDelegate(HasAnyWorkshops), new GameMenuOption.OnConsequenceDelegate(_ => GameMenu.SwitchToMenu("town_workshop_stash")), false, 6, false);
            starter.AddGameMenu("town_workshop_stash", "You are visiting your workshops.", new OnInitDelegate(args => { args.MenuTitle = new TextObject("Workshops", null); }), GameOverlays.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.none, (object)null);
            starter.AddGameMenuOption("town_workshop_stash", "town_workshop_stash_browse", "Browse your stash", new GameMenuOption.OnConditionDelegate(StashCondition), new GameMenuOption.OnConsequenceDelegate(StashConsequence), false, -1, false);
            starter.AddGameMenuOption("town_workshop_stash", "town_workshop_stash_toggle_input", "Use materials from stash: {STASH_INPUT}", new GameMenuOption.OnConditionDelegate(ProductionCondition), new GameMenuOption.OnConsequenceDelegate(ToggleInput), false, -1, false);
            starter.AddGameMenuOption("town_workshop_stash", "town_workshop_stash_toggle_output", "Place finished products in stash: {STASH_OUTPUT}", new GameMenuOption.OnConditionDelegate(ProductionCondition), new GameMenuOption.OnConsequenceDelegate(ToggleOutput), false, -1, false);
            starter.AddGameMenuOption("town_workshop_stash", "town_workshop_stash_back", "Back to town center", new GameMenuOption.OnConditionDelegate(BackCondition), new GameMenuOption.OnConsequenceDelegate(_ => GameMenu.SwitchToMenu("town")), true, -1, false);
        }

        private static bool ProductionCondition(MenuCallbackArgs args)
        {
            var stash = GetCurrentSettlementStash();
            MBTextManager.SetTextVariable("STASH_INPUT", stash.InputFromStash ? "Yes" : "No");
            MBTextManager.SetTextVariable("STASH_OUTPUT", stash.OutputToStash ? "Yes" : "No");

            args.optionLeaveType = GameMenuOption.LeaveType.Craft;
            return true;
        }

        private static void ToggleInput(MenuCallbackArgs args)
        {
            var stash = GetCurrentSettlementStash();
            stash.InputFromStash = !stash.InputFromStash;
            GameMenu.SwitchToMenu("town_workshop_stash");
        }

        private static void ToggleOutput(MenuCallbackArgs args)
        {
            var stash = GetCurrentSettlementStash();
            stash.OutputToStash = !stash.OutputToStash;
            GameMenu.SwitchToMenu("town_workshop_stash");
        }

        private static bool BackCondition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Leave;
            return true;
        }

        private static bool HasAnyWorkshops(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
            return Settlement.CurrentSettlement.GetComponent<Town>().Workshops.Any(x => x.Owner == Hero.MainHero);
        }

        public override void SyncData(IDataStore dataStore)
        {
            return;
        }

        private static bool StashCondition(MenuCallbackArgs args)
        {
            bool disableOption;
            TextObject disabledText;
            bool canPlayerDo = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.Trade, out disableOption, out disabledText);
            args.optionLeaveType = GameMenuOption.LeaveType.Trade;
            return MenuHelper.SetOptionProperties(args, canPlayerDo, disableOption, disabledText);
        }

        private static void StashConsequence(MenuCallbackArgs args)
        {
            var stash = GetCurrentSettlementStash();
            InventoryManager.OpenScreenAsStash(stash.Stash);
        }

        private static TownWorkshopStash GetCurrentSettlementStash()
        {
            var town = Settlement.CurrentSettlement.GetComponent<Town>();
            var stash = MBObjectManager.Instance.GetObject<TownWorkshopStash>(x => x.Town == town);

            if (stash == null)
            {
                stash = MBObjectManager.Instance.CreateObject<TownWorkshopStash>();
                stash.Town = town;
            }

            return stash;
        }
    }
}
