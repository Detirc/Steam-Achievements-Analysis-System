using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using Steam_Achievements_Analysis_System.Helpers;
using Steam_Achievements_Analysis_System.View;
using Steam_Achievements_Analysis_System.YourOutputDirectory;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Office.Interop.Word;
using Chart = Microsoft.Office.Interop.Excel.Chart;
using Application = Microsoft.Office.Interop.Word.Application;



namespace Steam_Achievements_Analysis_System.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Game> games;
        private Game selectedGame;
        private SteamGameAchivmentContext _context;
        public ICommand GenerateChartCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand OpenAchievementsCommand { get; }
        public ICommand DeleteCommand { get; }
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
        public MainWindowViewModel(SteamGameAchivmentContext steamGameAchivmentContext)
        {
            this._context = steamGameAchivmentContext;
            LoadGamesCommand = new RelayCommand(LoadGames);
            OpenAchievementsCommand = new RelayCommand(OpenAchievements);
            GenerateChartCommand = new RelayCommand(GenerateChart);
            GenerateReportCommand = new RelayCommand(GenerateReport);
            DeleteCommand = new RelayCommand(DeleteGame);
        }
        public RelayCommand LoadGamesCommand { get; }

        public ObservableCollection<Game> Games
        {
            get { return games; }
            set
            {
                if (value != games)
                {
                    games = value;
                    OnPropertyChanged(nameof(Games));
                }
            }
        }
        
        private void DeleteGame()
        {
            if (SelectedGame != null)
            {
                Delete(selectedGame);
            }
        }

        private void OpenAchievements()
        {
            if (SelectedGame != null)
            {
                AchievementsWindowViewModel achievementsViewModel = new AchievementsWindowViewModel(SelectedGame);
                AchievementsWindow achievementsWindow = new AchievementsWindow(achievementsViewModel);
                achievementsWindow.Show();
            }
        }
        private async void LoadGames()
        {
            // получение даннях из бд
            var gamesFromDb = _context.Games.AsNoTracking().
                Include(g => g.Achievements).ThenInclude(a => a.AchievementPercentages)
                .ToList();


            if (gamesFromDb.Any())
            {
                // Если в базе есть данные, спрашиваем пользователя о перезаписи
                var result = MessageBox.Show("Желаете перезаписать данные из Steam API?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await LoadGamesFromApi();
                }
                else
                {
                    // Если пользователь отказывается от перезаписи, используем данные из базы
                    Games = new ObservableCollection<Game>(gamesFromDb);
                }
            }
            else
            {
                // Если в базе нет данных, просто загружаем данные из Steam API
                await LoadGamesFromApi();
            }
        

    
        }
        private async System.Threading.Tasks.Task LoadGamesFromApi()
        {
            var steamApiHelper = new SteamApiHelper();

            try
            {
                // Очистка текущих данных в базе и добавление новых
                _context.Games.RemoveRange(_context.Games);

              

               await _context.SaveChangesAsync();
                // Получение данных из Steam API
                var gamesFromApi = await steamApiHelper.GetAppListAsync();

                // Очистка текущих данных в базе и добавление новых
         
               
                _context.Games.AddRange(gamesFromApi);
            
               await _context.SaveChangesAsync();

                // Обновление свойства Games
                Games = new ObservableCollection<Game>(gamesFromApi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных {ex.Message}");
            }
        }

        private void GenerateChart()
        {
            if (SelectedGame != null)
            {
                CreateExcelChart(SelectedGame);

            }
        }
        private void GenerateReport()
        {
            if (SelectedGame != null)
            {
                CreateReportWord(SelectedGame);
            }
        }
        private void CreateExcelChart(Game game)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = false;
            Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;

            worksheet.Cells[1, 1] = "Игра: " + game.GameName;
            worksheet.Cells[2, 1] = "Достижение";
            worksheet.Cells[2, 2] = "Процент выполнения";

            int row = 3;
            foreach (var achievement in game.Achievements)
            {
                worksheet.Cells[row, 1] = achievement.AchivmentName;

                double percentage = 0.0;
                var firstPercentage = achievement.AchievementPercentages.FirstOrDefault();
                if (firstPercentage != null)
                {
                    percentage = firstPercentage.Percentage;
                }

                worksheet.Cells[row, 2] = percentage;

                row++;
            }

            Microsoft.Office.Interop.Excel.ChartObjects chartObjects = (Microsoft.Office.Interop.Excel.ChartObjects)worksheet.ChartObjects();
            Microsoft.Office.Interop.Excel.ChartObject chartObject = chartObjects.Add(100, 20, 375, 225);
            Microsoft.Office.Interop.Excel.Chart chart = chartObject.Chart;

            Microsoft.Office.Interop.Excel.Range chartRange = worksheet.get_Range("A2", "B" + (row - 1));
            chart.SetSourceData(chartRange);
            chart.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlBarClustered; // Измененный тип диаграммы
            chart.HasTitle = true;
            chart.ChartTitle.Text = $"График выполнения достижений для игры: {game.GameName}";

            try
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{game.GameName}_AchievementsChart.xlsx");
                workbook.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                workbook.Close(false);
                excelApp.Quit();

                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(excelApp);
            }
        }


    

private void Delete(Game game)
    {
        var result = MessageBox.Show("Вы уверены, что хотите удалить эту игру?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            // Удаление связанных AchievementPercentages
            var achievementPercentages = _context.AchievementPercentages
                .Where(ap => ap.Achievement.AppId == game.AppId)
                .ToList();

            _context.AchievementPercentages.RemoveRange(achievementPercentages);

            // Удаление связанных Achievements
            var achievements = _context.Achievements
                .Where(a => a.AppId == game.AppId)
                .ToList();

            _context.Achievements.RemoveRange(achievements);

            // Удаление самого объекта Game из коллекции
            Games.Remove(game);

            // Удаление самого объекта Game из базы данных
            _context.Games.Remove(game);

            // Сохранение изменений
            _context.SaveChanges();
        }
    }

   
        private void CreateReportWord(Game game)
        {
            string excelChartPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{game.GameName}_AchievementsChart.xlsx");

            if (!File.Exists(excelChartPath))
            {
                MessageBox.Show("Файл с графиком не найден. Пожалуйста, создайте график и повторите попытку.");
                return;
            }
            // Путь к  шаблону Word
            string wordTemplatePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Шаблон Отчёта.docx");

            // Создание нового Word-приложения
            Application wordApp = new Application();

            // Открываем шаблон Word
            Document doc = wordApp.Documents.Open(wordTemplatePath);

            // Данные для замены
            string gameName = game.GameName;

            // Замена данных в документе Word
            ReplaceTextInWord(doc, "[GameName]", gameName);

            // Находим и замена плейсхолдера для списка достижений
            ReplaceTextInWord(doc, "[AchievementListPlaceholder]", "");

            // Добавление данных о достижениях
            Microsoft.Office.Interop.Word.Range achievementRange = doc.Range().Paragraphs.Last.Range;

            foreach (var achievement in game.Achievements)
            {
                // Заполнение информации о достижении
                string achievementInfo = $"{achievement.AchivmentName} - {achievement.AchievementPercentages.FirstOrDefault().Percentage.ToString("F1")}%";

                // Добавление информации в документ
                achievementRange.Text += achievementInfo + "\n";
            }

            // Находим и заменяем плейсхолдер для графика
            ReplaceTextInWord(doc, "[ChartPlaceholder]", "");
        
            // Вставка графика из Excel
            CopyExcelChartToClipboard(game);

            // Вставка графика в Word
            Microsoft.Office.Interop.Word.Range chartWordRange = doc.Range().Paragraphs.Last.Range;
            chartWordRange.PasteSpecial(DataType: 3);

          
            // Получение пути к директории "Мои документы"
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Создание пути для сохранения файла
            string filePath = Path.Combine(myDocumentsPath, $"{game.GameName}_AchievementsReport.docx");

            // Сохранение результата
            doc.SaveAs2(filePath);

            // Закрытие Word
            doc.Close();
            wordApp.Quit();

            MessageBox.Show("Отчет успешно создан.");
        }

        private void ReplaceTextInWord(Document doc, string placeholder, string replacement)
        {
            doc.Range().Find.Execute(placeholder, ReplaceWith: replacement);
        }


        // Метод для копирования графика из Excel в буфер обмена
        static void CopyExcelChartToClipboard(Game game)
        {
            // Создание нового Excel-приложения
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

            // Открываем ваш Excel файл
            Workbook workbook = excelApp.Workbooks.Open($"C:\\Users\\deni_\\OneDrive\\Документы\\{game.GameName}_AchievementsChart.xlsx");

            // Предположим, что у вас есть график на листе с индексом 1
            Microsoft.Office.Interop.Excel.ChartObjects chartObjects = (ChartObjects)workbook.Sheets[1].ChartObjects();
            ChartObject chartObject = chartObjects.Item(1);

            // Изменение в этой строке - обращение к свойству Chart
            Chart chart = chartObject.Chart;

            // Теперь вы можете использовать свойство ChartArea, если это необходимо
            chart.ChartArea.Copy();

            // Закрытие Excel
            workbook.Close();
            excelApp.Quit();
        }








        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

