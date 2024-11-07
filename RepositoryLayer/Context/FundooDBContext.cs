﻿using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class FundooDBContext : DbContext
    {
        public FundooDBContext(DbContextOptions options) : base(options) { }

        public DbSet<UserEntity>? Users { get; set; }
    }
}