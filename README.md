# City Builder test

### Architecture

The game architecture is composed of GameManager and another four components:

* Building component
* Map component
* Resource component
* UI component

**GameManager**

`GameManager` is a singletone. It takes care of other game components.
In the game manager we can set up starting amount of resources and the building types.
From this component, we start the initialization of other game components.

**Building component**

This component is managed by a `BuildingManager` - takes care of buildings creation.
Other classes from this components are `Building`, `BuildingEventHandler` and `BuildingUI`
 * `Building` is main object of certain building
 * `BuildingEventHandler` is a set of methods for building selection and movement 
 * `BuildingUI` is a simply class for managing building world space UI

**Map component**

This component is managed by a `MapManager`.
Here we can to define the map size in edit mode, so we can use map from 8x8 to 20x20 fields.
Also, there are set of functions for adding and removing buildings on the map (grid)

In `Grid` class we keep the data on field occupancy in two dimensional bool array.
`Grid` has methods for manipulation of these data.

**Resource component**

This component is managed by a `ResourcesManager`.
The use of all resources is managed through this component.

**UI component**

This component is managed by a `UIManager` - manager for screen space canvas UI elements

`BuildingBuyButton` is class for dynamically added buttons for buying a building. It is also part of the UI component.


### Description

I split the app on the components so that, for example, the building does not depend on the position to which it will be placed.

For collision between buildings, I used a 2D bool array instead of using built-in coliders, so it is quickly possible to check whether the required part of the terrain is free or not.

When the game is initialized, I use array of building prefabs from `GameManager` 
to create buttons in UI Build mode panel.
On this way we could just to add new prefab into GameManager's inspector 
and game will dynamically add buttons in the menu.

Creating of new type of building is pretty easy from existing prefabs.
We just need to set up few parameters and a new model.

In the edit mode, there is very easy to create different map sizes over
unity inspector panel.

Although not mentioned, I made the possibility of removing the building by 
positioning out of the map (the building must be completely out of bounds of the map).

### What would I do differently if I had more time?

* I would simplify the methods for positioning in the `MapManager`.
* I would not remove buildings (`building.Remove();`) from the MapManager class - this class is only for setting buildings on the map. I think it brakes the architecture.
* Displaying building details, I would completely switch to `BuildingUI` instead of `Building`.
* I would make the basic building class, and from it I would inherit the resource-building class, decoration-building etc.
* Some common logic from `Building` i would switch to `BuildingManager`.
* I would use the namespace for the components.
