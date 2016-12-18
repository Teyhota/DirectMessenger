using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Plugins;
using SDG;
using SDG.Unturned;
using UnityEngine;

public class DirectMessengerConfig : IRocketPluginConfiguration
{
    public static DirectMessengerConfig Instance;

    //public bool Enabled;

    public void LoadDefaults()
    {
        
    }
}