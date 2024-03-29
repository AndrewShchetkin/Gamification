using Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
	public GameController gameController;
	public Color[] colors;
	public static int colorIndex;

	private static HexCell SelectedCell;
	public HexGrid hexGrid;
	public PopupCapture popupCapture;

	public bool applyElevation;

	// включен ли цветовой редактор
	bool applyColor;	

	/// <summary>
	/// Активный цвет (выбранный на интерфейсе)
	/// </summary>
	private Color activeColor;

	/// <summary>
	/// Активный тип местности (для сохранения)
	/// </summary>
	//int activeTerrainTypeIndex;
	int activeElevation;
	enum OptionalToggle
	{
		Ignore, Yes, No
	}
	//Переключатель стен
	OptionalToggle walledMode;

	delegate void EditCellFunc(HexCell cell);
	// Размер кисти
	int brushSize;
	ILogger logger;
	bool isEdit = false;
	public HexMapEditor(ILogger logger)
	{
		this.logger = logger;
	}

    void Start()
    {
		//SelectColor(-1);
		Load();
	}
    void Update()
	{		
		HandleInput();
	}	
	
	/// <summary>
	/// Обработчик клика мыши по гриду(каждый апдейт кадра)
	/// </summary>
	void HandleInput()
	{        
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (!Physics.Raycast(inputRay, out hit))
		{
			return;
		}
		var cell = hexGrid.GetCell(hit.point);
		if (SelectedCell != cell)
		{
			SelectedCell?.DisableHighlight();
			SelectedCell = cell;
		}
		cell.EnableHighlight(Color.green);
        
		if (Input.GetMouseButtonDown((int)PointerEventData.InputButton.Left) && !EventSystem.current.IsPointerOverGameObject())
		{
			if (isEdit)
			{				
				if (brushSize == 0)
				{
					EditCell(cell);
				}
                else
                {
					EditCells(cell);
                }
			}
            else
            {
				var neighbors = cell.GetNeighbors();

				if (neighbors.Any(n => n.OwnerId == gameController.GetPlayerTeam().id))
                {
					ConfirmCapture(cell);
				}			
			}			
		}
	}

	void ConfirmCapture(HexCell cell)
    {
		popupCapture.CurrentHexCell = cell;
    }
	/// <summary>
	///  Метод редактирования нескольких ячеек (для размера кисти кисти)
	/// </summary>
	/// <param name="center"></param>
	void EditCells(HexCell center)
	{
		int centerX = center.coordinates.X;
		int centerZ = center.coordinates.Z;

		for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++)
		{
			for (int x = centerX - r; x <= centerX + brushSize; x++)
			{
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
		for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
		{
			for (int x = centerX - brushSize; x <= centerX + r; x++)
			{
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
	}

    public void EditCell(HexCell cell)
    {
        if (cell)
        {
            if (applyColor)
            {
                cell.ColorIndex = Array.IndexOf(hexGrid.colors, activeColor);
                cell.Color = activeColor;
            }
            if (applyElevation)
            {
                cell.Elevation = activeElevation;
            }
			if (walledMode != OptionalToggle.Ignore)
			{
				cell.Walled = walledMode == OptionalToggle.Yes;
			}
		}
    }

    public void CaptureCell(HexCell cell)
	{
		if (cell)
		{
			var playerTeam = gameController.GetPlayerTeam();
			cell.ownerColorHighligh = playerTeam.colorIndex;
			cell.OwnerId = playerTeam.id;
			Cell updatedCell = new Cell(cell.ColorIndex, cell.Elevation, cell.coordinates.X, cell.coordinates.Y, cell.coordinates.Z, playerTeam.id);
			UpdateTargetCell(updatedCell);
			gameController.DemoPoints -= 1;
		}
	}

	public void TeamCellDestribution()
    {
		var teams = gameController.Teams;
		var extremeChunks = hexGrid.GetExtremeChunks();
        for (int i = 0; i < teams.Count; i++)
        {
			var cell = extremeChunks[i].GetRandomCell();
			cell.ownerColorHighligh = teams[i].colorIndex;
			cell.OwnerId = teams[i].id;
			Cell updatedCell = new Cell(cell.ColorIndex, cell.Elevation, cell.coordinates.X, cell.coordinates.Y, cell.coordinates.Z, teams[i].id);
			UpdateTargetCell(updatedCell);
		}
    }

	//public void CaptureCell(HexCell cell)
	//{
	//	if (cell && gameController.GetPlayerTeam() != null)
	//	{
	//		cell.ownerColorHighligh = Color.green;
	//		cell.OwnerId = Guid.Parse(gameController.GetPlayerTeam().id);
	//	}
	//}

	/// <summary>
	/// Метод для захвата ячейки из React
	/// </summary>
	/// <param name="cell"></param>
	public void OnCaptureCell(Cell cell)
    {
		var cellCoordinates = new HexCoordinates(cell.x, cell.z);
		var selectedCell = hexGrid.GetCell(cellCoordinates);
		selectedCell.ownerColorHighligh = gameController.GetTeamColor(cell.ownerId);
		selectedCell.OwnerId = cell.ownerId;
    }

    

    public void SelectColor(int index)
	{
		applyColor = index >= 0;
		if (applyColor)
		{
			activeColor = hexGrid.colors[index];
		}
	}

	public void SetElevation(float elevation)
	{
		activeElevation = (int)elevation;
	}

	/// <summary>
	/// Метод отключения/включения ползунка высоты
	/// </summary>
	/// <param name="toggle"></param>
	public void SetApplyElevation(bool toggle)
	{
		applyElevation = toggle;
	}

	/// <summary>
	/// Метод отвчающий за размер кисти
	/// </summary>
	/// <param name="size"></param>
	public void SetBrushSize(float size)
	{
		brushSize = (int)size;
	}

	/// <summary>
	/// Переключатель редактирования карты
	/// </summary>
	/// <param name="size"></param>
	public void SetEdit(bool toggle)
	{
		isEdit = toggle;
	}

	/// <summary>
	/// Режим простановки стен
	/// </summary>
	/// <param name="mode"></param>
	public void SetWalledMode(int mode)
	{
		walledMode = (OptionalToggle)mode;
	}

	/// <summary>
	/// Метод управления индексом активного типа местности
	/// </summary>
	/// <param name="index"></param>
	//public void SetTerrainTypeIndex(int index)
	//{
	//	activeTerrainTypeIndex = index;
	//}

	/// <summary>
	/// Метод показа координат(меток) ячеек
	/// </summary>
	/// <param name="visible"></param>
	public void ShowUI(bool visible)
	{
		hexGrid.ShowUI(visible);
	}

	[DllImport("__Internal")]
	private static extern void SaveMap(string mapData);
	[DllImport("__Internal")]
	private static extern void LoadMap();
	[DllImport("__Internal")]
	private static extern void UpdateCell(string cell);

	public void Save()
	{
		var mapData = new SaveMapData(new List<Cell>());
		hexGrid.Save(mapData);
		string jsonMap = JsonUtility.ToJson(mapData);
#if UNITY_WEBGL == true && UNITY_EDITOR == false
				SaveMap(jsonMap);
#endif
	}

	public void UpdateTargetCell(Cell cell)
	{
		string jsonCell = JsonUtility.ToJson(cell);
#if UNITY_WEBGL == true && UNITY_EDITOR == false
				UpdateCell(jsonCell);
#endif
	}
	/// <summary>
	/// Метод загрузки карты (Unity->React)
	/// </summary>
	public void Load()
	{
#if UNITY_WEBGL == true && UNITY_EDITOR == false
		LoadMap();
#endif
#if UNITY_EDITOR == true
		SetMapData("");
#endif
	}
	/// <summary>
	/// Метод загрузки карты (React->Unity)
	/// </summary>
	/// <param name="mapJson"></param>
	public void SetMapData(string mapJson)
	{
#if UNITY_EDITOR == true
		mapJson = "{\"cells\":[{\"id\":85,\"color\":1,\"elevation\":1,\"x\":0,\"y\":0,\"z\":0,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":86,\"color\":1,\"elevation\":2,\"x\":4,\"y\":-20,\"z\":16,\"ownerId\":null},{\"id\":87,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-21,\"z\":16,\"ownerId\":null},{\"id\":88,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-22,\"z\":16,\"ownerId\":null},{\"id\":89,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-23,\"z\":16,\"ownerId\":null},{\"id\":90,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-24,\"z\":16,\"ownerId\":null},{\"id\":91,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-25,\"z\":16,\"ownerId\":null},{\"id\":92,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-26,\"z\":16,\"ownerId\":null},{\"id\":93,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-27,\"z\":16,\"ownerId\":null},{\"id\":94,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-28,\"z\":16,\"ownerId\":null},{\"id\":95,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-29,\"z\":16,\"ownerId\":null},{\"id\":96,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-30,\"z\":16,\"ownerId\":null},{\"id\":97,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-31,\"z\":16,\"ownerId\":null},{\"id\":98,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-32,\"z\":16,\"ownerId\":null},{\"id\":99,\"color\":0,\"elevation\":0,\"x\":-8,\"y\":-9,\"z\":17,\"ownerId\":null},{\"id\":100,\"color\":0,\"elevation\":0,\"x\":-7,\"y\":-10,\"z\":17,\"ownerId\":null},{\"id\":101,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-19,\"z\":16,\"ownerId\":null},{\"id\":102,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-18,\"z\":16,\"ownerId\":null},{\"id\":103,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-17,\"z\":16,\"ownerId\":null},{\"id\":104,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-16,\"z\":16,\"ownerId\":null},{\"id\":105,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-25,\"z\":15,\"ownerId\":null},{\"id\":106,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-26,\"z\":15,\"ownerId\":null},{\"id\":107,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-27,\"z\":15,\"ownerId\":null},{\"id\":108,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-28,\"z\":15,\"ownerId\":null},{\"id\":109,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-29,\"z\":15,\"ownerId\":null},{\"id\":110,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-30,\"z\":15,\"ownerId\":null},{\"id\":111,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-31,\"z\":15,\"ownerId\":null},{\"id\":112,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-11,\"z\":17,\"ownerId\":null},{\"id\":113,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-32,\"z\":15,\"ownerId\":null},{\"id\":114,\"color\":0,\"elevation\":0,\"x\":-7,\"y\":-9,\"z\":16,\"ownerId\":null},{\"id\":115,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-10,\"z\":16,\"ownerId\":null},{\"id\":116,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-11,\"z\":16,\"ownerId\":null},{\"id\":117,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-12,\"z\":16,\"ownerId\":null},{\"id\":118,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-13,\"z\":16,\"ownerId\":null},{\"id\":119,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-14,\"z\":16,\"ownerId\":null},{\"id\":120,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-15,\"z\":16,\"ownerId\":null},{\"id\":121,\"color\":0,\"elevation\":0,\"x\":-8,\"y\":-8,\"z\":16,\"ownerId\":null},{\"id\":122,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-24,\"z\":15,\"ownerId\":null},{\"id\":123,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-12,\"z\":17,\"ownerId\":null},{\"id\":124,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-14,\"z\":17,\"ownerId\":null},{\"id\":125,\"color\":0,\"elevation\":0,\"x\":-8,\"y\":-10,\"z\":18,\"ownerId\":null},{\"id\":126,\"color\":0,\"elevation\":0,\"x\":-7,\"y\":-11,\"z\":18,\"ownerId\":null},{\"id\":127,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-12,\"z\":18,\"ownerId\":null},{\"id\":128,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-13,\"z\":18,\"ownerId\":null},{\"id\":129,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-14,\"z\":18,\"ownerId\":null},{\"id\":130,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-15,\"z\":18,\"ownerId\":null},{\"id\":131,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-16,\"z\":18,\"ownerId\":null},{\"id\":132,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-17,\"z\":18,\"ownerId\":null},{\"id\":133,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-18,\"z\":18,\"ownerId\":null},{\"id\":134,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-19,\"z\":18,\"ownerId\":null},{\"id\":135,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-20,\"z\":18,\"ownerId\":null},{\"id\":136,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-21,\"z\":18,\"ownerId\":null},{\"id\":137,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-22,\"z\":18,\"ownerId\":null},{\"id\":138,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-23,\"z\":18,\"ownerId\":null},{\"id\":139,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-24,\"z\":18,\"ownerId\":null},{\"id\":140,\"color\":0,\"elevation\":0,\"x\":-9,\"y\":-9,\"z\":18,\"ownerId\":null},{\"id\":141,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-33,\"z\":17,\"ownerId\":null},{\"id\":142,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-32,\"z\":17,\"ownerId\":null},{\"id\":143,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-31,\"z\":17,\"ownerId\":null},{\"id\":144,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-15,\"z\":17,\"ownerId\":null},{\"id\":145,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-16,\"z\":17,\"ownerId\":null},{\"id\":146,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-17,\"z\":17,\"ownerId\":null},{\"id\":147,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-18,\"z\":17,\"ownerId\":null},{\"id\":148,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-19,\"z\":17,\"ownerId\":null},{\"id\":149,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-20,\"z\":17,\"ownerId\":null},{\"id\":150,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-21,\"z\":17,\"ownerId\":null},{\"id\":151,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-13,\"z\":17,\"ownerId\":null},{\"id\":152,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-22,\"z\":17,\"ownerId\":null},{\"id\":153,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-24,\"z\":17,\"ownerId\":null},{\"id\":154,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-25,\"z\":17,\"ownerId\":null},{\"id\":155,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-26,\"z\":17,\"ownerId\":null},{\"id\":156,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-27,\"z\":17,\"ownerId\":null},{\"id\":157,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-28,\"z\":17,\"ownerId\":null},{\"id\":158,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-29,\"z\":17,\"ownerId\":null},{\"id\":159,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-30,\"z\":17,\"ownerId\":null},{\"id\":160,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-23,\"z\":17,\"ownerId\":null},{\"id\":161,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-25,\"z\":18,\"ownerId\":null},{\"id\":162,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-23,\"z\":15,\"ownerId\":null},{\"id\":163,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-21,\"z\":15,\"ownerId\":null},{\"id\":164,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-16,\"z\":13,\"ownerId\":null},{\"id\":165,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-17,\"z\":13,\"ownerId\":null},{\"id\":166,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-18,\"z\":13,\"ownerId\":null},{\"id\":167,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-19,\"z\":13,\"ownerId\":null},{\"id\":168,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-20,\"z\":13,\"ownerId\":null},{\"id\":169,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-21,\"z\":13,\"ownerId\":null},{\"id\":170,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-22,\"z\":13,\"ownerId\":null},{\"id\":171,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-23,\"z\":13,\"ownerId\":null},{\"id\":172,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-24,\"z\":13,\"ownerId\":null},{\"id\":173,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-25,\"z\":13,\"ownerId\":null},{\"id\":174,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-26,\"z\":13,\"ownerId\":null},{\"id\":175,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-27,\"z\":13,\"ownerId\":null},{\"id\":176,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-28,\"z\":13,\"ownerId\":null},{\"id\":177,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-29,\"z\":13,\"ownerId\":null},{\"id\":178,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-30,\"z\":13,\"ownerId\":null},{\"id\":179,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-15,\"z\":13,\"ownerId\":null},{\"id\":180,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-14,\"z\":13,\"ownerId\":null},{\"id\":181,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-13,\"z\":13,\"ownerId\":null},{\"id\":182,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-12,\"z\":13,\"ownerId\":null},{\"id\":183,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-20,\"z\":12,\"ownerId\":null},{\"id\":184,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-21,\"z\":12,\"ownerId\":null},{\"id\":185,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-22,\"z\":12,\"ownerId\":null},{\"id\":186,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-23,\"z\":12,\"ownerId\":null},{\"id\":187,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-24,\"z\":12,\"ownerId\":null},{\"id\":188,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-25,\"z\":12,\"ownerId\":null},{\"id\":189,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-26,\"z\":12,\"ownerId\":null},{\"id\":190,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-31,\"z\":13,\"ownerId\":null},{\"id\":191,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-27,\"z\":12,\"ownerId\":null},{\"id\":192,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-29,\"z\":12,\"ownerId\":null},{\"id\":193,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-30,\"z\":12,\"ownerId\":null},{\"id\":194,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-7,\"z\":13,\"ownerId\":null},{\"id\":195,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-8,\"z\":13,\"ownerId\":null},{\"id\":196,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-9,\"z\":13,\"ownerId\":null},{\"id\":197,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-10,\"z\":13,\"ownerId\":null},{\"id\":198,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-11,\"z\":13,\"ownerId\":null},{\"id\":199,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-28,\"z\":12,\"ownerId\":null},{\"id\":200,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-22,\"z\":15,\"ownerId\":null},{\"id\":201,\"color\":0,\"elevation\":0,\"x\":-7,\"y\":-7,\"z\":14,\"ownerId\":null},{\"id\":202,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-9,\"z\":14,\"ownerId\":null},{\"id\":203,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-30,\"z\":14,\"ownerId\":null},{\"id\":204,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-31,\"z\":14,\"ownerId\":null},{\"id\":205,\"color\":0,\"elevation\":0,\"x\":-7,\"y\":-8,\"z\":15,\"ownerId\":null},{\"id\":206,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-9,\"z\":15,\"ownerId\":null},{\"id\":207,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-10,\"z\":15,\"ownerId\":null},{\"id\":208,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-11,\"z\":15,\"ownerId\":null},{\"id\":209,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-12,\"z\":15,\"ownerId\":null},{\"id\":210,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-13,\"z\":15,\"ownerId\":null},{\"id\":211,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-14,\"z\":15,\"ownerId\":null},{\"id\":212,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-15,\"z\":15,\"ownerId\":null},{\"id\":213,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-16,\"z\":15,\"ownerId\":null},{\"id\":214,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-17,\"z\":15,\"ownerId\":null},{\"id\":215,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-18,\"z\":15,\"ownerId\":null},{\"id\":216,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-19,\"z\":15,\"ownerId\":null},{\"id\":217,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-20,\"z\":15,\"ownerId\":null},{\"id\":218,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-29,\"z\":14,\"ownerId\":null},{\"id\":219,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-28,\"z\":14,\"ownerId\":null},{\"id\":220,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-27,\"z\":14,\"ownerId\":null},{\"id\":221,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-26,\"z\":14,\"ownerId\":null},{\"id\":222,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-10,\"z\":14,\"ownerId\":null},{\"id\":223,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-11,\"z\":14,\"ownerId\":null},{\"id\":224,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-12,\"z\":14,\"ownerId\":null},{\"id\":225,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-13,\"z\":14,\"ownerId\":null},{\"id\":226,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-14,\"z\":14,\"ownerId\":null},{\"id\":227,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-15,\"z\":14,\"ownerId\":null},{\"id\":228,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-16,\"z\":14,\"ownerId\":null},{\"id\":229,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-8,\"z\":14,\"ownerId\":null},{\"id\":230,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-17,\"z\":14,\"ownerId\":null},{\"id\":231,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-19,\"z\":14,\"ownerId\":null},{\"id\":232,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-20,\"z\":14,\"ownerId\":null},{\"id\":233,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-21,\"z\":14,\"ownerId\":null},{\"id\":234,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-22,\"z\":14,\"ownerId\":null},{\"id\":235,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-23,\"z\":14,\"ownerId\":null},{\"id\":236,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-24,\"z\":14,\"ownerId\":null},{\"id\":237,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-25,\"z\":14,\"ownerId\":null},{\"id\":238,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-18,\"z\":14,\"ownerId\":null},{\"id\":239,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-26,\"z\":18,\"ownerId\":null},{\"id\":240,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-27,\"z\":18,\"ownerId\":null},{\"id\":241,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-28,\"z\":18,\"ownerId\":null},{\"id\":242,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-30,\"z\":22,\"ownerId\":null},{\"id\":243,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-31,\"z\":22,\"ownerId\":null},{\"id\":244,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-32,\"z\":22,\"ownerId\":null},{\"id\":245,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-33,\"z\":22,\"ownerId\":null},{\"id\":246,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-34,\"z\":22,\"ownerId\":null},{\"id\":247,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-35,\"z\":22,\"ownerId\":null},{\"id\":248,\"color\":0,\"elevation\":0,\"x\":-11,\"y\":-12,\"z\":23,\"ownerId\":null},{\"id\":249,\"color\":0,\"elevation\":0,\"x\":-10,\"y\":-13,\"z\":23,\"ownerId\":null},{\"id\":250,\"color\":0,\"elevation\":0,\"x\":-9,\"y\":-14,\"z\":23,\"ownerId\":null},{\"id\":251,\"color\":0,\"elevation\":0,\"x\":-8,\"y\":-15,\"z\":23,\"ownerId\":null},{\"id\":252,\"color\":0,\"elevation\":0,\"x\":-7,\"y\":-16,\"z\":23,\"ownerId\":null},{\"id\":253,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-17,\"z\":23,\"ownerId\":null},{\"id\":254,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-18,\"z\":23,\"ownerId\":null},{\"id\":255,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-19,\"z\":23,\"ownerId\":null},{\"id\":256,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-20,\"z\":23,\"ownerId\":null},{\"id\":257,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-29,\"z\":22,\"ownerId\":null},{\"id\":258,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-28,\"z\":22,\"ownerId\":null},{\"id\":259,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-27,\"z\":22,\"ownerId\":null},{\"id\":260,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-26,\"z\":22,\"ownerId\":null},{\"id\":261,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-35,\"z\":21,\"ownerId\":null},{\"id\":262,\"color\":0,\"elevation\":0,\"x\":-11,\"y\":-11,\"z\":22,\"ownerId\":null},{\"id\":263,\"color\":0,\"elevation\":0,\"x\":-10,\"y\":-12,\"z\":22,\"ownerId\":null},{\"id\":264,\"color\":0,\"elevation\":0,\"x\":-9,\"y\":-13,\"z\":22,\"ownerId\":null},{\"id\":265,\"color\":0,\"elevation\":0,\"x\":-8,\"y\":-14,\"z\":22,\"ownerId\":null},{\"id\":266,\"color\":0,\"elevation\":0,\"x\":-7,\"y\":-15,\"z\":22,\"ownerId\":null},{\"id\":267,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-16,\"z\":22,\"ownerId\":null},{\"id\":268,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-21,\"z\":23,\"ownerId\":null},{\"id\":269,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-17,\"z\":22,\"ownerId\":null},{\"id\":270,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-19,\"z\":22,\"ownerId\":null},{\"id\":271,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-20,\"z\":22,\"ownerId\":null},{\"id\":272,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-21,\"z\":22,\"ownerId\":null},{\"id\":273,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-22,\"z\":22,\"ownerId\":null},{\"id\":274,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-23,\"z\":22,\"ownerId\":null},{\"id\":275,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-24,\"z\":22,\"ownerId\":null},{\"id\":276,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-25,\"z\":22,\"ownerId\":null},{\"id\":277,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-18,\"z\":22,\"ownerId\":null},{\"id\":278,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-34,\"z\":21,\"ownerId\":null},{\"id\":279,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-22,\"z\":23,\"ownerId\":null},{\"id\":280,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-24,\"z\":23,\"ownerId\":null},{\"id\":281,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-20,\"z\":24,\"ownerId\":null},{\"id\":282,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-21,\"z\":24,\"ownerId\":null},{\"id\":283,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-22,\"z\":24,\"ownerId\":null},{\"id\":284,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-23,\"z\":24,\"ownerId\":null},{\"id\":285,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-24,\"z\":24,\"ownerId\":null},{\"id\":286,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-25,\"z\":24,\"ownerId\":null},{\"id\":287,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-26,\"z\":24,\"ownerId\":null},{\"id\":288,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-27,\"z\":24,\"ownerId\":null},{\"id\":289,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-28,\"z\":24,\"ownerId\":null},{\"id\":290,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-29,\"z\":24,\"ownerId\":null},{\"id\":291,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-30,\"z\":24,\"ownerId\":null},{\"id\":292,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-31,\"z\":24,\"ownerId\":null},{\"id\":293,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-32,\"z\":24,\"ownerId\":null},{\"id\":294,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-33,\"z\":24,\"ownerId\":null},{\"id\":295,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-34,\"z\":24,\"ownerId\":null},{\"id\":296,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-19,\"z\":24,\"ownerId\":null},{\"id\":297,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-18,\"z\":24,\"ownerId\":null},{\"id\":298,\"color\":0,\"elevation\":0,\"x\":-7,\"y\":-17,\"z\":24,\"ownerId\":null},{\"id\":299,\"color\":0,\"elevation\":0,\"x\":-8,\"y\":-16,\"z\":24,\"ownerId\":null},{\"id\":300,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-25,\"z\":23,\"ownerId\":null},{\"id\":301,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-26,\"z\":23,\"ownerId\":null},{\"id\":302,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-27,\"z\":23,\"ownerId\":null},{\"id\":303,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-28,\"z\":23,\"ownerId\":null},{\"id\":304,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-29,\"z\":23,\"ownerId\":null},{\"id\":305,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-30,\"z\":23,\"ownerId\":null},{\"id\":306,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-31,\"z\":23,\"ownerId\":null},{\"id\":307,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-23,\"z\":23,\"ownerId\":null},{\"id\":308,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-32,\"z\":23,\"ownerId\":null},{\"id\":309,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-34,\"z\":23,\"ownerId\":null},{\"id\":310,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-35,\"z\":23,\"ownerId\":null},{\"id\":311,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-36,\"z\":23,\"ownerId\":null},{\"id\":312,\"color\":0,\"elevation\":0,\"x\":-12,\"y\":-12,\"z\":24,\"ownerId\":null},{\"id\":313,\"color\":0,\"elevation\":0,\"x\":-11,\"y\":-13,\"z\":24,\"ownerId\":null},{\"id\":314,\"color\":0,\"elevation\":0,\"x\":-10,\"y\":-14,\"z\":24,\"ownerId\":null},{\"id\":315,\"color\":0,\"elevation\":0,\"x\":-9,\"y\":-15,\"z\":24,\"ownerId\":null},{\"id\":316,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-33,\"z\":23,\"ownerId\":null},{\"id\":317,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-33,\"z\":21,\"ownerId\":null},{\"id\":318,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-32,\"z\":21,\"ownerId\":null},{\"id\":319,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-31,\"z\":21,\"ownerId\":null},{\"id\":320,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-25,\"z\":19,\"ownerId\":null},{\"id\":321,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-26,\"z\":19,\"ownerId\":null},{\"id\":322,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-27,\"z\":19,\"ownerId\":null},{\"id\":323,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-28,\"z\":19,\"ownerId\":null},{\"id\":324,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-29,\"z\":19,\"ownerId\":null},{\"id\":325,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-30,\"z\":19,\"ownerId\":null},{\"id\":326,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-31,\"z\":19,\"ownerId\":null},{\"id\":327,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-32,\"z\":19,\"ownerId\":null},{\"id\":328,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-33,\"z\":19,\"ownerId\":null},{\"id\":329,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-34,\"z\":19,\"ownerId\":null},{\"id\":330,\"color\":0,\"elevation\":0,\"x\":-10,\"y\":-10,\"z\":20,\"ownerId\":null},{\"id\":331,\"color\":0,\"elevation\":0,\"x\":-9,\"y\":-11,\"z\":20,\"ownerId\":null},{\"id\":332,\"color\":0,\"elevation\":0,\"x\":-8,\"y\":-12,\"z\":20,\"ownerId\":null},{\"id\":333,\"color\":0,\"elevation\":0,\"x\":-7,\"y\":-13,\"z\":20,\"ownerId\":null},{\"id\":334,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-14,\"z\":20,\"ownerId\":null},{\"id\":335,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-24,\"z\":19,\"ownerId\":null},{\"id\":336,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-23,\"z\":19,\"ownerId\":null},{\"id\":337,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-22,\"z\":19,\"ownerId\":null},{\"id\":338,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-21,\"z\":19,\"ownerId\":null},{\"id\":339,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-29,\"z\":18,\"ownerId\":null},{\"id\":340,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-30,\"z\":18,\"ownerId\":null},{\"id\":341,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-31,\"z\":18,\"ownerId\":null},{\"id\":342,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-32,\"z\":18,\"ownerId\":null},{\"id\":343,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-33,\"z\":18,\"ownerId\":null},{\"id\":344,\"color\":0,\"elevation\":0,\"x\":-9,\"y\":-10,\"z\":19,\"ownerId\":null},{\"id\":345,\"color\":0,\"elevation\":0,\"x\":-8,\"y\":-11,\"z\":19,\"ownerId\":null},{\"id\":346,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-15,\"z\":20,\"ownerId\":null},{\"id\":347,\"color\":0,\"elevation\":0,\"x\":-7,\"y\":-12,\"z\":19,\"ownerId\":null},{\"id\":348,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-14,\"z\":19,\"ownerId\":null},{\"id\":349,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-15,\"z\":19,\"ownerId\":null},{\"id\":350,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-16,\"z\":19,\"ownerId\":null},{\"id\":351,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-17,\"z\":19,\"ownerId\":null},{\"id\":352,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-18,\"z\":19,\"ownerId\":null},{\"id\":353,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-19,\"z\":19,\"ownerId\":null},{\"id\":354,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-20,\"z\":19,\"ownerId\":null},{\"id\":355,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-13,\"z\":19,\"ownerId\":null},{\"id\":356,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-16,\"z\":20,\"ownerId\":null},{\"id\":357,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-17,\"z\":20,\"ownerId\":null},{\"id\":358,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-18,\"z\":20,\"ownerId\":null},{\"id\":359,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-15,\"z\":21,\"ownerId\":null},{\"id\":360,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-16,\"z\":21,\"ownerId\":null},{\"id\":361,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-17,\"z\":21,\"ownerId\":null},{\"id\":362,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-18,\"z\":21,\"ownerId\":null},{\"id\":363,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-19,\"z\":21,\"ownerId\":null},{\"id\":364,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-20,\"z\":21,\"ownerId\":null},{\"id\":365,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-21,\"z\":21,\"ownerId\":null},{\"id\":366,\"color\":0,\"elevation\":0,\"x\":-7,\"y\":-14,\"z\":21,\"ownerId\":null},{\"id\":367,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-22,\"z\":21,\"ownerId\":null},{\"id\":368,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-24,\"z\":21,\"ownerId\":null},{\"id\":369,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-25,\"z\":21,\"ownerId\":null},{\"id\":370,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-26,\"z\":21,\"ownerId\":null},{\"id\":371,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-27,\"z\":21,\"ownerId\":null},{\"id\":372,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-28,\"z\":21,\"ownerId\":null},{\"id\":373,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-29,\"z\":21,\"ownerId\":null},{\"id\":374,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-30,\"z\":21,\"ownerId\":null},{\"id\":375,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-23,\"z\":21,\"ownerId\":null},{\"id\":376,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-19,\"z\":12,\"ownerId\":null},{\"id\":377,\"color\":0,\"elevation\":0,\"x\":-8,\"y\":-13,\"z\":21,\"ownerId\":null},{\"id\":378,\"color\":0,\"elevation\":0,\"x\":-10,\"y\":-11,\"z\":21,\"ownerId\":null},{\"id\":379,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-19,\"z\":20,\"ownerId\":null},{\"id\":380,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-20,\"z\":20,\"ownerId\":null},{\"id\":381,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-21,\"z\":20,\"ownerId\":null},{\"id\":382,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-22,\"z\":20,\"ownerId\":null},{\"id\":383,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-23,\"z\":20,\"ownerId\":null},{\"id\":384,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-24,\"z\":20,\"ownerId\":null},{\"id\":385,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-25,\"z\":20,\"ownerId\":null},{\"id\":386,\"color\":0,\"elevation\":0,\"x\":-9,\"y\":-12,\"z\":21,\"ownerId\":null},{\"id\":387,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-26,\"z\":20,\"ownerId\":null},{\"id\":388,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-28,\"z\":20,\"ownerId\":null},{\"id\":389,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-29,\"z\":20,\"ownerId\":null},{\"id\":390,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-30,\"z\":20,\"ownerId\":null},{\"id\":391,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-31,\"z\":20,\"ownerId\":null},{\"id\":392,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-32,\"z\":20,\"ownerId\":null},{\"id\":393,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-33,\"z\":20,\"ownerId\":null},{\"id\":394,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-34,\"z\":20,\"ownerId\":null},{\"id\":395,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-27,\"z\":20,\"ownerId\":null},{\"id\":396,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-35,\"z\":24,\"ownerId\":null},{\"id\":397,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-18,\"z\":12,\"ownerId\":null},{\"id\":398,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-16,\"z\":12,\"ownerId\":null},{\"id\":399,\"color\":0,\"elevation\":0,\"x\":23,\"y\":-26,\"z\":3,\"ownerId\":null},{\"id\":400,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-2,\"z\":4,\"ownerId\":null},{\"id\":401,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-3,\"z\":4,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":402,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-4,\"z\":4,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":403,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-5,\"z\":4,\"ownerId\":null},{\"id\":404,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-6,\"z\":4,\"ownerId\":null},{\"id\":405,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-7,\"z\":4,\"ownerId\":null},{\"id\":406,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-8,\"z\":4,\"ownerId\":null},{\"id\":407,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-9,\"z\":4,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":408,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-10,\"z\":4,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":409,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-11,\"z\":4,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":410,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-12,\"z\":4,\"ownerId\":null},{\"id\":411,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-13,\"z\":4,\"ownerId\":null},{\"id\":412,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-14,\"z\":4,\"ownerId\":null},{\"id\":413,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-15,\"z\":4,\"ownerId\":null},{\"id\":414,\"color\":0,\"elevation\":0,\"x\":22,\"y\":-25,\"z\":3,\"ownerId\":null},{\"id\":415,\"color\":0,\"elevation\":0,\"x\":21,\"y\":-24,\"z\":3,\"ownerId\":null},{\"id\":416,\"color\":0,\"elevation\":0,\"x\":20,\"y\":-23,\"z\":3,\"ownerId\":null},{\"id\":417,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-22,\"z\":3,\"ownerId\":null},{\"id\":418,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-6,\"z\":3,\"ownerId\":null},{\"id\":419,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-7,\"z\":3,\"ownerId\":null},{\"id\":420,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-8,\"z\":3,\"ownerId\":null},{\"id\":421,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-9,\"z\":3,\"ownerId\":null},{\"id\":422,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-10,\"z\":3,\"ownerId\":null},{\"id\":423,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-11,\"z\":3,\"ownerId\":null},{\"id\":424,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-12,\"z\":3,\"ownerId\":null},{\"id\":425,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-16,\"z\":4,\"ownerId\":null},{\"id\":426,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-13,\"z\":3,\"ownerId\":null},{\"id\":427,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-15,\"z\":3,\"ownerId\":null},{\"id\":428,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-16,\"z\":3,\"ownerId\":null},{\"id\":429,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-17,\"z\":3,\"ownerId\":null},{\"id\":430,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-18,\"z\":3,\"ownerId\":null},{\"id\":431,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-19,\"z\":3,\"ownerId\":null},{\"id\":432,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-20,\"z\":3,\"ownerId\":null},{\"id\":433,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-21,\"z\":3,\"ownerId\":null},{\"id\":434,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-14,\"z\":3,\"ownerId\":null},{\"id\":435,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-5,\"z\":3,\"ownerId\":null},{\"id\":436,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-17,\"z\":4,\"ownerId\":null},{\"id\":437,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-19,\"z\":4,\"ownerId\":null},{\"id\":438,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-16,\"z\":5,\"ownerId\":null},{\"id\":439,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-17,\"z\":5,\"ownerId\":null},{\"id\":440,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-18,\"z\":5,\"ownerId\":null},{\"id\":441,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-19,\"z\":5,\"ownerId\":null},{\"id\":442,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-20,\"z\":5,\"ownerId\":null},{\"id\":443,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-21,\"z\":5,\"ownerId\":null},{\"id\":444,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-22,\"z\":5,\"ownerId\":null},{\"id\":445,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-23,\"z\":5,\"ownerId\":null},{\"id\":446,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-24,\"z\":5,\"ownerId\":null},{\"id\":447,\"color\":0,\"elevation\":0,\"x\":20,\"y\":-25,\"z\":5,\"ownerId\":null},{\"id\":448,\"color\":0,\"elevation\":0,\"x\":21,\"y\":-26,\"z\":5,\"ownerId\":null},{\"id\":449,\"color\":0,\"elevation\":0,\"x\":22,\"y\":-27,\"z\":5,\"ownerId\":null},{\"id\":450,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-3,\"z\":6,\"ownerId\":null},{\"id\":451,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-4,\"z\":6,\"ownerId\":null},{\"id\":452,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-5,\"z\":6,\"ownerId\":null},{\"id\":453,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-15,\"z\":5,\"ownerId\":null},{\"id\":454,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-14,\"z\":5,\"ownerId\":null},{\"id\":455,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-13,\"z\":5,\"ownerId\":null},{\"id\":456,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-12,\"z\":5,\"ownerId\":null},{\"id\":457,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-20,\"z\":4,\"ownerId\":null},{\"id\":458,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-21,\"z\":4,\"ownerId\":null},{\"id\":459,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-22,\"z\":4,\"ownerId\":null},{\"id\":460,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-23,\"z\":4,\"ownerId\":null},{\"id\":461,\"color\":0,\"elevation\":0,\"x\":20,\"y\":-24,\"z\":4,\"ownerId\":null},{\"id\":462,\"color\":0,\"elevation\":0,\"x\":21,\"y\":-25,\"z\":4,\"ownerId\":null},{\"id\":463,\"color\":0,\"elevation\":0,\"x\":22,\"y\":-26,\"z\":4,\"ownerId\":null},{\"id\":464,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-18,\"z\":4,\"ownerId\":null},{\"id\":465,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-3,\"z\":5,\"ownerId\":null},{\"id\":466,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-5,\"z\":5,\"ownerId\":null},{\"id\":467,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-6,\"z\":5,\"ownerId\":null},{\"id\":468,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-7,\"z\":5,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":469,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-8,\"z\":5,\"ownerId\":null},{\"id\":470,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-9,\"z\":5,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":471,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-10,\"z\":5,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":472,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-11,\"z\":5,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":473,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-4,\"z\":5,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":474,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-6,\"z\":6,\"ownerId\":null},{\"id\":475,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-4,\"z\":3,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":476,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-2,\"z\":3,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":477,\"color\":0,\"elevation\":0,\"x\":21,\"y\":-21,\"z\":0,\"ownerId\":null},{\"id\":478,\"color\":0,\"elevation\":0,\"x\":22,\"y\":-22,\"z\":0,\"ownerId\":null},{\"id\":479,\"color\":0,\"elevation\":0,\"x\":23,\"y\":-23,\"z\":0,\"ownerId\":null},{\"id\":480,\"color\":0,\"elevation\":0,\"x\":24,\"y\":-24,\"z\":0,\"ownerId\":null},{\"id\":481,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-1,\"z\":1,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":482,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-2,\"z\":1,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":483,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-3,\"z\":1,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":484,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-4,\"z\":1,\"ownerId\":null},{\"id\":485,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-5,\"z\":1,\"ownerId\":null},{\"id\":486,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-6,\"z\":1,\"ownerId\":null},{\"id\":487,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-7,\"z\":1,\"ownerId\":null},{\"id\":488,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-8,\"z\":1,\"ownerId\":null},{\"id\":489,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-9,\"z\":1,\"ownerId\":null},{\"id\":490,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-10,\"z\":1,\"ownerId\":null},{\"id\":491,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-11,\"z\":1,\"ownerId\":null},{\"id\":492,\"color\":0,\"elevation\":0,\"x\":20,\"y\":-20,\"z\":0,\"ownerId\":null},{\"id\":493,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-19,\"z\":0,\"ownerId\":null},{\"id\":494,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-18,\"z\":0,\"ownerId\":null},{\"id\":495,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-17,\"z\":0,\"ownerId\":null},{\"id\":496,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-1,\"z\":0,\"ownerId\":null},{\"id\":497,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-2,\"z\":0,\"ownerId\":null},{\"id\":498,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-3,\"z\":0,\"ownerId\":null},{\"id\":499,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-4,\"z\":0,\"ownerId\":null},{\"id\":500,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-5,\"z\":0,\"ownerId\":null},{\"id\":501,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-6,\"z\":0,\"ownerId\":null},{\"id\":502,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-7,\"z\":0,\"ownerId\":null},{\"id\":503,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-12,\"z\":1,\"ownerId\":null},{\"id\":504,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-8,\"z\":0,\"ownerId\":null},{\"id\":505,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-10,\"z\":0,\"ownerId\":null},{\"id\":506,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-11,\"z\":0,\"ownerId\":null},{\"id\":507,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-12,\"z\":0,\"ownerId\":null},{\"id\":508,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-13,\"z\":0,\"ownerId\":null},{\"id\":509,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-14,\"z\":0,\"ownerId\":null},{\"id\":510,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-15,\"z\":0,\"ownerId\":null},{\"id\":511,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-16,\"z\":0,\"ownerId\":null},{\"id\":512,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-9,\"z\":0,\"ownerId\":null},{\"id\":513,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-3,\"z\":3,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":514,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-13,\"z\":1,\"ownerId\":null},{\"id\":515,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-15,\"z\":1,\"ownerId\":null},{\"id\":516,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-11,\"z\":2,\"ownerId\":null},{\"id\":517,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-12,\"z\":2,\"ownerId\":null},{\"id\":518,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-13,\"z\":2,\"ownerId\":null},{\"id\":519,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-14,\"z\":2,\"ownerId\":null},{\"id\":520,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-15,\"z\":2,\"ownerId\":null},{\"id\":521,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-16,\"z\":2,\"ownerId\":null},{\"id\":522,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-17,\"z\":2,\"ownerId\":null},{\"id\":523,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-18,\"z\":2,\"ownerId\":null},{\"id\":524,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-19,\"z\":2,\"ownerId\":null},{\"id\":525,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-20,\"z\":2,\"ownerId\":null},{\"id\":526,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-21,\"z\":2,\"ownerId\":null},{\"id\":527,\"color\":0,\"elevation\":0,\"x\":20,\"y\":-22,\"z\":2,\"ownerId\":null},{\"id\":528,\"color\":0,\"elevation\":0,\"x\":21,\"y\":-23,\"z\":2,\"ownerId\":null},{\"id\":529,\"color\":0,\"elevation\":0,\"x\":22,\"y\":-24,\"z\":2,\"ownerId\":null},{\"id\":530,\"color\":0,\"elevation\":0,\"x\":23,\"y\":-25,\"z\":2,\"ownerId\":null},{\"id\":531,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-10,\"z\":2,\"ownerId\":null},{\"id\":532,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-9,\"z\":2,\"ownerId\":null},{\"id\":533,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-8,\"z\":2,\"ownerId\":null},{\"id\":534,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-7,\"z\":2,\"ownerId\":null},{\"id\":535,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-16,\"z\":1,\"ownerId\":null},{\"id\":536,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-17,\"z\":1,\"ownerId\":null},{\"id\":537,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-18,\"z\":1,\"ownerId\":null},{\"id\":538,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-19,\"z\":1,\"ownerId\":null},{\"id\":539,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-20,\"z\":1,\"ownerId\":null},{\"id\":540,\"color\":0,\"elevation\":0,\"x\":20,\"y\":-21,\"z\":1,\"ownerId\":null},{\"id\":541,\"color\":0,\"elevation\":0,\"x\":21,\"y\":-22,\"z\":1,\"ownerId\":null},{\"id\":542,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-14,\"z\":1,\"ownerId\":null},{\"id\":543,\"color\":0,\"elevation\":0,\"x\":22,\"y\":-23,\"z\":1,\"ownerId\":null},{\"id\":544,\"color\":0,\"elevation\":0,\"x\":24,\"y\":-25,\"z\":1,\"ownerId\":null},{\"id\":545,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-1,\"z\":2,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":546,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-2,\"z\":2,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":547,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-3,\"z\":2,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":548,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-4,\"z\":2,\"ownerId\":null},{\"id\":549,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-5,\"z\":2,\"ownerId\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\"},{\"id\":550,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-6,\"z\":2,\"ownerId\":null},{\"id\":551,\"color\":0,\"elevation\":0,\"x\":23,\"y\":-24,\"z\":1,\"ownerId\":null},{\"id\":552,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-7,\"z\":6,\"ownerId\":null},{\"id\":553,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-8,\"z\":6,\"ownerId\":null},{\"id\":554,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-9,\"z\":6,\"ownerId\":null},{\"id\":555,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-11,\"z\":10,\"ownerId\":null},{\"id\":556,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-12,\"z\":10,\"ownerId\":null},{\"id\":557,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-13,\"z\":10,\"ownerId\":null},{\"id\":558,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-14,\"z\":10,\"ownerId\":null},{\"id\":559,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-15,\"z\":10,\"ownerId\":null},{\"id\":560,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-16,\"z\":10,\"ownerId\":null},{\"id\":561,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-17,\"z\":10,\"ownerId\":null},{\"id\":562,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-18,\"z\":10,\"ownerId\":null},{\"id\":563,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-19,\"z\":10,\"ownerId\":null},{\"id\":564,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-20,\"z\":10,\"ownerId\":null},{\"id\":565,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-21,\"z\":10,\"ownerId\":null},{\"id\":566,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-22,\"z\":10,\"ownerId\":null},{\"id\":567,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-23,\"z\":10,\"ownerId\":null},{\"id\":568,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-24,\"z\":10,\"ownerId\":null},{\"id\":569,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-25,\"z\":10,\"ownerId\":null},{\"id\":570,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-10,\"z\":10,\"ownerId\":null},{\"id\":571,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-9,\"z\":10,\"ownerId\":null},{\"id\":572,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-8,\"z\":10,\"ownerId\":null},{\"id\":573,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-7,\"z\":10,\"ownerId\":null},{\"id\":574,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-16,\"z\":9,\"ownerId\":null},{\"id\":575,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-17,\"z\":9,\"ownerId\":null},{\"id\":576,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-18,\"z\":9,\"ownerId\":null},{\"id\":577,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-19,\"z\":9,\"ownerId\":null},{\"id\":578,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-20,\"z\":9,\"ownerId\":null},{\"id\":579,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-21,\"z\":9,\"ownerId\":null},{\"id\":580,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-22,\"z\":9,\"ownerId\":null},{\"id\":581,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-26,\"z\":10,\"ownerId\":null},{\"id\":582,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-23,\"z\":9,\"ownerId\":null},{\"id\":583,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-25,\"z\":9,\"ownerId\":null},{\"id\":584,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-26,\"z\":9,\"ownerId\":null},{\"id\":585,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-27,\"z\":9,\"ownerId\":null},{\"id\":586,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-28,\"z\":9,\"ownerId\":null},{\"id\":587,\"color\":0,\"elevation\":0,\"x\":20,\"y\":-29,\"z\":9,\"ownerId\":null},{\"id\":588,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-5,\"z\":10,\"ownerId\":null},{\"id\":589,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-6,\"z\":10,\"ownerId\":null},{\"id\":590,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-24,\"z\":9,\"ownerId\":null},{\"id\":591,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-15,\"z\":9,\"ownerId\":null},{\"id\":592,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-27,\"z\":10,\"ownerId\":null},{\"id\":593,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-29,\"z\":10,\"ownerId\":null},{\"id\":594,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-26,\"z\":11,\"ownerId\":null},{\"id\":595,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-27,\"z\":11,\"ownerId\":null},{\"id\":596,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-28,\"z\":11,\"ownerId\":null},{\"id\":597,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-29,\"z\":11,\"ownerId\":null},{\"id\":598,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-30,\"z\":11,\"ownerId\":null},{\"id\":599,\"color\":0,\"elevation\":0,\"x\":-6,\"y\":-6,\"z\":12,\"ownerId\":null},{\"id\":600,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-7,\"z\":12,\"ownerId\":null},{\"id\":601,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-8,\"z\":12,\"ownerId\":null},{\"id\":602,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-9,\"z\":12,\"ownerId\":null},{\"id\":603,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-10,\"z\":12,\"ownerId\":null},{\"id\":604,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-11,\"z\":12,\"ownerId\":null},{\"id\":605,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-12,\"z\":12,\"ownerId\":null},{\"id\":606,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-13,\"z\":12,\"ownerId\":null},{\"id\":607,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-14,\"z\":12,\"ownerId\":null},{\"id\":608,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-15,\"z\":12,\"ownerId\":null},{\"id\":609,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-25,\"z\":11,\"ownerId\":null},{\"id\":610,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-24,\"z\":11,\"ownerId\":null},{\"id\":611,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-23,\"z\":11,\"ownerId\":null},{\"id\":612,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-22,\"z\":11,\"ownerId\":null},{\"id\":613,\"color\":0,\"elevation\":0,\"x\":-5,\"y\":-6,\"z\":11,\"ownerId\":null},{\"id\":614,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-7,\"z\":11,\"ownerId\":null},{\"id\":615,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-8,\"z\":11,\"ownerId\":null},{\"id\":616,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-9,\"z\":11,\"ownerId\":null},{\"id\":617,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-10,\"z\":11,\"ownerId\":null},{\"id\":618,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-11,\"z\":11,\"ownerId\":null},{\"id\":619,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-12,\"z\":11,\"ownerId\":null},{\"id\":620,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-28,\"z\":10,\"ownerId\":null},{\"id\":621,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-13,\"z\":11,\"ownerId\":null},{\"id\":622,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-15,\"z\":11,\"ownerId\":null},{\"id\":623,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-16,\"z\":11,\"ownerId\":null},{\"id\":624,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-17,\"z\":11,\"ownerId\":null},{\"id\":625,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-18,\"z\":11,\"ownerId\":null},{\"id\":626,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-19,\"z\":11,\"ownerId\":null},{\"id\":627,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-20,\"z\":11,\"ownerId\":null},{\"id\":628,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-21,\"z\":11,\"ownerId\":null},{\"id\":629,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-14,\"z\":11,\"ownerId\":null},{\"id\":630,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-14,\"z\":9,\"ownerId\":null},{\"id\":631,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-13,\"z\":9,\"ownerId\":null},{\"id\":632,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-12,\"z\":9,\"ownerId\":null},{\"id\":633,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-6,\"z\":7,\"ownerId\":null},{\"id\":634,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-7,\"z\":7,\"ownerId\":null},{\"id\":635,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-8,\"z\":7,\"ownerId\":null},{\"id\":636,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-9,\"z\":7,\"ownerId\":null},{\"id\":637,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-10,\"z\":7,\"ownerId\":null},{\"id\":638,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-11,\"z\":7,\"ownerId\":null},{\"id\":639,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-12,\"z\":7,\"ownerId\":null},{\"id\":640,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-13,\"z\":7,\"ownerId\":null},{\"id\":641,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-14,\"z\":7,\"ownerId\":null},{\"id\":642,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-15,\"z\":7,\"ownerId\":null},{\"id\":643,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-16,\"z\":7,\"ownerId\":null},{\"id\":644,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-17,\"z\":7,\"ownerId\":null},{\"id\":645,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-18,\"z\":7,\"ownerId\":null},{\"id\":646,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-19,\"z\":7,\"ownerId\":null},{\"id\":647,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-20,\"z\":7,\"ownerId\":null},{\"id\":648,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-5,\"z\":7,\"ownerId\":null},{\"id\":649,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-4,\"z\":7,\"ownerId\":null},{\"id\":650,\"color\":0,\"elevation\":0,\"x\":21,\"y\":-27,\"z\":6,\"ownerId\":null},{\"id\":651,\"color\":0,\"elevation\":0,\"x\":20,\"y\":-26,\"z\":6,\"ownerId\":null},{\"id\":652,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-10,\"z\":6,\"ownerId\":null},{\"id\":653,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-11,\"z\":6,\"ownerId\":null},{\"id\":654,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-12,\"z\":6,\"ownerId\":null},{\"id\":655,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-13,\"z\":6,\"ownerId\":null},{\"id\":656,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-14,\"z\":6,\"ownerId\":null},{\"id\":657,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-15,\"z\":6,\"ownerId\":null},{\"id\":658,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-16,\"z\":6,\"ownerId\":null},{\"id\":659,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-21,\"z\":7,\"ownerId\":null},{\"id\":660,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-17,\"z\":6,\"ownerId\":null},{\"id\":661,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-19,\"z\":6,\"ownerId\":null},{\"id\":662,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-20,\"z\":6,\"ownerId\":null},{\"id\":663,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-21,\"z\":6,\"ownerId\":null},{\"id\":664,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-22,\"z\":6,\"ownerId\":null},{\"id\":665,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-23,\"z\":6,\"ownerId\":null},{\"id\":666,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-24,\"z\":6,\"ownerId\":null},{\"id\":667,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-25,\"z\":6,\"ownerId\":null},{\"id\":668,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-18,\"z\":6,\"ownerId\":null},{\"id\":669,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-22,\"z\":7,\"ownerId\":null},{\"id\":670,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-23,\"z\":7,\"ownerId\":null},{\"id\":671,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-24,\"z\":7,\"ownerId\":null},{\"id\":672,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-20,\"z\":8,\"ownerId\":null},{\"id\":673,\"color\":0,\"elevation\":0,\"x\":13,\"y\":-21,\"z\":8,\"ownerId\":null},{\"id\":674,\"color\":0,\"elevation\":0,\"x\":14,\"y\":-22,\"z\":8,\"ownerId\":null},{\"id\":675,\"color\":0,\"elevation\":0,\"x\":15,\"y\":-23,\"z\":8,\"ownerId\":null},{\"id\":676,\"color\":0,\"elevation\":0,\"x\":16,\"y\":-24,\"z\":8,\"ownerId\":null},{\"id\":677,\"color\":0,\"elevation\":0,\"x\":17,\"y\":-25,\"z\":8,\"ownerId\":null},{\"id\":678,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-26,\"z\":8,\"ownerId\":null},{\"id\":679,\"color\":0,\"elevation\":0,\"x\":11,\"y\":-19,\"z\":8,\"ownerId\":null},{\"id\":680,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-27,\"z\":8,\"ownerId\":null},{\"id\":681,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-5,\"z\":9,\"ownerId\":null},{\"id\":682,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-6,\"z\":9,\"ownerId\":null},{\"id\":683,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-7,\"z\":9,\"ownerId\":null},{\"id\":684,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-8,\"z\":9,\"ownerId\":null},{\"id\":685,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-9,\"z\":9,\"ownerId\":null},{\"id\":686,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-10,\"z\":9,\"ownerId\":null},{\"id\":687,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-11,\"z\":9,\"ownerId\":null},{\"id\":688,\"color\":0,\"elevation\":0,\"x\":20,\"y\":-28,\"z\":8,\"ownerId\":null},{\"id\":689,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-17,\"z\":12,\"ownerId\":null},{\"id\":690,\"color\":0,\"elevation\":0,\"x\":10,\"y\":-18,\"z\":8,\"ownerId\":null},{\"id\":691,\"color\":0,\"elevation\":0,\"x\":8,\"y\":-16,\"z\":8,\"ownerId\":null},{\"id\":692,\"color\":0,\"elevation\":0,\"x\":18,\"y\":-25,\"z\":7,\"ownerId\":null},{\"id\":693,\"color\":0,\"elevation\":0,\"x\":19,\"y\":-26,\"z\":7,\"ownerId\":null},{\"id\":694,\"color\":0,\"elevation\":0,\"x\":20,\"y\":-27,\"z\":7,\"ownerId\":null},{\"id\":695,\"color\":0,\"elevation\":0,\"x\":21,\"y\":-28,\"z\":7,\"ownerId\":null},{\"id\":696,\"color\":0,\"elevation\":0,\"x\":-4,\"y\":-4,\"z\":8,\"ownerId\":null},{\"id\":697,\"color\":0,\"elevation\":0,\"x\":-3,\"y\":-5,\"z\":8,\"ownerId\":null},{\"id\":698,\"color\":0,\"elevation\":0,\"x\":-2,\"y\":-6,\"z\":8,\"ownerId\":null},{\"id\":699,\"color\":0,\"elevation\":0,\"x\":9,\"y\":-17,\"z\":8,\"ownerId\":null},{\"id\":700,\"color\":0,\"elevation\":0,\"x\":-1,\"y\":-7,\"z\":8,\"ownerId\":null},{\"id\":701,\"color\":0,\"elevation\":0,\"x\":1,\"y\":-9,\"z\":8,\"ownerId\":null},{\"id\":702,\"color\":0,\"elevation\":0,\"x\":2,\"y\":-10,\"z\":8,\"ownerId\":null},{\"id\":703,\"color\":0,\"elevation\":0,\"x\":3,\"y\":-11,\"z\":8,\"ownerId\":null},{\"id\":704,\"color\":0,\"elevation\":0,\"x\":4,\"y\":-12,\"z\":8,\"ownerId\":null},{\"id\":705,\"color\":0,\"elevation\":0,\"x\":5,\"y\":-13,\"z\":8,\"ownerId\":null},{\"id\":706,\"color\":0,\"elevation\":0,\"x\":6,\"y\":-14,\"z\":8,\"ownerId\":null},{\"id\":707,\"color\":0,\"elevation\":0,\"x\":7,\"y\":-15,\"z\":8,\"ownerId\":null},{\"id\":708,\"color\":0,\"elevation\":0,\"x\":0,\"y\":-8,\"z\":8,\"ownerId\":null},{\"id\":709,\"color\":0,\"elevation\":0,\"x\":12,\"y\":-36,\"z\":24,\"ownerId\":null}]}";
#endif
		SaveMapData map = JsonUtility.FromJson<SaveMapData>(mapJson);
		hexGrid.Load(map);
        if (map.cells.Any(c => c.ownerId != null))
        {
			TeamCellDestribution();
		}
	}
	/// <summary>
	/// Метод захвата ячейки (React(SignalR) -> Unity)
	/// </summary>
	/// <param name="cellJson"></param>
	public void GrabCell(string cellJson)
    {
		Cell cell = JsonUtility.FromJson<Cell>(cellJson);
		OnCaptureCell(cell);
	}    
}