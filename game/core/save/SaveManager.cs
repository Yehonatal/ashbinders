using System;
using System.IO;
using System.Text.Json;
using Ashbinders.Core.Logging;

namespace Ashbinders.Core.Save;

public class SaveManager
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public SaveData CurrentSaveData { get; private set; } = new();

    public bool SaveToFile(string filePath)
    {
        try
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            CurrentSaveData.SaveTimestampUtc = DateTime.UtcNow;
            var json = JsonSerializer.Serialize(CurrentSaveData, JsonOptions);
            File.WriteAllText(filePath, json);
            GameLogger.LogInfo($"Successfully saved game state to: {filePath}");
            return true;
        }
        catch (Exception ex)
        {
            GameLogger.LogError($"Failed to save game state to {filePath}: {ex.Message}");
            return false;
        }
    }

    public bool LoadFromFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                GameLogger.LogWarning($"Save file does not exist at: {filePath}");
                return false;
            }

            var json = File.ReadAllText(filePath);
            var loaded = JsonSerializer.Deserialize<SaveData>(json, JsonOptions);
            if (loaded != null)
            {
                CurrentSaveData = loaded;
                GameLogger.LogInfo($"Successfully loaded save from: {filePath}");
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            GameLogger.LogError($"Failed to load save file from {filePath}: {ex.Message}");
            return false;
        }
    }

    public void Reset()
    {
        CurrentSaveData = new SaveData();
    }
}
