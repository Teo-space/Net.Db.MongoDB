//Host.CreateDefaultBuilder(args).ConfigureServices(services => services.AddHostedService<Worker>()).Build().Run();
using MongoDB.Driver;
using MoreLinq;
using Net.Db.MongoDB;


var dbClient = new MongoClient("mongodb://127.0.0.1:27017");
var dbList = dbClient.ListDatabases().ToList();
//print($"dbList : {dbList.Count}", $"{dbList.ToDelimitedString("\n")}");


IMongoDatabase testdb = dbClient.GetDatabase("testdb");
var TestCollection = testdb.GetCollection<Entity>("TestCollection");
//Tests.PerfomanceInsertOne(TestCollection);

Tests.PerfomanceInsertMany(TestCollection, 1000000);


/*
Tests.PerfomanceInsertMany(TestCollection, 1000000);

---10.000
[[2023.09.23 10:16.17.360]]Perfomance.InsertMany(-0,16)
[[2023.09.23 10:16.17.693]]Perfomance.InsertMany.End(324,21)
[[2023.09.23 10:16.17.717]]TestCollection.Count : 10000(24,81)
[[2023.09.23 10:16.17.718]]Perfomance.GetAll(0,46)
[[2023.09.23 10:16.17.831]]Perfomance.GetAll.End(113,47)
[[2023.09.23 10:16.17.832]]Perfomance.InsertOne.DeleteMany(0,48)
[[2023.09.23 10:16.18.000]]Perfomance.InsertOne.DeleteMany.End(167,76)

---100.000
[[2023.09.23 10:16.50.056]]Perfomance.InsertMany(-0,17)
[[2023.09.23 10:16.51.414]]Perfomance.InsertMany.End(1349,16)
[[2023.09.23 10:16.51.463]]TestCollection.Count : 100000(49,03)
[[2023.09.23 10:16.51.464]]Perfomance.GetAll(0,47)
[[2023.09.23 10:16.51.964]]Perfomance.GetAll.End(500,58)
[[2023.09.23 10:16.51.965]]Perfomance.InsertOne.DeleteMany(0,5)
[[2023.09.23 10:16.52.940]]Perfomance.InsertOne.DeleteMany.End(975,37)

---1000.000
[[2023.09.23 10:18.03.186]]Perfomance.InsertMany(-0,12)
[[2023.09.23 10:18.12.862]]Perfomance.InsertMany.End(9667,36)
[[2023.09.23 10:18.13.147]]TestCollection.Count : 1000000(285,29)
[[2023.09.23 10:18.13.148]]Perfomance.GetAll(0,47)
[[2023.09.23 10:18.16.762]]Perfomance.GetAll.End(3614,67)
[[2023.09.23 10:18.16.763]]Perfomance.InsertOne.DeleteMany(0,44)
[[2023.09.23 10:18.27.077]]Perfomance.InsertOne.DeleteMany.End(10314,14)



*/
