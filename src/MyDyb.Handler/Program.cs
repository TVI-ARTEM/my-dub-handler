using Microsoft.AspNetCore.Hosting;
using MyDyb.Handler;

var builder = Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(x => x.UseStartup<Startup>());

var app = builder.Build();

app.Run();