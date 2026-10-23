public struct PriceChange
{
	public int Before;

	public int After;

	public int Delta => After - Before;
}
