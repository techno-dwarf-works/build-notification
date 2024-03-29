﻿using System;

namespace Better.BuildNotification.Runtime.MessageData
{
    /// <summary>
    ///   <para>Target build platform.</para>
    /// </summary>
    public enum BuildPlatform
    {
        NoTarget = -2, // 0xFFFFFFFE
        [Obsolete("BlackBerry has been removed in 5.4")] BB10 = -1, // 0xFFFFFFFF
        /// <summary>
        ///   <para>Build a macOS standalone (Intel 64-bit).</para>
        /// </summary>
        StandaloneOSX = 2,
        /// <summary>
        ///   <para>Build a macOS Intel 32-bit standalone. (This build target is deprecated)</para>
        /// </summary>
        [Obsolete("StandaloneOSXIntel has been removed in 2017.3")] StandaloneOSXIntel = 4,
        /// <summary>
        ///   <para>Build a Windows standalone.</para>
        /// </summary>
        StandaloneWindows = 5,
        /// <summary>
        ///   <para>Build an iOS player.</para>
        /// </summary>
        iOS = 9,
        [Obsolete("PS3 has been removed in >=5.5")] PS3 = 10, // 0x0000000A
        [Obsolete("XBOX360 has been removed in 5.5")] XBOX360 = 11, // 0x0000000B
        /// <summary>
        ///   <para>Build an Android .apk standalone app.</para>
        /// </summary>
        Android = 13, // 0x0000000D
        /// <summary>
        ///   <para>Build a Linux standalone.</para>
        /// </summary>
        [Obsolete("StandaloneLinux has been removed in 2019.2")] StandaloneLinux = 17, // 0x00000011
        /// <summary>
        ///   <para>Build a Windows 64-bit standalone.</para>
        /// </summary>
        StandaloneWindows64 = 19, // 0x00000013
        /// <summary>
        ///   <para>WebGL.</para>
        /// </summary>
        WebGL = 20, // 0x00000014
        /// <summary>
        ///   <para>Build an Windows Store Apps player.</para>
        /// </summary>
        WSAPlayer = 21, // 0x00000015
        /// <summary>
        ///   <para>Build a Linux 64-bit standalone.</para>
        /// </summary>
        StandaloneLinux64 = 24, // 0x00000018
        /// <summary>
        ///   <para>Build a Linux universal standalone.</para>
        /// </summary>
        [Obsolete("StandaloneLinuxUniversal has been removed in 2019.2")] StandaloneLinuxUniversal = 25, // 0x00000019
        [Obsolete("Use WSAPlayer with Windows Phone 8.1 selected")] WP8Player = 26, // 0x0000001A
        /// <summary>
        ///   <para>Build a macOS Intel 64-bit standalone. (This build target is deprecated)</para>
        /// </summary>
        [Obsolete("StandaloneOSXIntel64 has been removed in 2017.3")] StandaloneOSXIntel64 = 27, // 0x0000001B
        [Obsolete("BlackBerry has been removed in 5.4")] BlackBerry = 28, // 0x0000001C
        [Obsolete("Tizen has been removed in 2017.3")] Tizen = 29, // 0x0000001D
        [Obsolete("PSP2 is no longer supported as of Unity 2018.3")] PSP2 = 30, // 0x0000001E
        /// <summary>
        ///   <para>Build a PS4 Standalone.</para>
        /// </summary>
        PS4 = 31, // 0x0000001F
        [Obsolete("PSM has been removed in >= 5.3")] PSM = 32, // 0x00000020
        /// <summary>
        ///   <para>Build a Xbox One Standalone.</para>
        /// </summary>
        XboxOne = 33, // 0x00000021
        [Obsolete("SamsungTV has been removed in 2017.3")] SamsungTV = 34, // 0x00000022
        /// <summary>
        ///   <para>Build to Nintendo 3DS platform.</para>
        /// </summary>
        [Obsolete("Nintendo 3DS support is unavailable since 2018.1")] N3DS = 35, // 0x00000023
        [Obsolete("Wii U support was removed in 2018.1")] WiiU = 36, // 0x00000024
        /// <summary>
        ///   <para>Build to Apple's tvOS platform.</para>
        /// </summary>
        tvOS = 37, // 0x00000025
        /// <summary>
        ///   <para>Build a Nintendo Switch player.</para>
        /// </summary>
        Switch = 38, // 0x00000026
        Lumin = 39, // 0x00000027
        /// <summary>
        ///   <para>Build a Stadia standalone.</para>
        /// </summary>
        Stadia = 40, // 0x00000028
        /// <summary>
        ///   <para>Build a CloudRendering standalone.</para>
        /// </summary>
        CloudRendering = 41, // 0x00000029
        [Obsolete("GameCoreScarlett is deprecated, please use GameCoreXboxSeries (UnityUpgradable) -> GameCoreXboxSeries", false)] GameCoreScarlett = 42, // 0x0000002A
        GameCoreXboxSeries = 42, // 0x0000002A
        GameCoreXboxOne = 43, // 0x0000002B
        /// <summary>
        ///   <para>Build to PlayStation 5 platform.</para>
        /// </summary>
        PS5 = 44, // 0x0000002C
        EmbeddedLinux = 45, // 0x0000002D
    }
}