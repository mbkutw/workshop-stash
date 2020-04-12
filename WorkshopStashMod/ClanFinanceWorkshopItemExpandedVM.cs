using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement;
using TaleWorlds.Library;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;

namespace WorkshopStashMod
{
    public class ClanFinanceWorkshopItemExpandedVM : ClanFinanceWorkshopItemVM
    {
        protected virtual int InvestAmount { get; set; } = 1000;
        private readonly Workshop _ownWorkshopCopy;
        private MBBindingList<ClanFinanceWorkshopProductionsVM> _productions;

        public ClanFinanceWorkshopItemExpandedVM(Workshop workshop,
            Action<ClanFinanceIncomeItemBaseVM> onSelection,
            Action onRefresh) : base(workshop, onSelection, onRefresh)
        {
            _ownWorkshopCopy = workshop;
            _productions = new MBBindingList<ClanFinanceWorkshopProductionsVM>();
            RefreshValues();
        }

        public MBBindingList<ClanFinanceWorkshopProductionsVM> Productions
        {
            get => _productions;
            set
            {
                if (value == _productions) { return; }
                _productions = value;
                this.OnPropertyChanged(nameof(Productions));
            }
        }

        protected override void PopulateActionList()
        {
            base.PopulateActionList();
            var enabled = CanInvestInWorkshop(out string explanation);
            ActionList.Add(new StringItemWithEnabledAndHintVM(ExecuteInvestCapital, "Invest capital", enabled, null, explanation));
        }

        private bool CanInvestInWorkshop(out string explanation)
        {
            if (InvestAmount <= Hero.MainHero.Gold)
            {
                explanation = $"Invest {InvestAmount} denars in the workshop.";
                return true;
            }
            else
            {
                explanation = $"You can not afford to invest {InvestAmount} denars in the workshop.";
                return false;
            }
        }

        protected virtual void ExecuteInvestCapital(object identifier)
        {
            if (this.IncomeTypeAsEnum != IncomeTypes.Workshop || this._ownWorkshopCopy == null)
                return;
            Hero.MainHero.ChangeHeroGold(-InvestAmount);
            _ownWorkshopCopy.ChangeGold(InvestAmount);

            ItemProperties.Clear();
            PopulateStatsList();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            RefreshProductions();
        }

        protected virtual void RefreshProductions()
        {
            _productions.Clear();
            var inputTypes = _ownWorkshopCopy.WorkshopType.Productions.SelectMany(x => x.Inputs).Select(x => x.Item1).Distinct().ToHashSet();

            foreach (var inputType in inputTypes)
            {
                _productions.Add(new ClanFinanceWorkshopProductionsVM(_ownWorkshopCopy, inputType));
            }
        }
    }
}
