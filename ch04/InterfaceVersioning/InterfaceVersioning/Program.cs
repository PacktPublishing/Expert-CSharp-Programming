using InterfaceVersioning;

Customer customer1 = new("John Doe", DateTime.Now.AddYears(-2));

RegularCustomer customer2 = new("Jane Doe", DateTime.Now.AddYears(-2));

CustomerWithDiscount(customer1);
CustomerWithDiscount(customer2);

void CustomerWithDiscount(ICustomer customer)
{
    Console.WriteLine($"Customer: {customer.Name}, Discount: {customer.Discount()}");
}
