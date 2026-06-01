using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Checkout;
using Nutria.Domain.Dtos.Order;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;


namespace nutria.Application.UseCases.Order.Commands;

public abstract record CreateOrderCommand(CheckoutRequest CheckoutRequest) : IRequest<OrderResponse>;

internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppdbContext _appdbContext;
    
    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, AppdbContext appdbContext)
    {
        _unitOfWork = unitOfWork;
        _appdbContext = appdbContext;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
      if (request.CheckoutRequest.Items == null || request.CheckoutRequest.Items.Count == 0)
            throw new ArgumentException("El carrito no puede estar vacío.");

        // 1. Instanciar la orden base (entidad de persistencia)
        var order = new Nutria.Domain.Models.Order
        {
            UserId = request.CheckoutRequest.UserId,
            RestaurantId = request.CheckoutRequest.RestaurantId,
            DeliveryAddress = request.CheckoutRequest.DeliveryAddress,
            OrderStatus = "Pendiente", 
            OrderItems = new List<OrderItem>()
        };

        decimal totalAmount = 0;

        // 2. Procesar cada ítem del carrito
        foreach (var itemDto in request.CheckoutRequest.Items)
        {
            // Consultar el Meal a la BD para obtener el precio real y asegurar que pertenezca al restaurante
            var meal = await _appdbContext.Meals.FirstOrDefaultAsync(m => m.Id == itemDto.MealId, cancellationToken: cancellationToken);
            
            if (meal == null)
                throw new Exception($"El platillo con ID {itemDto.MealId} no existe.");
            
            if (meal.RestaurantId != request.CheckoutRequest.RestaurantId)
                throw new Exception($"El platillo '{meal.Name}' no pertenece al restaurante seleccionado.");

            // Crear el OrderItem congelando el precio unitario
            var orderItem = new OrderItem
            {
                MealId = meal.Id,
                Quantity = itemDto.Quantity,
                UnitPrice = meal.Price
            };

            order.OrderItems.Add(orderItem);
            
            // Sumar al total
            totalAmount += (meal.Price * itemDto.Quantity);
        }

        order.TotalAmount = totalAmount;

        // 3. Persistencia utilizando Unit of Work
        _unitOfWork.Repository<Nutria.Domain.Models.Order>().AddAsync(order);
        await _unitOfWork.SaveChanges();

        // 4. Mapear y proyectar el resultado final al DTO usando LINQ
        // Consultamos de nuevo usando Linq Projections para traer los nombres asociados sin cargar objetos circulares completos
        var responseDto = await _appdbContext.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Meal)
            .Where(o => o.Id == order.Id)
            .Select(o => new OrderResponse
            {
                Id = o.Id,
                UserId = o.UserId,
                UserName = o.User.FullName,
                RestaurantId = o.RestaurantId,
                RestaurantName = o.Restaurant.Name,
                TotalAmount = o.TotalAmount,
                OrderStatus = o.OrderStatus,
                DeliveryAddress = o.DeliveryAddress,
                CreatedAt = o.CreatedAt,
                OrderItems = o.OrderItems.Select(oi => new OrderResponseItem
                {
                    Id = oi.Id,
                    MealId = oi.MealId,
                    MealName = oi.Meal.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return responseDto ?? throw new Exception("Error al generar la respuesta del pedido.");
    }
}