![License](https://img.shields.io/badge/License-MIT-green)

# KnobsterVoice Suite
Do you want to use your Knobster in DCS World or FS2020/FS2024, or any game/program you want?
Create your VoiceAttack profiles and use them to control your Knobster from VoiceAttack (by voice, joystick buttons, etc).

## How it works ⚙️
You have to create Voice Attack profiles with KnobsterVoiceEncoder application. Then assign your game key bindings to your Knobster (push/relase, inner knob and outer knob). Before starting your sim, you must start the KnobsterVoice application and... that's all!

You can expand your profile in VoiceAttack to assign categories, modify voice commands, assign joystick buttons, etc.

## Installation
[.Net Framework 4.8](https://dotnet.microsoft.com/es-es/download/dotnet-framework/thank-you/net48-web-installer) required

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Make
Open solution in Visual Studio (2019/2022). You will find 3 projects:
- KnobsterVoiceEncoder: WinForms application
- KnobsterVoice: C++ console application

## Deploy
To deploy applications (SolutionDir is your solution root folder):
- KnobsterVoiceEncoder: build normal release in (2019/2022) and look at SolutionDir\KnobsterVoiceEncoder\bin\Release and take: KnobsterVoice.exe
- KnobsterVoice: build normal release in VisualStudio (2019/2022) and look at SolutionDir\Win32\Relase and take: KnobsterVoice.exe and libknobster.dll

## Todo 📋
Integrate suite into one application (maybe wrapper for KnobsterLib?)
Reference manual
AutoInstallation (vsix?)

## Languages used
KnobsterVoiceEncoder: WinForms C#.Net
KnobsterVoice: C++

## Greetings
Thanks to Corjan (SimmInnovations) for the Knobster lib

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file, but remember, you can do Paypal [DONATIONS](https://www.paypal.com/paypalme/xmasmusicsoft) 🎁
