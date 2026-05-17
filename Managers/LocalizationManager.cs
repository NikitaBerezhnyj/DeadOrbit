using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DeadOrbit.Managers
{
    public static class LocalizationManager
    {
        public const string CurrentLanguage = "uk"; // "en" or "uk"

        private static Dictionary<string, string> _translations = new();

        public static void Initialize()
        {
            string filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Content",
                "Localization",
                $"{CurrentLanguage}.json"
            );

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Файл локалізації не знайдено: {filePath}");
            }

            try
            {
                string jsonString = File.ReadAllText(filePath);
                _translations =
                    JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString)
                    ?? new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка завантаження локалізації: {ex.Message}");
            }
        }

        public static string Get(string key)
        {
            if (_translations.TryGetValue(key, out string value))
            {
                return value;
            }
            return $"[{key}]";
        }

        public static string Get(string key, params object[] args)
        {
            return string.Format(Get(key), args);
        }
    }
}
