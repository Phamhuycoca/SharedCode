using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedApplication.BaseHandler.Command;
using SharedCode.Application.Handler;
using SharedCode.Application.Responses;
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

    public async Task<ApiResponse<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = _context.Set<TEntity>().Where("x => id == @0", request.id).FirstOrDefault();
            if (result == null) 
            {
                throw new AppException(System.Net.HttpStatusCode.NotFound,"Không tìm thấy thông tin");
            }
            _context.Set<TEntity>().Remove(result);
            var affected = await _context.SaveChangesAsync();
            return new ApiResponse<bool>
            {
                Data = affected > 0,
                StatusCode = 200,
                Message = "Xóa thành công"
            };
        }
        catch (Exception ex2)
        {
            Exception ex = ex2;
            throw new AppException(System.Net.HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
