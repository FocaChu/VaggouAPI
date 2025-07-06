using System.Text.Json.Serialization;

namespace VaggouAPI
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
