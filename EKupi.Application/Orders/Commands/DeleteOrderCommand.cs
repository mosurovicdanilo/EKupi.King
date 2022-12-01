using EKupi.Application.Common.Exceptions;
using EKupi.Application.Products.Commands;
using EKupi.Domain.Entities;
using EKupi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Orders.Commands
{
    public class DeleteOrderCommand : IRequest
    {
        public DeleteOrderCommand(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private IApplicationDbContext _context;
        public DeleteOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            foreach(OrderDetail detail in order.OrderDetails)
            {
                _context.Products.Find(detail.ProductId).UnitsInStock += detail.Quantity;
            }

            _context.OrderDetails.RemoveRange(_context.OrderDetails.Where(x => x.ProductId == request.Id));
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
