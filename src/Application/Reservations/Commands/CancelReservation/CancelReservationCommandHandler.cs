using Application.Common.Interfaces;
using Domain.Common.Models;
using Domain.Reservations;
using MediatR;

namespace Application.Reservations.Commands.CancelReservation
{
	public class CancelReservationCommandHandler(IAppDbContext _context)
		: IRequestHandler<CancelReservationCommand, Result<Guid>>
	{
		public async Task<Result<Guid>> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
		{
			var reservation = await _context
				.Set<Reservation>()
				.FindAsync([request.Id], cancellationToken);

			if (reservation is null)
			{
				return Error.NotFound("Reservation not found.");
			}

			reservation.CancelReservation();
			await _context.SaveChangesAsync(cancellationToken);

			return reservation.Id;
		}
	}
}
