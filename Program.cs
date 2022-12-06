using System.Collections;
using System.Text.RegularExpressions;

Console.WriteLine("Analyzing supply stacks...");

var inputGroups = File.ReadAllText("stacks_input.txt").Split("\r\n\r\n");
var moveInstructions = inputGroups[1].Split(Environment.NewLine);
var crateRows = inputGroups[0].Split(Environment.NewLine);
var crateNumberRow = crateRows.Last();
var crateLabelPositions = GetLabelPositions(crateNumberRow);
var crateStacks = PopulateCrateStacks(crateLabelPositions, crateRows);

// Puzzle 1
foreach (var moveInstruction in moveInstructions)
{
    var (transferCount, fromStack, toStack) = ParseInstructions(moveInstruction);
    for (var i = 0; i < transferCount; i++)
    {
        var crate = crateStacks[fromStack].Pop();
        crateStacks[toStack].Push(crate);
    }
}

var topCrates = string.Join("", crateStacks.Select(crateStack => (char) (crateStack.Value.Peek() ?? "")).ToList());
Console.WriteLine($"Puzzle 1 has crates: {topCrates} at the top");

// Puzzle 2
crateStacks.Clear();
crateStacks = PopulateCrateStacks(crateLabelPositions, crateRows);

// execute move instructions
foreach (var moveInstruction in moveInstructions)
{
    var (transferCount, fromStack, toStack) = ParseInstructions(moveInstruction);
    // Reverse move crates transferCount number of times
    crateStacks[fromStack].ToArray()
        .Take(transferCount)
        .Reverse().ToList()
        .ForEach(supply =>
        {
            crateStacks[fromStack].Pop();
            crateStacks[toStack].Push(supply);
        });
}

topCrates = string.Join("", crateStacks.Select(crateStack => (char) (crateStack.Value.Peek() ?? "")).ToList());
Console.WriteLine($"Puzzle 2 has crates: {topCrates} at the top");

Console.WriteLine("Finished analyzing supply stacks.");

// functions

(int transferCount, int fromStack, int toStack) ParseInstructions(string instruction)
{
    var instructionParts = Regex.Matches(instruction, @"\d+")
        .Select(m => m.Value)
        .ToArray();
    var transferCount = int.Parse(instructionParts[0]);
    var fromStack = int.Parse(instructionParts[1]);
    var toStack = int.Parse(instructionParts[2]);
    return (transferCount, fromStack, toStack);
}

Dictionary<int, Stack> PopulateCrateStacks(IReadOnlyList<int> labelPositions, IReadOnlyList<string> supplyCratesInput)
{
    var crateStacks = new Dictionary<int, Stack>();
    for (var i = 0; i < labelPositions.Count; i++) crateStacks.Add(i + 1, new Stack());

    for (var i = 0; i < labelPositions.Count; i++)
    {
        var alphabetPosition = labelPositions[i];
        // for each crate row, except the last one, add crate labels to stack
        for (var j = supplyCratesInput.Count - 1; j >= 0; j--)
        {
            var crateRow = supplyCratesInput[j];
            var crateLabel = crateRow[alphabetPosition];
            if (char.IsLetter(crateLabel)) crateStacks[i + 1].Push(crateLabel);
        }
    }

    return crateStacks;
}

List<int> GetLabelPositions(string row)
{
    var positions = new List<int>();
    row
        .Select((c, idx) => new {character = c, index = idx})
        .ToList()
        .ForEach(c =>
        {
            if (char.IsDigit(c.character)) positions.Add(c.index);
        });
    return positions;
}