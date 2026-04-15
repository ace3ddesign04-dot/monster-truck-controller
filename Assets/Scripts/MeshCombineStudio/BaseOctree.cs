using UnityEngine;

namespace MeshCombineStudio
{
	public class BaseOctree
	{
		public class Cell
		{
			public Cell mainParent;

			public Cell parent;

			public Cell[] cells;

			public bool[] cellsUsed;

			public Bounds bounds;

			public int cellIndex;

			public int cellCount;

			public int level;

			public int maxLevels;

			public Cell()
			{
			}

			public Cell(Vector3 position, Vector3 size, int maxLevels)
			{
				bounds = new Bounds(position, size);
				this.maxLevels = maxLevels;
			}

			public Cell(Cell parent, int cellIndex, Bounds bounds)
			{
				if (parent != null)
				{
					maxLevels = parent.maxLevels;
					mainParent = parent.mainParent;
					level = parent.level + 1;
				}
				this.parent = parent;
				this.cellIndex = cellIndex;
				this.bounds = bounds;
			}

			public void SetCell(Cell parent, int cellIndex, Bounds bounds)
			{
				if (parent != null)
				{
					maxLevels = parent.maxLevels;
					mainParent = parent.mainParent;
					level = parent.level + 1;
				}
				this.parent = parent;
				this.cellIndex = cellIndex;
				this.bounds = bounds;
			}

			protected int AddCell<T, U>(ref T[] cells, Vector3 position, out bool maxCellCreated) where T : Cell, new()where U : Cell, new()
			{
				Vector3 vector = position - this.bounds.min;
				float x = vector.x;
				Vector3 extents = this.bounds.extents;
				int num = (int)(x / extents.x);
				float y = vector.y;
				Vector3 extents2 = this.bounds.extents;
				int num2 = (int)(y / extents2.y);
				float z = vector.z;
				Vector3 extents3 = this.bounds.extents;
				int num3 = (int)(z / extents3.z);
				int num4 = num + num2 * 4 + num3 * 2;
				if (cells == null)
				{
					cells = new T[8];
				}
				if (cellsUsed == null)
				{
					cellsUsed = new bool[8];
				}
				if (!cellsUsed[num4])
				{
					Vector3 min = this.bounds.min;
					float x2 = min.x;
					Vector3 extents4 = this.bounds.extents;
					float x3 = x2 + extents4.x * ((float)num + 0.5f);
					Vector3 min2 = this.bounds.min;
					float y2 = min2.y;
					Vector3 extents5 = this.bounds.extents;
					float y3 = y2 + extents5.y * ((float)num2 + 0.5f);
					Vector3 min3 = this.bounds.min;
					float z2 = min3.z;
					Vector3 extents6 = this.bounds.extents;
					Bounds bounds = new Bounds(new Vector3(x3, y3, z2 + extents6.z * ((float)num3 + 0.5f)), this.bounds.extents);
					if (level == maxLevels - 1)
					{
						cells[num4] = (new U() as T);
						cells[num4].SetCell(this, num4, bounds);
						maxCellCreated = true;
					}
					else
					{
						maxCellCreated = false;
						cells[num4] = new T();
						cells[num4].SetCell(this, num4, bounds);
					}
					cellsUsed[num4] = true;
					cellCount++;
				}
				else
				{
					maxCellCreated = false;
				}
				return num4;
			}

			public void RemoveCell(int index)
			{
				cells[index] = null;
				cellsUsed[index] = false;
				cellCount--;
				if (cellCount == 0 && parent != null)
				{
					parent.RemoveCell(cellIndex);
				}
			}

			public bool InsideBounds(Vector3 position)
			{
				position -= bounds.min;
				float x = position.x;
				Vector3 size = bounds.size;
				if (!(x >= size.x))
				{
					float y = position.y;
					Vector3 size2 = bounds.size;
					if (!(y >= size2.y))
					{
						float z = position.z;
						Vector3 size3 = bounds.size;
						if (!(z >= size3.z) && !(position.x <= 0f) && !(position.y <= 0f) && !(position.z <= 0f))
						{
							return true;
						}
					}
				}
				return false;
			}

			public void Reset(ref Cell[] cells)
			{
				cells = null;
				cellsUsed = null;
			}
		}
	}
}
