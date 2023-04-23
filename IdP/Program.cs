using IdP;
using IdP.Common;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

const string ISSUER = "https://jnsoft.se";
const string TOKEN_SECRET = "secret";


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/ping", () => "pong");

app.MapPost("/token", ([FromBody]TokenGenerationRequest req) =>
{
    byte[] never_use_a_static_salt = new byte[] { 0x33, 0xa5, 0x17, 0xed, 0x3f, 0x4d, 0x8a, 0x64, 0x76, 0x65, 0x64, 0x39, 0xbf, 0x9d, 0x5d, 0x2c };
    byte[] key = SecurityHelper.GetKeyFromPassword(TOKEN_SECRET.ToSecureString(), never_use_a_static_salt);
    string signingkey = key.ToBase64();
    string jwt = JwtHelper.GenerateJwtToken(req, ISSUER, key);
    return jwt;

});

app.Run();

