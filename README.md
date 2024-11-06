# Unity Music-Driven Reactive Visualizer

A dynamic, music-driven visualizer built in Unity, where lights, shapes, and shaders respond to real-time audio analysis. This project combines 
envelope following and beat detection techniques to extract audio characteristics to provide an immersive visual experience to music of choice.

![Game Mode Showcase of Project](Showcase1.gif)

---

## Table of Contents
-  [Overview](#overview)
- [Features](#features)
- [Project Structure](#project-structure)
  - [Scripts](#scripts)
  - [Materials](#materials)
  - [Shaders](#shaders)
  - [Prefabs](#prefabs)
- [How To Use](#how-to-use)
- [Song Credit](#song-credit)
- [License](#license)

---

## Overview
This Unity-based audiovisual experience immerses the viewer in a dynamic, music-responsive environment. At the core of the scene is a collection of reflective orbs with custom metallic textures, floating and morphing in response to the music. These orbs emit light, synchronized to the beat, creating a powerful visual effect. Surrounding the orbs, vibrant lights of varying colors move in harmony with the music, adding layers of depth and energy. The entire scene is encapsulated within a pulsating spherical background that reacts to audio frequencies, creating a unified, immersive visual spectacle. Each element of the scene—from the metallic orbs to the surrounding lights—responds in real-time, transforming sound into a vivid, interactive visual experience.


## Features

- **Real-Time Audio Analysis**: Analyze audio to produce amplitude and frequency data for controlling visual elements.
- **Dynamic Light and Shape Reactions**: Synchronized lights and shapes that adapt to musical characteristics.
- **Custom Shader Effects**: Shaders that adjust color, brightness, and surface texture based on audio data.
- **Easy Setup and Customization**: Pre-built prefabs and materials enable quick scene configuration and adjustments.

---

## Project Structure


### Scripts

1. **AudioAnalyser.cs**
   - Analyzes audio source data and outputs overall intensity of the sound and detected beats to the visualisation parts.

2. **LightController.cs**
   - Manages the behavior and intensity of lights, synchronizing them with audio data.
    
3. **LightMovement.cs**
   - Controls the smooth movement and animations of lights, syncing them with audio peaks for added visual dynamism.

4. **LightOrb.cs**
   - Controls orb-specific properties such as glow and intensity, using audio data to drive visual reactions.

5. **MorphOrb.cs**
   - Morphs the shape of orbs in response to audio data, with parameters to adjust speed and extent of morphing.

6. **OrbManager.cs**
   - Centralizes control of orb behaviors, ensuring consistent synchronization across the scene.

7. **RemoveSkybox.cs**
   - Provides a utility function to remove or toggle the skybox, allowing for enhanced focus on the main visuals.

8. **SphereController.cs**
   - Manages the background sphere, adjusting size, movement, and other transformations in sync with audio data.

### Materials

- **BackgroundSphereMaterial.mat**
  - Material for the background sphere, enhancing visual aesthetics and setting a cohesive scene backdrop.

- **ORB_MAT.mat**
  - Material applied to the responsive orbs, supporting shader effects for real-time adjustments based on audio data.

### Shaders

- **SphereShader.shader**
  - Custom shader used to create dynamic color changes and surface pulsation on spheres.
  - Allows color shifts and intensity adjustments driven by audio frequency analysis.

### Prefabs

- **Orb.prefab**
  - Pre-configured orb with `LightOrb`, `MorphOrb`, and custom material for quick scene integration.
  - Easily customizable through Unity’s Inspector panel.

- **Point_Light.prefab**
  - Audio-reactive light prefab, designed to adapt color and brightness in response to music.

---

## How to Use This Project

1. **Clone or download the repository** from GitHub.
2. **Open the project in Unity** (compatible with Unity version 2020.3+).
3. **Add your audio source**:
   - Drag a song of choice into the MUSIC folder.
   - Click on the AudioSource in Hierarchy view.
   - Replace the AudioClip in Inspector view with your song.
4. **Run on a VR headset**:
   - This project was configured to run on Quest 2, reconfiguration might be needed for other headsets.
   - If using Quest 2, connect the device (in Developer mode) via cable to your computer
   - Go to File tab -> Build & Run
   - Run project on headset

![Scene Mode Showcase of Project](Showcase2.gif)

## Song Credit
Reference song used for project construction is Rise - Screamarts (BIG UP!)


## License

This project is available under the [MIT License](LICENSE), making it free to use and modify.

