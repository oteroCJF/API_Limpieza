﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Service.EventHandler.Commands.Contratos
{
    public class ContratoDeleteCommand : IRequest<int>
    {
        public int Id { get; set; }
        public DateTime FechaEliminacion { get; set; }

    }
}
