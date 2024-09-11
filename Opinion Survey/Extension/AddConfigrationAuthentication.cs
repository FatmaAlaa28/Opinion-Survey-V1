using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Opinion_Survey.Extension
{
    public static class AddConfigrationAuthentication
    {
        public static void AddCustomJwtAuthen(this IServiceCollection services ,ConfigurationManager configuration)
        {
            services.AddAuthentication(option =>
            {

                //   ناخد منها الاسكيماJwtBearer 
                // هيا طريقة بتتعرف بيها ع التوكين ف المكان اللي
                // احنا بنحطهوله ف الهيدر ف الكولينج للايند بوينت
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // لو اليوزر داخل وهو مش اوثورايز يحوله علي اللوج ان
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })//configration of Algorithm of JWTBearer to read Token
                .AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                // parameter in appsetting.json
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    // لو عايز اتاكد ان issuer هو هو
                    // دا ف حالة ان البرنامج موجود ع سيرفر واحد لكن لو انا
                    // مايكرو سيرفس ف مينفعش افعل الخاصية دي علشان ممكن اكون اكتر من سيرفر
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:Issuer"],

                    // يفضل انها تبقي فولس علشان انا مش عارف مين اللي هيطلب الريكويست ومنين
                    ValidateAudience = false,

                    // لازم علشان اتاكد ان ال security key هو هو 
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]))
                };
                // Log Google
            }).AddGoogle(options =>
                 {
                     options.ClientId = configuration["Authentication:Google:ClientId"];
                     options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                     options.CallbackPath = "/signin-google"; // Adjust this as needed
                 });
            
        }
    }
}


