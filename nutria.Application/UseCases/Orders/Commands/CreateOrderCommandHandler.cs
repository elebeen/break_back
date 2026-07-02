using MediatR;
using Nutria.Domain.Dtos.Checkout;
using Nutria.Domain.Dtos.Order;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Orders.Commands;

public record CreateOrderCommand(CheckoutRequest CheckoutRequest) : IRequest<OrderResponse>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var checkout = request.CheckoutRequest;

        if (checkout.Items == null || checkout.Items.Count == 0)
            throw new ArgumentException("El carrito no puede estar vacío.");

        var mealIds = checkout.Items
            .Select(x => x.MealId)
            .ToList();

        var meals = await _unitOfWork.Meals.GetMealsByIdsAsync(mealIds);

        if (meals.Count != mealIds.Count)
            throw new Exception("Uno o más platillos no existen.");

        var order = new Order
        {
            UserId = checkout.UserId,
            RestaurantId = checkout.RestaurantId,
            DeliveryAddress = checkout.DeliveryAddress,
            OrderStatus = "Pendiente",
            OrderItems = new List<OrderItem>()
        };

        decimal totalAmount = 0;

        foreach (var item in checkout.Items)
        {
            var meal = meals.First(x => x.Id == item.MealId);

            if (meal.RestaurantId != checkout.RestaurantId)
                throw new Exception(
                    $"El platillo '{meal.Name}' no pertenece al restaurante seleccionado.");

            var orderItem = new OrderItem
            {
                MealId = meal.Id,
                Quantity = item.Quantity,
                UnitPrice = meal.Price
            };

            order.OrderItems.Add(orderItem);

            totalAmount += meal.Price * item.Quantity;
        }

        order.TotalAmount = totalAmount;

        await _unitOfWork.Orders.AddAsync(order);

        await _unitOfWork.SaveChanges();

        var response = await _unitOfWork.Orders
            .GetOrderDetailsByIdAsync(order.Id);

        return response
               ?? throw new Exception("No se pudo generar la orden.");
    }
}