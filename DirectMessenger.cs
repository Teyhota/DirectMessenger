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

namespace PM
{
	public class Plugin : RocketPlugin<DirectMessengerConfig>
	{
		//globals
		public Dictionary<Steamworks.CSteamID, Steamworks.CSteamID> setPlayers = new Dictionary<Steamworks.CSteamID, Steamworks.CSteamID>();
		public Dictionary<Steamworks.CSteamID, Steamworks.CSteamID> lastReceived = new Dictionary<Steamworks.CSteamID, Steamworks.CSteamID>();

		public static Plugin Instance;

		protected override void Load()
		{
            Instance = this;
            Rocket.Core.Logging.Logger.Log("Loading plugin...", ConsoleColor.Cyan);
            Rocket.Core.Logging.Logger.LogWarning("Plugin by: Teyhota");
            Rocket.Core.Logging.Logger.LogWarning("Plugin Version: 1.0.0.0");
            Rocket.Core.Logging.Logger.LogWarning("Recommended Unturned Version: 3.17.9.1");
            Rocket.Core.Logging.Logger.Log("...DirectMessenger has been loaded!", ConsoleColor.Cyan);
            Rocket.Core.Logging.Logger.LogWarning("Use /dm <player> <message> to send a DM to a player!");
            Rocket.Core.Logging.Logger.LogWarning("Use /qm set <player> to set a player as a QM!");
            Rocket.Core.Logging.Logger.LogWarning("Use /qm <message> to send a QM to the set player!");
            Rocket.Core.Logging.Logger.LogWarning("Use /r <message> to reply to you most recent DM!");
            Rocket.Core.Logging.Logger.Log("Got errors? Please report them on my website!", ConsoleColor.Red);
            Rocket.Core.Logging.Logger.Log("Got suggestions? Let me know on my website!", ConsoleColor.Magenta);
            Rocket.Core.Logging.Logger.LogWarning("My Website >> Rocket.TeyhotasDedicated.tk");
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
			U.Events.OnPlayerDisconnected += Events_OnPlayerDisconnected;
		}
		protected override void Unload()
		{
			Rocket.Core.Logging.Logger.Log("Unloading plugin...", ConsoleColor.Red);
            Rocket.Core.Logging.Logger.Log("...DirectMessenger has been unloaded!", ConsoleColor.Red);

            U.Events.OnPlayerConnected -= Events_OnPlayerConnected;
			U.Events.OnPlayerDisconnected -= Events_OnPlayerDisconnected;
			base.Unload ();
		}
		private void Events_OnPlayerConnected(UnturnedPlayer player)
		{
			lastReceived.Add(player.CSteamID, player.CSteamID);
		}
		private void Events_OnPlayerDisconnected(UnturnedPlayer player)
		{
			lastReceived.Remove(player.CSteamID);
		}
	}
}