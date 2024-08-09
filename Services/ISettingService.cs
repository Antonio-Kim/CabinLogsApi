using CabinLogsApi.Models;

public interface ISettingService
{
    public Task<List<Setting>> GetSettings();
    public Task<List<Setting>> UpdateSettings(Setting setting);
}