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

public class UpdateCommandHanlder<TDbContext, TEntity> : BaseCommandHanlder<TDbContext, TEntity> where TDbContext : DbContext where TEntity : class
{
    public UpdateCommandHanlder(TDbContext context, IMapper mapper, IMediator mediator)
        : base(context, mapper, mediator)
    {
    }

    public async Task<ApiResponse<TDto>> Handle<TDto>(UpdateCommand<TDto> request, CancellationToken cancellationToken) where TDto : class
    {
        try
        {
            DbSet<TEntity> _repo = _context.Set<TEntity>();
            TEntity entity = await _repo.FindAsync(request.id);
            if (entity == null)
            {
                throw new AppException(System.Net.HttpStatusCode.NotFound,"Không tìm thấy thông tin");
            }

            MapToEntity(request.data, entity);
            _repo.Update(entity);
            if (await _context.SaveChangesAsync(cancellationToken) >= 1)
            {
                //return _mapper.Map<TEntity, TDto>(entity);
                return new ApiResponse<TDto>
                {
                    Data = _mapper.Map<TEntity, TDto>(entity),
                    Message = "Cập nhật thông tin thành công",
                    StatusCode = (int)System.Net.HttpStatusCode.OK
                };
            }

            throw new AppException(System.Net.HttpStatusCode.InternalServerError,"Có lỗi xảy ra trong quá trình cập nhật");
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected virtual void MapToEntity<TDto>(TDto data, TEntity entity) where TDto : class
    {
        _mapper.Map(data, entity);
    }
}
