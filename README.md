# Virtual Worlds


# Resources


## Literature



*   Braitenberg Vehicles ([link](https://www.usna.edu/Users/cs/crabbe/SI475/current/vehicles.pdf))
*   The Book of Shaders ([link](https://thebookofshaders.com/))
*   Evolving Virtual Creatures - Karl Sims ([link](https://www.karlsims.com/papers/siggraph94.pdf))
*   Evolving Virtual Creatures Demo ([link](https://www.karlsims.com/evolved-virtual-creatures.html))
*   Richard Feynman and The Connection Machine ([link](https://longnow.org/essays/richard-feynman-connection-machine/))
*   Signed Distance Fields ([link](https://jasmcole.com/2019/10/03/signed-distance-fields/))
*   Compute Shader - Catlike Coding ([link](https://catlikecoding.com/unity/tutorials/basics/compute-shaders/))


## Videos



*   Ant and Slime Simulation - Sebastian Lague ([link](https://www.youtube.com/watch?v=X-iSQQgOd1A))
*   Procedural Moons and Planets - Sebastian Lague ([link](https://www.youtube.com/watch?v=lctXaT9pxA0))
*   Natural Selection - Primer ([link](https://www.youtube.com/watch?v=0ZGbIKd0XrM&t=428s))
*   Procedural Mesh Generation (Series) - Sebastian Lague ([link](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3))
*   Mesh using Compute Shaders -Ned Makes Games ([link](https://www.youtube.com/watch?v=AiWCPiGr10o&list=LL&index=2))

Channels



*   Sebastian Lague ([link](https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ))
*   Primer ([link](https://www.youtube.com/channel/UCKzJFdi57J53Vr_BkTfN3uQ))


# 


# Environment



## Variable Characteristics



*   scope
    *   local
    *   regional
    *   global
*   appearance
    *   visibility
    *   weight
    *   color
    *   texture
    *   consistence
*   movement
    *   static
    *   kinematic
    *   dynamic


## Types of Environment Variables


<table>
  <tr>
   <td>Type
   </td>
   <td>Scope
   </td>
   <td>Movement
   </td>
  </tr>
  <tr>
   <td>objects
   </td>
   <td>local
   </td>
   <td>static
   </td>
  </tr>
  <tr>
   <td>creatures
   </td>
   <td>local
   </td>
   <td>dynamic
   </td>
  </tr>
  <tr>
   <td>time
   </td>
   <td>global
   </td>
   <td>kinematic
   </td>
  </tr>
  <tr>
   <td>land
   </td>
   <td>local/ regional/ global
   </td>
   <td>static
   </td>
  </tr>
  <tr>
   <td>atmosphere
   </td>
   <td>regional/ global
   </td>
   <td>dynamic
   </td>
  </tr>
</table>



## 


## Sensable Environment Parameters

Geographics



*   position
*   height
*   steepness

Weather



*   light
*   humidity
*   temperature
*   wind

Regional/ Local Characteristics



*   color
*   smell
*   texture
*   consistence
*   consistency over time


## Environmental Objects



*   object drops
*   vehicle drops


# 


# Braitenberg Vehicles


## Vehicle #1: Getting Around

Base Components



*   point sensor
*   sensor-to-motor connection (activation function)
*   coherent noise


## Vehicle #2: Fear and Connection

Added Components



*   a second point sensor
*   sensor-to-motor connection
    *   direct connection (fear)
    *   cross connection (aggression)


# 


# The Vehicle


## Characteristics

Life Stats



*   health
*   age

Appearance



*   size
*   color
*   voice
*   smell

Mental Skills



*   memory
*   will power
*   investigation of the environment
*   creativity
*   sensitivity
    *   to environment
    *   to other vehicles

Physical Skills



*   speed
*   strength
*   agility
*   endurance
*   perception

Social Skills



*   selfishness
*   intimidation
*   persuasion
*   charisma


## 


## Abilities

Movement



*   translate
*   rotate
*   jump

Interaction



*   use
*   create
*   hold
*   destroy
*   give
*   communicate
*   hunt/ escape
*   hurt/ get hurt

Sense



*   see
*   hear
*   feel
*   taste
*   smell

Evolve



*   remember/ forget
*   learn/ improve skill
*   adapt/ copy behavior
*   change appearance
*   be born/ die
*   heal/ get hurt
*   age


## 


## Vehicle Creation

Factors



*   randomness
*   inheritance
*   environmental effects

Instantiation



*   random
*   by genetics
    *   one existing vehicle
    *   multiple existing vehicles
*   by source

Properties



*   random
*   type
*   genetics


# 


# Tasks



* :ok_hand: create environment

    * :ok_hand: create coherent noise in the environment
    * :ok_hand: add color
    * :ok_hand: fix auto update
    * :ok_hand: tessellate mesh
* :ok_hand: create creature
    * :ok_hand: create model
    * :ok_hand: adjust movement
    * :ok_hand: add point sensor to creature
    * :ok_hand: connect point sensor to wheel
    * :ok_hand: create another sensor
    * :ok_hand: connect the sensor to another wheel
    * :ok_hand: make creature speed dependent on slope (gradient)
* :ok_hand: populate the environment with objects
* :point_up: make the creatures interact with objects
* :point_up: make the creatures perceive objects
* :point_up: make the creatures alter the environment
* :point_up: make these changes perceptible to other creatures
* :point_up: add randomness to the environment
* :point_up: add randomness to the creature
* :point_up: create environment using a compute shader
* :point_up: implement physarum using a compute shader