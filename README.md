Here is the preview of the project:
[![picture](readme-img/picture.png)](http://www.youtube.com/watch?v=lFDNwI_1yHk "Automatic 3D Landscape Builder")


The 3D landscape consists of 5 Main Objects. The implementations of each Main Objects are at the following:

1. Terrain
This a 3D object in which the mesh vertices formed by a heightmap produced by "DiamondSquareTerrain" script.

The heightmap is initialised with vertices for the points in the map, triangles for polygon of the terrain, and uvs for base texture coordinates of the Mesh.
Then, Diamond-square Algorithm is applied where the y-values at each vertices is generated.

After that, average y-values of vertices in the terrain is calculated. If it doesn't reach acceptable level, new heightmap will be generated. This will be done until the average y-values is not too extreme.
Afterwards, Animation Curve is applied on y-values of vertices in the heightmap to generate more realistic landscape.

Then, "PhongTerrainShader" is used to give texture to the terrain in the map depending on their heights.
Finally, the mesh is created with the vertices, triangles, and uvs.

2. CameraObject
This is a 3D object in the form of a sphere in which it is used for moving and rotating its child object, "MainCamera".
In the CameraObject, movement along with the pitch and yaw are controlled by "Camera Object Controller" script meanwhile the collision detection is determined by "Rigid Body Collision" script.

Camera Object Controller controls the position of the object based on input. Inputs are: WASD, QE (For moving along the y axis), Mouse (Move the mouse to rotate the screen)
Rigid Body Collision modify some of the collision physics such as the object moving randomly upon collision

3. SunPivot
This is a 3D object in the form of a sphere in which it is where the Sun is rotating while lighting with Directional Light.
The movement of the sun is determined by "Day Night" script in SunPivot.

An Object is used as a pivot for the sun where it will rotate hence causing the sun to move according to its rotation. Based on this rotation, the directional light is also being rotated hence causing this day/night event. The Day/Night script handles the rotation of directional light based on the rotation of the sun pivot.

4. Boundaries
This is an object made up of its child objects which are 4 planes: Plane (Left), Plane (Right), Plane (Front), Plane (Back).
Each plane has mesh collider to act as limit boundaries for CameraObject so that it does not move outside the bounds of landscape.

5. Water
This is a plane representing water with waves. The texture and wave animation is created by the "WaveShader".
In regards to texture of the water, the "WaveShader" determines the Glossiness, Metallic, and Color of the water.
Besides, the "WaveShader" also determines Amplitude, Speed, and Wavelength of the waves in the water. 

References:
    -> Diamond Square Algorithm - https://www.youtube.com/watch?v=1HV8GbFnCik
    -> Phong Shading for Terrain - https://janhalozan.com/2017/08/12/phong-shader/
    -> Wave Shader - https://catlikecoding.com/unity/tutorials/flow/waves/

