﻿using LanchesTeste.Areas.Admin.Servicos;
using LanchesTeste.Context;
using LanchesTeste.Models;
using LanchesTeste.Repositories;
using LanchesTeste.Repositories.Interfaces;
using LanchesTeste.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace LanchesTeste;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 3;
            options.Password.RequiredUniqueChars = 1;
        });

        services.Configure<ConfigurationImagens>(Configuration.GetSection("ConfigurationPastaImagens"));

        services.AddTransient<ILanchesRepository, LancheRepository>();
        services.AddTransient<ICategoriaRepository, CategoriaRepository>();
        services.AddTransient<IPedidoRepository, PedidoRepository>();
        services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
        services.AddScoped<RelatorioVendasService>();
        services.AddScoped<GraficosVendasService>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin",
                politica =>
                {
                    politica.RequireRole("Admin");
                });
        });

        services.AddScoped(sp  => CarrinhoCompra.GetCarrinho(sp));

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddPaging(options =>
        {
            options.ViewName= "Bootstrap4";
            options.PageParameterName= "pageindex";
        });

        services.AddControllersWithViews();

        services.AddMemoryCache();
        services.AddSession();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedUserRoleInitial seedUserRoleInitial)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseRouting();

        //cria os perfis
        seedUserRoleInitial.SeedRoles();
        //cria os Usuarios e atributos do perfil
        seedUserRoleInitial.SeedUsers();

        app.UseSession();

        app.UseAuthentication();
        app.UseAuthorization();
       

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                 name: "categoriaFiltro",
                 pattern: "Lanche/{action}/{categoria?}",
                 defaults: new {controller = "Lanche", Action = "List" });


            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}