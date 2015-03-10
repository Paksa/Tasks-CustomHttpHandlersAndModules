using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lab.Day1.Homework.JsonAndAjax.Infrastructure.HttpHelpers.HttpErrors
{
    public interface IHttpResult
    {
        int Status { get; }
        bool IsCancellingRequest { get; }
    }
}
