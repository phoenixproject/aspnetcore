# Asp Net Core (DotNet Core 5 ou Dot Net)

### [Exemplos do aplicativo em console](https://github.com/phoenixproject/aspnetcore/tree/master/_SON/_CONSOLE)<br/>

### [Exemplos do aplicado em Asp Net Core](https://github.com/phoenixproject/aspnetcore/tree/master/_SON/_ASPNETCORE)<br/>

#### Para uso do VS Code

- Instalar o plugin C#
- Instalar o Nuget Package Manager


##### Configurações iniciais de um projeto Asp Net Core

```csharp

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
		services.AddRazorPages();

		// Helth Checks para monitoramento de balanceamento de carga
		services.AddHealthChecks();

		// Adicionando compatibilidade de versão
		services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

		// Para adicionar Sessão
		services.AddDistributedMemoryCache();
		services.AddSession(options =>
		{
			//options.IdleTimeout = TimeSpan.FromMinutes(100);
			// Set a short timeout for easy testing.
			options.IdleTimeout = TimeSpan.FromDays(14);
			options.Cookie.HttpOnly = true;
			// Make the session cookie essential
			options.Cookie.IsEssential = true;
		});
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Error");
		}

		app.UseStaticFiles();

		// Uso de política de cookies
		app.UseCookiePolicy();

		app.UseRouting();

		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapRazorPages();
		});

		// Adicionando URL para conferência de Health Check
		app.UseHealthChecks("/health");

		// Para adicionar Sessão (Tem que ser antes de UseMvc ou UseEndpoints obrigatoriamente)
		app.UseSession();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
		});
	}
}
```

##### Para Conectar no banco de Dados SQL Server


```bash
Scaffold-DbContext "Data Source=my_server_name;Initial Catalog=MYDATABASE;Integrated Security=False;Persist Security Info=False;
User ID=usrapp;Password=%`$hd77dha33" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\_MYMODELS -force
```

Repare que se existir o caractere '%' no password de seu banco de dados para ser utilizado o Scaffold onde houver % deverá conter seguidamente uma crase (`).

Meu password original: %$hd77dha33