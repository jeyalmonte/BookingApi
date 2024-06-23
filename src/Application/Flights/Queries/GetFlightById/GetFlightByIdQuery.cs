using Application.Flights.Dtos;
using Domain.Common.Models;
using MediatR;

namespace Application.Flights.Queries.GetFlightById;

public record GetFlightByIdQuery(Guid Id) : IRequest<Result<FlightResponse>>;
