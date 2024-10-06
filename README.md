# Unity Nullable Enabler

Package to automatically enable nullable reference in Visual Studio for Unity

## How it works
1. Forces the SdkStyleProjectGeneration class written for the Visual Studio Code editor to be enabled (it hooks into its internal code using Harmony 2.2.2 to do this)
2. Enables nullable references to projects with the same assembly name as the one in the csc.rsp file that says "-nullable enabe" so that nullable intellisense can work.

## How to install

1. Install from the git URL in the package manager (*marked packages are required packages and must be installed in order)
   - \* Visual Studio Nullable Enabler : `https://github.com/Rumi727/Unity-Nullable-Enabler.git?path=Packages/com.rumi.visual-studio.nullable`
