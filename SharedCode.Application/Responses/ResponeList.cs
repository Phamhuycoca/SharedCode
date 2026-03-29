using Autofac.Features.Metadata;
using SharedApplication.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Application.Responses;

public class ResponeList<T> where T : class
{
    public Meta meta { get; set; } = new Meta();


    public List<T> data { get; set; } = new List<T>();


    public ResponeList()
    {
    }

    public ResponeList(Meta _meta, List<T> _data)
    {
        meta = _meta;
        data = _data;
    }
}
