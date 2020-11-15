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
    /// Logika interakcji dla klasy MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        //pozycje początkowe, punkt startu na mapie
        private int startX = 2;
        private int startY = 2;
        //pozycje poprzednie na mapie (kolumny na gridzie)
        private int previousColumn = 2;
        private int previousRow = 2;

        public MapWindow(int locationX, int locationY)
        {
            InitializeComponent();
            selectCurrentLocation(locationX, locationY);
        }

        public void selectCurrentLocation(int x, int y)
        {
            //zmienia kolor poprzedniej lokalizacji na domyślny
            worldMap.Children
                .Cast<Button>()
                .First(e => Grid.GetRow(e) == previousRow && Grid.GetColumn(e) == previousColumn).Background = Brushes.Bisque;

            int column = startX + x;
            int row = startY - y;

            previousColumn = column;
            previousRow = row;

            //zmienia kolor obecnej lokalizacji na zielony
            worldMap.Children
                .Cast<Button>()
                .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column).Background = Brushes.LightGreen;
        }
    }
}
