using System;
using System.Collections.Generic;
using CL.UCN.DISC.PDIS.SCE.Server.ZeroIce.Model;
using GenFu;
using Microsoft.Extensions.Logging;

namespace CL.UCN.DISC.PDIS.SCE.Server.Controller {

    /// <summary>
    /// Interface de generacion de datos.
    /// </summary>
    public interface IDataGenerator<T> where T : class {
        List<T> Collection();

        List<T> Collection(int length);

        T Instance();
    }

    /// <summary>
    /// Implementacion concreta, pero generica.
    /// </summary>
    public class DataGenerator<T> : IDataGenerator<T> where T : class, new() {

        public List<T> Collection() => A.ListOf<T>();

        public List<T> Collection(int length) => A.ListOf<T>(length);

        public T Instance() => A.New<T>();

    }

    /// <summary>
    /// Servicio de generacion de datos.
    /// </summary>
    public class DataGeneratorService {

        /// <summary>
        /// The Logger.
        /// </summary>
        private readonly ILogger<DataGeneratorService> _logger;

        /// <summary>
        /// Generador de datos de Personas.
        /// </summary>
        private readonly IDataGenerator<Persona> _personaGen = new DataGenerator<Persona>();

        /// <summary>
        /// Generador de datos de Vehiculos.
        /// </summary>
        private readonly IDataGenerator<Vehiculo> _vehiculoGen = new DataGenerator<Vehiculo>();

        /// <summary>
        /// Constructor del generador de datos.
        /// </summary>
        public DataGeneratorService(ILogger<DataGeneratorService> logger) {

            _logger = logger;

            _logger.LogDebug(LE.Generate, "Configuring ..");
            
            // Contador para avanzar por los ruts.
            var rutIndex = 0;

            // Arreglo no tan al azar de ruts.
            string[] ruts = {
                "19691840K",
                "153331290",
                "203784904",
                "13489303K",
                "148294083",
                "160038895",
                "16992003K",
                "148961345"
            };

            var placaIndex = 0;

            string[] placas = {
                "CA-FA-23",
                "BD-JL-10",
                "FJ-CM-27",
                "LA-OB-19",
                "OR-MK-01",
                "RA-FR-24",
                "DI-BR-22",
                "CS-TL-99",
                "KN-SB-02",
                "ST-NS-13",
                "CR-CY-38",
                "ST-WR-04",
                "BN-HA-03",
                "MT-RD-29",
            };

            // Lista con las definiciones de Tipo.
            var tipoValues = Enum.GetValues(typeof(Tipo));
            var rnd = new Random();

            _logger.LogDebug(LE.Generate, "Using {0} ruts.", ruts.Length);

            // Configuracion del POCO de Persona.
            _logger.LogDebug(LE.Generate, "Configuring ThePersona generator ..");
            A.Configure<Persona>()
                .Fill(x => x.id, 0)
                .Fill(x => x.rut, () => ruts[rutIndex++])
                .Fill(x => x.nombres).AsFirstName()
                .Fill(x => x.apellidos).AsLastName()
                .Fill(x => x.email, x => { return string.Format("{0}.{1}@ucn.cl", x.nombres, x.apellidos).ToLower(); })
                .Fill(x => x.movil).AsPhoneNumber()
                .Fill(x => x.unidad).AsMusicGenreName()
                .Fill(x => x.anexo).AsPhoneNumber();
                //.Fill(x => x.rol () => )

            _logger.LogDebug(LE.Generate, "Using {0} placas.", placas.Length);
            _logger.LogDebug(LE.Generate, "Configuring TheVehiculo generator ..");

            A.Configure<Vehiculo>()
                .Fill(x => x.id, 0)
                .Fill(x => x.placa, () => placas[placaIndex++])
                .Fill(x => x.marca).AsCanadianProvince()
                .Fill(x => x.tipo, () => (Tipo) tipoValues.GetValue(rnd.Next(tipoValues.Length)))
                .Fill(x => x.anio, "2017");

        }

        /// <summary>
        /// Genera un listado de personas de forma dinamica.
        /// </summary>
        public List<Persona> GeneratePersonas() {
            return _personaGen.Collection(8);
        }

        /// <summary>
        /// Genera un listado de vehiculos de forma dinamica.
        /// </summary>
        public List<Vehiculo> GenerateVehiculos() {
            return _vehiculoGen.Collection(14);
        }

    }
}