using System;
using System.IO;
using System.Text.Json;

namespace ProPresenter7WEB.DesktopApplication.Helpers
{
    public static class ModelCacheHelper
    {
        public static void SaveModelState<TModel>(TModel model)
        {
            var filePath = PrepareFilePath(nameof(TModel), true);

            File.WriteAllText(filePath, JsonSerializer.Serialize(model));
        }

        public static TModel? ReadModelState<TModel>()
        {
            var filePath = PrepareFilePath(nameof(TModel), false);

            if (File.Exists(filePath))
            {
                var content = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<TModel>(content);
            }

            return default(TModel);
        }

        private static string PrepareFilePath(string filename, bool createFolder)
        {
            var filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ProPresenter7WEB",
                $"{filename}.json");

            if (createFolder)
            {
                var directoryName = Path.GetDirectoryName(filePath);

                if (directoryName == null)
                {
                    throw new InvalidOperationException($"Directory name from {filePath} cannot be null.");
                }

                Directory.CreateDirectory(directoryName);
            }

            return filePath;
        }
    }
}
