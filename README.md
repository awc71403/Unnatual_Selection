# Tile-Based Party - Fall 2020 

DONT PANIC AND MAKE GAMES !!!

# About the Base Project

In this semester, we will be developing a tile-based game. This base project includes a simple template of a 2d tile-based game with basical movement, tiles, and characters made. And we will use this project as a sample to learn how to make games in Unity. At the end of the semster, we will (hopefully) have our own tile-based game done.

# Implementation Summary:
The way the base project functions is all the tiles are placed using the top left corner as the starting tile. The tile placements are read from a .txt file and can be edited easily for different maps. Each tile has a highlighter animation equipped and has a variable to hold any characters on the tile. There are different selection states which are basically a bunch of if/else statements. The movement system is a simple recursive algorithm to find paths and highlights them all. If the tile is highlighted while you have selected a character, then you can move to it. Most of the movement/attack options are based around if the tile is highlighted or not.

## Preparation

Before we actually take a step into the game development, here are something you should do to get ready!

1. Download and install UnityHub from https://unity3d.com/get-unity/download. Be aware that UnityHub is not the **Unity3D GameEngine**, it is a software that helps you download and organize your projects and engines of different versions.
2. Download Unity 2019.1.8f1 (make sure you have correct version number!). We will use this version of the engine to develop our games. There is no restrictions on what modules you should install.
3. Download GitHub desktop from https://desktop.github.com/. We will mainly use GitHub as our version control system. If you do not have a GitHub account, please sign up. If you have troubles dealing with GitHub, you can always discord me anytime.
4. The base project is here https://github.com/awc71403/tilegame_partyproject. Make sure you have forked the project (ATTENTION: Please do FORK instead of CLONE).

## An Intro to Unity:

Check the document here: https://docs.google.com/document/d/1v7z0DdEw7tjqu52R-4adhVVC3oGLB7_nJyqyeO_OSnI/edit?usp=sharing

## Project Structure:

The **Assets** folder under the root directory is where we store assets created for this project, including scripts, sprites, sfx, and etc. You will see different sub-folders inside, with names like **Prefabs**, **Scenes**, **Scripts** and etc. The name of the folder indicates what content should this folder hold. For example, we put all the sfx in the **Audios** folder, and all the scripts into the **Scripts** folder. It will be easy for other people to check your work!
