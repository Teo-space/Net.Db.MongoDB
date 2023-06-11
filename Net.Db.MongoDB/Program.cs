//Host.CreateDefaultBuilder(args).ConfigureServices(services => services.AddHostedService<Worker>()).Build().Run();
using MongoDB.Bson;
using MongoDB.Driver;
using MoreLinq;
using Net.Db.MongoDB;


var dbClient = new MongoClient("mongodb://127.0.0.1:27017");
var dbList = dbClient.ListDatabases().ToList();
print($"dbList : {dbList.Count}", $"{dbList.ToDelimitedString("\n")}");


IMongoDatabase testdb = dbClient.GetDatabase("testdb");
//testdb.CreateCollection("TestCollection");
var TestCollection = testdb.GetCollection<Entity>("TestCollection");


{
    print("", $"InsertOne");
    print($"TestCollection : {TestCollection.CountDocuments(x => true)}");

    var entity = new Entity()
    {
        Id = Guid.NewGuid(),
        CreatedAt = DateTime.Now,
        EditedAt = DateTime.Now,

        Name = "Name",
        Description = "Description",
        Value = 100,
    };

    TestCollection.InsertOne(entity);
    print($"TestCollection : {TestCollection.CountDocuments(x => true)}");
    var e = TestCollection.Find<Entity>(x => x.Id == entity.Id).FirstOrDefault()?.ToJson();
    print(e);
    TestCollection.DeleteOne(x => x.Id == entity.Id);
    print($"TestCollection : {TestCollection.CountDocuments(x => true)}");


    TestCollection.InsertOne(entity);

    var eForUpdate = TestCollection.AsQueryable().FirstOrDefault(x => x.Id == entity.Id);
    print(eForUpdate.ToJson());

    var update = Builders<Entity>.Update.Set(x => x.Name, eForUpdate.Name + "____UPDATED");
    TestCollection.UpdateOne(x => x.Id == entity.Id, update);
    var eUpdated = TestCollection.FindOneAndDelete(x => x.Id == entity.Id);
    print(eUpdated.ToJson());


}


{
    print("", $"InsertMany");
    print($"TestCollection : {TestCollection.CountDocuments(x => true)}");

    List<Entity> forInsert = new();
    for (int i = 0; i < 10; i++)
    {
        var e = new Entity()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            EditedAt = DateTime.Now,

            Name = $"Name_{i}",
            Description = "Description",

            Value = i*100,
        };
        forInsert.Add(e);
    }

    
    TestCollection.InsertMany(forInsert);
    print($"TestCollection : {TestCollection.CountDocuments(x => true)}");

    TestCollection.Find<Entity>(x => true)
        .Skip(0)
        .Limit(3)
        .ToList()
        .ForEach(x => print(x.ToJson()));


    TestCollection.DeleteMany(x => true);
    print($"TestCollection : {TestCollection.CountDocuments(x => true)}");


}



{
    print();
    print("Perfomance.InsertOne");

    for (int i = 0; i < 10000; i++)
    {
        var e = new Entity()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            EditedAt = DateTime.Now,

            Name = $"Name_{i}",
            Description = "Description",

            Value = i,
        };
        TestCollection.InsertOne(e);
    }
    print("Perfomance.InsertOne.End");
    print($"TestCollection.Count : {TestCollection.CountDocuments(x => true)}");

    print("Perfomance.InsertMany");

    List<Entity> forInsert = new();
    for (int i = 0; i < 50000; i++)
    {
        var e = new Entity()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            EditedAt = DateTime.Now,

            Name = $"Name_{i}",
            Description = "Description",

            Value = i * 100,
        };
        forInsert.Add(e);
    }
    TestCollection.InsertMany(forInsert);
    print("Perfomance.InsertMany.End");
    print($"TestCollection.Count : {TestCollection.CountDocuments(x => true)}");

    print("Perfomance.GetAll");
    var all = TestCollection.Find<Entity>(x => true).ToList();
    print("Perfomance.GetAll.End");

    print("Perfomance.InsertOne.DeleteMany");
    TestCollection.DeleteMany(x => true);
    print("Perfomance.InsertOne.DeleteMany.End");
}


