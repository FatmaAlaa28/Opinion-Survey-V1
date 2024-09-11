using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Opinion_Survey.Confirm_Email;
using Opinion_Survey.DTO;
using Opinion_Survey.Extension;
using Opinion_Survey.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
namespace Opinion_Survey
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            /*
              builder.Services.AddIdentity<User,IdentityRole>(  option => { 
                 option.SignIn.RequireConfirmedEmail = true; }) 
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddTransient<IEmailSender, EmailSender>();*/

            // add configration of JWT for external request
            //custom configration in folder Extension


            builder.Services.AddCustomJwtAuthen(builder.Configuration);


            //log google
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins() // Specify your frontend URL here
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials(); // If you need to include credentials like cookies or authorization headers
                    });
            });
            //Log Google

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            // log google
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("AllowSpecificOrigins");
            // Log Google


            app.UseAuthentication(); // ������ �� ���� token
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}