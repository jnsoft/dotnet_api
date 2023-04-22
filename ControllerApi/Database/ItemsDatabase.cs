using System.Security.Cryptography;

namespace ControllerApi.Database
{
    public interface IItemsDatabase
    {
        Dictionary<Guid, Item> Items { get; }

        Item[] GenerateItems(int n);
    }

    public class ItemsDatabase : IItemsDatabase
    {
        private Dictionary<Guid, Item> items { get; init; } = new Dictionary<Guid, Item>();

        public Dictionary<Guid, Item> Items => items; 

        public Item[] GenerateItems(int n)
        {
            List<Item> items = new List<Item>();
            for (int i = 0; i < n; i++)
            {
                Item item = new Item { Id = Guid.NewGuid(), Name = $"Item {RandomNumberGenerator.GetInt32(1000, 1000000)}" };
                items.Add(item);
            }
            return items.ToArray();
        }
    }
}
