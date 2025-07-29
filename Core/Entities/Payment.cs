using MongoDB.Bson;

namespace Core.Entities
{
    public class Payment
    {
        public ObjectId Id { get; set; }
        public decimal Amount { get; set; }

        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
