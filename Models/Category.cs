using System;
using System.Collections.Generic;

namespace ToDoAPI.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<ToDo> ToDos { get; set; } = new List<ToDo>();
}
