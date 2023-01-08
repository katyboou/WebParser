using System.ComponentModel.DataAnnotations;

namespace WebParser_PW2.Data
{
    public class Shop
    {
        [Key]
        public int ShopId { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }
    }
}
