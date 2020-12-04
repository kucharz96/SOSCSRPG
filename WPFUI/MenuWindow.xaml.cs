using Engine.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPFUI.Windows;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private const string SAVE_GAME_FILE_EXTENSION = "soscsrpg";

        public static MenuWindow menuWindow;
        public static bool isClosing;
        public static bool isSaved;

        private static MainWindow mainWindow;
        private OpenFileDialog openFileDialog;

        public MenuWindow()
        {
            InitializeComponent();
            menuWindow = this;
            isClosing = false;
            isSaved = false;
        }

        public static void DeactivateMainWindow() => mainWindow = null;

        private void BeginBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isSaved = false;
            if (mainWindow == null)
            {
                mainWindow = new MainWindow();
            }
            mainWindow.Show();
            this.Hide();
        }

        private void LoadGameBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            openFileDialog = new OpenFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                mainWindow = new MainWindow(SaveGameService.LoadLastSaveOrCreateNew(openFileDialog.FileName));
            }

            if (mainWindow != null)
            {
                mainWindow.Show();
                this.Hide();
            } 
        }

        private void SaveGameBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mainWindow != null)
            {
                mainWindow.SaveGame();
                isSaved = true;
            }
        }

        private void MenuWindow_OnClosing(object sender, CancelEventArgs e)
        {
            isClosing = true;
            if (mainWindow != null && !isSaved)
            {
                YesNoWindow message =
                    new YesNoWindow("Save Game", "Do you want to save your game?");
                message.Owner = GetWindow(this);
                message.ShowDialog();

                if (message.ClickedYes)
                {
                    mainWindow.SaveGame();
                }
            }
            foreach (var window in Application.Current.Windows)
            {
                if (!(window is MenuWindow))
                    ((Window)window).Close();
            }
        }

        private void LeaveBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void ControlsBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ControlsScreen controlsScreen = new ControlsScreen();
            controlsScreen.Show();
        }
    }
}
