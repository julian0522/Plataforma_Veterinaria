using Microsoft.AspNetCore.Identity;
using Veterinaria.Interfaces;
using Veterinaria.Models;
using Veterinaria.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepositorioEspecie, RepositorioEspecie>();
builder.Services.AddTransient<IRepositorioRaza, RepositorioRaza>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IRepositorioCliente, RepositorioCliente>();
builder.Services.AddTransient<IRepositorioSucursal, RepositorioSucursal>();
builder.Services.AddTransient<IServicioUsuario, ServicioUsuario>();
builder.Services.AddTransient<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddTransient<IRepositorioMascota, RepositorioMascota>();
builder.Services.AddTransient<IRepositorioConsultaVeterinaria, RepositorioConsultaVeterinaria>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IUserStore<Usuario>, UsuarioStore>();
builder.Services.AddTransient<SignInManager<Usuario>>();
builder.Services.AddIdentityCore<Usuario>(opciones =>
{
    opciones.Password.RequireDigit = true;
    opciones.Password.RequireLowercase = true;
    opciones.Password.RequireUppercase = true;
    opciones.Password.RequireNonAlphanumeric = true;
}).AddErrorDescriber<MensajesDeErrorIdentity>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme, opciones => 
{
    opciones.LoginPath = "/usuario/login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../Rotativa");

app.Run();

