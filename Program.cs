using System.Collections;
using System.Text.RegularExpressions;

Console.WriteLine("Analyzing supply stacks...");

var inputGroups = File.ReadAllText("stacks_input.txt").Split("\r\n\r\n");
var moveInstructions = inputGroups[1].Split(Environment.NewLine);
var cratesInput = inputGroups[0].Split(Environment.NewLine);
var crateNumbersInput = cratesInput.Last();

var labelPositions = new List<int>();
crateNumbersInput
    .Select((c, idx) => new {character = c, index = idx})
    .ToList()
    .ForEach(c =>
    {
        if (char.IsDigit(c.character)) labelPositions.Add(c.index);
    });


// create stacks 
var crateStacks = new Dictionary<int, Stack>();
for (var i = 0; i < labelPositions.Count; i++)
{
    crateStacks.Add(i + 1, new Stack());
}

for (var i = 0; i < labelPositions.Count; i++)
{
    var alphabetPosition = labelPositions[i];
    // for each crate row, except the last one, add crate labels to stack
    for (var j = cratesInput.Length - 1; j >= 0; j--)
    {
        var crateRow = cratesInput[j];
        var crateLabel = crateRow[alphabetPosition];
        if (char.IsLetter(crateLabel)) crateStacks[i + 1].Push(crateLabel);
    }
}

// execute move instructions
foreach (var moveInstruction in moveInstructions)
{
    var moveInstructionParts = Regex.Matches(moveInstruction, @"\d+")
        .Select(m => m.Value)
        .ToArray();
    var transferCount = int.Parse(moveInstructionParts[0]);
    var fromStack = int.Parse(moveInstructionParts[1]);
    var toStack = int.Parse(moveInstructionParts[2]);

    // move crates transferCount number of times
    for (var i = 0; i < transferCount; i++)
    {
        var crate = crateStacks[fromStack].Pop();
        crateStacks[toStack].Push(crate);
    }
}

var topOfEachStack = string.Join("", crateStacks.Select(crateStack => (char) (crateStack.Value.Peek() ?? "")).ToList());
Console.WriteLine(topOfEachStack);

Console.WriteLine($"Finished analyzing supply stacks.");