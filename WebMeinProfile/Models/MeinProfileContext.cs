﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebMeinProfile.Models;

public partial class MeinProfileContext : DbContext
{
    public MeinProfileContext()
    {
    }

    public MeinProfileContext(DbContextOptions<MeinProfileContext> options)
        : base(options)
    {
    }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=ANLSDWI;Database=MeinProfile;uid=students;pwd=12345678;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
