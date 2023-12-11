using Dapper;
using MySqlConnector;
using Newtonsoft.Json;

namespace Common;
public class MyProjectInfoConfig
{
    public static MyProjectInfoConfig Instance { get; set; }
    
    public string APIKey { get; set; }
    public string ConnectionString { get; set; }
    public string ServerAddress { get; set; }

    public static void Refresh()
    {
        try
        {
            var configurationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyProjectInfo.config");
            MyProjectInfoConfig myProjectInfo = JsonConvert.DeserializeObject<MyProjectInfoConfig>(File.ReadAllText(configurationPath));

            Instance = myProjectInfo;
            Console.WriteLine($"MyProjectInfoConfig Refresh Success");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}