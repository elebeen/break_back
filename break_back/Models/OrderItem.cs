using System;
using System.Collections.Generic;

namespace break_back.Models;

public partial class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid MealId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Meal Meal { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
