using System.ComponentModel.DataAnnotations;

namespace WebParser_PW2.Data
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string Name { get; set; }
    }
}
