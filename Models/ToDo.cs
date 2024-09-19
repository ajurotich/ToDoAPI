﻿using System;
using System.Collections.Generic;

namespace ToDoAPI.Models;

public partial class ToDo
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public bool Done { get; set; }

    public virtual Category Category { get; set; } = null!;
}
