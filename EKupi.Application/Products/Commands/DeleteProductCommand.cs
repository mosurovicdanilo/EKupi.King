using EKupi.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Products.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public DeleteProductCommand(int id) 
        {
            Id = id;
        }
        public int Id { get; set; }
    }

    public class DeleteProductCOmmandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private IApplicationDbContext _context;
        public DeleteProductCOmmandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                throw new Exception();
            }
            product.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
