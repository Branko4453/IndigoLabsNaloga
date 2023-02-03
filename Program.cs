using IndigoLabsNaloga;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Web.Helpers;
using System.Web.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/region/lastweek", () =>
{
    List<LastWeekSet> lastWeekSets = DataMethods.GetLastWeekSets();

    lastWeekSets = lastWeekSets.OrderByDescending(x => x.AverageCases).ToList();

    
    try
    {
        return lastWeekSets;
    }
    catch (Exception ex)
    {
        return null;
    }

});

app.MapGet("/todoitems/{region}/{from}/{to}", async (string region, string from, string to) =>
{
    try
    {
        var results = DataMethods.GetCases(region, from, to);

        if (results is null) return Results.NotFound();

        return Results.Ok(results);
    }
    catch (Exception ex) {

        return Results.BadRequest(ex.ToString());
    
    }


    
});




app.Run();

