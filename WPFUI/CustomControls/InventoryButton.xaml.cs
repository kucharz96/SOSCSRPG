using Engine.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUI.CustomControls
{
    /// <summary>
    /// Logika interakcji dla klasy InventoryButton.xaml
    /// </summary>
    public partial class InventoryButton : UserControl
    {
        public GameItem item { get; set; }

        public InventoryButton()
        {
            InitializeComponent();
        }

        public void setItem(GameItem item)
        {
            this.item = item;
            if (!string.IsNullOrEmpty(item.ImagePath))
            {
                image.Source = new BitmapImage(new Uri(item.ImagePath));
                if (item.IsInQuickChoose && Window.GetWindow(this) is InventoryWindow)
                {
                   button.Opacity = 0.4;
                   image.Opacity = 0.4;
                }
            }
        }

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
            
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            var inventoryTo = sender as InventoryButton;
            var inventoryFrom = e.Data.GetData(typeof(InventoryButton)) as InventoryButton;

            if(Window.GetWindow(inventoryFrom) is MainWindow && Window.GetWindow(inventoryTo) is InventoryWindow)
            {
                //operacja niedozwolona
                return;
            }


            if(inventoryFrom.item != null)
            {
                //przerzut z plecaka do quicksa
                if(Window.GetWindow(inventoryTo) is MainWindow && Window.GetWindow(inventoryFrom) is InventoryWindow)
                {
                    if (inventoryFrom.item.IsInQuickChoose)
                    {
                        //operacja niedozwolona
                        return;
                    }
                    else
                    {
                        var window = (MainWindow)Window.GetWindow(inventoryTo);
                        var session = window.GetGameSession();
                        session.CurrentPlayer.QuickChoiceItems.InsertItem(inventoryFrom.item);

                        if (inventoryTo.item == null)
                        {
                            inventoryTo.setItem(inventoryFrom.item);
                            inventoryFrom.setItem(inventoryFrom.item);
                        }
                        else
                        {
                            session.CurrentPlayer.QuickChoiceItems.RemoveItem(inventoryTo.item);
                            inventoryTo.setItem(inventoryFrom.item);
                            var iv = (InventoryWindow)Window.GetWindow(inventoryFrom);
                            iv.Rebulid();


                        }
                    }

                }
                else
                {
                    if (inventoryTo.item == null)
                    {
                        inventoryTo.setItem(inventoryFrom.item);
                        inventoryFrom.item = null;
                        inventoryFrom.image.Source = null;
                    }
                    else
                    {
                        GameItem temp = inventoryTo.item;
                        inventoryTo.setItem(inventoryFrom.item);
                        inventoryFrom.setItem(temp);


                    }
                }

                
            }
        }
    }
}
