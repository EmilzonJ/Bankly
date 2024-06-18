namespace Domain.Collections;

public class AccountCustomer : Account
{
    public required List<Customer> Customer { get; set; }
}
