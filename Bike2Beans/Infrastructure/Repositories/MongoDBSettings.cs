namespace Bike2Beans.Infrastructure.Repositories;

public class MongoDBSettings
{
    public const string SectionName = "MongoDBSettings";

    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;

}
