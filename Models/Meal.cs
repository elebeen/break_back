using System;
using System.Collections.Generic;

namespace break_back.Models;

public partial class Meal
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsActive { get; set; }

    public virtual NutritionalInfo? NutritionalInfo { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}
