using System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Controller for a carousel card displaying a CarouselItem
/// </summary>
public class CarouselCardController : UIController
{
    private const string CarouselTitleId = "carousel-title";
    private const string CarouselCardId = "carousel-card";
    private const float MAXPressDuration = 0.15f;
    private const float PressCancelDistance = 10f;

    /// <summary>
    /// The item currently displayed by the card
    /// </summary>
    public CarouselItem Item => item;
    
    /// <summary>
    /// Fired when the user clicks on the card. Passes itself as an argument for easy callback re-use
    /// </summary>
    public Action<CarouselCardController> Clicked;
    
    private Label carouselTitleLabel;
    private VisualElement card;
    
    private CarouselItem item;
    private float lastPressTime;
    private Vector2 lastPressPosition;
    
    public CarouselCardController(VisualElement rootElement) : base(rootElement) {}

    /// <summary>
    /// Set the item to be displayed by the card
    /// </summary>
    /// <param name="item">CarouselItem to be displayed</param>
    public void SetItem(CarouselItem item)
    {
        this.item = item;
        card.style.backgroundImage = item.Graphic;
        carouselTitleLabel.text = item.Title;
    }

    protected override void CollectElements()
    {
        base.CollectElements();
        carouselTitleLabel = root.Q<Label>(CarouselTitleId);
        card = root.Q(CarouselCardId);
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
        if (Vector2.Distance(evt.position, lastPressPosition) > PressCancelDistance)
        {
            lastPressTime = -1;
        }
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (Time.time - lastPressTime < MAXPressDuration)
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