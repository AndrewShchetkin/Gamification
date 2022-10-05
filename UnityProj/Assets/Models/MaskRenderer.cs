using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

/// <summary>
/// Component that controls the compute shader and assigns the necessary variables
/// </summary>
public class MaskRenderer : MonoBehaviour
{
    private void Start()
    {
        GameController.SetReadyObject(this.GetType());
    }
    /// <summary>
    /// Each cell registers itself at startup using this function
    /// I really wouldn't do it like this in a large game project but it is fine for a tutorial
    /// </summary>
    /// <param name="cell">The cell object to add to the list</param>
    public static void RegisterCell(HexCell cell)
    {
        BufferElements.Add(new CellBufferElement 
        {
            PositionX = cell.transform.position.x + XOffset,
            PositionY = cell.transform.position.z + YOffset,
            Visibility = cell.Visibility
        });
    }

    // Не понятно с чем связанный оффсет по координатам, зависит от размера текстуры
    public const int XOffset = -4;
    public const int YOffset = -10;

    private int frames = 1;

    //Properties

    /// <summary>
    /// The compute shader to use for rendering the mask
    /// </summary>
    [SerializeField]
    private ComputeShader computeShader = null;

    /// <summary>
    /// The size the mask should have
    /// Idealy this is a power of two
    /// </summary>
    [Range(64, 4096)]
    [SerializeField]
    private int TextureSize = 1024;

    /// <summary>
    /// The size of the hex grid in actual units
    /// This is used to scale the mask texture so it stretches across the map
    /// </summary>
    [SerializeField]
    private float MapSize = 0;

    /// <summary>
    /// Radius of a grid cell
    /// </summary>
    [SerializeField]
    private float Radius = 1.0f;

    /// <summary>
    /// Blend distance between visible and hidden area
    /// </summary>
    [SerializeField, Range(0.0f, 1.0f)]
    private float BlendDistance = 0.8f;

    private RenderTexture maskTexture;

    //Store thos properties so we can avoid string lookups in Update
    private static readonly int textureSizeId = Shader.PropertyToID("_TextureSize");
    private static readonly int cellCountId = Shader.PropertyToID("_CellCount");
    private static readonly int mapSizeId = Shader.PropertyToID("_MapSize");

    private static readonly int radiusId = Shader.PropertyToID("_Radius");
    private static readonly int blendId = Shader.PropertyToID("_Blend");

    private static readonly int maskTextureId = Shader.PropertyToID("_Mask");

    private static readonly int cellBufferId = Shader.PropertyToID("_CellBuffer");

    //This is the struct we parse to the compute shader for each cell
    public struct CellBufferElement
    {
        public float PositionX;
        public float PositionY;
        public float Visibility;
    }

    public static List<CellBufferElement> BufferElements { get; set; } = new List<CellBufferElement>();

    public static bool IsBufferHaveChanges { get; set; } 

    private static ComputeBuffer buffer = null;

    /// <summary>
    /// Initialization
    /// </summary>
    private void Awake()
    {
        foreach (RenderTextureFormat format in Enum.GetValues(typeof(RenderTextureFormat)))
        {
            var isWork = SystemInfo.SupportsRandomWriteOnRenderTextureFormat(format);
            Debug.Log($"{format} work: {isWork}");
        }
        var formatTexture = SystemInfo.GetCompatibleFormat(GraphicsFormat.A10R10G10B10_XRSRGBPack32, FormatUsage.Linear);
        //Create a new render texture for the mask
        
        maskTexture = new RenderTexture(TextureSize, TextureSize, 0, formatTexture)
        { 
           //enableRandomWrite = true 
        };
        maskTexture.Create();

        //Set the texture dimension and the mask texture in the compute shader
        computeShader.SetInt(textureSizeId, TextureSize);
        computeShader.SetTexture(0, maskTextureId, maskTexture);

        //We are using the mask texture and the map size in multiple materials
        //Setting it as a global variable is easier in this case
        Shader.SetGlobalTexture(maskTextureId, maskTexture);
        Shader.SetGlobalFloat(mapSizeId, MapSize);
    }

    private void OnDestroy()
    {
        buffer?.Dispose();

        if (maskTexture != null)
            DestroyImmediate(maskTexture);
    }

    //Setup all buffers and variables
    public void Update()
    {
        return;
        frames++;
        
        if (buffer == null)
            buffer = new ComputeBuffer(BufferElements.Count * 3, sizeof(float));

        if (frames % 2 != 0)
        {
            frames = 1;
            return;
        }

        if (!IsBufferHaveChanges)
        {
            return;
        } 

        //Set the buffer data and parse it to the compute shader
        buffer.SetData(BufferElements);
        computeShader.SetBuffer(0, cellBufferId, buffer);

        //Set other variables needed in the compute function
        computeShader.SetInt(cellCountId, BufferElements.Count);
        computeShader.SetFloat(radiusId, Radius / MapSize);
        computeShader.SetFloat(blendId, BlendDistance / MapSize * 50);

        //Execute the compute shader
        //Our thread group size is 8x8=64, 
        //thus we have to dispatch (TextureSize / 8) * (TextureSize / 8) thread groups
        computeShader.Dispatch(0, Mathf.CeilToInt(TextureSize / 8.0f), Mathf.CeilToInt(TextureSize / 8.0f), 1);

        IsBufferHaveChanges = false;
        frames = 1;
    }
}
