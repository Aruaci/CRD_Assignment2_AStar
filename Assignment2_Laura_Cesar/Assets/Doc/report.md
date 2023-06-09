# Report EbCRD Part 2 Assignment 2

## Requirements
- [x] **PART 1:** A* path finding algorithm.
- [x] **PART 2:** A path following animation using easeInOutBack curve. The interpolation should be applied on rotation.
- [x] **PART 3**: The project should be a VR project
- [x] **PART 4**: Additional design to make the scene looks nice
- [x] **PART 5**: Documentation

## Part 1: A*
To calculate the shortest path I had to do some steps before:

**Grid Creation:**
First the grid gets procedurally generated based on the width and height variables by instantiating a `GridCell` Prefab: 
```csharp
Instantiate(_gridCell, transform);
Vector3 position = new Vector3(x, 0, y);
cell.transform.position = position;
```
**Object Detection & Material Assignment:**
To detect if there is an object on a tile I perform a raycast in the up direction of each tile. If there is a hit, I compare it with various tags to see which kind of object it is and set a boolean flag accordingly. Example for obstacle:
```csharp 
if (hitObject.CompareTag("Player"))
   {
      _hasPlayer = true;
      return;
    }
```
Using the boolean flags, I assign the correct Materials:
```csharp
if (_hasPlayer || _hasPath)
   {
      meshRenderer.material = playerMaterial;
   }
```

**Node Class:**
To not having to work with whole game objects while computing the algorithm, I created a Node class with the intention that each `GridCell` is a Node.

**Implementing A Star:**
I used lists to store the open, closed and neighbouring nodes.
I did not implement any optimizations like heaps.

## Part 2: Movement & Rotation

**Movement:**
I store the transforms of the Nodes of the path and move the player along these positions.

**Rotation**
On rotation to the next path tile I rotate the player object using the formula of the link provided in the assignment slides.

## Part 3: VR
I have installed and imported the required packages.

## Part 4: Design
I have added low poly nature assets from the unity asset store to create a nicer looking environment. ([Link to Resource](https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-simple-nature-pack-162153))
In addition I used an asset also from the unity asset store to represent the object moving along the curve. Additionally I implemented conditions in the animation controller to reflect the idle, turning or walking states. ([Link to Resource](https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-simple-nature-pack-162153))