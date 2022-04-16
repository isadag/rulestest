# Rules engine sample / test

Sample app to test out Microsoft's Rules Engine: https://github.com/microsoft/RulesEngine


### User data that is being fed into rules engine:
```
var userData = new
{
    services = new List<int> { 222, 123, 789 },
    geoGraphicalCode = 1231,
    numberOfItems = 10,
};
```

### Rules json:
```
[
    {
        "WorkflowName": "DiscountWithCustomInputNames",
        "Rules": [
            {
                "RuleName": "GiveDiscount10",
                "Expression": "new int[] { 123, 789, 222 }.All(value => services.Contains(value)) AND geoCode != 231",
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
                "Expression": "new int[] { 123, 789, 222 }.All(value => services.Contains(value)) AND geoCode == 231 AND numItems < 10",
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
                "Expression": "new int[] { 123, 789, 222 }.All(value => services.Contains(value)) AND geoCode == 231 AND numItems >= 10",
                "Actions": {
                    "OnSuccess": {
                        "Name": "OutputExpression",
                        "Context": {
                            "Expression": "199"
                        }
                    }
                }
            }
        ]
    },
    {
        "WorkflowName": "OtherDiscountRule",
        "Rules": [
            {
                "RuleName": "NoDiscount",
                "Expression": "new int[] { 999, 212 }.All(value => services.Contains(value)) AND geoCode != 231",
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
![](/readme_run_result.svg)
