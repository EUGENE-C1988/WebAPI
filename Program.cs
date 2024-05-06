using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var Configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    // �ˬd HTTP Header �� Authorization �O�_�� JWT Bearer Token
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    // �]�w JWT Bearer Token ���ˬd�ﶵ
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            #region  �t�m���ҵo���

            ValidateIssuer = false, // �O�_�n�ҥ����ҵo���
            ValidIssuer = Configuration["JWTConfig:Issuer"],

            ValidateAudience = false, // �O�_�n�ҥ����ұ�����
            ValidAudience = builder.Configuration["JWTConfig:Audience"],   // ValidAudience = "" // �p�G���ݭn���ұ����̥i�H����

            ValidateLifetime = false, // �O�_�n�ҥ����Ҧ��Įɶ�

            ValidateIssuerSigningKey = false, // �O�_�n�ҥ����Ҫ��_�A�@�뤣�ݭn�h���ҡA�]���q�`Token���u�|��ñ��


            // �o�̰t�m�O�ΨӸ�Http Request��Token�[�K
            // �p�GSecret Key����إ�Token�ҨϥΪ�Secret Key���@�˪��ܷ|�ɭP���ҥ���
            //ValidIssuer = Configuration["JWTConfig:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTConfig:SignKey"]))

            #endregion
        };
    });

//builder.Services.AddAuthorization();


//�B�zCORS������I�s
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
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

app.UseCors();

//JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
