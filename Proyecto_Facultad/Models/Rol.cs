﻿using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
