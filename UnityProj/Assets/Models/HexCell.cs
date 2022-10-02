using Assets.Models;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static MaskRenderer;

public class HexCell : MonoBehaviour
{
    /// <summary>
    /// Координаты ячейки
    /// </summary>
    public HexCoordinates coordinates;
    /// <summary>
    /// Цвет ячейки
    /// </summary>
    public Color Color
    {
        get
        {
            return HexMetrics.colors[ColorIndex];
        }
        set
        {
            if (color == value)
            {
                return;
            }
            color = value;
            Refresh();
        }
    }
    public Color color;
    
    // Индекс цвета для сохраенения для сохранения
    public int ColorIndex
    {
        get
        {
            return colorIndex;
        }
        set
        {
            if (colorIndex != value)
            {
                colorIndex = value;
                Refresh();
            }
        }
    }
    string ownerId;
    public Color ownerColorHighligh;
    public string OwnerId { 
        get
        {
            return ownerId;
        } 
        set
        {
            ownerId = value;
            EnableOwnerHighlight(ownerColorHighligh);
            //Array.ForEach(neighbors, n => n?.DisableFogOfWar());
            DisableFogOfWar();

            Debug.Log($"Ячейка с координатами {this.coordinates.X}, {this.coordinates.Y} захвачена группой с id: {value}");
        }
    }
    /// <summary>
    /// Переключатель стен
    /// </summary>
    public bool Walled
    {
        get
        {
            return walled;
        }
        set
        {
            if (walled != value)
            {
                walled = value;
                Refresh();
            }
        }
    }

    bool walled;

    private void DisableFogOfWar()
    {
        //var hexGridParent = GetComponentInParent<HexGrid>();
        if (GameController.PlayerTeam.id != OwnerId)
        {
            return;
        }
        ToggleVisibility();
        //var fogOfWar = hexGridParent.GetComponentsInChildren<FogOfWar>()
        //    .Where(f => f.hexCoordinates.Equals(coordinates))
        //    .Single();
        //var fogOfWarInstance = fogOfWar.gameObject.GetComponentInChildren<ParticleSystem>();
        
        //if (!fogOfWarInstance.isStopped)
        //{
        //    fogOfWarInstance.Stop();
        //    fogOfWarInstance.Clear();
        //}
    }

    // Для сохранения(сохраняем не цвет, а его индекс)    
    int colorIndex;

    /// <summary>
    /// Уровень высоты
    /// </summary>
    int elevation = int.MinValue;
    /// <summary>
    /// Ссылка на сегмент
    /// </summary>
    public HexGridChunk chunk;
    /// <summary>
    /// Массив соседей
    /// </summary>
    [SerializeField]
    HexCell[] neighbors;

    public RectTransform uiRect;

    private void Start()
    {
        MaskRenderer.RegisterCell(this);
    }

    void Refresh()
    {
        if (chunk) {
			chunk.Refresh();
            for (int i = 0; i < neighbors.Length; i++)
            {
                HexCell neighbor = neighbors[i];
                if (neighbor != null && neighbor.chunk != chunk)
                {
                    neighbor.chunk.Refresh();
                }
            }
        }
    }
    public int Elevation
    {
        get
        {
            return elevation;
        }
        set
        {
            if (elevation == value)
            {
                return;
            }
            elevation = value;
            RefreshPosition();
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            position.y +=
                (HexMetrics.SampleNoise(position).y * 2f - 1f) *
                HexMetrics.elevationPerturbStrength;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = -position.y;
            uiRect.localPosition = uiPosition;
            Refresh();
        }
    }
    /// <summary>
    /// Свойство для получения позиции (убирает разрывы при действии шума)
    /// </summary>
    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    /// <summary>
    /// Получение соседа ячейки в одном направлении
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    /// <summary>
    /// Метод получения соседей
    /// </summary>
    /// <returns></returns>
    public HexCell[] GetNeighbors()
    {
        return neighbors;
    }

    /// <summary>
    /// Метод установки соседа
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="cell"></param>
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }
    /// <summary>
    /// Получение типа склона по направлению
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public HexEdgeType GetEdgeType(HexDirection direction)
    {
        return HexMetrics.GetEdgeType(
            elevation, neighbors[(int)direction].elevation
        );
    }
    /// <summary>
    /// Определение наклона между двумя любымми ячейками
    /// </summary>
    /// <param name="otherCell"></param>
    /// <returns></returns>
    public HexEdgeType GetEdgeType(HexCell otherCell)
    {
        return HexMetrics.GetEdgeType(
            elevation, otherCell.elevation
        );
    }
    public void Save(SaveMapData map)
    {
        map.cells.Add(new Cell(colorIndex, elevation, coordinates.X, coordinates.Y, coordinates.Z));
    }

    public void Load(Cell cell)
    {
        colorIndex = cell.color;
        elevation = cell.elevation;
        RefreshPosition();
    }
    /// <summary>
    /// Обновление высоты после загрузыки
    /// </summary>
    void RefreshPosition()
    {
        Vector3 position = transform.localPosition;
        position.y = elevation * HexMetrics.elevationStep;
        position.y +=
            (HexMetrics.SampleNoise(position).y * 2f - 1f) *
            HexMetrics.elevationPerturbStrength;
        transform.localPosition = position;

        Vector3 uiPosition = uiRect.localPosition;
        uiPosition.z = -position.y;
        uiRect.localPosition = uiPosition;
    }

    /// <summary>
    /// Метод отключения выделения ячейки
    /// </summary>
    public void DisableHighlight()
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.enabled = false;
    }
    /// <summary>
    /// Метод включения выделения ячейки
    /// </summary>
    /// <param name="color"></param>
    public void EnableHighlight(Color color)
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.color = color;
        highlight.enabled = true;
    }

    public void EnableOwnerHighlight(Color color)
    {
        Image highlight = uiRect.GetChild(1).GetComponent<Image>();
        highlight.color = color;
        highlight.enabled = true;
    }

    #region[FogOfWar]
    /// <summary>
    /// Ranges from 0 to 1 with 0 indicating that the cell is not visible
    /// </summary>
    public float Visibility { get; private set; }


    /// <summary>
    /// Toggle the visibility and lerp to the new value from the current one
    /// Interupts itself if still in a animation
    /// </summary>
    public void ToggleVisibility()
    {
        //isVisible = !isVisible;
        StopAllCoroutines();
        StartCoroutine(AnimateVisibility(1.0f));
    }

    /// <summary>
    /// Visibility toggle animation
    /// Pretty basic animation coroutine, the animation takes 1 second
    /// </summary>
    /// <param name="targetVal">Visibility value to end up with</param>
    /// <returns>Yield</returns>
    private IEnumerator AnimateVisibility(float targetVal)
    {
        float startingTime = Time.time;
        float startingVal = Visibility;
        float lerpVal = 0.0f;
        while (lerpVal < 1.0f)
        {
            lerpVal = (Time.time - startingTime) / 1.0f;
            Visibility = Mathf.Lerp(startingVal, targetVal, lerpVal);

            var updatedCell = BufferElements.Single(b => b.PositionX == transform.position.x && b.PositionY == transform.position.z);
            BufferElements[BufferElements.IndexOf(updatedCell)] = new CellBufferElement
            {
                Visibility = Visibility,
                PositionX = transform.position.x,
                PositionY = transform.position.z
            };
            yield return null;
        }
        Visibility = targetVal;
    }
    #endregion
}
