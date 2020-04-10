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
            PriceBrush = "Clan.Finance.TotalIncome.Text";
            var index = town.Owner.ItemRoster.FindIndex(x => x.ItemCategory == _inputType);
            if (index < 0)
            {
                AmountInTown = 0.ToString();
                PriceInTown = 0.ToString();
            }
            else
            {
                AmountInTown = town.Owner.ItemRoster.GetElementNumber(index).ToString();
                var item = town.Owner.ItemRoster.GetItemAtIndex(index);
                var price = town.GetItemPrice(item);
                PriceInTown = price.ToString();

                if(price > _inputType.AverageValue)
                {
                    PriceBrush = "Clan.Finance.TotalExpenses.Text";
                }
            }

            var realItem = ItemObject.AllTradeGoods.FirstOrDefault(x => _inputType == x.ItemCategory);
            InputName = realItem.Name.ToString();
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