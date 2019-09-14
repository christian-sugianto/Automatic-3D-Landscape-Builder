The 3D landscape consists of 5 Main Objects. The implementations of each Main Objects are at the following:

1. Terrain
This a 3D object in which the mesh vertices formed by a heightmap produced by "DiamondSquareTerrain" script.
The heightmap is initialised with vertices for the points in the map, triangles for polygon of the terrain, and uvs which is base texture coordinates of the Mesh.
Then, Diamond-square Algorithm is applied where the y-values at each vertices is generated.
Lastly, the mesh is created with the vertices, triangles, and uvs.

2. CameraObject
This is a 3D object in the form of a sphere in which it is used for moving and rotationg its child object, "MainCamera".
In the CameraObject, movement along with the pitch and yaw are controlled by "Camera Object Controller" script meanwhile the collision detection is determined by "Rigid Body Collision" script.
* ADD BRIEF IMPLEMENTATION OF CAMERA OBJECT CONTROLLER"
* ADD BRIEF IMPLEMENTATION OF RIGID BODY COLLISION"

3. SunPivot
This is a 3D object in the form of a sphere in which it is where the Sun is rotating while lighting with Directional Light.
The movement of the sun is determined by "Day Night" script in SunPivot.
* ADD BRIEF IMPLEMENTATION OF DAY NIGHT"

4. Boundaries
This is an object made up of its child objects which are 4 planes: Plane (Left), Plane (Right), Plane (Front), Plane (Back).
Each plane has mesh collider to act as limit boundaries for CameraObject, so that it does not move outside the bounds of landscape.

5. Water
This is a 2D object in the form of a plane in which its movement

References:
    -> Diamond Square Algorithm - https://www.youtube.com/watch?v=1HV8GbFnCik
    -> Phong Shading for Terrain - https://janhalozan.com/2017/08/12/phong-shader/

