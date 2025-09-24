using AutoMapper;
using PruebaTecnicaBank.Core.DTOs;
using PruebaTecnicaBank.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PruebaTecnicaBank.Infrastructure.Mappings
{
    // <summary>
    // Clase que define los perfiles de mapeo para AutoMapper.
    // </summary>
    public class MappingProfile : Profile
    {
        // <summary>
        // Constructor que configura los mapeos entre DTOs y entidades.
        // </summary>
        public MappingProfile()
        {
            // Mapeo para Cliente
            CreateMap<ClienteCrearDto, Cliente>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignorar el Id al crear un cliente
                .ForMember(dest => dest.Sexo, opt => opt.MapFrom(src => Enum.Parse<Sexo>(src.Sexo))); // Convertir string a enum

            CreateMap<Cliente, ClienteRespuestaDto>()
                .ForMember(dest => dest.Sexo, opt => opt.MapFrom(src => src.Sexo.ToString())); // Convertir enum a string

            // Mapeo para Cuenta
            CreateMap<CuentaCrearDto, Cuenta>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignorar el Id al crear una cuenta
                .ForMember(dest => dest.NumeroCuenta, opt => opt.Ignore()) // Ignorar el número de cuenta (se asignará manualmente)
                .ForMember(dest => dest.Saldo, opt => opt.MapFrom(src => src.SaldoInicial)); // Asignar el saldo inicial

            CreateMap<Cuenta, CuentaRespuestaDto>(); // Mapeo de Cuenta a CuentaRespuestaDto

            // Mapeo para Transacción
            CreateMap<Transaccion, TransaccionRespuestaDto>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToString())); // Convertir el tipo de transacción a string
        }
    }
}
