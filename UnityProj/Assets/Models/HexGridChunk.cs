using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class HexGridChunk : MonoBehaviour
{
	HexCell[] cells;
	public int x;
	public int z;
	HexMesh hexMesh;
	Canvas gridCanvas;
	MaskMesh maskMesh;
	Material material;
	Texture2D texture;

	void Awake()
	{
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();
		maskMesh = hexMesh.GetComponentInChildren<MaskMesh>();
		var mapSizeX = HexGrid.allChunkCountX * HexMetrics.chunkSizeX;
		var mapSizeY = HexGrid.allChunkCountZ * HexMetrics.chunkSizeZ;
		texture = new Texture2D(mapSizeX, mapSizeY, TextureFormat.ARGB32, false);
		Color[] colors = new Color[mapSizeX * mapSizeY];
        for (int i = 0; i < colors.Length; i++)
        {
			colors[i] = new Color(1, 1, 1, 1);
		}
		texture.SetPixels(colors);
		texture.Apply();

		cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
		ShowUI(true);
	}

	public void UpdateTexture(int x, int y)
    {		
		material = maskMesh.GetComponent<Renderer>().material;
		texture.SetPixel(x,y, new Color(0, 0, 0, 0));
		texture.Apply();
		material.SetTexture("_Mask", texture);
	}

	//void Start()
	//{
	//	hexMesh.Triangulate(cells);
	//}

	public void Refresh()
	{
		enabled = true;
	}
	
	void LateUpdate()
	{
		hexMesh.Triangulate(cells);
		hexMesh.Copy(maskMesh.hexMesh);
		enabled = false;
	}

	/// <summary>
	/// Добавление ячеек в массив
	/// </summary>
	/// <param name="index"></param>
	/// <param name="cell"></param>
	public void AddCell(int index, HexCell cell)
	{
		cells[index] = cell;
		cell.chunk = this;
		cell.transform.SetParent(transform, false);
		cell.uiRect.SetParent(gridCanvas.transform, false);
	}

	/// <summary>
	/// Метод показа координат(меток) ячеек
	/// </summary>
	/// <param name="visible"></param>
	public void ShowUI(bool visible)
	{
		gridCanvas.gameObject.SetActive(visible);
	}

	public void SetCoordinatesChunck(int x, int z)
    {
		this.x = x;
		this.z = z;
    }

	public HexCell GetRandomCell()
    {
		return cells[UnityEngine.Random.Range(0, cells.Length)];
    }
}
