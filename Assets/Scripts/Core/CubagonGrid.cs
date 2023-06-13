using UnityEngine;

/// <summary>
/// Initialize grid on Awake a set the camera orthographic size 
/// </summary>
public class CubagonGrid : MonoBehaviour
{
    public static CubagonGrid Instance;


    [Header("Grid Settings")]
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public float cellSize;
    [SerializeField] private Transform origin;

    [Header("Board Graphics")]
    [SerializeField] private SpriteRenderer boardSprite;

    public GenericGrid<TileSO> grid;
    private DeviceOrientation currentOrientation;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        //Create grid starting at Vector3.zero
        if (boardSprite == null)
        {

            grid = A<TileSO>.Grid
                .WithSize(width, height)
                .WithCellSize(cellSize)
                .Build();
            return;
        }

        //Align sprite at the center
        transform.position = boardSprite.transform.position - boardSprite.bounds.size / 2;
        //Create grid at origin position
        cellSize = boardSprite.bounds.size.x / width;
        grid = A<TileSO>.Grid
                .WithSize(width, height)
                .WithCellSize(cellSize)
                .WithOrigin(origin.position)
                .Build();

        //Match camera orthographic size to grid size with margin
        SetCameraOrthoSize(Input.deviceOrientation);
        
    }
    private void Update()
    {
        if (Input.deviceOrientation != currentOrientation)
            SetCameraOrthoSize(Input.deviceOrientation);
    }

    private void SetCameraOrthoSize(DeviceOrientation deviceOrientation)
    {
        currentOrientation = deviceOrientation;
        switch (currentOrientation)
        {
            case DeviceOrientation.Portrait:
            case DeviceOrientation.PortraitUpsideDown:
                Camera.main.orthographicSize = boardSprite.bounds.size.x *1.5f;
                break;
            case DeviceOrientation.LandscapeLeft:
            case DeviceOrientation.LandscapeRight:
                Camera.main.orthographicSize = boardSprite.bounds.size.x / 1.5f;
                break;

            case DeviceOrientation.Unknown:
                if(Screen.width > Screen.height)
                    Camera.main.orthographicSize = boardSprite.bounds.size.x / 1.5f;
                else
                    Camera.main.orthographicSize = boardSprite.bounds.size.x * 1.5f;
                break;


        }
    }
}


