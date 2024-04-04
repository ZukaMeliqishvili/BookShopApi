using System.Diagnostics.Contracts;

namespace BookShopApi.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        public string Code {  get; set; }
        public decimal Rate {  get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
