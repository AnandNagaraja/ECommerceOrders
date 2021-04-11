using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrders.DAL
{
    public class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        protected BaseContext()
        { }

        protected BaseContext(DbContextOptions options) : base(options)
        { }

    }
}
