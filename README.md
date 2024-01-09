# NiGHTS-Open-Dreams
Open Source NiGHTS Unity Project, cleaned up for ease of navigation.
Comes with Drag-And-Drop Player and Prefabs to build your own levels.
Original Island left as an example to show how to use the scripts.

## Project Information
The Unity Project is made for Editor Version 2022.1.14f1 with the Universal Render Pipeline, and has Cinemachine, TextMeshPro, Shader Graph, Visual Effect Graph, and ProBuilder already installed.
The project also comes with the following packages stored in the [Imported](https://github.com/Ichorix/NiGHTS-Open-Dreams/tree/0d3ee7d95afbddf83fd5990d3e7110ae56489313/NiGHTS%20Open%20Dreams/Assets/Imported) folder:
* [Bézier Path Creator](https://assetstore.unity.com/packages/tools/utilities/b-zier-path-creator-136082) by Sebastian Lague
* [Living Birds](https://assetstore.unity.com/packages/3d/characters/animals/birds/living-birds-15649) by Dinopunch
* [Controller Icon Pack](https://assetstore.unity.com/packages/2d/gui/icons/controller-icon-pack-128505) by NullSave
* [PlayerPrefs Editor](https://assetstore.unity.com/packages/tools/utilities/playerprefs-editor-167903) by BG Tools

Within the Imported folder, assets from various packages have been sorted into appropriate [Foliage](https://github.com/Ichorix/NiGHTS-Open-Dreams/tree/e57788342cd6cfb90495815e7add06aec4baa610/NiGHTS%20Open%20Dreams/Assets/Imported/Prefabs/!Foliage) and [Rocks](https://github.com/Ichorix/NiGHTS-Open-Dreams/tree/e57788342cd6cfb90495815e7add06aec4baa610/NiGHTS%20Open%20Dreams/Assets/Imported/Prefabs/!Rocks) folders.

These packages are
* [Conifers](https://assetstore.unity.com/packages/3d/vegetation/trees/conifers-botd-142076) by Forst, + CTI URP
* [Grass Flowers Pack Free](https://assetstore.unity.com/packages/2d/textures-materials/nature/grass-flowers-pack-free-138810) by ALP, along with some custom modifications
* [NatureStarterKit2](https://assetstore.unity.com/packages/3d/environments/nature-starter-kit-2-52977) by Shapes
* [Rock package](https://assetstore.unity.com/packages/3d/props/exterior/rock-package-118182) by shui861wy
* [Rocks and Boulders 2](https://assetstore.unity.com/packages/3d/props/exterior/rock-and-boulders-2-6947) by Manufactura K4
* [Simple Water Shader URP](https://assetstore.unity.com/packages/2d/textures-materials/water/simple-water-shader-urp-191449) by IgniteCoders

Some texture assets used from [Bumper Engine](https://gamejolt.com/games/BumperEngine19GT/506351) and [Digger - Terrain caves & overhangs](https://assetstore.unity.com/packages/tools/terrain/digger-terrain-caves-overhangs-135178)

Since [Digger - Terrain caves & overhangs](https://assetstore.unity.com/packages/tools/terrain/digger-terrain-caves-overhangs-135178) is a paid package (even though I was using the now unavailable LITE version), I have removed it from the project.
The result is that the edges of the island will look stretched now without its Triplanar terrain shaders.
Some terrain texture assets used came from the package too, but those textures orginated from DeviantArt User [AGF81](https://www.deviantart.com/agf81) and are provided with the Creative Commons Attribution 3.0 License.

Textures used:
* [Stone Texture - 10](https://www.deviantart.com/agf81/art/Stone-Texture-10-204337154)
* [Seamless Texture 5](https://www.deviantart.com/agf81/art/Seamless-Texture-5-160257878) 


## Known Issues
* Errors will display in the console whenever a custom UI Shader is shown. This is because the method that is used to get custom shaders onto Unity UI is entirely unintentional and really isn't supposed to work in the first place but I managed to figure out this workaround. To view these shaders, follow the path [NiGHTS Open Dreams/Assets/UI/Shaders](https://github.com/Ichorix/NiGHTS-Open-Dreams/tree/fdda19e59a698df3ddebd6e28a630cbff19ab6f5/NiGHTS%20Open%20Dreams/Assets/UI/Shaders)
* 
