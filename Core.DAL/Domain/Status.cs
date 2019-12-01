namespace Hurace.Core.DAL.Domain
{
    // DTO/Domain Object
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public override string ToString() =>
            $"RaceId(id:{Id}, Name:{Name})";
    }
}