namespace ASP.Seminar1.Models
{
    public class Storage : BaseModel
    {
        public int Count { get; set; }
        public virtual List<Product> Products { get; set; } = new List<Product>();

    }
}
