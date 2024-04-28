using Steam_Achievements_Analysis_System.YourOutputDirectory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Steam_Achievements_Analysis_System.ViewModel
{
    public class AchievementsWindowViewModel : INotifyPropertyChanged
    {
        private Game selectedGame;
        public Game SelectedGame
        {
            get { return selectedGame; }
            set
            {
                if (value != selectedGame)
                {
                    selectedGame = value;
                    OnPropertyChanged(nameof(SelectedGame));
                }
            }
        }

        public ObservableCollection<Achievement> Achievements { get; }
        public ICommand OpenBrowserCommand { get; }
        public AchievementsWindowViewModel(Game selectedGame)
        {
            SelectedGame = selectedGame;
            Achievements = new ObservableCollection<Achievement>(selectedGame.Achievements);
            OpenBrowserCommand = new RelayCommand(OpenBrowser);
        }
        private void OpenBrowser()
        {
            if (SelectedGame != null)
            {
                string urlToOpen = $"https://steamcommunity.com/stats/{SelectedGame.AppId}/achievements";

                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "chrome.exe",
                        Arguments = urlToOpen,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    // Обработка ошибок при открытии браузера
                    MessageBox.Show($"Error opening browser: {ex.Message}");
                }
            }
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
