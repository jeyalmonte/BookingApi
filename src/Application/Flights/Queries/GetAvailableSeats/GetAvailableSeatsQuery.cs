using Domain.Common.Models;
using MediatR;

namespace Application.Flights.Queries.GetAvailableSeats;

public record GetAvailableSeatsQuery(Guid FlightId) : IRequest<Result<int[]>>;
