# Unity Developer Code Test

====================================================================

## RUSLAN YEFIMOV's comment:
Optimized calculations of algorithm not changing any logic and increaed FPS by 6 times.
APK release build file can be found by this link:
https://drive.google.com/file/d/10M3hYXwI4gldHWIhEfELhN1fmV_DSeai/view?usp=sharing

====================================================================

This repository is based on

https://github.com/RafaelKuebler/Flocking

The original README from that repository is reproduced after these messages!

## How to complete this test

### Cloning and updating

Please clone this repository. It should run in the latest Unity editor. You will see, when you run it, a large flock of triangular "boids" flying around. These "boids" can react to each other, and create natural-seeming flocking behaviours.

Please then create a feature branch and complete your work in there. At the end, create a pull request to `main` with all your submission code, and we will review this code.

### The application output

We would like an Android build of the submission. We will evaluate the submission in Airplane Mode, so the submission **must** run in Airplane Mode. We will provide you with a Google Drive folder to submit this, or you may use a file sharing service of your choice.

### The main animated scene

Please then create a scene containing two flocks of these boids, one behind the other.

Create two assets for an individual boid. One asset should be for the front flock boids, and one for the back flock boids. The assets should be visually distinct and have a clear "forward" direction, to allow the flocking to be seen. You may choose to animate the individual boids; if you do so, you may choose to coordinate their animation using a modification of the Boid algorithm. If you wish, you may explain your choices with your submission.

The front flock should have boids roughly twice the size of the back flock, and should be visually distinct from them. Please use your discretion to choose how you will size the boids to create a pleasing effect. The two flocks should move independently of each other.

The scene should be configurable so that both the front and back flocks can have variable numbers of boids.

### The start and settings screen

Please create a scene containing a UI that will allow a user to set the number of boids in both the front and the back flock. Sensible defaults should be selected when the app is first run; after that, the numbers should be whatever values were chosen last.

The screen should also have a list containing all the combinations of flock sizes that have been run, and the measured frame rate from the main animated scene for the last run shown. Thus, the list might read

| front flock size | back flock size | frame rate |
| --- | --- | --- |
| 12 | 24 | 29.97 |
| 12 | 36 | 25 |
| 18 | 36 | 24 |

You may choose what should happen when a user runs the same two flock sizes more than once.

You may choose to split this table to a separate screen accessible by a button if you prefer.

### Persistence

We expect the list of frame rates to persist between runs; we expect the default flock sizes to be the last run flock sizes between runs.

# Good luck!

The original README for the boid system follows.

# Unity3D Flocking using Craig Reynolds' Boids


2D implementation of [Craig Reynolds' boids](http://www.cs.toronto.edu/~dt/siggraph97-course/cwr87/) in the Unity3D game engine.

<img alt="150 boids" src="https://user-images.githubusercontent.com/9216979/45744864-d3d7f400-bbff-11e8-9e3e-0bee1d2f5865.gif" width="500">

The boid's emergent flocking behaviour is caused by [3 rules](http://www.red3d.com/cwr/boids/):

* **Alignment**: steer towards the direction nearby boids are heading
* **Cohesion**: steer to move toward the average position of local flockmates
* **Separation**: steer to avoid colliding or getting too close to another boid

## Built With

* [Unity3D](https://unity3d.com/) - Game Engine

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Inspiration: [Processing Flocking implementation](https://processing.org/examples/flocking.html)
