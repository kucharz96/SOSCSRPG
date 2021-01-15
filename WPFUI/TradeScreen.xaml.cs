using System.Linq;
using System.Windows;
using Engine.Models;
using Engine.ViewModels;
using WPFUI.CustomControls;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for TradeScreen.xaml
    /// </summary>
    public partial class TradeScreen : Window
    {
        private GameSession _gameSession;
        public bool? isSelling;
        public TradeScreen(GameSession session)
        {
            InitializeComponent();
            _gameSession = session;
            isSelling = null;
            Rebuild();
        }

        public void Rebuild()
        {
            int counter = 0;
            foreach (var b in tradePlayerInventoryPack.Children.Cast<InventoryButton>())
            {
                b.item = null;
                b.image.Source = null;
            }


            foreach (var item in _gameSession.CurrentPlayer.Inventory.Items)
            {
                var button = tradePlayerInventoryPack.Children.Cast<InventoryButton>().ElementAt(counter);
                button.setItem(item, true);
                counter++;
            }

            int counterTrader = 0;
            foreach (var b in tradeTraderInventoryPack.Children.Cast<InventoryButton>())
            {
                b.item = null;
                b.image.Source = null;
            }


            foreach (var item in _gameSession.CurrentTrader.Inventory.Items)
            {
                var button = tradeTraderInventoryPack.Children.Cast<InventoryButton>().ElementAt(counterTrader);
                button.setItem(item, true);
                counterTrader++;
            }
        }

        public void SellItem(GameItem item)
        { 
            if (_gameSession.CurrentTrader.Inventory.Items.Count < 12)
            {
                _gameSession.CurrentPlayer.ReceiveGold(item.Price);
                _gameSession.CurrentTrader.AddItemToInventory(item);
                _gameSession.CurrentPlayer.RemoveItemFromInventory(item);
            }
            else
            {
                MessageBox.Show("Trader has not enought space");
            }
            isSelling = null;
        }

        public void BuyItem(GameItem item)
        {
            if (_gameSession.CurrentPlayer.Gold >= item.Price)
            {
                if (_gameSession.CurrentPlayer.Inventory.Items.Count < 12)
                {
                    _gameSession.CurrentPlayer.SpendGold(item.Price);
                    _gameSession.CurrentTrader.RemoveItemFromInventory(item);
                    _gameSession.CurrentPlayer.AddItemToInventory(item);
                }
                else
                {
                    MessageBox.Show("You have not enought space");
                }
            }
            else
            {
                MessageBox.Show("You do not have enough gold");
            }
        }
    }
}