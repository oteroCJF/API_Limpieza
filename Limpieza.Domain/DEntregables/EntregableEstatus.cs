﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Domain.DEntregables
{
    public class EntregableEstatus
    {
        public int EntregableId { get; set; }
        public int EstatusId { get; set; }
        public bool Multiple { get; set; }

    }
}
