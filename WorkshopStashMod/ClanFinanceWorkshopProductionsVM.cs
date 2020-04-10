using System.Globalization;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace WorkshopStashMod
{
    public class ClanFinanceWorkshopProductionsVM : ViewModel
    {
        private readonly Workshop _ownWorkshopCopy;
        private readonly ItemCategory _inputType;

        private string _inputName;
        private string _amountInStash;
        private string _amountInTown;
        private string _priceInTown;
        private string _priceBrush;
        private ImageIdentifierVM _imageIdentifier;

        public ClanFinanceWorkshopProductionsVM(Workshop ownWorkshopCopy, ItemCategory inputType)
        {
            _ownWorkshopCopy = ownWorkshopCopy;
            _inputType = inputType;

            RefreshValues();
        }

        public override void RefreshValues()
        {
            var town = _ownWorkshopCopy.Settlement.GetComponent<Town>();
            var stash = MBObjectManager.Instance.GetObject<TownWorkshopStash>(x => x.Town == town);
            AmountInStash = (stash?.Stash.Where(x => x.EquipmentElement.Item.ItemCategory == _inputType).Sum(x => x.Amount) ?? 0).ToString();


            var items = town.Owner.ItemRoster.Where(x => x.EquipmentElement.Item.ItemCategory == _inputType && x.Amount > 0);
            var totalAmount = items.Sum(x => x.Amount);
            AmountInTown = totalAmount.ToString();
            var price = totalAmount == 0 ? 0 : items.Sum(x => town.GetItemPrice(x) * x.Amount) / (float)totalAmount;
            PriceInTown = ((int)price).ToString();

            PriceBrush = "Clan.Finance.TotalIncome.Text";
            if (price > _inputType.AverageValue)
            {
                PriceBrush = "Clan.Finance.TotalExpenses.Text";
            }

            var realItem = ItemObject.All.FirstOrDefault(x => _inputType == x.ItemCategory);
            InputName = _inputType.GetName().ToString();
            ImageIdentifier = new ImageIdentifierVM(realItem);
        }

        [DataSourceProperty]
        public string InputName { get => _inputName; set { if (_inputName == value) return; OnPropertyChanged(nameof(InputName)); _inputName = value; } }
        [DataSourceProperty]
        public string AmountInStash { get => _amountInStash; set { if (_amountInStash == value) return; OnPropertyChanged(nameof(AmountInStash)); _amountInStash = value; } }
        [DataSourceProperty]
        public string AmountInTown { get => _amountInTown; set { if (_amountInTown == value) return; OnPropertyChanged(nameof(AmountInTown)); _amountInTown = value; } }
        [DataSourceProperty]
        public string PriceInTown { get => _priceInTown; set { if (_priceInTown == value) return; OnPropertyChanged(nameof(PriceInTown)); _priceInTown = value; } }
        [DataSourceProperty]
        public string PriceBrush { get => _priceBrush; set { if (_priceBrush == value) return; OnPropertyChanged(nameof(PriceBrush)); _priceBrush = value; } }
        [DataSourceProperty]
        public ImageIdentifierVM ImageIdentifier { get => _imageIdentifier; set { if (_imageIdentifier == value) return; OnPropertyChanged(nameof(ImageIdentifier)); _imageIdentifier = value; } }
    }
}