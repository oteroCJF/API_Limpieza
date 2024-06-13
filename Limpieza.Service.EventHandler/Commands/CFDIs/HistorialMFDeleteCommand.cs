﻿using MediatR;

namespace Limpieza.Service.EventHandler.Commands.CFDIs
{
    public class HistorialMFDeleteCommand : IRequest<int>
    {
        public int RepositorioId { get; set; }
    }
}
