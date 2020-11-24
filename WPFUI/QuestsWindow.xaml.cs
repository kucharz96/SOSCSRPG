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
using System.Windows.Shapes;
using WPFUI.CustomControls;

namespace WPFUI
{
    /// <summary>
    /// Logika interakcji dla klasy QuestsWindow.xaml
    /// </summary>
    public partial class QuestsWindow : Window
    {
        public QuestsWindow(List<QuestStatus> questStatuses)
        {
            InitializeComponent();
            int counter = 0;
            foreach (var q in questStatuses )
            {

                var item = quests.Children.Cast<QuestItem>().ElementAt(counter++);
                item.SetItem(q);
            }
        }
    }
}
