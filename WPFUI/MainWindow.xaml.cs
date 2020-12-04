using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Engine.EventArgs;
using Engine.Models;
using Engine.Services;
using Engine.ViewModels;
using Microsoft.Win32;
using WPFUI.CustomControls;
using WPFUI.Windows;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        //test
        //test2
        private const string SAVE_GAME_FILE_EXTENSION = "soscsrpg";

        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Dictionary<Key, Action> _userInputActions =
            new Dictionary<Key, Action>();
        private GameSession _gameSession;

        public MainWindow()
        {
            InitializeComponent();

            InitializeUserInputActions();

            SetActiveGameSessionTo(new GameSession());
            SetQuickChoiceItems();
        }

        public MainWindow(GameSession gameSession)
        {
            InitializeComponent();

            InitializeUserInputActions();

            SetActiveGameSessionTo(gameSession);
        }

        public GameSession GetGameSession()
        {
            return _gameSession;
        }
        private void SetQuickChoiceItems()
        {
            int counter = 0;
            var grid = quickChoiceButtons.Children.Cast<Grid>().ElementAt(0)
                        .Children.Cast<Grid>().ElementAt(0);
            foreach (var item in _gameSession.CurrentPlayer.QuickChoiceItems.getQuickChoiceItems())
            {
                var button = grid.Children.Cast<InventoryButton>().ElementAt(counter);
                button.setItem(item);
                counter++;
            }
        }
        public void SaveGame()
        {
            SaveFileDialog saveFileDialog =
                new SaveFileDialog
                {
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
                };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveGameService.Save(_gameSession, saveFileDialog.FileName);
            }
        }

        private static void CloseMenuWindow()
        {
            if (!MenuWindow.isClosing)
            {
                MenuWindow.DeactivateMainWindow();
                MenuWindow.menuWindow.Show();
            }
        }

        private void MoveNorth()
        {
            _gameSession.MoveNorth();
            setCurrentLocationOnMap();
        }

        private void MoveWest()
        {
            _gameSession.MoveWest();
            setCurrentLocationOnMap();
        }

        private void MoveEast()
        {
            _gameSession.MoveEast();
            setCurrentLocationOnMap();
        }

        private void MoveSouth()
        {
            _gameSession.MoveSouth();
            setCurrentLocationOnMap();
        }

        private void ItemUsed(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as InventoryButton).item;
            if(item != null)
            {
                if (item.Category == GameItem.ItemCategory.Weapon)
                {
                    _gameSession.CurrentPlayer.CurrentWeapon = item;
                    _gameSession.AttackCurrentMonster();
                }
                else if (item.Category == GameItem.ItemCategory.Consumable)
                {
                    _gameSession.CurrentPlayer.CurrentConsumable = item;
                    _gameSession.UseCurrentConsumable();
                }
                else if (item.Category == GameItem.ItemCategory.None)
                {
                    _gameSession.CurrentPlayer.CurrentWeapon = null;
                    _gameSession.CurrentPlayer.CurrentConsumable = null;
                }
            }
        }

        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            GameMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            GameMessages.ScrollToEnd();
        }

        private void OnClick_DisplayTradeScreen(object sender, RoutedEventArgs e)
        {
            if (_gameSession.CurrentTrader != null)
            {
                TradeScreen tradeScreen = new TradeScreen(_gameSession);
                tradeScreen.Owner = this;
                tradeScreen.DataContext = _gameSession;
                tradeScreen.ShowDialog();
            }
        }

        private void CraftItems(object sender, RoutedEventArgs e)
        {
            CraftingWindow craftingWindow = new CraftingWindow(_gameSession);
            if(craftingWindow.ShowDialog() == true)
            {

            }
        }

        private void OnClick_Craft(object sender, RoutedEventArgs e)
        {
            Recipe recipe = ((FrameworkElement)sender).DataContext as Recipe;
            _gameSession.CraftItemUsing(recipe);
        }

        private void InitializeUserInputActions()
        {
            _userInputActions.Add(Key.W, () => MoveNorth());
            _userInputActions.Add(Key.A, () => MoveWest());
            _userInputActions.Add(Key.S, () => MoveSouth());
            _userInputActions.Add(Key.D, () => MoveEast());
            _userInputActions.Add(Key.Z, () => _gameSession.AttackCurrentMonster());
            //_userInputActions.Add(Key.C, () => _gameSession.UseCurrentConsumable());
            _userInputActions.Add(Key.I, () => Inventory_Click(this, new RoutedEventArgs()));
            _userInputActions.Add(Key.M, () => MapButton_Click(this, new RoutedEventArgs()));
            _userInputActions.Add(Key.Q, () => OpenQuests(this, new RoutedEventArgs()));
            _userInputActions.Add(Key.C, () => CraftItems(this, new RoutedEventArgs()));
            _userInputActions.Add(Key.T, () => OnClick_DisplayTradeScreen(this, new RoutedEventArgs()));
            _userInputActions.Add(Key.Escape, () => OnClick_OpenMenu(this, new RoutedEventArgs()));
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_userInputActions.ContainsKey(e.Key))
            {
                _userInputActions[e.Key].Invoke();
            }
        }

        /*private void SetTabFocusTo(string tabName)
        {
            foreach(object item in PlayerDataTabControl.Items)
            {
                if (item is TabItem tabItem)
                {
                    if (tabItem.Name == tabName)
                    {
                        tabItem.IsSelected = true;
                        return;
                    }
                }
            }
        }*/

        private void SetActiveGameSessionTo(GameSession gameSession)
        {
            // Unsubscribe from OnMessageRaised, or we will get double messages
            _messageBroker.OnMessageRaised -= OnGameMessageRaised;

            _gameSession = gameSession;
            DataContext = _gameSession;

            // Clear out previous game's messages
            GameMessages.Document.Blocks.Clear();

            _messageBroker.OnMessageRaised += OnGameMessageRaised;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!MenuWindow.isSaved)
            {
                YesNoWindow message =
                    new YesNoWindow("Save Game", "Do you want to save your game?");
                message.Owner = GetWindow(this);
                message.ShowDialog();

                if (message.ClickedYes)
                {
                    SaveGame();
                }
            }
            closeMapWindow();
            CloseMenuWindow();
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            var location = _gameSession.CurrentLocation;
            MapWindow map = new MapWindow(location.XCoordinate, location.YCoordinate);
            map.Show();
        }

        private void setCurrentLocationOnMap()
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window is MapWindow)
                {
                    ((MapWindow)window).selectCurrentLocation(_gameSession.CurrentLocation.XCoordinate, _gameSession.CurrentLocation.YCoordinate);
                }
            }
        }
        private void closeMapWindow()
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window is MapWindow)
                {
                    ((MapWindow)window).Close();
                }
            }
        }

        private void OnClick_OpenMenu(object sender, RoutedEventArgs e)
        {
            MenuWindow.menuWindow.Show();
        }

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            InventoryWindow inventoryWindow = new InventoryWindow(_gameSession);
            inventoryWindow.Show();
        }
        private void OpenQuests(object sender, RoutedEventArgs e)
        {
            QuestsWindow questWindow = new QuestsWindow(_gameSession.CurrentPlayer.Quests.ToList());
            questWindow.Show();
        }
        private void QuickItem0_Drop(object sender, DragEventArgs e)
        {

        }
    }
}