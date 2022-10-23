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

	void Awake()
	{
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();
		maskMesh = hexMesh.GetComponentInChildren<MaskMesh>();
		material = maskMesh.GetComponent<Renderer>().material;

		cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
		ShowUI(true);
	}

	public void UpdateTexture()
    {
		material.SetTexture("_Mask", HexGrid.texture);
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
