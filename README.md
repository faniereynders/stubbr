# Stubbr: Quick stubs for ASP.NET Core
Building back-end services sometimes require stubbing of stuff. Stubbr is middleware for ASP.NET Core that provide stubs from given pre-defined request endpoints configuration. Stubs can be sourced from local disk or a remote location. The stub configuration system is built ontop of ASP.NET Core's configuration model, having support for JSON, XML and INI files out of the box.

### Installing
Stubbr can be installed from Nuget using the following command:
```
Install-Package Stubbr
```
Or you can also manually add Stubbr to the `project.json` file
```
{
  "dependencies": {
     "Stubbr": "0.0.1"
  }
}
```

### Usage
Using Stubbr should be easy and simple. All it requires is some configuration about the request and details on the response that must be provided. Given the following configuration within `mystubs-config.json`:
```
{
  "Stubs": {
    "GET /foo/bar": {
      "Headers": {
        "X-Header-1": "X-Header-1-Value",
        "Content-Type": "application/json"
      },
      "Body": "./stubs/response1.json",
      "Status": 200
    },
    "POST /api/foo/bar/baz": {
      "Headers": {
        "X-Header-3": "X-Header-3-Value",
        "Content-Type": "application/xml"
      },
      "Body": "./stubs/response2.xml",
      "Status": 201
    }
  }
}
```

Simply load the configuration into the configuration system of ASP.NET Core:

```csharp
public Startup(IHostingEnvironment env)
{
  var builder = new ConfigurationBuilder()
  
  //...
  builder.AddJsonFile("stubs.config.json");
  
  Configuration = builder.Build();
}

public IConfigurationRoot Configuration { get; }
```

Following the Add/Use convention pattern in ASP.NET Core, we add Stubbr to the `IServiceCollection`, passing in the configuration section and then registering it within the pipeline:

```csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddStubs(Configuration.GetSection("Stubs"));
}

public void Configure(IApplicationBuilder app)
{
  app.UseStubs();
}
```

Thats it - now when you request these configured endpoints, the stub responses will be returned.
