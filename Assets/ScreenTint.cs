using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTint : MonoBehaviour
{
    [SerializeField] Color tintedColor;
    [SerializeField] Color untintedColor;
    public float tintSpeed = 0.5f;
    float f; // variable for tinting process

    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Tint()
    {
        // avoid couroutine conflicts
        StopAllCoroutines();

        f = 0f;
        StartCoroutine(TintScreen());
    }

    public void UnTint()
    {
        // avoid couroutine conflicts
        StopAllCoroutines();

        f = 0f;
        StartCoroutine(UnTintScreen());
    }

    private IEnumerator TintScreen()
    {
        while (f < 1f)
        {
            f += Time.deltaTime * tintSpeed;
            f = Mathf.Clamp(f, 0f, 1f);

            Color c = image.color;
            c = Color.Lerp(untintedColor, tintedColor, f);
            image.color = c;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator UnTintScreen()
    {
        while (f < 1f)
        {
            f += Time.deltaTime * tintSpeed;
            f = Mathf.Clamp(f, 0f, 1f);

            Color c = image.color;
            c = Color.Lerp(tintedColor, untintedColor, f);
            image.color = c;

            yield return new WaitForEndOfFrame();
        }
    }
}
