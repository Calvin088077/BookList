var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.myList>("mylist");

builder.Build().Run();
