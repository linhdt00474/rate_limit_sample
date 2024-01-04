using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var rateLimiterOptions = new RateLimiterOptions();

//Read more: https://www.infoworld.com/article/3696320/how-to-use-the-rate-limiting-algorithms-in-aspnet-core.html

//A concurrency limiter controls the maximum number of simultaneous requests to a resource.
//For example, to set PermitLimit is 10 makes only 10 requests can access the api at a time.
rateLimiterOptions.AddConcurrencyLimiter("concurrency", options =>
{
    options.PermitLimit = 1; // maximum number of request at a time
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // order for queue processing
    options.QueueLimit = 0;// maximum number of request can be in queue at a time
});

//Handle the connection based on token
//In the token bucket algorithm, each token in the bucket represents a request.
//A token is removed from the bucket whenever a request is served.
//If the bucket becomes empty, the next request is rejected or throttled.
//As time passes, the bucket refills at a fixed rate.

//rateLimiterOptions.AddTokenBucketLimiter("tokenBased", options =>
//{
//    options.TokenLimit = 10; //maximum number of tokens that can be in token bucket
//    options.QueueProcessingOrder = QueueProcessingOrder.NewestFirst; // order for queue processing
//    options.QueueLimit = 0; //maximum number of unprocessed tokens waiting in queue
//    options.ReplenishmentPeriod = TimeSpan.FromSeconds(10); //specifies how often tokens can be replenished. Replenishing is triggered either by set autoReplenishment is true, or by calling TryReplenish()
//    options.TokensPerPeriod = 10; // number of tokens can be added to the token bucket in replenishing process, token count will not exceed tokenLimit
//    options.AutoReplenishment = true; //specify token can be replenished automatically or not
//});

//Divide time to fixed windows, for example: by setting PermitLimit to 3 and Window to 10 seconds,
//you allow a maximum of three requests every 10 seconds.

//rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
//{
//    options.PermitLimit = 3;
//    options.Window = TimeSpan.FromSeconds(10);
//    options.AutoReplenishment = true;
//    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//    options.QueueLimit = 2;
//});


//Divide time to fixed windows like the FixedWindow, but also divide the time windows into segment,
//at each interval of a segment, the windows slides/move by one segment,
//read more to understand: https://devblogs.microsoft.com/dotnet/announcing-rate-limiting-for-dotnet/
//rateLimiterOptions.AddSlidingWindowLimiter("sliding", options =>
//{
//    options.PermitLimit = 30;
//    options.Window = TimeSpan.FromSeconds(60);
//    options.SegmentsPerWindow = 2;
//    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//    options.QueueLimit = 2;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter(rateLimiterOptions);

app.MapControllers();

app.Run();
