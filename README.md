# Spasticity Assessment
Spasticity Assessment Device for Children with Cerebral Palsy

This is the README for the *REHYB - Spasticity Assessment Device* project, developed by the Human Robotics Group at Imperial College London, Bioengineering Department. 

## 01 - SpasticityCoach1 folder
The *SpasticityCoach1* folder contains the Unity project with the demo scenes, assets and C# scripts:

![Unity EMG Demo](https://github.com/REHYB/SpasticityCoach/blob/master/UnityDemo.PNG)

**Main scenes:** UMA-Sitting-EMG.unity

**Main scripts:**
- **ClientMotion.cs**: controls the patient avatar.
- **ClientRoutine_Sitting.cs**: Defines the demo steps; including text and instructor avatar motion.
- **SaveRoutine.cs**: Contains the function called at the end of the routine that saves the EMG data into CSV scripts.
- **ModelColour.cs**: Contains the function to modify the colour of the instructor avatar.
  
### 01.A - Folder Assets/UI:
Modified library to plot data in real-time. This project uses mainly the LineChart. You can read some more comments on the LineChart controllers [here](https://docs.google.com/document/d/1VrCxR2o3_ZQFfntAAjsJKQfnsYi8PoBsLPYp7bbAatE/edit?usp=sharing):
- **/Awesome Charts (folder)**:
  - **/Core/Scripts (folder)**:
    - **Default.cs**: Change the default size, thickness of lines, x-axis range, etc for the charts
    - **DataSet.cs**: Defines the List<T> entries for the charts
  - **/Core/Examples/LineChart folder**:
    Contains the main scripts controlling each individual chart. Currently, all the EMG chart controllers are available in this folder.

- **/ProgressBars (folder)**:
  - **ProgressBar.cs**: Allow for the creation of both linear and radial progress bars. Prefabs were created as resources inside the Unity Editor. You can access them by opening the GameObject windows using right-click and choosing "UI>Linear Progress Bar" or "UI>Radial Progress Bar"

- **/TextMesh Pro (folder)**: Allows for the creation of text boxes in the scene.



### 01.B - Folder Assets/Thalmic Myo:
Contains main script for interfacing with the Thalmic Myo (/Myo) and storing EMG data (/MyoEMG).
- **/Myo (folder)**: Contains the main scripts interfacing with the Thalmic Myo.
  - **/Myo/Scripts (folder)**: 
    - **ThalmicHub.cs**: Main file allowing access to one or more Myo bands. For every Myo band, there should be a children script ThalmicMyo.cs
    - **ThalmicMyo.cs**: Children from ThalmicHub.cs; retrieves IMU and EMG information from the Myo band
    - **StoreEMG.cs**: Stores real-time EMG data as a list, retrievable by other scripts.
  - **/Myo/Scripts/Myo.NET (folder)**: 
    - **EventTypes.cs**: Defines the Events retrieving the IMU, EMG, etc data from the Myo
    - **Myo.cs**: Main file defining what is shared by the Thalmic Myo in the Unity interface.

- **/MyoEMG (folder)**:
  Contains real-time EMG moving average filtering script and functions to save EMG data to a CSV file.
  - **CsvReadWrite.cs**: contains functions to save EMG lists from the StoreEMG.cs file into CSVs. CSV files are store in Assets/MyoEMG/CSV
  - **DataFltr**: contains the real-time moving average filter function. Default window size is 10.
  - **/CSV (folder)**: this folder contains the CSVs for the raw EMG and processed EMG (after using the moving average filter).



### 01.C - Other Folders in /Assets:
Other folders of interest in /Assets are:
- **/3D Models**:
  - 3D models for static GameObjects in the scene, such as *ChairPack*, *WorkshopModels*.
  - 3D models for dynamic GameObject in the scene, such as *HumanMale2*.
  - ***Dynamically-generated GameObjects**, such as the elastic exercise band in ***ExerciseBand***.

- **/Scenes**: Scenes that are not currently used but might be useful at some point.



## 02 - TinyTile folder
Contains BLE advertising script for the TinyTile.

