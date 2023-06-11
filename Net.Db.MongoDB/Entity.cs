using MongoDB.Bson.Serialization.Attributes;

namespace Net.Db.MongoDB;

class Entity
{
    [BsonId]
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime EditedAt { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public long Value { get; set; }
}

