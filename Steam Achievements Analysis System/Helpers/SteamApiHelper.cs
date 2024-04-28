using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Steam_Achievements_Analysis_System.YourOutputDirectory;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace Steam_Achievements_Analysis_System.Helpers
{
    public class SteamApiHelper
    {
        private readonly string apiKey = "MY API";
        private readonly HttpClient httpClient = new HttpClient();

        public SteamApiHelper()
        {
           
        }

        public async Task<List<Game>> GetAppListAsync()
        {
            string endpoint = $"http://api.steampowered.com/ISteamApps/GetAppList/v2?key={apiKey}&format=json";

            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string appListJson = await response.Content.ReadAsStringAsync();

                return await ParseAppListJson(appListJson);
            }
            else
            {
                throw new HttpRequestException($"Ошибка при запросе: {response.StatusCode}");
            }
        }

        public async Task<List<Achievement>> GetAchievements(int gameId)
        {
            string endpoint = $"http://api.steampowered.com/ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002/?key={apiKey}&gameid={gameId}&format=json";

            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string achivListJson = await response.Content.ReadAsStringAsync();
                return ParseAchievementsJson(achivListJson, gameId);
            }
            else
            {
                return new List<Achievement>();
            }
        }

        private List<Achievement> ParseAchievementsJson(string json, int gameId)
        {
            List<Achievement> achievements = new List<Achievement>();

            dynamic dynamicObject = JObject.Parse(json);

            if (dynamicObject?.achievementpercentages?.achievements != null)
            {
                JArray achievementsArray = dynamicObject.achievementpercentages.achievements;

                foreach (JToken achievementData in achievementsArray)
                {
                    Achievement achievement = new Achievement
                    {
                        AppId = gameId,
                        AchivmentName = achievementData.Value<string>("name")
                    };

                    AchievementPercentage percentage = new AchievementPercentage
                    {
                        AchievementId = achievement.AchievementId,
                        Percentage = achievementData.Value<double>("percent")
                    };

                    achievement.AchievementPercentages.Add(percentage);
                    achievements.Add(achievement);
                }
            }

            return achievements;
        }

        private async Task<List<Game>> ParseAppListJson(string json)
        {
            List<Game> games = new List<Game>();

            dynamic dynamicObject = JObject.Parse(json);
            int count = 0; // счетчик обработанных элементов
            int limit = 100;

            if (dynamicObject?.applist?.apps != null)
            {
                JArray appsArray = dynamicObject.applist.apps;

                foreach (var appData in appsArray)
                {
                    if (appData["appid"] != null && appData["name"] != null)
                    {
                        Game game = new Game
                        {
                            AppId = appData["appid"].Value<int>(),
                            GameName = appData["name"].Value<string>(),
                        };

                        // Попытка получения достижений, если есть
                        var achievements = await GetAchievements(game.AppId);

                        // Проверка, валидны ли достижения
                        if (achievements.Any())
                        {
                            // Связывание достижений с игрой
                            game.Achievements = achievements;
                            games.Add(game);
                            count++;
                        }

                        if (count >= limit)
                        {
                            MessageBox.Show("Цикл завершил работу");
                            break; // прекратить обработку после достижения лимита
                        }
                    }
                }
            }

            return games;
        }

    }
}
