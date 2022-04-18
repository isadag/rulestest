using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RulesEngine.Models;
using System.Text.Json;

namespace rulestest
{
    public class RulesEngineService : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostLifetime;
        private readonly IHostEnvironment _hostingEnv;
        private readonly ILogger<RulesEngineService> _logger;

        public RulesEngineService(
            ILogger<RulesEngineService> logger,
            IHostApplicationLifetime hostLifetime,
            IHostEnvironment hostingEnv)
        {
            _hostLifetime = hostLifetime;
            _hostingEnv = hostingEnv;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting evaluation...");
            string json = string.Empty;
            using (StreamReader r = new StreamReader("rules.json"))
            {
                json = r.ReadToEnd();
            }

            // Get the rules to evaluate
            var workflows = JsonSerializer.Deserialize<Workflow[]>(json);
            var re = new RulesEngine.RulesEngine(workflows, _logger);

            // Simulate user input
            var userData = new
            {
                services = new List<string> { "212", "999" },
                geoGraphicalCode = 7231,
                numberOfItems = 4,
            };

            // "Expression": "new int[] { 123, 789, 222 }.All(value => services.Contains(value))"

            // Rule parameter, otherwise "services" is not recognized in json
            var ruleParams = new List<RuleParameter> {
                new RuleParameter("services", userData.services),
                new RuleParameter("geoCode", userData.geoGraphicalCode),
                new RuleParameter("numItems", userData.numberOfItems)
                }.ToArray();

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;
            foreach (var workflow in workflows)
            {
                // Evaluate the rules against user input
                var resultList = await re.ExecuteAllRulesAsync(workflow.WorkflowName, ruleParams);

                //Check success for rule
                foreach (var result in resultList)
                {
                    Console.WriteLine($"RESULT: Workflow {workflow.WorkflowName} => Rule - {result.Rule.RuleName}, IsSuccess - {result.IsSuccess}");
                    if (result.ActionResult?.Output != null)
                    {
                        Console.WriteLine($"RESULT: Output is {result.ActionResult.Output}"); //ActionResult.Output contains the evaluated value of the action
                    }
                }
            }
            // Stop the app
            _hostLifetime.StopApplication();

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting up");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping");
            return base.StopAsync(cancellationToken);
        }
    }
}