using System;
namespace PersonApi.Model
{
	public class Persona
	{

        public int idPersona { get; set; }

        public string? NombrePersona { get; set; }

        public string? ApellidoPat { get; set; }

        public string? ApellidoMat { get; set; }

        public int EdadPersona { get; set; }

        public DateTime FechaNac { get; set; }

        public string? GeneroPersona { get; set; }
    }
}

