# Vizualizer
Vizualizer is a 3D mesh visualizer for PC, Mac, Linux, iOS, and Android. It was made using the Unity3D Engine

# Usage
![image](https://user-images.githubusercontent.com/7050512/180479486-490ae5d5-3985-4c07-9fc7-9fa056ce6de3.png)

1. The top toolbar allows you to choose your mesh, material, and texture. You can mix and match meshes, materials, and textures freely. Selecting any of these will open a carousel style **picker**. In the picker simply scroll left and right to choose your desired asset.
2. These are your primary editing controls. In order from top to bottom the controls are
    1. Translation
    2. Rotation
    3. Scale
    4. Lighting
    5. Effects
3. These are your fine tuning controls. These change depending on which primary editing control you currently have selected.
    - Translation: Choose the plane you wish to translate along
    - Rotation: Choose the axis around which you wish to rotate
    - Scale: Choose the axis you wish to scale. U for uniform scaling.
    - Lighting: From left to right the sliders represent
        1. Light angle
        2. Light azimuth (orbit around object)
        3. Light intensity
        4. Light temperature
    - Effects: From top to bottom the toggles represent
        1. Bloom
        2. Vignette
        3. Depth of Field
        4. Chromatic Aberration
        5. Film Grain
        6. Panini Projection

# Development
First you must install Unity 2021.3.0 LTS. If building for mobile be sure to include the relevent modules for build support. UI is built using UI Toolkit. The primary entry point is in the VisualizerMain scene. Everything starts and ends with the VisualizerInterface script on PrimaryInterface game object.

# Acknowledgements

[Unity Logo 3D Model - Yanez Designs](https://sketchfab.com/3d-models/unity-logo-a9299dd053cb46a0b2dfcffd378f1088)
[Dwarven Expedition Pack - Tobyfredson](https://assetstore.unity.com/packages/3d/environments/dungeons/dwarven-expedition-pack-154571)
