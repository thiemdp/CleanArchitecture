using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Shared.Constants.Permission
{
    [AttributeUsage(AttributeTargets.Field )]
    public class MasterAttribute: Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class TenantAttribute : Attribute
    {
    }
}
