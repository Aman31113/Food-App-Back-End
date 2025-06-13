using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;
using Online_food_delivery_system.Repository;
using Online_food_delivery_system.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FoodDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cstring")));
builder.Services.AddScoped<IPayment, PaymentRepository>();
builder.Services.AddScoped<IOrder, OrderRepository>();
builder.Services.AddScoped<IMenuItem, MenuItemRepository>();
builder.Services.AddScoped<IRestaurant, RestaurantRepository>();
builder.Services.AddScoped<ICustomer, CustomerRepository>();
builder.Services.AddScoped<ITokenGenerate, TokenService>();
builder.Services.AddScoped<IDelivery, DeliveryRepository>();
builder.Services.AddScoped<IAgent, AgentRepository>();
builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<RestaurantService>();
builder.Services.AddScoped<MenuItemService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<DeliveryService>();
builder.Services.AddScoped<AgentService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});//cyclic error

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]!)),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                           new OpenApiSecurityScheme
                             {
                                 Reference = new OpenApiReference
                                 {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "Bearer"
                                 }
                             },
                             Array.Empty<string>()

                     }
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();







/////////////////////////////////////////////




//using System.Text; // Provides classes for character encodings (like UTF-8). Needed for JWT key.
//using Microsoft.AspNetCore.Authentication.JwtBearer; // Provides JWT Bearer authentication middleware.
//using Microsoft.EntityFrameworkCore; // Provides Entity Framework Core functionalities, specifically for DbContext and migrations.
//using Microsoft.IdentityModel.Tokens; // Provides security tokens and validation parameters, used for JWT.
//using Microsoft.OpenApi.Models; // Provides classes for OpenAPI (Swagger) document generation, used for security definitions.
//using Online_food_delivery_system.Interface; // Imports the namespace containing your interface definitions (e.g., IPayment, IOrder).
//using Online_food_delivery_system.Models; // Imports the namespace containing your application's data models (e.g., FoodDbContext, User).
//using Online_food_delivery_system.Repository; // Imports the namespace containing your repository implementations.
//using Online_food_delivery_system.Service; // Imports the namespace containing your service implementations.

//// This is the starting point for building your web application.
//// WebApplication.CreateBuilder(args) initializes a new WebApplicationBuilder instance,
//// which provides default configurations and properties for building an ASP.NET Core app.
//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container (Dependency Injection container).
//// Services are objects that your application components (like controllers, other services) depend on.
//// ASP.NET Core uses Dependency Injection to provide these services where they are needed.

//// Registers the MVC (Model-View-Controller) services, which include support for controllers,
//// API endpoints, routing, model binding, and more. This is essential for building web APIs.
//builder.Services.AddControllers();

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//// Registers services required for API exploration, which helps Swagger (OpenAPI)
//// to automatically discover your API endpoints and their details.
//builder.Services.AddEndpointsApiExplorer();

//// for your API, allowing tools like Swagger UI to display interactive API documentation.
//builder.Services.AddSwaggerGen();

//// Configures and registers the FoodDbContext with the Dependency Injection container.
//// This makes your database context available for injection into other services or controllers.
//builder.Services.AddDbContext<FoodDbContext>(options =>
//    // Specifies that Entity Framework Core should use SQL Server as the database provider.
//    // It retrieves the connection string named "cstring" from your application's configuration
//    // (e.g., appsettings.json).
//    options.UseSqlServer(builder.Configuration.GetConnectionString("cstring")));

//// Below are registrations for your custom interfaces, repositories, and services.
//// AddScoped means a new instance of the service is created for each HTTP request.
//// This is a common lifetime for services that interact with a DbContext.

//// Registers IPayment interface to be resolved by PaymentRepository implementation.
//builder.Services.AddScoped<IPayment, PaymentRepository>();
//// Registers IOrder interface to be resolved by OrderRepository implementation.
//builder.Services.AddScoped<IOrder, OrderRepository>();
//// Registers IMenuItem interface to be resolved by MenuItemRepository implementation.
//builder.Services.AddScoped<IMenuItem, MenuItemRepository>();
//// Registers IRestaurant interface to be resolved by RestaurantRepository implementation.
//builder.Services.AddScoped<IRestaurant, RestaurantRepository>();
//// Registers ICustomer interface to be resolved by CustomerRepository implementation.
//builder.Services.AddScoped<ICustomer, CustomerRepository>();
//// Registers ITokenGenerate interface to be resolved by TokenService implementation.
//builder.Services.AddScoped<ITokenGenerate, TokenService>();
//// Registers IDelivery interface to be resolved by DeliveryRepository implementation.
//builder.Services.AddScoped<IDelivery, DeliveryRepository>();
//// Registers IAgent interface to be resolved by AgentRepository implementation.
//builder.Services.AddScoped<IAgent, AgentRepository>();
//// Registers IUser interface to be resolved by UserRepository implementation.
//builder.Services.AddScoped<IUser, UserRepository>();

//// Registers your service classes directly. In a real-world scenario, you might also
//// use interfaces for these services, but direct registration is also valid.
//builder.Services.AddScoped<PaymentService>();
//builder.Services.AddScoped<CustomerService>();
//builder.Services.AddScoped<RestaurantService>();
//builder.Services.AddScoped<MenuItemService>();
//builder.Services.AddScoped<OrderService>();
//builder.Services.AddScoped<DeliveryService>();
//builder.Services.AddScoped<AgentService>();
//builder.Services.AddScoped<UserService>();

//// Reconfigures the MVC services (specifically JSON serialization - Serialization is the process of converting an object's state
//// (its data and properties) into a format that can be easily stored or transmitted.) after AddControllers().
//// This is common to add specific formatters or behaviors.
//builder.Services.AddControllers().AddNewtonsoftJson(options =>
//{
//    // Configures Newtonsoft.Json (the JSON serializer) to handle reference loop errors.
//    // This typically occurs when you have circular references in your object graph (e.g.,
//    // a User has a list of Orders, and each Order has a reference back to the User).
//    // Setting Ignore prevents the serializer from getting stuck in an infinite loop
//    // and throws an error instead, returning a partial object.
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
//}); // cyclic error

//// Configures the application to use JWT Bearer authentication.
//// This means the application will expect and validate JSON Web Tokens (JWTs) for authentication.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    // Adds specific options for JWT Bearer authentication.
//    .AddJwtBearer(options =>
//    {
//        // Defines the parameters for validating incoming JWTs.
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            // Requires that the signing key used to sign the token is validated.
//            // This is crucial for verifying the token's authenticity and preventing tampering.
//            ValidateIssuerSigningKey = true,
//            // Specifies the secret key used for signing and validating tokens.
//            // It's retrieved from your application's configuration (e.g., appsettings.json)
//            // under the "TokenKey" entry and converted to bytes using UTF-8 encoding.
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]!)),
//            // Disables validation of the token's issuer. In production, you would typically
//            // set this to true and validate against a known issuer.
//            ValidateIssuer = false,
//            // Disables validation of the token's audience. In production, you would typically
//            // set this to true and validate against a known audience (e.g., your API's URL).
//            ValidateAudience = false
//        };
//    });

//// Configures Cross-Origin Resource Sharing (CORS) policies.
//// CORS is a security feature in web browsers that restricts web pages from making
//// requests to a different domain than the one that served the web page.
//builder.Services.AddCors(options =>
//{
//    // Defines a CORS policy named "AllowReactApp".
//    options.AddPolicy("AllowReactApp",
//        builder =>
//        {
//            // Specifies the origin(s) that are allowed to make requests to this API.
//            // "http://localhost:5173" is a common default for React development servers.
//            builder.WithOrigins("http://localhost:5173")
//                    // Allows any HTTP header in the request.
//                    .AllowAnyHeader()
//                    // Allows any HTTP method (GET, POST, PUT, DELETE, etc.).
//                    .AllowAnyMethod()
//                    // Allows credentials (like cookies or HTTP authentication headers)
//                    // to be sent with cross-origin requests.
//                    .AllowCredentials();
//        });
//});

//// Reconfigures SwaggerGen to add security definitions, specifically for JWT authentication.
//builder.Services.AddSwaggerGen(c =>
//{
//    // Adds a security definition to Swagger UI, allowing users to input a JWT token
//    // in the Swagger UI to authenticate their requests.
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization", // The name of the header where the token will be sent
//        Type = SecuritySchemeType.Http, // Specifies it's an HTTP scheme
//        Scheme = "Bearer", // The authentication scheme (e.g., "Bearer YOUR_TOKEN_HERE")
//        BearerFormat = "JWT", // Specifies the format of the token
//        In = ParameterLocation.Header, // Indicates the token is passed in the request header
//        Description = "JWT Authorization header using the Bearer scheme." // Description for the UI
//    });
//    // Adds a security requirement, linking the "Bearer" security definition to operations
//    // that require authentication. This tells Swagger UI to add an "Authorize" button
//    // for endpoints that are marked as requiring authorization.
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                   {
//                    {
//                        new OpenApiSecurityScheme
//                        {
//                            Reference = new OpenApiReference
//                            {
//                                Type = ReferenceType.SecurityScheme,
//                                Id = "Bearer" // Refers to the security definition defined above
//                            }
//                        },
//                        Array.Empty<string>() // Scope requirements (empty for general Bearer token)
//                    }
//    });
//});

//// This line builds the WebApplication instance based on the configurations
//// defined in the 'builder'. From this point, you start configuring the
//// HTTP request processing pipeline (middleware).
//var app = builder.Build();

//// Configure the HTTP request pipeline.
//// Middleware components are added to this pipeline in a specific order.
//// Requests will flow through this pipeline.

//// Checks if the application is running in the Development environment.
//// This is important for environment-specific configurations.
//if (app.Environment.IsDevelopment())
//{
//    // Adds the Swagger middleware to the pipeline. This serves the OpenAPI JSON specification.
//    app.UseSwagger();
//    // Adds the Swagger UI middleware to the pipeline. This provides the interactive
//    // web-based documentation for your API. These are typically only enabled in development.
//    app.UseSwaggerUI();
//}

//// Enforces the use of HTTPS by redirecting incoming HTTP requests to HTTPS.
//// This is a security best practice for web applications.
//app.UseHttpsRedirection();

//// Enables the CORS policy named "AllowReactApp" that was defined earlier.
//// This middleware must be placed before UseRouting and UseAuthorization.
//app.UseCors("AllowReactApp");

//// Adds the authentication middleware to the pipeline. This middleware is responsible
//// for identifying the user from the incoming request (e.g., by validating a JWT token)
//// and attaching the user's identity to the HttpContext.
//app.UseAuthentication();

//// Adds the authorization middleware to the pipeline. This middleware checks if the
//// authenticated user has the necessary permissions to access a specific resource
//// or perform a specific action. It must come after UseAuthentication.
//app.UseAuthorization();

//// Adds endpoint routing to the pipeline. This middleware maps incoming requests
//// to specific controller actions based on the defined routes.
//app.MapControllers();

//// Starts the ASP.NET Core web application, making it listen for incoming HTTP requests.
//// This is the blocking call that keeps the application running.
//app.Run();
