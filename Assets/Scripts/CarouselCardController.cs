using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CarouselCardController : UIController
{
    private const string carouselTitleId = "carousel-title";
    private const float maxPressDuration = 0.15f;

    public CarouselItem Item => item;
    
    public Action<CarouselCardController> Clicked;
    
    private Label carouselTitleLabel;
    
    private CarouselItem item;
    private float lastPressTime;
    
    public CarouselCardController(VisualElement rootElement) : base(rootElement) {}

    public void SetItem(CarouselItem item)
    {
        this.item = item;
        root.style.backgroundImage = item.Graphic;
        carouselTitleLabel.text = item.Title;
    }

    protected override void CollectElements()
    {
        base.CollectElements();
        carouselTitleLabel = root.Q<Label>(carouselTitleId);
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
        lastPressTime = -1;
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
    }
}