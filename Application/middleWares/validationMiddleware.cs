// using FluentValidation;
// using FluentValidation.Results;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.DependencyInjection;
// using System;
// using System.Linq;
// using System.Threading.Tasks;

// public class validationMiddleware
// {
//     private readonly RequestDelegate _next;

//     public validationMiddleware(RequestDelegate next)
//     {
//         _next = next ?? throw new ArgumentNullException(nameof(next));
//     }

//     public async Task InvokeAsync(HttpContext context)
//     {
//         var endpoint = context.GetEndpoint();
//         if (endpoint != null)
//         {
//             foreach (var metadata in endpoint.Metadata)
//             {
//                 if (metadata is IValidator validator)
//                 {
//                     // Model türünü doğrulayıcıdan alıyoruz
//                     var modelType = validator.GetType().BaseType.GenericTypeArguments[0];

//                     // Modelin bir örneğini oluşturuyoruz
//                     var model = Activator.CreateInstance(modelType);

//                     // Doğrulama işlemini gerçekleştiriyoruz
//                     var result = validator.Validate(new ValidationContext<object>(model));

//                     if (!result.IsValid)
//                     {
//                         context.Response.StatusCode = StatusCodes.Status400BadRequest;
//                         await context.Response.WriteAsJsonAsync(result.Errors.Select(e => e.ErrorMessage));
//                         return;
//                     }
//                 }
//             }
//         }

//         await _next(context);
//     }
// }
