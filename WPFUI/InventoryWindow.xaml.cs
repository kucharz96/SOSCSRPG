using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFUI.CustomControls;

namespace WPFUI
{
    /// <summary>
    /// Logika interakcji dla klasy InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow : Window
    {
        private GameSession _gameSession;
        public InventoryWindow(GameSession session)
        {
            InitializeComponent();
            _gameSession = session;
            Rebulid();
            
            
            
        }

        public void Rebulid()
        {
            int counter = 0;
            foreach (var b in inventoryPack.Children.Cast<InventoryButton>())
            {
                b.item = null;
                b.image.Source = null;
            }


            foreach (var item in _gameSession.CurrentPlayer.Inventory.Items)
            {
                var button = inventoryPack.Children.Cast<InventoryButton>().ElementAt(counter);
                button.setItem(item);


                counter++;
            }
        }
    }
}
