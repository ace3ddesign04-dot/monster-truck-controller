using System.Collections.Generic;
using UnityEngine;

namespace GeNa
{
	public class Quadtree<T>
	{
		private class QuadtreeNode
		{
			public Vector2 Position
			{
				get;
				private set;
			}

			public T Value
			{
				get;
				private set;
			}

			public QuadtreeNode(Vector2 position, T value)
			{
				Position = position;
				Value = value;
			}
		}

		private readonly int nodeCapacity = 32;

		private readonly List<QuadtreeNode> nodes;

		private Quadtree<T>[] children;

		private Rect boundaries;

		public int Count
		{
			get;
			private set;
		}

		public Quadtree(Rect boundaries, int nodeCapacity = 32)
		{
			this.boundaries = boundaries;
			this.nodeCapacity = nodeCapacity;
			nodes = new List<QuadtreeNode>(nodeCapacity);
		}

		public bool Insert(float x, float y, T value)
		{
			Vector2 position = new Vector2(x, y);
			QuadtreeNode node = new QuadtreeNode(position, value);
			return Insert(node);
		}

		public bool Insert(Vector2 position, T value)
		{
			QuadtreeNode node = new QuadtreeNode(position, value);
			return Insert(node);
		}

		private bool Insert(QuadtreeNode node)
		{
			if (!boundaries.Contains(node.Position))
			{
				return false;
			}
			if (children != null)
			{
				Vector2 position = node.Position;
				Quadtree<T> quadtree;
				if (position.y < children[2].boundaries.yMin)
				{
					Vector2 position2 = node.Position;
					quadtree = ((!(position2.x < children[1].boundaries.xMin)) ? children[1] : children[0]);
				}
				else
				{
					Vector2 position3 = node.Position;
					quadtree = ((!(position3.x < children[1].boundaries.xMin)) ? children[3] : children[2]);
				}
				if (quadtree.Insert(node))
				{
					Count++;
					return true;
				}
			}
			if (nodes.Count < nodeCapacity)
			{
				nodes.Add(node);
				Count++;
				return true;
			}
			Subdivide();
			return Insert(node);
		}

		public IEnumerable<T> Find(Rect range)
		{
			if (Count == 0 || !boundaries.Overlaps(range, allowInverse: false))
			{
				yield break;
			}
			if (children == null)
			{
				for (int index2 = 0; index2 < nodes.Count; index2++)
				{
					QuadtreeNode node = nodes[index2];
					if (range.Contains(node.Position))
					{
						yield return node.Value;
					}
				}
			}
			else
			{
				for (int index = 0; index < children.Length; index++)
				{
					Quadtree<T> child = children[index];
					foreach (T item in child.Find(range))
					{
						yield return item;
					}
				}
			}
		}

		public bool Remove(float x, float z, T value)
		{
			return Remove(new Vector2(x, z), value);
		}

		public bool Remove(Vector2 position, T value)
		{
			if (Count == 0)
			{
				return false;
			}
			if (!boundaries.Contains(position))
			{
				return false;
			}
			if (children != null)
			{
				bool result = false;
				Quadtree<T> quadtree = (position.y < children[2].boundaries.yMin) ? ((!(position.x < children[1].boundaries.xMin)) ? children[1] : children[0]) : ((!(position.x < children[1].boundaries.xMin)) ? children[3] : children[2]);
				if (quadtree.Remove(position, value))
				{
					result = true;
					Count--;
				}
				if (Count <= nodeCapacity)
				{
					Combine();
				}
				return result;
			}
			for (int i = 0; i < nodes.Count; i++)
			{
				QuadtreeNode quadtreeNode = nodes[i];
				if (quadtreeNode.Position.Equals(position))
				{
					nodes.RemoveAt(i);
					Count--;
					return true;
				}
			}
			return false;
		}

		private void Subdivide()
		{
			children = new Quadtree<T>[4];
			float num = boundaries.width * 0.5f;
			float num2 = boundaries.height * 0.5f;
			for (int i = 0; i < children.Length; i++)
			{
				Rect rect = new Rect(boundaries.xMin + num * (float)(i % 2), boundaries.yMin + num2 * (float)(i / 2), num, num2);
				children[i] = new Quadtree<T>(rect);
			}
			Count = 0;
			for (int j = 0; j < nodes.Count; j++)
			{
				QuadtreeNode node = nodes[j];
				Insert(node);
			}
			nodes.Clear();
		}

		private void Combine()
		{
			for (int i = 0; i < children.Length; i++)
			{
				Quadtree<T> quadtree = children[i];
				nodes.AddRange(quadtree.nodes);
			}
			children = null;
		}
	}
}
