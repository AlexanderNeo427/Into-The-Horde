# Into the Horde 

### Description: 
Left 4 Dead 2 (L4D2) FPS zombie shooter clone game made in Unity over a 3-month period for my Individual Work Project (IWP) during my final year in Nanyang Polytechnic (NYP). Gameplay can be found [here](https://youtu.be/wWmGYGnthO8?si=VtW3qnG8m-6szfup)

### Technical Details/Gameplay Features
* Hand-rolled templated C# StateMachine to handle zombie behaviour, which is also used in this project to interface with Unity's Animation Controller
* Globally-accessible EventManager Singleton _(inheriting from a Templated Abstract Singleton)_. To de-couple code in the name of maintainability.
* Hitbox system
* Waypoint System to guide the player on where to go
* Used Unity's in-built Navigation Mesh for pathfinding
