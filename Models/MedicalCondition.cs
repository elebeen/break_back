using System;
using System.Collections.Generic;

namespace break_back.Models;

public partial class MedicalCondition
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
