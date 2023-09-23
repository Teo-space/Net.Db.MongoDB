namespace Net.Db.MongoDB;

using global::MongoDB.Bson;
using global::MongoDB.Driver;


internal class Tests
{
    public static void InsertOne(IMongoCollection<Entity> TestCollection)
    {
        print($"InsertOne Start");
        print($"CountDocuments : {TestCollection.CountDocuments(x => true)}");

        var entity = new Entity()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            EditedAt = DateTime.Now,

            Name = "Name",
            Description = "Description",
            Value = 100,
        };
        print($"InsertOne(entity)");
        TestCollection.InsertOne(entity);
        print($"CountDocuments : {TestCollection.CountDocuments(x => true)}");
        var e = TestCollection.Find<Entity>(x => x.Id == entity.Id).FirstOrDefault()?.ToJson();
        print(e);

        print($"DeleteOne");
        TestCollection.DeleteOne(x => x.Id == entity.Id);
        print($"CountDocuments : {TestCollection.CountDocuments(x => true)}");

        print($"InsertOne(entity)");
        TestCollection.InsertOne(entity);

        print($"AsQueryable().FirstOrDefault");
        var eForUpdate = TestCollection.AsQueryable().FirstOrDefault(x => x.Id == entity.Id);
        print(eForUpdate.ToJson());

        print($"update");
        var update = Builders<Entity>.Update.Set(x => x.Name, eForUpdate.Name + "____UPDATED");
        TestCollection.UpdateOne(x => x.Id == entity.Id, update);

        print($"FindOneAndDelete");
        var eUpdated = TestCollection.FindOneAndDelete(x => x.Id == entity.Id);
        print(eUpdated.ToJson());


    }

    public static void InsertMany(IMongoCollection<Entity> TestCollection)
    {
        print("", $"InsertMany");
        print($"CountDocuments : {TestCollection.CountDocuments(x => true)}");

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

                Value = i * 100,
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



    public static void PerfomanceInsertOne(IMongoCollection<Entity> TestCollection)
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

        print($"CountDocuments : {TestCollection.CountDocuments(x => true)}");
    }



    public static void PerfomanceInsertMany(IMongoCollection<Entity> TestCollection, int Count = 10000)
    {
        print("Perfomance.InsertMany");

        List<Entity> forInsert = new();
        for (int i = 0; i < Count; i++)
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




}
