using UnityEngine.UIElements;

/// <summary>
/// Base class for UIControllers for runtime UI
/// </summary>
public class UIController
{
    protected VisualElement root;
    
    public UIController(VisualElement rootElement)
    {
        root = rootElement;
        CollectElements();
        RegisterCallbacks();
    }
    
    protected virtual void CollectElements() {}
    protected virtual void RegisterCallbacks() {}
}