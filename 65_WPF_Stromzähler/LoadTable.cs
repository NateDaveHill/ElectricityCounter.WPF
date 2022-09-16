using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StromzählerContext;

namespace _65_WPF_Stromzähler;

public class LoadTable
{
    public List<CounterValue> loadTable()
    {
        using SzContext context = new();

        return context.CounterValues.Include(x => x.Counter).Select(x => new CounterValue
        {
            CounterId = x.CounterId,
            Counter = x.Counter,
            Id = x.Id,
            Value = x.Value,
            Date = x.Date
        }).OrderByDescending(x => x.Date).ToList();
    }
}