using Microsoft.EntityFrameworkCore;

namespace ToDoAPI;


public class Program {
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddCors(
			options => {
				options.AddDefaultPolicy(
					policy => {
						policy
						.AllowAnyHeader()
						.AllowAnyMethod()
						.WithOrigins("OriginPolicy", "http://localhost:3000");
					}
				);
			}
		);

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddDbContext<ToDoAPI.Models.TodoContext>(
			options => {
				options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoDB")); //has to match conn string key in appsettings
			}
		);

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if(app.Environment.IsDevelopment()) {
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.UseCors();

		app.Run();
	}
}
