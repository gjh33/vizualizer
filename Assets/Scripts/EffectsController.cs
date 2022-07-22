using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectsController : MonoBehaviour
{
    [SerializeField] private Volume EffectVolume;

    public void SetBloom(bool on)
    {
        if (EffectVolume.profile.TryGet(out Bloom bloom))
        {
            bloom.active = on;
        }
    }
    
    public void SetVignette(bool on)
    {
        if (EffectVolume.profile.TryGet(out Vignette vignette))
        {
            vignette.active = on;
        }
    }
    
    public void SetDepthOfField(bool on)
    {
        if (EffectVolume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = on;
        }
    }
    
    public void SetChromaticAberration(bool on)
    {
        if (EffectVolume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.active = on;
        }
    }
    
    public void SetFilmGrain(bool on)
    {
        if (EffectVolume.profile.TryGet(out FilmGrain filmGrain))
        {
            filmGrain.active = on;
        }
    }
    
    public void SetPaniniProjection(bool on)
    {
        if (EffectVolume.profile.TryGet(out PaniniProjection paniniProjection))
        {
            paniniProjection.active = on;
        }
    }

    private void Reset()
    {
        EffectVolume = GetComponent<Volume>();
    }
}
