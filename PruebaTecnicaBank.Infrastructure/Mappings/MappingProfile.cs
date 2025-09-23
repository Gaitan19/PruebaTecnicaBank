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
            CreateMap<ClientCreateDto, Client>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignorar el Id al crear un cliente

            CreateMap<Client, ClientResponseDto>(); // Mapeo de Cliente a ClienteResponseDto

            // Mapeo para Cuenta
            CreateMap<AccountCreateDto, Account>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignorar el Id al crear una cuenta
                .ForMember(dest => dest.AccountNumber, opt => opt.Ignore()) // Ignorar el número de cuenta
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.InitialBalance)); // Asignar el saldo inicial

            CreateMap<Account, AccountResponseDto>(); // Mapeo de Cuenta a AccountResponseDto

            // Mapeo para Transacción
            CreateMap<Transaction, TransactionResponseDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString())); // Convertir el tipo de transacción a string
        }
    }
}
