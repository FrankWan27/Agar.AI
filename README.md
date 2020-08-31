
# Agar.AI
Uses the [NEAT algorithm](https://en.wikipedia.org/wiki/Neuroevolution_of_augmenting_topologies) to teach an artifical intelligence how to play the popular game [Agar.io](https://agar.io/)
## Example
![Example Agario2](https://github.com/FrankWan27/Agar.AI/blob/master/Images/agario2.gif?raw=true)
![Example Agario](https://github.com/FrankWan27/FrankWan27.github.io/blob/master/images/agar.gif?raw=true)
## Table of Contents
- [Overview](#overview)
- [Installation](#installation)
- [Controls](#controls)
- [Neural Network Structure](#neural-network-structure)
- [Evolution](#evolution)
- [Authors](#authors)

---
## Overview

### Game Rules
- Each player/AI controls one cell
- Larger cells eat smaller cells to absorb their mass
- A cell must be 25% larger than another cell to eat it
- Cells can also eat food particles to increase their mass by one
- As cells get larger, they have a larger field of view, but move slower
- A cell can eject half of their mass in an attempt to eat smaller cells at a range.

---
## Usage

Download the standalone executable here: [https://github.com/FrankWan27/Agar.AI/releases/tag/v1.0](https://github.com/FrankWan27/Agar.AI/releases/tag/v1.0)

Or try the WebGL version here (Loading genomes not supported): [https://frankwan27.github.io/agar.ai/](https://frankwan27.github.io/agar.ai/)

---

## Controls

![Instructions](https://github.com/FrankWan27/Agar.AI/blob/master/Images/agarinstruc.png?raw=true)


## Neural Network Structure

This neural network starts as 10x2, with one input layer and one output layer. Each connection is initialized with a random weight between -1 and +1. (Red and Green)

![Neural Net](https://github.com/FrankWan27/Agar.AI/blob/master/Images/NNet.png?raw=true)

### Inputs:
1. Angle to the closest player
2. Distance to the closest player
3. Size difference to the closest player (larger or smaller)
4. Angle to the second closest player
5. Distance to the second closest player
6. Size difference to the second closest player (larger or smaller)
7. Angle to the closest food
8. Distance to the closest food
9. Angle to the second closest food
10. Distance to the second closest food

### Outputs:
1. The angle player wants to move in
2. Forward movement speed (from 0 to 1)

## Evolution

Each neural network uses the [NEAT algorithm](https://en.wikipedia.org/wiki/Neuroevolution_of_augmenting_topologies) to evolve and improve. As each neural network evolves, it has a chance to mutate new hidden nodes and new connections. New nodes are randomly given one of the following activation functions: ReLU, ELU, LeLU, Tanh, Sigmoid, None, and Multiplication. For multiplication, the incoming nodes are multiplied together rather than added. 

Example of an evolved neural network:

![Neural Net](https://github.com/FrankWan27/Agar.AI/blob/master/Images/NNet2.png?raw=true)

---


## Authors

* **Frank Wan** - [Github](https://github.com/FrankWan27)
