![DynaHub Logo](/imgs/DynaHubLogo.png)

# DynaHub
>DynaHub is a Dynamo extension that facilitates the **interaction between Dynamo and GitHub**.

DynaHub is still in its **alpha version**.  
So, for the purposes of managing expectations, please be mindful that this is a proof of concept. Any contribution to the development of this idea will be well received.

## Table of contents
* [An Octocat plays Meccano](#an-octocat-plays-meccano)
* [Talk techy to me](#talk-techy-to-me)
* [Current limitations](#current-limitations)
* [Future features](#future-features)
* [How to install](#how-to-install)

## An Octocat plays Meccano
At Ridley/Willow we recognised the need to store all our Dynamo graphs in a centralised cloud-based safe place. Even better if it was possible to track changes to those graphs, thus avoiding unwanted modification. So, yeah, GitHub sounded like the perfect solution!

Also, going on github, finding your file, downloading it, opening Revit, opening Dynamo, opening the .dyn file, etc. sounded a bit cumbersome and time consuming.

But, fear no more. Dynamo allows extensions now. So we came up with DynaHub, a way to connect to your GitHub account and grab the graphs stored there, but **without leaving Dynamo**!

At the moment, DynaHub enables you to **login** to your GitHub account  
![DynaHub Login](/imgs/Login.gif)

And then you can **browse** what's in your repo and **open** the online-stored graph  
![DynaHub Browse](/imgs/Browse.gif)

## Talk techy to me
* The only functionality implemented thus far is browsing to a simple (*) repo structure, download the _.dyn_ file requested by the user in a _temp_ folder created within Dynamo folders and then open it in Dynamo.

* When the user is done using the graph, the only thing to do is going to be to close Dynamo. When closing Dynamo, the _temp_ folder created by DynaHub will be deleted with all the files inside.

* DynaHub uses **[OctoKit](https://github.com/octokit/octokit.net)**, a NuGet package developed by GitHub for an easier usage within the .NET framework.

## Current limitations
(*) Right now DynaHub is unable to read complex repository structures. At the moment it will pick up all the elements that are at the repo's root level or one level below (folders)
![DynaHub Struc](/imgs/FoldStruct.png)

Nothing under this level will be picked up. For now.

## Future features
* Handle more complex repo structures
* Search bar for quicker lookup of files
* Integrate GitHub Issues (for better communication and and graph requests)
* Forking repos
* Pull requests

We are also happy to hear your suggestions / feature requests!  
Please feel free to shoot an email to atassera@ridleyco.com or log an issue on the repository.

## How to install
There are two ways for installing DynaHub:  
1. Build the solution and copy the folder to _%AppData%Roaming\Dynamo\Dynamo Core\2.0\packages_ to install for the Dynamo Sandbox (stand-alone version) or to _%AppData%Roaming\Dynamo\Dynamo Revit\2.0\packages_ to install for the Revit plugin;

2. Open Dynamo (from Revit or the stand-alone Sandbox), go to the Dynamo Package Manager and look for _DynaHub_  
![DynaHub PM](/imgs/PackageManager.png)