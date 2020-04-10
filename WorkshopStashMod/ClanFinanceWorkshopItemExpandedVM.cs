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

namespace WorkshopStashMod
{
    public class ClanFinanceWorkshopItemExpandedVM : ClanFinanceWorkshopItemVM
    {
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

        public override void RefreshValues()
        {
            base.RefreshValues();
            RefreshProductions();
        }

        protected void RefreshProductions()
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
