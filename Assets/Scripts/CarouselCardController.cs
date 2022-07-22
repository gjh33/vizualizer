using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CarouselCardController : UIController
{
    private const string carouselTitleId = "carousel-title";
    private const string carouselCardId = "carousel-card";
    private const float maxPressDuration = 0.15f;
    private const float pressCancelDistance = 10f;

    public CarouselItem Item => item;
    
    public Action<CarouselCardController> Clicked;
    
    private Label carouselTitleLabel;
    private VisualElement card;
    
    private CarouselItem item;
    private float lastPressTime;
    private Vector2 lastPressPosition;
    
    public CarouselCardController(VisualElement rootElement) : base(rootElement) {}

    public void SetItem(CarouselItem item)
    {
        this.item = item;
        card.style.backgroundImage = item.Graphic;
        carouselTitleLabel.text = item.Title;
    }

    protected override void CollectElements()
    {
        base.CollectElements();
        carouselTitleLabel = root.Q<Label>(carouselTitleId);
        card = root.Q(carouselCardId);
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        root.RegisterCallback<PointerDownEvent>(OnPointerDown);
        root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        root.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (Vector2.Distance(evt.position, lastPressPosition) > pressCancelDistance)
        {
            lastPressTime = -1;
        }
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (Time.time - lastPressTime < maxPressDuration)
        {
            Clicked?.Invoke(this);
        }
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        lastPressTime = Time.time;
        lastPressPosition = evt.position;
    }
}