using AutoMapper;
using HD.Application.Tickets.DTOs;
using HD.Domain.Entities;

namespace HD.Application.Tickets;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Ticket, TicketDto>()
            .ForMember(dest => dest.AssignedToUserName, opt => opt.MapFrom(src =>
                src.AssignedToUser != null
                    ? $"{src.AssignedToUser.FirstName} {src.AssignedToUser.LastName}"
                    : null))
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src =>
                $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}"));

        CreateMap<CreateTicketDto, Ticket>();
    }
}
