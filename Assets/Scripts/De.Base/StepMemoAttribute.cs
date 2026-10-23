using System;

[AttributeUsage(AttributeTargets.Field)]
public sealed class StepMemoAttribute : Attribute
{
	public string Memo { get; }

	public StepMemoAttribute(string memo)
	{
		Memo = memo;
	}
}
