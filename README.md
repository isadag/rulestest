# Rules engine sample / test

Sample app to test out Microsoft's Rules Engine: https://github.com/microsoft/RulesEngine


### User data that is being fed into rules engine:
```
// Simulate user input
var userData = new
{
    services = new List<string> { "212", "999" },
    geoGraphicalCode = 7231,
    numberOfItems = 4,
    someString = "abc",
};
```

### Using custom evaluations from code in expressions in rules.json
Create and register a static class with static methods, see `Utils.cs`, register it and pass it in to the rules engine like:

```
// Register our utils class so we can use our custom evaluator
var reSettingsWithCustomTypes = new ReSettings
{
    CustomTypes = new Type[] {
        typeof(Utils)
    }
};
var re = new RulesEngine.RulesEngine(workflows, _logger, reSettingsWithCustomTypes);
```

#### Use the custom methods in the expressions like so:
```
{
    "RuleName": "RuleWithCustomEvaluators",
    "Enabled": true,
    "Expression": "Utils.MustContainAll(\"212\", services) == true AND Utils.StartsWithA(someString)"
}
```


### Rules json:
```
[
    {
        "WorkflowName": "DiscountWithCustomInputNames",
        "Rules": [
            {
                "RuleName": "GiveDiscount10",
                "Expression": "\"222, 123, 789\".Split(',').ToList().Select(x => x.Trim()).ToList().All(value => services.Contains(value)) AND geoCode != 231",
                "Actions": {
                    "OnSuccess": {
                        "Name": "OutputExpression",
                        "Context": {
                            "Expression": "399"
                        }
                    }
                }
            },
            {
                "RuleName": "GiveDiscount20",
                "Enabled": true,
                "Expression": "new int[] { 123, 789, 222 }.Select(x => x.ToString()).All(value => services.Contains(value)) AND geoCode == 231 AND numItems < 10",
                "Actions": {
                    "OnSuccess": {
                        "Name": "OutputExpression",
                        "Context": {
                            "Expression": "299"
                        }
                    }
                }
            },
            {
                "RuleName": "GiveDiscount30",
                "Enabled": false,
                "Expression": "new int[] { 123, 789, 222 }.Select(x => x.ToString()).All(value => services.Contains(value)) AND geoCode == 231 AND numItems >= 10",
                "Actions": {
                    "OnSuccess": {
                        "Name": "OutputExpression",
                        "Context": {
                            "Expression": "199"
                        }
                    }
                }
            },
            {
                "RuleName": "RuleWithCustomEvaluators",
                "Enabled": true,
                "Expression": "Utils.MustContainAll(\"212\", services) == true AND Utils.StartsWithA(someString)"
            }
        ]
    },
    {
        "WorkflowName": "OtherDiscountRule",
        "Rules": [
            {
                "Enabled": true,
                "RuleName": "NoDiscount",
                "Expression": "new int[] { 999, 212 }.Select(x => x.ToString()).All(value => services.Contains(value)) AND geoCode != 231",
                "Actions": {
                    "OnSuccess": {
                        "Name": "OutputExpression",
                        "Context": {
                            "Expression": "459837953"
                        }
                    },
                    "OnFailure": {
                        "Name": "OutputExpression",
                        "Context": {
                            "Expression": "12312"
                        }
                    }
                }
            }
        ]
    }
]
```


### Execution result:
![](/readme_run_result.png)
