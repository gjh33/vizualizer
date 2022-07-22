using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Controller for post processing effects at runtime
/// </summary>
public class EffectsController : MonoBehaviour
{
    [Tooltip("The post processing volume to control")]
    [SerializeField] private Volume EffectVolume;
    
    /// <summary>
    /// Enables the bloom module on the controlled volume
    /// </summary>
    public bool Bloom
    {
        get => GetBloom();
        set => SetBloom(value);
    }
    
    /// <summary>
    /// Enables the vignette module on the controlled volume
    /// </summary>
    public bool Vignette
    {
        get => GetVignette();
        set => SetVignette(value);
    }
    
    /// <summary>
    /// Enables the depth of field module on the controlled volume
    /// </summary>
    public bool DepthOfField
    {
        get => GetDepthOfField();
        set => SetDepthOfField(value);
    }
    
    /// <summary>
    /// Enables the chromatic aberration module on the controlled volume
    /// </summary>
    public bool ChromaticAberration
    {
        get => GetChromaticAberration();
        set => SetChromaticAberration(value);
    }
    
    /// <summary>
    /// Enables the film grain module on the controlled volume
    /// </summary>
    public bool FilmGrain
    {
        get => GetFilmGrain();
        set => SetFilmGrain(value);
    }
    
    /// <summary>
    /// Enables the panini projection module on the controlled volume
    /// </summary>
    public bool PaniniProjection
    {
        get => GetPaniniProjection();
        set => SetPaniniProjection(value);
    }

    private void SetBloom(bool on)
    {
        if (EffectVolume.profile.TryGet(out Bloom bloom))
        {
            bloom.active = on;
        }
    }
    
    private bool GetBloom()
    {
        if (EffectVolume.profile.TryGet(out Bloom bloom))
        {
            return bloom.active;
        }
        return false;
    }
    
    private void SetVignette(bool on)
    {
        if (EffectVolume.profile.TryGet(out Vignette vignette))
        {
            vignette.active = on;
        }
    }
    
    private bool GetVignette()
    {
        if (EffectVolume.profile.TryGet(out Vignette vignette))
        {
            return vignette.active;
        }
        return false;
    }
    
    private void SetDepthOfField(bool on)
    {
        if (EffectVolume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = on;
        }
    }
    
    private bool GetDepthOfField()
    {
        if (EffectVolume.profile.TryGet(out DepthOfField depthOfField))
        {
            return depthOfField.active;
        }
        return false;
    }
    
    private void SetChromaticAberration(bool on)
    {
        if (EffectVolume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.active = on;
        }
    }
    
    private bool GetChromaticAberration()
    {
        if (EffectVolume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            return chromaticAberration.active;
        }
        return false;
    }
    
    private void SetFilmGrain(bool on)
    {
        if (EffectVolume.profile.TryGet(out FilmGrain filmGrain))
        {
            filmGrain.active = on;
        }
    }
    
    private bool GetFilmGrain()
    {
        if (EffectVolume.profile.TryGet(out FilmGrain filmGrain))
        {
            return filmGrain.active;
        }
        return false;
    }
    
    private void SetPaniniProjection(bool on)
    {
        if (EffectVolume.profile.TryGet(out PaniniProjection paniniProjection))
        {
            paniniProjection.active = on;
        }
    }
    
    private bool GetPaniniProjection()
    {
        if (EffectVolume.profile.TryGet(out PaniniProjection paniniProjection))
        {
            return paniniProjection.active;
        }
        return false;
    }

    private void Reset()
    {
        EffectVolume = GetComponent<Volume>();
    }
}
