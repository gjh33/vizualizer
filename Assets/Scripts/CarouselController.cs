using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CarouselController : UIController
{
    private const float CarouselSpeed = 0.1f;
    
    private VisualTreeAsset carouselCardTemplate;

    public Action<CarouselItem> ItemSelected;
    
    private Label titleLabel;
    
    private int currentIndex = -1;
    private float targetOffset = 0;
    private float currentVelocity = 0;
    private List<CarouselCardController> cards = new List<CarouselCardController>();
    private bool dragging = false;

    public CarouselController(VisualElement rootElement, VisualTreeAsset cardTemplate) : base(rootElement)
    {
        carouselCardTemplate = cardTemplate;
    }

    public void Update()
    {
        if (!dragging)
        {
            float currentOffset = root.style.translate.value.x.value;
            float newX = Mathf.SmoothDamp(currentOffset, targetOffset, ref currentVelocity, CarouselSpeed);
            root.style.translate = new Translate(newX, 0, 0);
        }
    }

    public void SetItems(IEnumerable<CarouselItem> items)
    {
        root.Clear();
        cards.Clear();
        foreach (CarouselItem item in items)
        {
            TemplateContainer element = carouselCardTemplate.Instantiate();
            CarouselCardController card = new CarouselCardController(element);
            card.SetItem(item);
            root.Add(element);
            cards.Add(card);
            
            card.Clicked += OnCardClicked;
        }

        // Have to delay our index init until styles have been resolved
        root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        // Initialize our index to the center card
        SetIndex(Mathf.CeilToInt(root.childCount / 2f) - 1);
        // Snap immediately
        root.style.translate = new Translate(targetOffset, 0, 0);
        root.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnCardClicked(CarouselCardController controller)
    {
        ItemSelected?.Invoke(controller.Item);
    }

    private void SetIndex(int index)
    {
        currentIndex = index;
        targetOffset = GetXOffsetForIndex(index);
    }

    private void SnapToNearestIndex()
    {
        // NOTE: could probably do this more efficient with wizardry position math
        // and round to int with a division of some kind. But i'm a mere mortal,
        // and given the time constraint, I won't prematurely optimize this.
        float minDistance = float.MaxValue;
        int minIndex = -1;
        for (int i = 0; i < root.childCount; i++)
        {
            float distance = GetXOffsetForIndex(i) - root.style.translate.value.x.value;
            if (Mathf.Abs(distance) < Mathf.Abs(minDistance))
            {
                minDistance = distance;
                minIndex = i;
            }
        }

        // Favor the index in the direction of velocity
        float sign = Mathf.Sign(minDistance);
        float velocitySign = Mathf.Sign(currentVelocity);
        if (sign != velocitySign && currentVelocity != 0)
        {
            minIndex += Mathf.RoundToInt(sign);
            minIndex = Mathf.Clamp(minIndex, 0, root.childCount - 1);
        }
        
        SetIndex(minIndex);
    }
    
    private float GetXOffsetForIndex(int index)
    {
        float cardHalfWidth = root[0].resolvedStyle.width / 2f;
        float leftMostOffset = root.resolvedStyle.width / 2f - cardHalfWidth;
        return leftMostOffset - index * cardHalfWidth * 2f;
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        root.RegisterCallback<PointerDownEvent>(OnPointerDown);
        root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        root.RegisterCallback<PointerOutEvent>(OnPointerOut);
        root.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    private void OnPointerOut(PointerOutEvent evt)
    {
        if (dragging)
        {
            dragging = false;
            SnapToNearestIndex();
        }
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (dragging)
        {
            dragging = false;
            SnapToNearestIndex();
        }
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!dragging) return;
        float newX = root.style.translate.value.x.value;
        newX += evt.deltaPosition.x;
        // So when we let go velocity continues into the damp
        currentVelocity = evt.deltaPosition.x;
        root.style.translate = new Translate(newX, 0, 0);
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        dragging = true;
    }
}