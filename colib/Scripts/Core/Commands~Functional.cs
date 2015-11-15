using System;
using System.Collections.Generic;

namespace CoLib
{

public static partial class Commands
{
	/// <summary>
	/// Takes an Enumerable of a given type, and a function that converts
	/// T into a CommandDelegate, the executes them in parallel.
	/// </summary>
	/// <param name="collection">A collection of objects.</param>
	/// <param name="factory">The conversion method.</param>
	public static CommandDelegate ForEachParallel<T>(this IEnumerable<T> collection, Func<T, CommandDelegate> factory)
	{
		CheckArgumentNonNull(collection, "collection");
		CheckArgumentNonNull(factory, "factory");
		var commands = new List<CommandDelegate>();
		foreach (var item in collection) {
			CommandDelegate output = factory(item);
			commands.Add(output);
		}
		return Commands.Parallel(commands.ToArray());
	}

	/// <summary>
	/// Takes an Enumerable of a given type, and a function that converts
	/// T into a CommandDelegate, the executes them sequentially.
	/// </summary>
	/// <param name="collection">A collection of objects.</param>
	/// <param name="factory">The conversion method.</param>
	public static CommandDelegate ForEachSequence<T>(this IEnumerable<T> collection, Func<T, CommandDelegate> factory)
	{
		CheckArgumentNonNull(collection, "collection");
		CheckArgumentNonNull(factory, "factory");
		var commands = new List<CommandDelegate>();
		foreach (var item in collection) {
			CommandDelegate output = factory(item);
			commands.Add(output);
		}
		return Commands.Sequence(commands.ToArray());
	}
}

}
