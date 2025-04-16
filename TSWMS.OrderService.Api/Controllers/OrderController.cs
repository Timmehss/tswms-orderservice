#region Usings

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models;

#endregion

namespace TSWMS.OrderService.Api.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderManager _orderManager;
    private readonly IMapper _mapper;

    public OrderController(IOrderManager orderManager, IMapper mapper)
    {
        _orderManager = orderManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        try
        {
            var orders = await _orderManager.GetOrdersAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found.");
            }

            return Ok(_mapper.Map<List<OrderDto>>(orders));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        try
        {
            var order = _mapper.Map<Order>(orderDto);

            await _orderManager.CreateOrderAsync(order);

            return CreatedAtAction(nameof(GetOrders), new { id = order.OrderId }, order);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

}
