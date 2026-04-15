using System;
using System.Collections;
using System.Collections.Generic;

public class WMG_List<T> : IEnumerable<T>, IEnumerable
{
	public List<T> list
	{
		get;
		private set;
	}

	public int Count => list.Count;

	public T this[int index]
	{
		get
		{
			return list[index];
		}
		set
		{
			list[index] = value;
			this.Changed(arg1: false, arg2: false, arg3: true, index);
		}
	}

	public event Action<bool, bool, bool, int> Changed = delegate
	{
	};

	public WMG_List()
	{
		list = new List<T>();
	}

	public void SetList(IEnumerable<T> collection)
	{
		List<T> list = new List<T>(this.list);
		this.list = new List<T>(collection);
		if (list.Count == this.list.Count)
		{
			bool flag = false;
			int num = -1;
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].Equals(this.list[i]))
				{
					if (flag)
					{
						this.Changed(arg1: false, arg2: false, arg3: false, -1);
						return;
					}
					num = i;
					flag = true;
				}
			}
			if (num != -1)
			{
				this.Changed(arg1: false, arg2: false, arg3: true, num);
			}
		}
		else
		{
			this.Changed(arg1: false, arg2: true, arg3: false, -1);
		}
	}

	public void SetListNoCb(IEnumerable<T> collection, ref List<T> _list)
	{
		list = new List<T>(collection);
		_list = new List<T>(collection);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return list.GetEnumerator();
	}

	public void Add(T item)
	{
		list.Add(item);
		this.Changed(arg1: false, arg2: true, arg3: false, -1);
	}

	public void AddNoCb(T item, ref List<T> _list)
	{
		list.Add(item);
		_list.Add(item);
	}

	public void Remove(T item)
	{
		list.Remove(item);
		this.Changed(arg1: false, arg2: true, arg3: false, -1);
	}

	public void RemoveAt(int index)
	{
		list.RemoveAt(index);
		this.Changed(arg1: false, arg2: true, arg3: false, -1);
	}

	public void RemoveAtNoCb(int index, ref List<T> _list)
	{
		list.RemoveAt(index);
		_list.RemoveAt(index);
	}

	public void AddRange(IEnumerable<T> collection)
	{
		list.AddRange(collection);
		this.Changed(arg1: false, arg2: true, arg3: false, -1);
	}

	public void RemoveRange(int index, int count)
	{
		list.RemoveRange(index, count);
		this.Changed(arg1: false, arg2: true, arg3: false, -1);
	}

	public void Clear()
	{
		list.Clear();
		this.Changed(arg1: false, arg2: true, arg3: false, -1);
	}

	public void Sort()
	{
		list.Sort();
		this.Changed(arg1: false, arg2: false, arg3: false, -1);
	}

	public void Sort(Comparison<T> comparison)
	{
		list.Sort(comparison);
		this.Changed(arg1: false, arg2: false, arg3: false, -1);
	}

	public void Insert(int index, T item)
	{
		list.Insert(index, item);
		this.Changed(arg1: false, arg2: true, arg3: false, -1);
	}

	public void InsertRange(int index, IEnumerable<T> collection)
	{
		list.InsertRange(index, collection);
		this.Changed(arg1: false, arg2: true, arg3: false, -1);
	}

	public void RemoveAll(Predicate<T> match)
	{
		list.RemoveAll(match);
		this.Changed(arg1: false, arg2: true, arg3: false, -1);
	}

	public void Reverse()
	{
		list.Reverse();
		this.Changed(arg1: false, arg2: false, arg3: false, -1);
	}

	public void Reverse(int index, int count)
	{
		list.Reverse(index, count);
		this.Changed(arg1: false, arg2: false, arg3: false, -1);
	}

	public void SetValNoCb(int index, T val, ref List<T> _list)
	{
		list[index] = val;
		_list[index] = val;
	}

	public void SizeChangedViaEditor()
	{
		this.Changed(arg1: true, arg2: true, arg3: false, -1);
	}

	public void ValueChangedViaEditor(int index)
	{
		this.Changed(arg1: true, arg2: false, arg3: true, index);
	}

	public void SetListViaEditor(IEnumerable<T> collection)
	{
		list = new List<T>(collection);
	}

	public void SetValViaEditor(int index, T val)
	{
		list[index] = val;
	}
}
