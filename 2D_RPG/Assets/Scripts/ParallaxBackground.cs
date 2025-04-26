using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float parallaxFactorX = 0.1f;  // ˮƽ�Ӳ�ϵ��
    public float parallaxFactorY = 0.05f; // ��ֱ�Ӳ�ϵ����ͨ����ˮƽС���������Ȼ��Ч����

    public bool infiniteHorizontal = true; // �Ƿ�ˮƽѭ��
    public bool infiniteVertical = true;   // �Ƿ�ֱѭ��

    private Transform cameraTransform;
    private float textureUnitSizeX;
    private float textureUnitSizeY;
    private Vector3 startPosition;
    private Vector3 lastCameraPosition;
    private SpriteRenderer spriteRenderer;

    // �洢��������������
    private GameObject[] backgroundCopies = new GameObject[8]; // �洢���8��������3x3�����ȥ���ģ�

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ��¼��ʼλ��
        startPosition = transform.position;

        // ���㾫��ߴ磨���絥λ��
        Sprite sprite = spriteRenderer.sprite;
        textureUnitSizeX = sprite.texture.width / sprite.pixelsPerUnit * transform.localScale.x;
        textureUnitSizeY = sprite.texture.height / sprite.pixelsPerUnit * transform.localScale.y;

        // ����ѭ������
        if (infiniteHorizontal || infiniteVertical)
        {
            SetupRepeatingBackground();
        }
    }

    void SetupRepeatingBackground()
    {
        int index = 0;

        // ������Χ�ı���������3x3����������ԭʼ����
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                // �������ģ���ǰ����
                if (x == 0 && y == 0) continue;

                // ������Ҫ�ķ����ϴ�������
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

                // �洢��������
                backgroundCopies[index++] = copy;
            }
        }
    }

    void LateUpdate()
    {
        // ��������ƶ�
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Ӧ���Ӳ�Ч��
        transform.position += new Vector3(
            deltaMovement.x * parallaxFactorX,
            deltaMovement.y * parallaxFactorY,
            0);

        lastCameraPosition = cameraTransform.position;

        // ��������ѭ��
        HandleInfiniteScrolling();
    }

    void HandleInfiniteScrolling()
    {
        // ֻ����Ҫѭ���������ִ��
        if (!infiniteHorizontal && !infiniteVertical) return;

        Vector3 cameraRelativePosition = cameraTransform.position;
        float camRelativeX = cameraRelativePosition.x * (1 - parallaxFactorX);
        float camRelativeY = cameraRelativePosition.y * (1 - parallaxFactorY);

        // ����һ���Ӿ��Ͼ�ֹ������λ�ã�����������
        Vector3 virtualPosition = new Vector3(camRelativeX, camRelativeY, cameraRelativePosition.z);

        // ����λ��ƫ��
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

        // ���¶�λ������
        transform.position = new Vector3(
            camRelativeX - offsetX,
            camRelativeY - offsetY,
            transform.position.z
        );

        // �������б�������
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