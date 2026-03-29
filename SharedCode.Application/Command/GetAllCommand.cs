using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedApplication.BaseHandler.Command;

public record GetAllCommand
{
    [FromRoute]
    public int page { get; set; } = 1;
    [FromRoute]
    public int page_size { get; set; } = 20;
    [FromRoute]
    public string? search { get; set; }
    [FromRoute]
    public string? filter { get; set; }
    [FromRoute]
    public string? sort { get; set; }
}
