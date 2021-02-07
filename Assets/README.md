# WFMagForUnity

WFMagForUnity is a Unity project containing source code of algorithms from games of the past.

## Description

In the wonderful [Wireframe](https://wireframe.raspberrypi.org/) magazine, dedicated to video games and their programming,
there is a column "**Toolbox Source Code**" that describes some of the algorithms used by some famous video games of the past.

The source code, however, is written in Python and uses Pygame Zero. 

So I decided to make a conversion in C# using Unity as engine. 

I hope the idea can be interesting. 

## System Requirements

The project  Unity 2020.1.3f1

## Usage

The Unity project has been structured to contain as many directories as there are issues of the *magazine completed*. 

So you will have, for example, the directoty **NR_001**, **NR_002** and so on. 

Each directory then contains, among other directories, a directory **Scenes** that will contain the scene to load to run the related code.  

So, for example, if you want to see how the source code for issue 1 of the magazine works, start the **Explosion** scene from the **NR_001/Scenes** directory.

But if you want you can also start the **MAIN/Scenes/Main** scene that will display a *visual navigator* of the Wiregrame magazine covers, in order of number.

From here you can click on the desired cover to start the related scene that will show the running code.

## Commands

- **ESC**, return to the Main scene (only if the application starts from Main).
- **P**, pause the code.
- **Q**, exit the application.

## FAQ

- [Is the code optimized?](#Is-the-code-optimized) 
- [How was the code conversion from Python (Pygame) done?](#How-was-the-code-conversion-from-Python-(Pygame)-done)
- [Can I submit bugs?](#Can-I-submit-bugs)
- [Can I request additional features?](#Can-I-request-additional-features)
- [Is there an executable version of the project?](#Is-there-an-executable-version-of-the-project)

## Is the code optimized?

No, the goal was not to write the best or the most optimized code. The goal was just to make an easy conversion of the code in the magazine.

## How was the code conversion from Python (Pygame) done? 

The challenge was to do a conversion of Python constructs to C# constructs. 

An attempt was made to meet and satisfy this challenge wherever possible.  

From Pygame to Unity of course it was different. 

They are two different engines and therefore it was not possible to attempt a direct conversion. 

## Can I submit bugs?

Certainly, use the **Issues** section. 

## Can I request additional features?

Certainly, use the **Issues** section. 

## Is there an executable version of the project?

Yes, you can download it from: https://thp1972.itch.io/wfmagforunity

## Additional informations

The Unity project and all the C# code was created by Pelegrino \~thp\~ Principe.

The images was taken from https://www.kenney.nl.

The code example is licenced under Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported.
https://creativecommons.org/licenses/by-nc-sa/3.0/
