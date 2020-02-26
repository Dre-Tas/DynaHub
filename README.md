![DynaHub Logo](/imgs/DynaHubLogo.png)

# DynaHub
>DynaHub is a Dynamo extension that facilitates the **interaction between Dynamo and GitHub**.

DynaHub is still in its **alpha version**.  
So, for the purposes of managing expectations, please be mindful that this is a proof of concept. Any contribution to the development of this idea will be well received.

## News: 26/02/2020
>Upgraded security! :alien:   
One of the pain points has been that if companies wanted their employees to login with a **Personal Access Token**, they had to share that token in plain text. With the latest update (2.4.1) you can store the token in **Windows Credential Manager** and DynaHub will automatically look in there. Moreover, you can **encrypt** the token with your own encrypting logic, store the string in Credential Manager and add a dll with the decryption logic. All in a "plug & play" fashion, so (hopefully) super simple!
## News: 16/08/2019
>DynaHub now allows you to structure the folders in your repo in the way you prefer. Differently from the previous release, you can now have a folder structure as complex as you wish. This will be correctly (hopefully) shown in the browser's window!

## Table of contents
* [An Octocat plays Meccano](#an-octocat-plays-meccano)
* [Store and encrypt a PAT](#store-and-encrypt-a-PAT)
* [Future features](#future-features)
* [How to install](#how-to-install)

## An Octocat plays Meccano
At Ridley/Willow we recognised the need to store all our Dynamo graphs in a centralised, cloud-based, safe place. Even better if it was possible to track changes to those graphs, thus avoiding unwanted modification. So, yeah, GitHub sounded like the perfect solution!

Also, going on github, finding your file, downloading it, opening Revit, opening Dynamo, opening the .dyn file, etc. sounded a bit cumbersome and time consuming.

But, fear no more. Dynamo allows extensions now. So we came up with DynaHub, a way to connect to your GitHub account and grab the graphs stored there, but **without leaving Dynamo**!

At the moment, DynaHub enables you to **login** to your GitHub account  
![DynaHub Login](/imgs/Login_updated.gif)

And then you can **browse** what's in your repo and **open** the online-stored graph  
![DynaHub Browse](/imgs/Browse.gif)

Finally, did it ever happen to you too that you make a graph for someone to use and they get back to you saying _"Doesn't work! It's broken!! Everything's red and yellow!"_ and you realise it's just they don't have the right packages?  
Load your zipped packages (they HAVE TO be zipped to work...but this makes it much faster anyway) in a folder called _packages_ to allow everyone to download the right packages with the **Get Packages** feature  
![DynaHub Get Packages](/imgs/GetPackages.gif)

## Store and encrypt a PAT
To secure your GitHub Personal Access Token in the system, without revealing it in plain text, you can store it in Windows Credential Manager.
To do so, open the Credential Manager (type in Windows menu), the click on Windows Credentials and *Add a generic credential* and name it *DynaHub* (in *Internet or network address*)
![Windows Credential Manager](/imgs/CredManager.png)

DynaHub will now, when you try to login with a token, automatically look in the Credential Manager to see if there's something named *DynaHub*. If it exists, it will atomatically populate the token field for you. Just click Login!

The other cool thing is this: if the Credential Manager is not secure enough for you (you can still programmatically access the token if you know how to), you can encrypt the token with your own encrypting logic before storing it. To decrypt the token automatically in DynaHub, create a dll with the DEcrypting logic and store it wherever you want. Let DynaHub know where you have put your dll by creating a config file called *DynaHub_config.ini* and store it here: *C:\ProgramData\DynaHub*. To create the ini you can use the sample in this repo and get rid of the [SAMPLE] prefix. Fill in all the necessary info and you're good to go! :space_invader:  

## Future features
* Search bar for quicker lookup of files
* Pushing new graphs to remote
* Forking repos
* Pull requests
* Integrate GitHub Issues (for better communication and and graph requests)

We are also happy to hear your suggestions / feature requests!  
Please feel free to shoot an email to andrea.tassera7@gmail.com or log an issue on the repository.

## How to install
There are two ways for installing DynaHub:  
1. Build the solution and copy the folder to _%AppData%Roaming\Dynamo\Dynamo Core\2.0\packages_ to install for the Dynamo Sandbox (stand-alone version) or to _%AppData%Roaming\Dynamo\Dynamo Revit\2.0\packages_ to install for the Revit plugin;

2. Open Dynamo (from Revit or the stand-alone Sandbox), go to the Dynamo Package Manager and look for _DynaHub_

3. Download the latest version!

![DynaHub PM](/imgs/PackageManager.png)