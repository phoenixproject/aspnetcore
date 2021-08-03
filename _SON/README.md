### Asp Net Core (DotNet Core 5 ou Dot Net)

##### [Sintaxe básica de escrita e formatação no GitHub](https://docs.github.com/pt/github/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax)<br/>
##### [Criar e realçar blocos de código](https://docs.github.com/pt/github/writing-on-github/working-with-advanced-formatting/creating-and-highlighting-code-blocks)<br/>

#### [Exemplos do aplicativo em console](https://github.com/phoenixproject/aspnetcore/tree/master/_SON/_CONSOLE)<br/>

#### [Exemplos da aplicação em Asp Net Core](https://github.com/phoenixproject/aspnetcore/tree/master/_SON/_ASPNETCORE)<br/>

#### Para uso do VS Code

- Instalar o plugin C#
- Instalar o Nuget Package Manager

#### Para uso do Visual Studio

- Escolha um projeto do tipo Asp Net Core do tipo __MVC__ e defina a última versão da biblioteca .Net (no nosso caso por enquanto a 5).

![Alt text](https://github.com/phoenixproject/aspnetcore/blob/master/_MEDIA/01_projeto_asp_net_core_mvc.png?raw=true "Projeto Asp Net Core MVC")

![Alt text](https://github.com/phoenixproject/aspnetcore/blob/master/_MEDIA/02_projeto_asp_net_core_mvc.png?raw=true "Projeto Asp Net Core MVC definindo a biblioteca")

#### 01 - Configurações iniciais de um projeto Asp Net Core (classe Startup.cs)

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
		services.AddControllersWithViews();

		services.AddRazorPages();

		// Helth Checks para monitoramento de balanceamento de carga
		services.AddHealthChecks();

		// Adicionando compatibilidade de versão
		services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

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

#### 02 - Estrutura de pastas e arquivos de um projeto Asp Net Core MVC

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

###### Considerações sobre os arquivos de configuração

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

#### 03 - Controllers

- São responsáveis por toda lógica de fluxo de acesso da aplicação;
- São uma das camadas da arquitetura MVC;
- Um controlador (controller) é o responsável por renderizar as páginas html visualizadas;
- O controlador de nome __HomeController__ é julgado pelo Asp Net Core como controlador inicial (mas acho que isso também pode ser alterado);

- Para que um controller seja definido é necessário ter sem o nome _Controller_ consecutivo do nome do controlador, bem como herdar da classe _Controller_.

Exemplo:

```csharp
public class HomeController : Controller {}

public class ConfigurationController : Controller {}

public class ContactsController : Controller {}	
```

Caso deseja retornar a partir de um controller retornando mensagens para o cliente arquivos, código JSON, status HTTP é necessário criar um método que seu retorno seja do tipo __ActionResult__.

Todo o método do controller (_Action_) tem seu método principal.

- Para criarmos um novo controller 

![Alt text](https://github.com/phoenixproject/aspnetcore/blob/master/_MEDIA/05_projeto_asp_net_core_criando_controller.png?raw=true "Controller novo")

- Para definirmos o que poderá ser retornado em cada __Action__.

	- Podemos retornar uma view html;

	```csharp
	retun View();
	```
	
	- Podemos retornar um arquivo;

	```csharp
	retun File();
	```
	
	- Podemos retornar um JSON;

	```csharp
	retun JSON;
	```
	
	- Podemos retornar inclusive um conteúdo;

	```csharp
	retun Content("Hello World");
	```

#### 04 - Rotas

Rotas são basicamente caminhos para chegar aos controllers.

Em __AdminController__ acesse a rota como: _http://localhost:12671/Admin/Mensagem_ (12671 é um número aleatório);

```csharp
	public IActionResult Mensagem()
	{
		return Content("Mensagem Teste de Conteúdo");
	}
```

- **Mapeando Ações (Actions)**

- Alterando rotas padrões

Da forma como está abaixo além do mapeamento automático do AspNet Core ter sido alterado você terá de colocar um novo caminho, algo como isso: _http://localhost:12671/painel/admin_ e as Actions podem ser mapeadas com verbos HTTP;

```csharp
[Route("painel/admin")]
public class AdminController : Controller
{
	[HttpGet("")]			
	public IActionResult Mensagem()
	{
		return Content("Mensagem Teste de Conteúdo");
	}
}
```
- Adicionando mais rotas para a mesma action

Também pode ser acessada da forma abaixo com mais de um nome de rota para para a mesma ação;

```csharp
[Route("painel/admin")]
public class AdminController : Controller
{
	[HttpGet("teste")]		
	[HttpGet("")]			
	public IActionResult Mensagem()
	{
		return Content("Mensagem Teste de Conteúdo");
	}
	
	[HttpGet("givetitle")]			
	public IActionResult Titulo()
	{
		return Content("Mensagem Teste de Título");
	}
}
```

- Definindo parâmetros para rotas

É possível também chamar a rota passando parâmetros da forma abaixo para acessar da seguinte forma: _http://localhost:12671/principal/Miguel_

```csharp
[Route("painel/admin")]
public class AdminController : Controller
{	
	[HttpGet("principal/{}")]			
	public IActionResult Index(string nome)
	{
		return Content("Olá " + nome);
	}
}
```

- Alterando parâmetros de rotas

Chamando a rota passando parâmetros da forma abaixo para acessar da seguinte forma: _http://localhost:12671/principal/10_

```csharp
[Route("painel/admin")]
public class AdminController : Controller
{	
	[HttpGet("principal/{numero}")]			
	public IActionResult Index(int numero)
	{
		return Content("O valor é " + numero);
	}
}
```

- Definindo tipos para parâmetros de Actions

Chamando a rota passando parâmetros da forma abaixo para acessar da seguinte forma: _http://localhost:12671/principal/10_ e restringindo o parâmetro pelo tipo.

```csharp
[Route("painel/admin")]
public class AdminController : Controller
{	
	[HttpGet("principal/{numero:int}")]			
	public IActionResult Index(int numero)
	{
		return Content("O valor é " + numero);
	}
}
```

- Tornando parâmetros opcionais para Actions.

Chamando a rota passando parâmetros da forma abaixo para acessar da seguinte forma: _http://localhost:12671/principal/10_ e restringindo o parâmetro pelo tipo.

```csharp
[Route("painel/admin")]
public class AdminController : Controller
{	
	[HttpGet("principal/{numero:int?}")]			
	public IActionResult Index(int numero)
	{
		return Content("O valor é " + numero);
	}
}
```

- Adicionando mais de um parâmetro

Chamando a rota passando parâmetros da forma abaixo para acessar da seguinte forma: _http://localhost:12671/principal/10/Miguel_ e restringindo o parâmetro pelo tipo.

```csharp
[Route("painel/admin")]
public class AdminController : Controller
{	
	[HttpGet("principal/{numero:int?}/{nome}")]			
	public IActionResult Index(int numero, string nome)
	{
		return Content("O valor é " + numero + " e o nome é: " + nome);
	}
}
```

- Passando parâmetros com Query String

Chamando a rota passando parâmetros com query string da forma abaixo para acessar da seguinte forma: _http://localhost:12671/principal/nome=Miguel_ e restringindo o parâmetro pelo tipo.

```csharp
[Route("painel/admin")]
public class AdminController : Controller
{	
	[HttpGet("principal/{numero:int?}/{nome}")]			
	public IActionResult Index()
	{
		string nome = Request.Query["nome"];
		return Content("O nome é: " + nome);
	}
}
```

#### 05 - Views

As views servem para renderizar o que é feito num controller. Dentro do diretório __Views__ deve existir um diretório com o mesmo nome de seu __controller__ e dentro deste diretório, páginas __cshtml__ com os mesmos nomes de suas __Actions__. Por exemplo:

Classe _TesteController.cs_

```csharp
namespace AspNetCoreMVC.Controllers
{
    public class TesteController : Controller
    {
        [HttpGet("view")]
        public ActionResult Testando()
        {
            return View();
        }
    }
}
```

View _Testando.cshtml_

```html
<h1>Testando minha View</h1>
```

Para executar a página: _http://localhost:12671/view_.

__Ou__

Classe _TesteController.cs_

```csharp
namespace AspNetCoreMVC.Controllers
{
    public class TesteController : Controller
    {        
        public ActionResult Testando()
        {
            return View();
        }
    }
}
```

View _Testando.cshtml_

```html
<h1>Testando minha View</h1>

```

Para executar a página: _http://localhost:12671/Teste/Testando_.

#### 06 - Passando informações (variáveis) para serem exibidas na view

Classe _TesteController.cs_

```csharp
namespace AspNetCoreMVC.Controllers
{
    public class TesteController : Controller
    {
        [HttpGet("view")]
        public ActionResult Testando()
        {
			viewData["helloWorld"] = false;
            return View();
        }
    }
}
```

View _Testando.cshtml_

```html
<h1>Testando minha View  @ViewData["helloWorld"]</h1>

```

Para executar a página: _http://localhost:12671/view_. 

__Ou__

Classe _TesteController.cs_

```csharp
namespace AspNetCoreMVC.Controllers
{
    public class TesteController : Controller
    {
        [HttpGet("view")]
        public ActionResult Testando()
        {
			viewData["helloWorld"] = false;
            return View();
        }
    }
}
```

View _Testando.cshtml_

```csharp
@if((bool)ViewData["helloWorld"]){
	<h1>Testando minha View</h1>
}
else{
	<h1>Teste down!</h1>
}
```

Para executar a página: _http://localhost:12671/view_. 

__Observação:__

Também é possível retornar uma outra view que esteja dentro da pasta do seu controller no diretório __Views__ se ela for informada no método.

```csharp
namespace AspNetCoreMVC.Controllers
{
    public class TesteController : Controller
    {        
        public ActionResult Testando()
        {			
            return View("outraview");
        }
    }
}
```

#### 07 - Passando dados para formulários básicos

View _form.cshtml_

```html
<form action="/painel/admin/dados" method="post">
	<input type="email" name="email" />
	<input type="text" name="nome" />
	<button>Enviar</button>
</form>
```

Controller _AdminController_

```csharp
[Route("painel/admin")]
    public class AdminController : Controller
    {
        [HttpGet("form")]
        public IActionResult form()
        {
            return View();
        }

        [HttpPost("dados")]
        public IActionResult dados()
        {
            StringValues nome;
            StringValues email;

            Request.Form.TryGetValue("nome", out nome);
            Request.Form.TryGetValue("email", out email);

            return Content("Formulário enviado" + nome + " " + email);
        }
    }
```

Para executar a página: _http://localhost:12671/painel/admin/form_. 

#### 08 - Trabalhando com Models

Para se se ter um model é possível inserir definições para certos atributos da classe no intuito de trabalhar melhor

```csharp
    public class Teste
    {
        [Required]
        [MaxLength(60)]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }
    }
```

Você também pode criar um Controller do tipo MVC

![Alt text](https://github.com/phoenixproject/aspnetcore/blob/master/_MEDIA/06_projeto_asp_net_core_novo_controller_mvc.png?raw=true "Novo Controller MVC")

E escolher integrar uma classe já com View e Entity Framewok também na modalidade MVC, embora isso possa ser feito manualmente mais tarde.

![Alt text](https://github.com/phoenixproject/aspnetcore/blob/master/_MEDIA/08_projeto_asp_net_core_novo_controller_mvc_com_model.png?raw=true "Nova Classe e View na modalidade com Entity Framework")

**Ou**

Criar um Controller do tipo API

![Alt text](https://github.com/phoenixproject/aspnetcore/blob/master/_MEDIA/07_projeto_asp_net_core_novo_controller_api.png?raw=true "Novo Controller API")

E escolher integrar uma classe já com Entity Framewok também na modalidade API, também embora isso possa ser feito manualmente mais tarde.

![Alt text](https://github.com/phoenixproject/aspnetcore/blob/master/_MEDIA/09_projeto_asp_net_core_novo_controller_api_com_model.png?raw=true "Nova Classe na modalidade com Entity Framework")

#### 09 - Exibição da informações em Views passando argumentos ou não

**Para exibir o conteúdo de uma View junto um layout (laoyut principal no caso)**

```csharp
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@{
	Layout = "~/Views/Shared/_Layout.cshtml";
}

<p>
	Teste de Layout
</p>
```

Ao executar a página: _http://localhost:12671/Teste_ será chamado o método Index() abaixo do controller _Teste_. 

```csharp
	public ActionResult Index()
	{
		return View();
	}
```

E o resultado esperado é este:

![Alt text](https://github.com/phoenixproject/aspnetcore/blob/master/_MEDIA/10_projeto_asp_net_core_nova_view_mvc.png?raw=true "Nova View")

**Para exibir o conteúdo de uma View junto um layout nulo**

```csharp
@{
	Layout = null;
}

<p>AnotherAction view</p>
```

Ao executar a página: _http://localhost:12671/Teste/AnotherAction_ será chamado o método AnotherAction(). 

```csharp
	public ActionResult AnotherAction()
	{
		return View();
	}
```

**Para exibir o conteúdo de uma View a partir de determinada Action**

```csharp
@{
	Layout = null;
}

Some View
```

Ao executar a página: _http://localhost:12671/Teste/AnotherView_ será chamado o método AnotherAction(). 

```csharp
		public IActionResult AnotherView()
		{
			return View("SomeView");
		}
```

**Para exibir um parâmetro em uma View vindo de determinada Action**

```csharp
@model string

@{
	Layout = null;
}

ViewWithParameter view

<hr />

@Model
```

Ao executar a página: _http://localhost:12671/Teste/ViewWithParameter_ será chamado o método AnotherAction(). 

```csharp
	public IActionResult ViewWithParameter()
	{
		string SomeString = "I am a string";

		return View("ViewWithParameter", SomeString);
	}
```

**Para exibir parâmetros em uma View vindo de determinada Action**

```csharp
@model string
@{
	Layout = null;
}

PassingData view

<hr />

@ViewBag.Fruit

<br />

@ViewData["Color"]

<br />

@TempData["Number"]
```

Ao executar a página: _http://localhost:12671/Teste/PassingData_ será chamado o método PassingData(). 

```csharp
	public IActionResult PassingData()
	{
		ViewBag.Fruit = "Apples";
		ViewData["Color"] = "Red";

		TempData["Number"] = 5;

		return View();
	}
```

**Para exibir parâmetros em uma View por Query String**

```csharp
@model string

@{
	Layout = null;
}

QueryString view

<hr />

@ViewBag.Name

<br />

@ViewBag.LastName
```

Ao executar a página: _localhost:12671/Teste/QueryString?name=Jonh&lastname=Jonhson_ será chamado o método QueryString(). 

```csharp
	// localhost:12671/Teste/QueryString?name=Jonh&lastname=Jonhson
	public IActionResult QueryString(string name, string lastname)
	{
		ViewBag.Name = name;

		ViewBag.Lastname = lastname;

		return View();
	}
```

**Para redirecionar uma Action**

Ao executar a página: _localhost:12671/Teste/Redirect2_ será chamado o método QueryString(). 

```csharp
	public RedirectResult Redirect2()
	{
		return Redirect("http://www.google.com");
	}
```

**Para Redirecionar para outro controller e ainda passando nome da Action e nome do Controller**

```csharp
@{
	Layout = null;
}

PassingData view

<hr />

@ViewBag.Fruit

<br />

@ViewData["Color"]

<br />

@TempData["Number"]
```

Ao executar a página: _localhost:12671/Teste/Redirect3_ será chamado chamada a view PassingData. 

```csharp
	// Redirecionando para o outro controller
	public IActionResult Redirect3()
	{
		return RedirectToAction("PassingData", "AnotherController");
	}
```

**Para Redirecionar para outro controller e ainda passando nome da Action e nome do Controller**

```csharp
@{
	Layout = null;
}

PassingData view

<hr />

@ViewBag.Fruit

<br />

@ViewData["Color"]

<br />

@TempData["Number"]
```

Ao executar a página: _localhost:12671/Teste/Redirect3_ será chamado chamada a view PassingData. 

```csharp
	// Redirecionando para o outro controller
	public IActionResult Redirect3()
	{
		return RedirectToAction("PassingData", "AnotherController");
	}
```

**Para redirecionar para outra Action do mesmo Controller**

```csharp
@model string

@{
	Layout = null;
}

PassingData view

<hr />

@ViewBag.Fruit

<br />

@ViewData["Color"]

<br />

@TempData["Number"]
```

Ao executar a página: _localhost:12671/Teste/Redirect_ será chamada a view PassingData. 

```csharp
	public IActionResult Redirect()
	{
		return RedirectToAction("PassingData");
	}
```

**Para exibir variáveis ViewData na view a partir de um controller renomeado**

```html
<h1>Testando minha View @ViewData["helloWorld"]</h1>
```

Ao executar a página: _localhost:12671/view_ será chamada a view Testando. 

```csharp
	[HttpGet("view")]
	public ActionResult Testando()
	{
		ViewData["helloWorld"] = false;
		return View();
	}
```

#### 10 - Configurações para Conectar no banco de Dados SQL Server

###### O que o código abaixo fará?

Caso deseje mapear seu banco de dados em formato de classe, abra o console do console do DotNet e execute o código abaixo.

```bash
Scaffold-DbContext "Data Source=my_server_name;Initial Catalog=MYDATABASE;Integrated Security=False;Persist Security Info=False;
User ID=usrapp;Password=%`$hd77dha33" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\_MYMODELS -force
```

Repare que se existir o caractere '%' no password de seu banco de dados para ser utilizado o Scaffold onde houver % deverá conter seguidamente uma crase (`).

Meu password original: %$hd77dha33