using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyIn : MonoBehaviour
{
    [Range(0, 1)]
    public float transition;

    /// Inside is assumed to be the start position of the RectTransform.
    private Vector2 inside;

    /// Outside is the position
    /// where the rect transform is completely outside of its canvas on the given side.
    private Vector2 outside;

    /// Reference to the rect transform component.
    private RectTransform rectTransform;
    /// Reference to the canvas component this RectTransform is placed on.
    private Canvas canvas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        inside = rectTransform.position;
    }

    void Update()
    {
        CalculateOutside();
        rectTransform.position = Vector2.Lerp(outside, inside, transition);
    }

    void CalculateOutside()
    {
        outside = inside + GetDifferenceToOutside();
    }

    Vector2 GetDifferenceToOutside()
    {
        var pos = inside;
        var size = canvas.scaleFactor * rectTransform.rect.size;
        var pivot = rectTransform.pivot;
        var canvasSize = canvas.pixelRect.size;

        var distanceToRight = canvasSize.x - pos.x;
        return new Vector2(distanceToRight + size.x * (pivot.x), 0f);
    }
}
