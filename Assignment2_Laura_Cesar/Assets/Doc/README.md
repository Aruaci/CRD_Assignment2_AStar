# About The Project
*This project is the second Assignment of the second part of the Engine-based Cross Reality Develodpment Course at UAS St. Pölten.*

**Implemented features:**
- Procedurally generated grid with size from user input
- A* path finding algorithm
- A path following animation using `easeInOutBack` curve with interpolation applied on rotation
- Path, Player, Goal and Obstacle Position Visualizations


## Built With
`Unity 2021.3.21f1`

# Getting Started
## Prerequisites
`Unity 2021.3.21.f1`

## Installation
Open the project folder in the Unity Hub and make sure to have the right Unity version Installed.

# Usage
Once the project is open and has fully loaded, navigate to the sample scene under `Assets/Scenes/SampleScene.unity` and open it.
When the scene has loaded, go into play mode to see the generated grid, path and animation.
On Start, the tile the player is on and tiles that are on the shortest path will be colored blue, the tile the goal is on will be colored green and the tiles the obstacles are on will be red.
Once the delay has passed, the player game object will start moving along the generated shortest path and upon moving from one tile to the other will rotate accordingly with an interpolation of an `easeInOutBack` curve applied.

**Feel free to play around with following factors using the Serialized Fields in the Unity Interface:**

On the `Grid` GameObject:
- `Width`: sets the width of the gid (also needs to be set for the `Grid Generator` Script above)
- `Height`: sets the height of the grid (also needs to be set for the `Grid Generator` Script above)
- `Move Speed`: How fast the player moves along its path
- `Rotation Duration`: How long a rotation should last (in seconds)
- `Move Start Delay`: How long until the moving animation starts (in seconds)

Create More Obstacles:
- If you want to have more obstacles in your scene, assign the Tag `"Obstacle"` to the Game Object

# Contact
Laura Cesar
cc211037@fhstp.ac.at