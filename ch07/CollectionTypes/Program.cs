using CollectionTypes;

if (args.Length == 0)
{
    Console.WriteLine("Please provide a valid argument: 'array', 'list', 'linkedlist', 'queue', 'stack', 'priorityqueue', 'dictionary', or 'sorteddictionary'.");
    return;
}

Dictionary<string, Action> actions = new(StringComparer.OrdinalIgnoreCase)
{
    { "array", ArraySample.Run },
    { "list", ListSample.Run },
    { "linkedlist", LinkedListSample.Run },
    { "queue", QueueSample.Run },
    { "stack", StackSample.Run },
    { "priorityqueue", PriorityQueueSample.Run },
    { "dictionary", DictionarySample.Run },
    { "sorteddictionary", SortedDictionarySample.Run }
};

if (actions.TryGetValue(args[0], out var action))
{
    action();
}
else
{
    Console.WriteLine("Invalid argument. Please provide one of the following: 'array', 'list', 'linkedlist', 'queue', 'stack', 'priorityqueue', 'dictionary', or 'sorteddictionary'.");
}
