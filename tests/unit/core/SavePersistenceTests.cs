using System.IO;
using Xunit;
using Ashbinders.Core.Save;

namespace Ashbinders.Tests.Unit.Core;

public class SavePersistenceTests
{
    [Fact]
    public void SaveManager_CanSaveAndReloadState_WithFidelity()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"ashbinders_test_{System.Guid.NewGuid()}.json");

        try
        {
            var saveManager = new SaveManager();
            saveManager.CurrentSaveData.Player.PositionX = 142.5f;
            saveManager.CurrentSaveData.Player.PositionY = 88.0f;
            saveManager.CurrentSaveData.Player.CurrentHealth = 75;
            saveManager.CurrentSaveData.Player.SocketedEmbers.Add("ember_motion_01");
            saveManager.CurrentSaveData.World.ActivatedDevices.Add("ancient_ember_device_01");

            var saveSuccess = saveManager.SaveToFile(tempFile);
            Assert.True(saveSuccess);
            Assert.True(File.Exists(tempFile));

            var reloadedManager = new SaveManager();
            var loadSuccess = reloadedManager.LoadFromFile(tempFile);

            Assert.True(loadSuccess);
            Assert.Equal(142.5f, reloadedManager.CurrentSaveData.Player.PositionX);
            Assert.Equal(88.0f, reloadedManager.CurrentSaveData.Player.PositionY);
            Assert.Equal(75, reloadedManager.CurrentSaveData.Player.CurrentHealth);
            Assert.Contains("ember_motion_01", reloadedManager.CurrentSaveData.Player.SocketedEmbers);
            Assert.Contains("ancient_ember_device_01", reloadedManager.CurrentSaveData.World.ActivatedDevices);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
