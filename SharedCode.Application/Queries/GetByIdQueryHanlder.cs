using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedApplication.BaseHandler.Command;
using SharedCode.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedApplication.BaseHandler.Handler;


public class GetByIdQueryHanlder<TDbContext, TEntity> : BaseQueryHanlder<TDbContext, TEntity> where TDbContext : DbContext where TEntity : class
{
    public GetByIdQueryHanlder(TDbContext context, IMapper mapper, IMediator mediator)
        : base(context, mapper, mediator)
    {
    }

    public async Task<TDto> Handle<TDto>(GetByIdCommand request, CancellationToken cancellationToken) where TDto : class
    {
        try
        {
            TEntity entity = await _context.Set<TEntity>().FindAsync(request.id);
            if (entity == null)
            {
                throw new Exception("Không tìm thấy thông tin");
            }

            return _mapper.Map<TEntity, TDto>(entity);
        }
        catch (Exception ex2)
        {
            Exception ex = ex2;
            throw new Exception(ex.Message);
        }
    }
}
