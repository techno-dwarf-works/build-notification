# BuildNotification

[![openupm](https://img.shields.io/npm/v/com.uurha.buildnotification?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.uurha.buildnotification/)

Allows you and your team to receive notification with build info started on you local/remote machine

This plugins works together with Build Notification Application.

## Install

1. Install Newtonsoft
   Json ([Unity](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.0/manual/index.html)
   or [DLL](https://www.newtonsoft.com/json) or [nuget](https://www.nuget.org/packages/Newtonsoft.Json/))

2. Project Settings -> Package Manager -> Scoped registries
   </br>

![image](https://user-images.githubusercontent.com/22265817/197618796-e4f99403-e119-4f35-8320-b233696496d9.png)

```json
"scopedRegistries": [
   {
      "name": "Arcuied Plugins",
      "url": "https://package.openupm.com",
      "scopes": [
      "com.uurha"
      ]
   }
]
```

Window -> PackageManager -> Packages: My Registries -> Arcueid Plugins -> BuildNotification
