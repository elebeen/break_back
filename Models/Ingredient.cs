using System;
using System.Collections.Generic;

namespace break_back.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsAllergen { get; set; }

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}
