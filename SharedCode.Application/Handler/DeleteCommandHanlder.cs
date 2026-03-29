using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedApplication.BaseHandler.Command;
using SharedCode.Application.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace SharedApplication.BaseHandler.Handler;

public class DeleteCommandHanlder<TDbContext, TEntity> : BaseCommandHanlder<TDbContext, TEntity> where TDbContext : DbContext where TEntity : class
{
    public DeleteCommandHanlder(TDbContext context, IMapper mapper, IMediator mediator)
        : base(context, mapper, mediator)
    {
    }

    public async Task<int> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = _context.Set<TEntity>().Where("x => id == @0", request.id).FirstOrDefault();
            if (result == null) 
            {
                throw new Exception("Không tìm thấy thông tin");
            }
            _context.Set<TEntity>().Remove(result);
            _context.SaveChanges();
            return 1;
        }
        catch (Exception ex2)
        {
            Exception ex = ex2;
            throw new Exception(ex.Message);
        }
    }
}
