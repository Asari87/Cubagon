using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

public class TileController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float maxScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        maxScale = transform.localScale.x;
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
    }
    public void Show()
    {
        spriteRenderer.enabled = true;
    }
    public void Show(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;
    }
    public async void Scale()
    {
        await ScaleTask();
    }


    private async Task ScaleTask()
    {
        float scale = maxScale / 2;
        while (scale != maxScale)
        {
            scale = Mathf.MoveTowards(scale, maxScale, 5 * Time.deltaTime);
            transform.localScale = Vector3.one * scale;
            await Task.Yield();
        }

    }
}
