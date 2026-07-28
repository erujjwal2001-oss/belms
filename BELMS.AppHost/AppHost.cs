var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BELMS_Api>("belms-api");

builder.AddProject<Projects.BELMS_Frontend>("belms-frontend");


builder.Build().Run();
