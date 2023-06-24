using Bot.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace Bot.Controllers;

[Route("api/v1/threads")]
public class ThreadCountController : BaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        dynamic threadCount = new ExpandoObject();
        ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
        threadCount.MinWorkerThreads = workerThreads;
        threadCount.MinCompletionPortThreads = completionPortThreads;

        ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
        threadCount.MaxWorkerThreads = workerThreads;
        threadCount.MaxCompletionPortThreads = completionPortThreads;

        return Ok(threadCount);
    }
}
