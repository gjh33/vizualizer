﻿using System;
using UnityEngine;
using UnityEngine.UIElements;

public class VerticalSliderController : UIController
{
    private const string slideContainerId = "slide-container";
    private const string knobId = "knob";
    
    public Action<float> OnValueChanged;
    
    public float Value { get => currentValue; set => SetValue(value); }

    private VisualElement slideContainer;
    private VisualElement knob;

    private float currentValue;
    private bool dragging;
    
    public VerticalSliderController(VisualElement rootElement) : base(rootElement) {}

    protected override void CollectElements()
    {
        base.CollectElements();
        slideContainer = root.Q(slideContainerId);
        knob = root.Q(knobId);
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        knob.RegisterCallback<PointerDownEvent>(OnKnobPointerDown);
        
        // NOTE: While we do capture the pointer, the functionality is broken
        // on touch devices. The events will not fire if the cursor is not over
        // the knob.
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
        Debug.Log("Move");
        if (dragging)
        {
            OnDragMove(evt.deltaPosition);
        }
    }

    private void OnKnobPointerCaptureOut(PointerCaptureOutEvent evt)
    {
        Debug.Log("Capture out");
        if (dragging) 
        {
            EndDrag();
        }
    }

    private void BeginDrag()
    {
        Debug.Log("Begin drag");
        dragging = true;
        knob.CapturePointer(0);
    }
    
    private void EndDrag()
    {
        Debug.Log("End drag");
        dragging = false;
        knob.ReleasePointer(0);
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