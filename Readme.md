# How to run the code

## Test mode

To see the results of the training in action you need just this code.

Ensure that:

* All brains in the Broadcast Hub in the Academy object are removed
* All the enabled Training Rooms in the Scene object are included in the Academy's Rooms field.
* The brain used is either Player Brain or, in case of one of the learning brains (RobotBrain-...) the model file field is filled with the corresponding model (only PPO brain use RobotBrain-Cont, other brains use RobotBrain-Disc)
* The corresponding brain is selected in the Car's RobotAgent component.


## Train mode

For the train mode you will need to download and install additional Python packages and ensure that you use Tensorflow version 1.7.1. You will find the  installation instructions [here](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Installation.md).
And the how to run the training instructions [here](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Training-ML-Agents.md).