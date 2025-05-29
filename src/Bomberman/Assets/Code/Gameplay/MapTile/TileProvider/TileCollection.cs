﻿using System.Collections.Generic;
using Gameplay.Map;
using UnityEngine;
using UnityEngine.Tilemaps;
using Menu = Constant.CreateAssetMenu;

namespace Gameplay.MapTile.TileProvider
{
	[CreateAssetMenu(menuName = Menu.Path.c_StaticData + nameof(TileCollection))]
	public sealed class TileCollection : ScriptableObject, ITileProvider
	{
		[SerializeField] TilesByTypeDictionary _tiles;

		public IReadOnlyDictionary<TileType, TileBase> Tiles => _tiles;

		public TileBase this[TileType type] =>
			Has(type) ? _tiles[type] : null;

		public bool Has(TileType type) =>
			_tiles.ContainsKey(type);
	}
}