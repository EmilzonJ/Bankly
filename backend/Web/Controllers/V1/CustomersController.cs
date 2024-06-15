using Application.Features.Customers.Commands;
using Application.Features.Customers.Models.Responses;
using Application.Features.Customers.Queries;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.V1;

public class CustomersController(ISender sender) : BaseController
{
    [HttpGet]
    public async Task<List<CustomerResponse>> GetAsync()
    {
        return await sender.Send(new GetCustomersQuery());
    }

    [HttpPost]
    public async Task<string> PostAsync(CreateCustomerCommand command)
    {
        return await sender.Send(command);
    }
}
