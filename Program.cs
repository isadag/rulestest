using System.Linq;
using System.Text.Json;
using RulesEngine.Models;

string json = string.Empty;
using (StreamReader r = new StreamReader("rules.json"))
{
    json = r.ReadToEnd();
}

// Get the rules to evaluate
var workflows = JsonSerializer.Deserialize<Workflow[]>(json);
var re = new RulesEngine.RulesEngine(workflows, null);

// Simulate user input
var userData = new
{
    services = new List<int> { 222,123, 789 },
    geoGraphicalCode = 1231,
    numberOfItems = 10,
};

// "Expression": "new int[] { 123, 789, 222 }.All(value => services.Contains(value))"

// Rule parameter, otherwise "services" is not recognized in json
var ruleParams = new List<RuleParameter> {
    new RuleParameter("services", userData.services),
    new RuleParameter("geoCode", userData.geoGraphicalCode),
    new RuleParameter("numItems", userData.numberOfItems)
}.ToArray();

// Evaluate the rules against user input
var resultList  = await re.ExecuteAllRulesAsync("DiscountWithCustomInputNames", ruleParams);

//Check success for rule
foreach (var result in resultList)
{
    Console.WriteLine($"Rule - {result.Rule.RuleName}, IsSuccess - {result.IsSuccess}");
    if(result.ActionResult != null){
          Console.WriteLine(result.ActionResult.Output); //ActionResult.Output contains the evaluated value of the action
      }
}