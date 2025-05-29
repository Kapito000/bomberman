﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Map
{
	public interface IGrid<T> : IEnumerable<Vector2Int>
	{
		Vector2Int Size { get; }
		bool Has(int x, int y);
		bool Has(T value, int x, int y);
		bool TrySet(T value, int x, int y);
		bool TryGet(int x, int y, out T value);
		IEnumerable<Vector2Int> AllCoordinates(T value);
		IEnumerable<(Vector2Int cell, T value)> WithValues();
		public IEnumerable<(Vector2Int cell, T value)> WithValues(Func<T, bool> where);
		IEnumerable<Vector2Int> AllCoordinates(Func<T, bool> where);
	}
}