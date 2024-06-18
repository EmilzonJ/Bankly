namespace Domain.Collections;

public class AccountWithCustomer : Account
{
    public required Customer Customer { get; set; }
}
