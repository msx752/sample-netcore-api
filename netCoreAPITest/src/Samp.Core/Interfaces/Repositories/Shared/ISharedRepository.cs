﻿using Microsoft.EntityFrameworkCore;
using System;

namespace Samp.Core.Interfaces.Repositories.Shared
{
    public interface ISharedRepository<TDbContext>
        : ISharedRepository
        where TDbContext : DbContext
    {
        IEFRepository<TEntity> Db<TEntity>() where TEntity : class;
    }

    public interface ISharedRepository : IDisposable
    {
        int SaveChanges();
    }
}