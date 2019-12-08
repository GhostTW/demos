using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data
{
    public class UserEntity
    {
        [BsonId]
        public int Id { get; set; }
        [BsonElement]
        public string Code { get; set; }
        [BsonElement]
        public string Password { get; set; }
        [BsonElement]
        public bool IsActive { get; set; }
    }
}