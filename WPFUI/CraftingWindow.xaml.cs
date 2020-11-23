using Engine.Models;
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

namespace WPFUI
{
    /// <summary>
    /// Logika interakcji dla klasy CraftingWindow.xaml
    /// </summary>
    public partial class CraftingWindow : Window
    {
        private GameSession _gameSession;
        public CraftingWindow(GameSession session)
        {
            InitializeComponent();
            this._gameSession = session;
            SetActiveGameSessionTo(this._gameSession);
        }

        private void SetActiveGameSessionTo(GameSession gameSession)
        {
            _gameSession = gameSession;
            DataContext = _gameSession;
        }

        private void CraftItem(object sender, RoutedEventArgs e)
        {
           if(items.SelectedIndex >= 0)
            {
                Recipe recipe = items.SelectedItem as Recipe;
                
                MessageBox.Show(_gameSession.CraftItemUsing(recipe));
            }
            
           
        }
    }
}
