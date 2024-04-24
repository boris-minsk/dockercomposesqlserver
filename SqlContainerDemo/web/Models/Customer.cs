namespace web.Models
{
    public class Customer
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public List<Customer> AllCustomers { get; set; }
    }
}
