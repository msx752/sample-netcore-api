﻿using Microsoft.EntityFrameworkCore;
using Samp.Core.Interfaces;
using Samp.Core.Interfaces.Repositories.Shared;

namespace Samp.Core.Database
{
    public abstract class ContextSeed<TDbContext>
        : ContextSeed
        , IContextSeed<TDbContext>
        where TDbContext : DbContext
    {
        private bool initiated = false;

        public ContextSeed(ISharedRepository<TDbContext> connection)
        {
            Repository = connection;
        }

        public ISharedRepository<TDbContext> Repository { get; }

        public override sealed void Execute()
        {
            if (initiated)
                return;

            initiated = true;

            CommitSeed();
        }
    }

    public abstract class ContextSeed : IContextSeed
    {
        public abstract void CommitSeed();

        public abstract void Execute();
    }
}