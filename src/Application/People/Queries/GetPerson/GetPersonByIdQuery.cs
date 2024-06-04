using Application.People.Dtos;
using Domain.Common.Models;
using MediatR;

namespace Application.People.Queries.GetPerson;

public record GetPersonByIdQuery(Guid Id) : IRequest<Result<PersonDto>>;
