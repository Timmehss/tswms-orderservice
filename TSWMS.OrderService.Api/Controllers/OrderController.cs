﻿#region Usings

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Shared.Interfaces;

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
            var orders = await _orderManager.GetOrders();

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
}
