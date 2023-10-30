using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PersonApi.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonApi.Controllers
{
    [Route("[controller]")]
    public class PersonaController : Controller
    {
        private readonly String StringConector;

        public PersonaController(IConfiguration config)
        {

            StringConector = config.GetConnectionString("OracleConnection");
        }



        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> ListarPersona()
        {
            try {

                using (OracleConnection conecta = new OracleConnection(StringConector))
                {

                    await conecta.OpenAsync();

                    string sentencia = "SELECT * FROM PERSONA";

                    List<Persona> personal = new List<Persona>();

                    using (OracleCommand comandos = new OracleCommand(sentencia, conecta))
                    

                    using (var lector = await comandos.ExecuteReaderAsync())
                    {

                            while (await lector.ReadAsync())
                            {

                                personal.Add(new Persona
                                {

                                    idPersona = lector.GetInt32(0),
                                    NombrePersona = lector.GetString(1),
                                    ApellidoPat = lector.GetString(2),
                                    ApellidoMat = lector.GetString(3),
                                    EdadPersona = lector.GetInt32(4),
                                    FechaNac = lector.GetDateTime(5),
                                    GeneroPersona = lector.GetString(6)

                                });


                            }

                            return StatusCode(200, personal);

                    }

                    



                }





            }
            catch(Exception ex)
            {
                return StatusCode(500, "No se pudo listar los registros por: " + ex);

            }
            

        }






        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ListarPersona(int id)
        {
            try {

                using (OracleConnection conectar = new OracleConnection(StringConector))
                {


                    await conectar.OpenAsync();

                    string sentencia = "SELECT * FROM PERSONA WHERE ID = :id";

                    Persona personal = new Persona();

                    using(OracleCommand comandos = new OracleCommand(sentencia, conectar))
                    {

                        comandos.Parameters.Add(new OracleParameter(":id", id));

                        using (var lector = await comandos.ExecuteReaderAsync())
                        {


                            if(await lector.ReadAsync())
                            {



                                personal.idPersona = lector.GetInt32(0);
                                personal.NombrePersona = lector.GetString(1);
                                personal.ApellidoPat = lector.GetString(2);
                                personal.ApellidoMat = lector.GetString(3);
                                personal.EdadPersona = lector.GetInt32(4);
                                personal.FechaNac = lector.GetDateTime(5);
                                personal.GeneroPersona = lector.GetString(6);


                                return StatusCode(200, personal);

                            }
                            else
                            {

                                return StatusCode(404, "No se encuentra el registro");

                            }

                            

                        }


                    }


                }

            }
            catch(Exception ex) {

                return StatusCode(500, "No se puede realizar la peticion por: " + ex);
            }

            
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> GuardarPersona ([FromBody]Persona persona)
        {

            try {

                using(OracleConnection conectar = new OracleConnection(StringConector))
                {

                    await conectar.OpenAsync();

                    string sentencia = "INSERT INTO PERSONA (ID,NOMBRE, APELLIDO_PAT, APELLIDO_MAT, EDAD, F_NAC, GENERO) VALUES (:idPersona, :NombrePersona, :ApellidoPat, :ApellidoMat, :EdadPersona, :FechaNac, :GeneroPersona)";

                    using(OracleCommand comandos = new OracleCommand(sentencia, conectar))
                    {

                        comandos.Parameters.Add(new OracleParameter("idPersona", persona.idPersona));
                        comandos.Parameters.Add(new OracleParameter("NombrePersona", persona.NombrePersona));
                        comandos.Parameters.Add(new OracleParameter("ApellidoPat", persona.ApellidoPat));
                        comandos.Parameters.Add(new OracleParameter("ApellidoMat", persona.ApellidoMat));
                        comandos.Parameters.Add(new OracleParameter("EdadPersona", persona.EdadPersona));
                        comandos.Parameters.Add(new OracleParameter("FechaNac", persona.FechaNac));
                        comandos.Parameters.Add(new OracleParameter("GeneroPersona", persona.GeneroPersona));

                        await comandos.ExecuteNonQueryAsync();

                        return StatusCode(201, $"Persona creada correctamente: {persona}");

                    }

                }


            }catch(Exception ex) {

                return StatusCode(500, "No se pudo guardar el registro por :" + ex);

            }








        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarPersona(int id, [FromBody]Persona persona)
        {

            try {


                using (OracleConnection conectar = new OracleConnection(StringConector))
                {

                    await conectar.OpenAsync();

                    string sentencia = "UPDATE PERSONA SET NOMBRE = :NombrePersona , APELLIDO_PAT=:ApellidoPat, APELLIDO_MAT = :ApellidoMat, EDAD =:EdadPersona , F_NAC = :FechaNac, GENERO = :GeneroPersona WHERE ID = :id";

                    using (OracleCommand comandos = new OracleCommand(sentencia, conectar))
                    {


                        comandos.Parameters.Add(new OracleParameter("NombrePersona", persona.NombrePersona));
                        comandos.Parameters.Add(new OracleParameter("ApellidoPat", persona.ApellidoPat));
                        comandos.Parameters.Add(new OracleParameter("ApellidoMat", persona.ApellidoMat));
                        comandos.Parameters.Add(new OracleParameter("EdadPersona", persona.EdadPersona));
                        comandos.Parameters.Add(new OracleParameter("FechaNac", persona.FechaNac));
                        comandos.Parameters.Add(new OracleParameter("GeneroPersona", persona.GeneroPersona));
                        comandos.Parameters.Add(new OracleParameter("id", id));

                        await comandos.ExecuteNonQueryAsync();

                        return StatusCode(200, "Registro editado correctamente");

                    }

                }

            }catch(Exception ex) {

                return StatusCode(500, "No se pudo editar la persona por :" + ex);

            }

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPersona(int id)
        {

            try {

                using (OracleConnection conectar = new OracleConnection(StringConector))
                {

                    await conectar.OpenAsync();

                    string sentencia = "DELETE FROM PERSONA WHERE ID = :id";

                    using(OracleCommand comandos = new OracleCommand(sentencia, conectar))
                    {

                        comandos.Parameters.Add(new OracleParameter("id", id));

                        var borrado = await comandos.ExecuteNonQueryAsync();

                        if(borrado == 0)
                        {

                            return StatusCode(404, "Registro no encontrado!!!");


                        }
                        else
                        {

                            return StatusCode(200, $"Persona con el ID {id} eliminada correctamente");

                        }

                    }

                }


            }catch(Exception ex) {

                return StatusCode(500, "No se pudo eliminar el registro por: " + ex);

            }




        }


    }
}

