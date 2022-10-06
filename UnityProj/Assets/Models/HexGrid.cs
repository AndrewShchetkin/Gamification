using Assets.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
	public int cellCountX, cellCountZ;
	public static int NoGameChunckCount = 2;

	public int CellsCount
    {
        get
        {
			return cells.Length;
        }		
    }
	/// <summary>
	/// Количество игровых сегментов на карте
	/// </summary>
	public static int ChunkCountX = 5, ChunkCountZ = 5;
	/// <summary>
	/// Количество всех сегментов на карте
	/// </summary>
	public static int allChunkCountX => ChunkCountX + NoGameChunckCount;
	public static int allChunkCountZ => ChunkCountZ + NoGameChunckCount;
	/// <summary>
	/// Цвет по умолчани(убрано для сохранения карты)
	/// </summary>
	//public Color defaultColor = Color.white;
	/// <summary>
	/// Цвет затронутой ячейки
	/// </summary>
	public Color touchedColor = Color.magenta;

	/// <summary>
	/// Префаб клетки
	/// </summary>
	public HexCell cellPrefab;
	/// <summary>
	/// Префаб лэйбла ячейки (текст координат)
	/// </summary>
	public Text cellLabelPrefab;
	// HexMesh hexMesh;
	/// <summary>
	/// Префаб сегмента
	/// </summary>
	public HexGridChunk chunkPrefab;

	/// <summary>
	/// Канвас грида
	/// </summary>
	// Canvas gridCanvas;

	/// <summary>
	/// Массив клеток
	/// </summary>
	HexCell[] cells;

	/// <summary>
	/// Массив неактивных клеток
	/// </summary>
	HexCell[] noGameCells;
	/// <summary>
	/// Текстура шума
	/// </summary>
	public Texture2D noiseSource;

	/// <summary>
	/// Массив сегментов
	/// </summary>
	HexGridChunk[] chunks;

	/// <summary>
	/// Массив неигровых сегментов
	/// </summary>
	HexGridChunk[] nonGameChunks;

	/// <summary>
	/// Для сохранения
	/// </summary>
	public Color[] colors;

    private void Start()
    {
		GameController.SetReadyObject(this.GetType());
		//hexMesh.Triangulate(cells);
	}

    /// <summary>
    /// Инициализация сетки
    /// </summary>
    void Awake () 
	{
		HexMetrics.noiseSource = noiseSource;
		HexMetrics.colors = colors;

		cellCountX = allChunkCountX * HexMetrics.chunkSizeX;
		cellCountZ = allChunkCountZ * HexMetrics.chunkSizeZ;

		CreateChunks();
		CreateCells();

		//CreateNonGameChuncks();
		//CreateNonGameCells();
	}

	void CreateCells()
	{
		cells = new HexCell[cellCountZ * cellCountX];

		for (int z = 0, i = 0; z < cellCountZ; z++)
		{
			for (int x = 0; x < cellCountX; x++)
			{
				CreateCell(x, z, i++);
			}
		}
	}
	void CreateNonGameCells()
	{
		noGameCells = new HexCell[((allChunkCountX + allChunkCountZ) * 2 + 4) * HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];

		for (int z = 0, i = 0; z < cellCountZ; z++)
		{
			for (int x = 0; x < cellCountX; x++)
			{
				CreateCell(x, z, i++);
			}
		}
	}

	void CreateChunks () 
	{
		chunks = new HexGridChunk[allChunkCountX * allChunkCountZ];

		for (int z = 0, i = 0; z < allChunkCountZ; z++) 
		{
			for (int x = 0; x < allChunkCountX; x++) 
			{
				HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
				chunk.SetCoordinatesChunck(x, z);
				chunk.transform.SetParent(transform);
			}
		}
	}

	void CreateNonGameChuncks()
    {
		nonGameChunks = new HexGridChunk[(allChunkCountX + allChunkCountZ) * 2 + 4];
		int count = 0;
		for (int x = 0; x < allChunkCountX + 1; x++)
		{
			HexGridChunk chunk = nonGameChunks[count++] = Instantiate(chunkPrefab);
			chunk.SetCoordinatesChunck(x, 0);
			chunk.transform.SetParent(transform);
		}

		for (int z = 0; z < allChunkCountZ + 1; z++)
		{
			HexGridChunk chunk = nonGameChunks[count++] = Instantiate(chunkPrefab);
			chunk.SetCoordinatesChunck(allChunkCountX + 1, z);
			chunk.transform.SetParent(transform);
		}

		for (int x = allChunkCountX + 1; x >= 0; x--)
		{
			HexGridChunk chunk = nonGameChunks[count++] = Instantiate(chunkPrefab);
			chunk.SetCoordinatesChunck(x, allChunkCountZ + 1);
			chunk.transform.SetParent(transform);
		}

		for (int z = allChunkCountZ; z >= 1; z--)
		{
			HexGridChunk chunk = nonGameChunks[count++] = Instantiate(chunkPrefab);
			chunk.SetCoordinatesChunck(0, z);
			chunk.transform.SetParent(transform);
		}
	}

	void OnEnable()
	{
		HexMetrics.noiseSource = noiseSource;
		HexMetrics.colors = colors;
	}
	//public void Refresh()
	//{
	//	 hexMesh.Triangulate(cells);
	//}

	/// <summary>
	/// Создание клетки
	/// </summary>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <param name="i"></param>
	void CreateCell(int x, int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		//cell.Color = defaultColor;

		if (x > 0)
		{
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0)
		{
			if ((z & 1) == 0)
			{
				cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
				if (x > 0)
				{
					cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
				}
			}
			else
			{
				cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
				if (x < cellCountX - 1)
				{
					cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
				}
			}
		}

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
		cell.uiRect = label.rectTransform;
		
		cell.Elevation = 0;

		AddCellToChunk(x, z, cell);
	}

	/// <summary>
	/// Добавление ячеек в ссегменты
	/// </summary>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <param name="cell"></param>
	void AddCellToChunk(int x, int z, HexCell cell)
	{
		int chunkX = x / HexMetrics.chunkSizeX;
		int chunkZ = z / HexMetrics.chunkSizeZ;
		HexGridChunk chunk = chunks[chunkX + chunkZ * allChunkCountX];

		int localX = x - chunkX * HexMetrics.chunkSizeX;
		int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
		chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
	}

	void AddNonGameCellsToChunk()
    {

    }

	public HexCell GetCell(Vector3 position)
	{
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
		return cells[index];
	}

	/// <summary>
	/// Получение ячейки по координатам (для кисти)
	/// </summary>
	/// <param name="coordinates"></param>
	/// <returns></returns>
	public HexCell GetCell(HexCoordinates coordinates)
	{
		int z = coordinates.Z;
		if (z < 0 || z >= cellCountZ)
		{
			return null;
		}
		int x = coordinates.X + z / 2;
		if (x < 0 || x >= cellCountX)
		{
			return null;
		}
		return cells[x + z * cellCountX];
	}

	/// <summary>
	/// Видимость меток ячеек
	/// </summary>
	/// <param name="visible"></param>
	public void ShowUI(bool visible)
	{
		for (int i = 0; i < chunks.Length; i++)
		{
			chunks[i].ShowUI(visible);
		}
	}
	/// <summary>
	/// Сохранение грида
	/// </summary>
	/// <param name="map"></param>
	public void Save(SaveMapData map)
	{
		for (int i = 0; i < cells.Length; i++)
		{
			cells[i].Save(map);
		}
	}

	public void Load(SaveMapData map)
	{
        foreach (var cell in map.cells)
        {
			var targetCell = cells.SingleOrDefault(c => c.coordinates.X == cell.x && c.coordinates.Y == cell.y && c.coordinates.Z == cell.z);
			targetCell.ColorIndex = cell.color;
			targetCell.Elevation = cell.elevation;			
            if (!string.IsNullOrEmpty(cell.ownerId))
            {
				targetCell.ownerColorHighligh = GameController.GetTeamColor(cell.ownerId);
				targetCell.OwnerId = cell.ownerId;
			}
        }
		for (int i = 0; i < chunks.Length; i++)
		{
			chunks[i].Refresh();
		}
	}

    public List<HexGridChunk> GetExtremeChunks()
    {
		var extremeChuncks = new List<HexGridChunk>();
        extremeChuncks.Add(chunks.Where(c => c.x == 0 && c.z == 0).Single());
		extremeChuncks.Add(chunks.Where(c => c.x == 0 && c.z == allChunkCountZ-1).Single());
		extremeChuncks.Add(chunks.Where(c => c.x == allChunkCountX-1 && c.z == allChunkCountZ-1).Single());
		extremeChuncks.Add(chunks.Where(c => c.x == allChunkCountX-1 && c.z == 0).Single());
        extremeChuncks.Add(chunks[allChunkCountX * allChunkCountZ/2]);

		return extremeChuncks;
	}
}
