﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Text.Json;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;

namespace Stock_Price_Service.Functions.SignalR;

public class SignalRHub()
{
    [Function(nameof(Negotiate))]
    public static async Task<IActionResult> Negotiate(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "negotiate")] HttpRequestData req,
    [SignalRConnectionInfoInput(HubName = "stockpricehub")] SignalRConnectionInfo connectionInfo)
    {
        var serializedConnectionInfo = JsonSerializer.Serialize(connectionInfo);
        return new OkObjectResult(serializedConnectionInfo);
    }
}
