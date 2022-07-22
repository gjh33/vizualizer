using System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Controller for vertical slider UI
/// </summary>
public class VerticalSliderController : UIController
{
    private const string SlideContainerId = "slide-container";
    private const string KnobId = "knob";
    
    /// <summary>
    /// Fired when the value of the slider changes
    /// </summary>
    public Action<float> OnValueChanged;
    
    /// <summary>
    /// The value of the slider between 0 and 1
    /// </summary>
    public float Value { get => currentValue; set => SetValue(value); }

    private VisualElement slideContainer;
    // what did you call me?
    private VisualElement knob;

    private float currentValue;
    private bool dragging;
    
    public VerticalSliderController(VisualElement rootElement) : base(rootElement) {}

    protected override void CollectElements()
    {
        base.CollectElements();
        slideContainer = root.Q(SlideContainerId);
        knob = root.Q(KnobId);
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        knob.RegisterCallback<PointerDownEvent>(OnKnobPointerDown);
        
        // NOTE: The capture functionality is broken
        // on touch devices. The events will not fire if the cursor is not over
        // the knob. So instead we use this approach of registering to the panel
        knob.panel.visualTree.RegisterCallback<PointerUpEvent>(OnKnobPointerUp);
        knob.panel.visualTree.RegisterCallback<PointerMoveEvent>(OnKnobPointerMove);
        knob.RegisterCallback<PointerCaptureOutEvent>(OnKnobPointerCaptureOut);
        slideContainer.RegisterCallback<GeometryChangedEvent>(OnSlideContainerGeometryChanged);
    }

    private void OnSlideContainerGeometryChanged(GeometryChangedEvent evt)
    {
        // Refresh visual on geometry change
        SetValue(currentValue);
    }

    private void OnKnobPointerDown(PointerDownEvent evt)
    {
        if (!dragging)
        {
            BeginDrag();
        }
    }

    private void OnKnobPointerUp(PointerUpEvent evt)
    {
        if (dragging)
        {
            EndDrag();
        }
    }

    private void OnKnobPointerMove(PointerMoveEvent evt)
    {
        if (dragging)
        {
            OnDragMove(evt.deltaPosition);
        }
    }

    private void OnKnobPointerCaptureOut(PointerCaptureOutEvent evt)
    {
        if (dragging) 
        {
            EndDrag();
        }
    }

    private void BeginDrag()
    {
        dragging = true;
    }
    
    private void EndDrag()
    {
        dragging = false;
    }
    
    private void OnDragMove(Vector2 delta)
    {
        float knobMaxY = slideContainer.layout.height - knob.layout.height;
        float knobMinY = 0;
        float knobBottom = knob.style.bottom.value.value - delta.y;
        knobBottom = Mathf.Clamp(knobBottom, knobMinY, knobMaxY);
        float percent = (knobBottom - knobMinY) / (knobMaxY - knobMinY);
        SetValue(percent);
    }

    private void SetValue(float value)
    {
        value = Mathf.Clamp01(value);
        currentValue = value;
        float trackHeight = slideContainer.layout.height;
        float knobHeight = knob.layout.height;
        float maxY = trackHeight - knobHeight;
        float y = maxY * value;
        knob.style.bottom = y;
        OnValueChanged?.Invoke(value);
    }
}