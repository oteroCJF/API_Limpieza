﻿using MediatR;
using Limpieza.Domain.DFirmantes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Service.EventHandler.Commands.Firmantes
{
    public class FirmantesUpdateCommand : IRequest<Firmante>
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int InmuebleId { get; set; }
        public string Tipo { get; set; }
        public string Escolaridad { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
