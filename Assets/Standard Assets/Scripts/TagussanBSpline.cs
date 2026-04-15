using UnityEngine;

public class TagussanBSpline
{
	private delegate float bf(float i);

	private delegate float Seq(int obj);

	private float[][] points;

	private int degree;

	private bool copy = true;

	private int dimension;

	private bf baseFunc;

	private int baseFuncRangeInt;

	public TagussanBSpline(Vector3[] points, int degree)
	{
		if (copy)
		{
			this.points = new float[points.Length][];
			for (int i = 0; i < points.Length; i++)
			{
				this.points[i] = new float[3]
				{
					points[i].x,
					points[i].y,
					points[i].z
				};
			}
		}
		this.degree = degree;
		dimension = 3;
		switch (degree)
		{
		case 2:
			baseFunc = basisDeg2;
			baseFuncRangeInt = 2;
			break;
		case 3:
			baseFunc = basisDeg3;
			baseFuncRangeInt = 2;
			break;
		case 4:
			baseFunc = basisDeg4;
			baseFuncRangeInt = 3;
			break;
		case 5:
			baseFunc = basisDeg5;
			baseFuncRangeInt = 3;
			break;
		}
	}

	private Seq seqAt(int dim)
	{
		float[][] points = this.points;
		int margin = degree + 1;
		return delegate(int n)
		{
			if (n < margin)
			{
				return points[0][dim];
			}
			return (points.Length + margin <= n) ? points[points.Length - 1][dim] : points[n - margin][dim];
		};
	}

	private float basisDeg2(float x)
	{
		if (-0.5 <= (double)x && (double)x < 0.5)
		{
			return 0.75f - x * x;
		}
		if (0.5 <= (double)x && (double)x <= 1.5)
		{
			return 1.125f + (-1.5f + x / 2f) * x;
		}
		if (-1.5 <= (double)x && (double)x < -0.5)
		{
			return 1.125f + (1.5f + x / 2f) * x;
		}
		return 0f;
	}

	private float basisDeg3(float x)
	{
		if (-1f <= x && x < 0f)
		{
			return 2f / 3f + (-1f - x / 2f) * x * x;
		}
		if (1f <= x && x <= 2f)
		{
			return 1.33333337f + x * (-2f + (1f - x / 6f) * x);
		}
		if (-2f <= x && x < -1f)
		{
			return 1.33333337f + x * (2f + (1f + x / 6f) * x);
		}
		if (0f <= x && x < 1f)
		{
			return 2f / 3f + (-1f + x / 2f) * x * x;
		}
		return 0f;
	}

	private float basisDeg4(float x)
	{
		if (-1.5 <= (double)x && (double)x < -0.5)
		{
			return 55f / 96f + x * (-5f / 24f + x * (-1.25f + (-5f / 6f - x / 6f) * x));
		}
		if (0.5 <= (double)x && (double)x < 1.5)
		{
			return 55f / 96f + x * (5f / 24f + x * (-1.25f + (5f / 6f - x / 6f) * x));
		}
		if (1.5 <= (double)x && (double)x <= 2.5)
		{
			return 1.62760413f + x * (-2.60416675f + x * (1.5625f + (-5f / 12f + x / 24f) * x));
		}
		if (-2.5 <= (double)x && (double)x <= -1.5)
		{
			return 1.62760413f + x * (2.60416675f + x * (1.5625f + (5f / 12f + x / 24f) * x));
		}
		if (-1.5 <= (double)x && (double)x < 1.5)
		{
			return 115f / 192f + x * x * (-0.625f + x * x / 4f);
		}
		return 0f;
	}

	private float basisDeg5(float x)
	{
		if (-2f <= x && x < -1f)
		{
			return 0.425f + x * (-0.625f + x * (-1.75f + x * (-1.25f + (-0.375f - x / 24f) * x)));
		}
		if (0f <= x && x < 1f)
		{
			return 0.55f + x * x * (-0.5f + (0.25f - x / 12f) * x * x);
		}
		if (2f <= x && x <= 3f)
		{
			return 2.025f + x * (-3.375f + x * (2.25f + x * (-0.75f + (0.125f - x / 120f) * x)));
		}
		if (-3f <= x && x < -2f)
		{
			return 2.025f + x * (3.375f + x * (2.25f + x * (0.75f + (0.125f + x / 120f) * x)));
		}
		if (1f <= x && x < 2f)
		{
			return 0.425f + x * (0.625f + x * (-1.75f + x * (1.25f + (-0.375f + x / 24f) * x)));
		}
		if (-1f <= x && x < 0f)
		{
			return 0.55f + x * x * (-0.5f + (0.25f + x / 12f) * x * x);
		}
		return 0f;
	}

	private float getInterpol(Seq seq, float t)
	{
		bf bf = baseFunc;
		int num = baseFuncRangeInt;
		int num2 = (int)Mathf.Floor(t);
		float num3 = 0f;
		for (int i = num2 - num; i <= num2 + num; i++)
		{
			num3 += seq(i) * bf(t - (float)i);
		}
		return num3;
	}

	public float[] calcAt(float t)
	{
		t *= (float)((degree + 1) * 2 + points.Length);
		if (dimension == 2)
		{
			return new float[2]
			{
				getInterpol(seqAt(0), t),
				getInterpol(seqAt(1), t)
			};
		}
		if (dimension == 3)
		{
			return new float[3]
			{
				getInterpol(seqAt(0), t),
				getInterpol(seqAt(1), t),
				getInterpol(seqAt(2), t)
			};
		}
		float[] array = new float[dimension];
		for (int i = 0; i < dimension; i++)
		{
			array[i] = getInterpol(seqAt(i), t);
		}
		return array;
	}
}
