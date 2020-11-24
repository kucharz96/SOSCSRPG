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
    /// Logika interakcji dla klasy QuestItem.xaml
    /// </summary>
    public partial class QuestItem : UserControl
    {
        public QuestItem()
        {
            InitializeComponent();
        }

        public void SetItem(QuestStatus status)
        {
            QuestName.Content = status.PlayerQuest.Name;
            if (status.IsCompleted)
            {
                QuestSymbol.Source = new BitmapImage(new Uri("pack://application:,,,/Engine;component/Images/Icons/check.png"));
            }
            else
            {
                QuestSymbol.Source = new BitmapImage(new Uri("pack://application:,,,/Engine;component/Images/Icons/xred.png"));

            }
        }

    }
}
