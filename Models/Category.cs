using System.Collections.ObjectModel;

namespace ASP.Seminar1.Models
{
    public class Category : BaseModel
    {
        public virtual List<Product> Products { get; set; } =  new List<Product>();
    }
}
