##### [Sintaxe básica de escrita e formatação no GitHub](https://docs.github.com/pt/github/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax)<br/>
##### [Criar e realçar blocos de código](https://docs.github.com/pt/github/writing-on-github/working-with-advanced-formatting/creating-and-highlighting-code-blocks)<br/>

## Asp Net Core (DotNet Core 5 ou Dot Net)

### [Exemplos do aplicativo em console](https://github.com/phoenixproject/aspnetcore/tree/master/_SON/_CONSOLE)<br/>

### [Exemplos do aplicado em Asp Net Core](https://github.com/phoenixproject/aspnetcore/tree/master/_SON/_ASPNETCORE)<br/>

#### Para uso do DotNet no VS Code

- Instalar o plugin C#
- Instalar o Nuget Package Manager

##### 01 - Configurações iniciais de um projeto Asp Net Core (classe Startup.cs)

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

##### 02 - Estrutura de pastas e arquivos de um projeto Asp Net Core MVC

###### Estrutura de pastas

- Diretório __bin__: é o local onde fica todo o seu projeto que foi compilado;
- Diretório __Controllers__: é onde ficam todos os controladores da aplicação;
- Diretório __Models__: é geralmente o local que usamos para referencias nossas classes de banco de dados;
- Diretório __obj__: tem similaridade de significado com a pasta bin só que guarda compilados temporários;
- Diretório __Properties__: Dentro deste diretório está o arquivo launchSettings.json que é usado para quando o app fica hospedado;
- Diretório __Views__: é o onde ficam guardados todos os arquivos html de seu projeto;
- Diretório __wwwroot__: é o local onde ficam todos seus arquivos estáticos (js, css, etc);
- Diretório __bin__: é o local onde fica todo o seu projeto que foi compilado;

###### Estrutura de arquivos

- Arquivo __appsettings.json__: é onde são definidas algumas configurações e inclusive de banco de dados;
- Arquivo __Program.cs__: é o local onde está o método Main (primeiro método a ser executado na app);
- Arquivo __Startup.cs__: é o arquivo que guarda todas as configurações de seu aplicativo Asp Net Core;
- Arquivo __aspcore.csproj__: é o local onde são informadas as dependências de seu projeto;

###### Considerações sobre arquivos

Na classe chamada __Program.cs__ mais especificamente no método abaixo, é o local onde será criado um servidor web.

```csharp
public static void Main(string[] args)
{
	CreateHostBuilder(args).Build().Run();
}
```

E no método a seguir (na mesma classe) será criado um host web usando as configurações da classe __Startup.cs__ conforme informado.

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
	Host.CreateDefaultBuilder(args)
		.ConfigureWebHostDefaults(webBuilder =>
		{
			webBuilder.UseStartup<Startup>();
		});
```

Na classe __Startup.cs__ existem dois tipos de configurações para serem feitas:

- Método __ConfigureServices__

Este método é o lugar onde geralmente são configurados os serviços, algo como conexão com banco de dados.

- Método __Configure__
  - Configuração __app.UseStaticFiles()__: define que arquivos estáticos como js, png, css, etc possam ser utilizados por sua aplicação;
  - Configuração __app.UseCookiePolicy()__: permite que sejam usados cookies em sua app;
  - Configuração __app.UseHttpsRedirection()__: permite que seja possível ser feito redirecionamento por https (caso esteja usando https);
  - Configuração __app.UseMvc()__: habilita o uso do MVC na aplicação, bem como sua configuração de controllers;

- Método __Startup(IConfiguration configuration)__ 
Este método inicial recebe o parâmetro __configuration__ que é o conteúdo do arquivo __appsettings.json__.

##### 03 - Configurações para Conectar no banco de Dados SQL Server

###### O que o código abaixo fará?

Caso deseje mapear seu banco de dados em formato de classe, abra o console do console do DotNet e execute o código abaixo.

```bash
Scaffold-DbContext "Data Source=my_server_name;Initial Catalog=MYDATABASE;Integrated Security=False;Persist Security Info=False;
User ID=usrapp;Password=%`$hd77dha33" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\_MYMODELS -force
```

Repare que se existir o caractere '%' no password de seu banco de dados para ser utilizado o Scaffold onde houver % deverá conter seguidamente uma crase (`).

Meu password original: %$hd77dha33