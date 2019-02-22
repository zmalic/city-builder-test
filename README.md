# city-builder-test-setup

*Test project for applicants:*

Please, don't spend more than 6 hours on it.
Meant to be open in Unity 2018.3.0f2

Goal:
Implement a simple city builder where you can place and move the building and produce resources from these buildings.

Desired set of features:

The game should have two main modes:
* Regular mode: in which player can select a building by clicking on it and see a building name on top of it and current production progress (or can start a new production if no production running)
* Build mode: the player can place a new building or move an existing building 
when player presses 'build mode' he should see a simple list with building's names and their prices where he can choose a new building to place
or the player can either select and move an existing building.

* Buildings should not be placeable on cells occupied by other buildings
* Placing a building cost resources

*Building types*

2 Types of decoration buildings:
* 'Bench' - a simple bench decoration which player can place.
Placing cost 150 gold and 50 steel
* 'Tree' - a simple decoration which player can place.  
Placing cost 50 gold and 200 wood

3 Types of production buildings:
* 'Residence' - a building which produces automatically 100 gold/coins every 10 seconds. After production is finished, the next one starts automatically. 
Placing a building cost 100 gold
* 'Wood production building' - a building which player first have to select and press 'start production' (just a simple button which appears on top of the building when we select it) and after it should start producing 50 wood in 10 seconds.
Placing a building cost 150 gold
* 'Steel production building' - a building which player first have to select and press 'start production' (just a simple button which appears on top of the building when we select it) and after it should start producing 50 steel in 10 seconds.
Placing a building cost 150 gold and 100 wood

*Additional notes*

* Player should be also able to select the building in regular mode by clicking on it and see a simple progressbar of current production progress. 


Resources: 
* Set of prefabs of buildings with different grid sizes
* Simple grid shader
* Test scene with a grid and a few prefabs placed.
* Some base UI
