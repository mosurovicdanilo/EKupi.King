﻿using EKupi.Application.Common.Exceptions;
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
    public class EditOrderCommand : IRequest
    {
        public EditOrderCommand()
        {
            OrderDetails = new List<UpdateOrderDetailDto>();
        }
        public long Id { get; set; }
        public string CustomerId { get; set; }
        public Guid OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public IEnumerable<UpdateOrderDetailDto> OrderDetails { get; set; }
    }

    public class UpdateOrderDetailDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class EditOrderCommandHandler : IRequestHandler<EditOrderCommand>
    {
        private readonly IApplicationDbContext _context;

        public EditOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.Id);
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            if(customer == null)
            {
                throw new NotFoundException("Customer not found");
            }

            order.CustomerId = request.CustomerId;
            order.OrderNumber = request.OrderNumber;
            order.OrderDate = request.OrderDate;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
