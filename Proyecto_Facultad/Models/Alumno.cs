using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Alumno
{
    public class FechaNoFuturaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateOnly fecha)
            {
                return fecha <= DateOnly.FromDateTime(DateTime.Now);
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"La fecha {name} no puede ser mayor que la fecha actual.";
        }
    }
    public int IdAlumno { get; set; }

    [DisplayName("Primer Nombre")]
    [Required(ErrorMessage = "El primer nombre es obligatorio")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string PrimerNombreAlumno { get; set; }


    [DisplayName("Segundo Nombre")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string SegundoNombreAlumno { get; set; }


    [DisplayName("Otros Nombres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ\s]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string OtrosNombresAlumno { get; set; }



    [DisplayName("Primer Apellido")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    public string PrimerApellidoAlumno { get; set; }

    [DisplayName("Segundo Apellido")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letraz")]
    public string SegundoApellidoAlumno { get; set; }




    [DisplayName("Apellido Conyugal")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string ApellidoCasado { get; set; }





    [DisplayName("Fecha de Nacimiento")]
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    [FechaNoFutura(ErrorMessage = "La fecha de nacimiento no puede ser mayor que la fecha actual.")]
    public DateOnly FechaNacimientoAlumno { get; set; }




    [DisplayName("Fecha de Bautizo")]
    [FechaNoFutura(ErrorMessage = "La fecha de bautizo no puede ser mayor que la fecha actual.")]
    public DateOnly? FechaBautizoAlumno { get; set; }




    [DisplayName("Dirección")]
    [Required(ErrorMessage = "La dirección es obligatoria")]
    public string Direccion { get; set; }



    [DisplayName("Teléfono")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El teléfono debe tener 8 dígitos.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe contener exactamente 8 dígitos sin guiones.")]
    [Required(ErrorMessage = "Este campo es obligatorio")]

    public string Telefono { get; set; }



    [DisplayName("DPI")]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "El DPI debe tener 13 dígitos.")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "El DPI debe contener exactamente 13 dígitos sin guiones.")]
    public string Dpi { get; set; }

    [DisplayName("NIT")]
    public string Nit { get; set; }

    [DisplayName("Estado Civil")]
    public string EstadoCivil { get; set; }

    public string Genero { get; set; }

    [DisplayName("Pertenece a la red")]
    public string NumeroCelula { get; set; }

    [DisplayName("Codigo Frater")]
    public string CodigoFrater { get; set; }

    [DisplayName("Inscrito")]
    public bool Becado { get; set; } 

    public bool Retiro { get; set; }

    public bool ReunionesEnConfianza { get; set; }

    public bool Otros { get; set; }

    public bool EstatusAlumno { get; set; }

    public virtual ICollection<AsignacionAlumno> AsignacionAlumnos { get; set; } = new List<AsignacionAlumno>();

    public virtual ICollection<AsistenciaAlumno> AsistenciaAlumnos { get; set; } = new List<AsistenciaAlumno>();
    public string NombreCompleto => $"{PrimerNombreAlumno} {PrimerApellidoAlumno}";
}
