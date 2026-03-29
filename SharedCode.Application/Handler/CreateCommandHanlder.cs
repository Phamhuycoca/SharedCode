using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedApplication.BaseHandler.Command;
using SharedCode.Application.Handler;
using SharedCode.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedApplication.BaseHandler.Handler;

public class CreateCommandHanlder<TDbContext, TEntity> : BaseCommandHanlder<TDbContext, TEntity> where TDbContext : DbContext where TEntity : class
{
    public CreateCommandHanlder(TDbContext context, IMapper mapper, IMediator mediator)
        : base(context, mapper, mediator)
    {
    }

    public async Task<ApiResponse<TDto>> Handle<TDto>(CreateCommand<TDto> request, CancellationToken cancellationToken) where TDto : class
    {
        try
        {
            TEntity entity = MapToEntity(request.data);
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
            if (await _context.SaveChangesAsync(cancellationToken) >= 1)
            {
                return new ApiResponse<TDto>
                {
                    Data = _mapper.Map<TEntity, TDto>(entity),
                    Message = "Thêm mới thông tin thành công",
                    StatusCode = (int)System.Net.HttpStatusCode.OK
                };
            }

            throw new AppException(System.Net.HttpStatusCode.InternalServerError,"Có lỗi xảy ra trong quá trình thêm mới");
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected virtual TEntity MapToEntity<TDto>(TDto data) where TDto : class
    {
        return _mapper.Map<TDto, TEntity>(data);
    }
}
