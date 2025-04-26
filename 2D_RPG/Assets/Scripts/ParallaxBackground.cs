using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float parallaxFactorX = 0.1f;  // 水平视差系数
    public float parallaxFactorY = 0.05f; // 垂直视差系数（通常比水平小，创造更自然的效果）

    public bool infiniteHorizontal = true; // 是否水平循环
    public bool infiniteVertical = true;   // 是否垂直循环

    private Transform cameraTransform;
    private float textureUnitSizeX;
    private float textureUnitSizeY;
    private Vector3 startPosition;
    private Vector3 lastCameraPosition;
    private SpriteRenderer spriteRenderer;

    // 存储背景副本的引用
    private GameObject[] backgroundCopies = new GameObject[8]; // 存储最多8个副本（3x3网格减去中心）

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 记录初始位置
        startPosition = transform.position;

        // 计算精灵尺寸（世界单位）
        Sprite sprite = spriteRenderer.sprite;
        textureUnitSizeX = sprite.texture.width / sprite.pixelsPerUnit * transform.localScale.x;
        textureUnitSizeY = sprite.texture.height / sprite.pixelsPerUnit * transform.localScale.y;

        // 设置循环背景
        if (infiniteHorizontal || infiniteVertical)
        {
            SetupRepeatingBackground();
        }
    }

    void SetupRepeatingBackground()
    {
        int index = 0;

        // 创建周围的背景副本（3x3网格，中心是原始对象）
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                // 跳过中心（当前对象）
                if (x == 0 && y == 0) continue;

                // 仅在需要的方向上创建副本
                if (!infiniteHorizontal && x != 0) continue;
                if (!infiniteVertical && y != 0) continue;

                GameObject copy = new GameObject("BackgroundCopy_" + x + "_" + y);
                copy.transform.SetParent(transform.parent);
                copy.transform.position = new Vector3(
                    transform.position.x + textureUnitSizeX * x,
                    transform.position.y + textureUnitSizeY * y,
                    transform.position.z
                );

                SpriteRenderer renderer = copy.AddComponent<SpriteRenderer>();
                renderer.sprite = spriteRenderer.sprite;
                renderer.sortingOrder = spriteRenderer.sortingOrder;
                renderer.sortingLayerID = spriteRenderer.sortingLayerID;

                copy.transform.localScale = transform.localScale;

                // 存储副本引用
                backgroundCopies[index++] = copy;
            }
        }
    }

    void LateUpdate()
    {
        // 计算相机移动
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // 应用视差效果
        transform.position += new Vector3(
            deltaMovement.x * parallaxFactorX,
            deltaMovement.y * parallaxFactorY,
            0);

        lastCameraPosition = cameraTransform.position;

        // 处理无限循环
        HandleInfiniteScrolling();
    }

    void HandleInfiniteScrolling()
    {
        // 只在需要循环的情况下执行
        if (!infiniteHorizontal && !infiniteVertical) return;

        Vector3 cameraRelativePosition = cameraTransform.position;
        float camRelativeX = cameraRelativePosition.x * (1 - parallaxFactorX);
        float camRelativeY = cameraRelativePosition.y * (1 - parallaxFactorY);

        // 创建一个视觉上静止不动的位置（相对于相机）
        Vector3 virtualPosition = new Vector3(camRelativeX, camRelativeY, cameraRelativePosition.z);

        // 计算位置偏移
        float offsetX = 0;
        float offsetY = 0;

        if (infiniteHorizontal)
        {
            offsetX = Mathf.Repeat(virtualPosition.x - startPosition.x, textureUnitSizeX);
        }

        if (infiniteVertical)
        {
            offsetY = Mathf.Repeat(virtualPosition.y - startPosition.y, textureUnitSizeY);
        }

        // 重新定位主背景
        transform.position = new Vector3(
            camRelativeX - offsetX,
            camRelativeY - offsetY,
            transform.position.z
        );

        // 更新所有背景副本
        UpdateBackgroundCopies();
    }

    void UpdateBackgroundCopies()
    {
        int index = 0;

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0) continue;

                if (!infiniteHorizontal && x != 0) continue;
                if (!infiniteVertical && y != 0) continue;

                if (backgroundCopies[index] != null)
                {
                    backgroundCopies[index].transform.position = new Vector3(
                        transform.position.x + textureUnitSizeX * x,
                        transform.position.y + textureUnitSizeY * y,
                        transform.position.z
                    );
                }

                index++;
            }
        }
    }
}