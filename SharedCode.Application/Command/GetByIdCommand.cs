using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedApplication.BaseHandler.Command;

public record GetByIdCommand
{
    [FromRoute]
    public Guid id { get; set; }
}
