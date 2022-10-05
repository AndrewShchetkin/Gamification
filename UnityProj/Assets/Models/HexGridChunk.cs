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
		texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);

		Color[] colors = new Color[64 * 64];
        for (int i = 0; i < 64 * 64; i++)
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
		texture.SetPixel(x, y, new Color(0, 0, 0, 0));
		texture.Apply();
		material.mainTexture = texture;
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
