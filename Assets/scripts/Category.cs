using System;

public class Category : IComparable<Category>
{
	private int id;
	private int index;

	public Category (int id = 0, int index = 0)
	{
		this.id = id;
		this.index = index;
	}

	public int getIndex()
	{
		return this.index;
	}

	public int CompareTo(Category other)
	{
		if(other == null)
		{
			return 1;
		}

		return 0;
	}

}