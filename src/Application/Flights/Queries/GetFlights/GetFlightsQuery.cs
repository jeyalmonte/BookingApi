using Application.Flights.Dtos;
using MediatR;

namespace Application.Flights.Queries.GetFlights;

public record GetFlightsQuery(string? Status) : IRequest<IReadOnlyList<FlightResponse>>;
