using System.Collections.Generic;

public static class Trails
{
	public static List<Trail> trails;

	static Trails()
	{
		trails = new List<Trail>();
		trails.Add(new Trail(0, "Snakebit", "Map1NG"));
		trails.Add(new Trail(1, "Mudpit", "Map1NG"));
		trails.Add(new Trail(2, "Long Road", "Map1NG"));
		trails.Add(new Trail(3, "The Strip", "MapDesertNG"));
		trails.Add(new Trail(4, "Table Top", "MapDesertNG"));
		trails.Add(new Trail(5, "Baja", "MapDesertNG"));
	}

	public static Trail GetByID(int id)
	{
		return trails.Find((Trail t) => t.id == id);
	}
}
