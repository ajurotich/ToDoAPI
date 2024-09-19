using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToDoAPI.Models;

public partial class TodoContext : DbContext {
	public TodoContext() {
	}

	public TodoContext(DbContextOptions<TodoContext> options)
		: base(options) {
	}

	public virtual DbSet<Category> Categories { get; set; }

	public virtual DbSet<ToDo> ToDos { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
		=> optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=Todo;Trusted_Connection=true;MultipleActiveResultSets=true;Encrypt=false");

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<Category>(entity => {
			entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
			entity.Property(e => e.Description).HasMaxLength(250);
			entity.Property(e => e.Name).HasMaxLength(50);
		});

		modelBuilder.Entity<ToDo>(entity => {
			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
			entity.Property(e => e.Description).HasMaxLength(300);
			entity.Property(e => e.Name).HasMaxLength(50);

			entity.HasOne(d => d.Category).WithMany(p => p.ToDos)
				.HasForeignKey(d => d.CategoryId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_ToDos_Categories");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
