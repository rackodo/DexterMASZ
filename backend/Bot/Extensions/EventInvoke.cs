using Bot.Abstractions;

namespace Bot.Extensions;

public static class EventInvoke
{
	public static void NotNull<T>(this T obj, string name, string msg = null) where T : class
	{
		if (obj is null)
			throw CreateNotNullException(name, msg);
	}

	private static ArgumentNullException CreateNotNullException(string name, string msg)
	{
		return msg is null ? new ArgumentNullException(name) : new ArgumentNullException(name, msg);
	}

	public static void Invoke(this AsyncEvent<Func<Task>> eventHandler)
	{
		foreach (var subscription in eventHandler.Subscriptions)
			_ = Task.Run(async () =>
			{
				try
				{
					await subscription.Invoke().ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					Console.WriteLine(
						$"Something went wrong while executing subscription {subscription.Target}/{subscription.Method.Name}.");
					Console.WriteLine(ex);
				}
			});
	}

	public static void Invoke<T>(this AsyncEvent<Func<T, Task>> eventHandler, T arg)
	{
		foreach (var subscription in eventHandler.Subscriptions)
			_ = Task.Run(async () =>
			{
				try
				{
					await subscription.Invoke(arg).ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					Console.WriteLine(
						$"Something went wrong while executing subscription {subscription.Target}/{subscription.Method.Name}.");
					Console.WriteLine(ex);
				}
			});
	}

	public static void Invoke<T1, T2>(this AsyncEvent<Func<T1, T2, Task>> eventHandler, T1 arg1, T2 arg2)
	{
		foreach (var subscription in eventHandler.Subscriptions)
			_ = Task.Run(async () =>
			{
				try
				{
					await subscription.Invoke(arg1, arg2).ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					Console.WriteLine(
						$"Something went wrong while executing subscription {subscription.Target}/{subscription.Method.Name}.");
					Console.WriteLine(ex);
				}
			});
	}

	public static void Invoke<T1, T2, T3>(this AsyncEvent<Func<T1, T2, T3, Task>> eventHandler, T1 arg1, T2 arg2,
		T3 arg3)
	{
		foreach (var subscription in eventHandler.Subscriptions)
			_ = Task.Run(async () =>
			{
				try
				{
					await subscription.Invoke(arg1, arg2, arg3).ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					Console.WriteLine(
						$"Something went wrong while executing subscription {subscription.Target}/{subscription.Method.Name}.");
					Console.WriteLine(ex);
				}
			});
	}

	public static void Invoke<T1, T2, T3, T4>(this AsyncEvent<Func<T1, T2, T3, T4, Task>> eventHandler, T1 arg1, T2 arg2,
		T3 arg3, T4 arg4)
	{
		foreach (var subscription in eventHandler.Subscriptions)
			_ = Task.Run(async () =>
			{
				try
				{
					await subscription.Invoke(arg1, arg2, arg3, arg4).ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					Console.WriteLine(
						$"Something went wrong while executing subscription {subscription.Target}/{subscription.Method.Name}.");
					Console.WriteLine(ex);
				}
			});
	}

	public static void Invoke<T1, T2, T3, T4, T5>(this AsyncEvent<Func<T1, T2, T3, T4, T5, Task>> eventHandler, T1 arg1,
		T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		foreach (var subscription in eventHandler.Subscriptions)
			_ = Task.Run(async () =>
			{
				try
				{
					await subscription.Invoke(arg1, arg2, arg3, arg4, arg5).ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					Console.WriteLine(
						$"Something went wrong while executing subscription {subscription.Target}/{subscription.Method.Name}.");
					Console.WriteLine(ex);
				}
			});
	}
}