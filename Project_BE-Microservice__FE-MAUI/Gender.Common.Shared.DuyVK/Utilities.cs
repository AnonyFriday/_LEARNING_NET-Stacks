using System.Text.Json;

namespace Gender.Common.Shared.DuyVK
{
    public class Utilities
    {
        // =================================
        // === Fields
        // =================================

        private static string loggerFilePath = Directory.GetCurrentDirectory() + @"\DataLog.txt";

        // =================================
        // === Methods
        // =================================

        // Serialize an object to a JSON string
        public static string ConvertObjectToJSONString<T>(T entity)
        {
            string jsonString = JsonSerializer.Serialize(entity, new JsonSerializerOptions { WriteIndented = false });
            return jsonString;
        }

        // Write a JSON string to a file
        public static async Task WriteLoggerFile(string content)
        {
            try
            {
                var path = Directory.GetCurrentDirectory();

                using (var file = File.Open(loggerFilePath, FileMode.Append, FileAccess.Write))
                using (var writer = new StreamWriter(file))
                {
                    await writer.WriteLineAsync(content);
                    await writer.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions related to file writing
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
