// ***********************************************************************
// Author   : ElektroStudios
// Modified : 13-April-2019
// ***********************************************************************

#region  Usings 

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

using log4net;

using Zeta.Bot;
using Zeta.Common;
using Zeta.Common.Plugins;

#endregion

#region  Plugin 

namespace LogDisabler {

    /// ----------------------------------------------------------------------------------------------------
    /// <summary>
    /// Plugin.
    /// </remarks>
    /// ----------------------------------------------------------------------------------------------------
    public sealed class Plugin : IPlugin {

#region  Private Fields 

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// The logger instance.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        private readonly ILog log = Logger.GetLoggerInstanceForType();

#endregion

#region  Properties 

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        public string Name {
            get {
                return "Log Disabler";
            }
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        public string Description {
            get {
                return "Disables the creation of log files.";
            }
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        public Version Version {
            get {
                return new Version(1, 0);
            }
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the plugin author.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        public string Author {
            get {
                return "ElektroStudios";
            }
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the user-interface configuration window.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        public Window DisplayWindow {
            get {
                return null;
            }
        }

#endregion

#region  Event Invocators 

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Called when this plugin is initialized.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        void IPlugin.OnInitialize() {
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Called when the bot sends a "pulse".
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        void IPlugin.OnPulse() {
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Called when this plugin is enabled.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        void IPlugin.OnEnabled() {
            this.log.Info("[Log Disabler] Plugin enabled.");

            string assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string logDirPath = Path.Combine(assemblyPath, "Logs");

            DirectoryInfo logDir = new DirectoryInfo(logDirPath);
            if (!logDir.Exists) {
                try {
                    logDir.Create();
                } catch (Exception ex) {
                    this.log.Error("[Log Disabler] Can't create 'Logs' directory: " + ex);
                }
            }

            WindowsIdentity id = WindowsIdentity.GetCurrent();
            FileSystemAccessRule rule = new FileSystemAccessRule(id.User, FileSystemRights.WriteData, AccessControlType.Deny);

            DirectorySecurity acl = new DirectorySecurity();
            acl.AddAccessRule(rule);
            try {
                logDir.SetAccessControl(acl);
            } catch (Exception ex) {
                this.log.Error("[Log Disabler] Can't set ACL for 'Logs' directory: " + ex);
            }

            // Delete any log file.
            foreach (FileInfo logFile in logDir.EnumerateFiles("*.txt")) {
                try {
                    logFile.Delete();
                } catch (Exception ex) {
                }
            }
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Called when this plugin is disabled.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        void IPlugin.OnDisabled() {
            this.log.Info("[Log Disabler] Plugin disabled.");

            string assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string logDirPath = Path.Combine(assemblyPath, "Logs");

            DirectoryInfo logDir = new DirectoryInfo(logDirPath);
            if (!logDir.Exists) {
                try {
                    logDir.Create();
                } catch (Exception ex) {
                    this.log.Error("[Log Disabler] Can't create 'Logs' directory: " + ex);
                }
            }

            WindowsIdentity id = WindowsIdentity.GetCurrent();
            FileSystemAccessRule rule = new FileSystemAccessRule(id.User, FileSystemRights.WriteData, AccessControlType.Allow);

            DirectorySecurity acl = new DirectorySecurity();
            acl.AddAccessRule(rule);
            try {
                logDir.SetAccessControl(acl);
            } catch (Exception ex) {
                this.log.Error("[Log Disabler] Can't set ACL for 'Logs' directory: " + ex);
            }
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Called when the bot is shutting down.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        void IPlugin.OnShutdown() {
        }

#endregion

#region  Public Methods 

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// Indicates whether or not the current <see cref="IPlugin"/> is equal to another <see cref="IPlugin"/>.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        /// <param name="other">
        /// An <see cref="IPlugin"/> to compare with this <see cref="IPlugin"/>.
        /// </param>
        /// ----------------------------------------------------------------------------------------------------
        /// <returns>
        /// <see langword="true" /> if the current <see cref="IPlugin"/> is equal to the <paramref name="other" /> parameter; 
        /// otherwise, <see langword="false" />.
        /// </returns>
        /// ----------------------------------------------------------------------------------------------------
        public bool Equals(IPlugin other) {
            string thisName = (this.Name + this.Author);
            string otherName = (other.Name + other.Author);
            return otherName.Equals(thisName, StringComparison.InvariantCultureIgnoreCase);
        }

#endregion

    }

}

#endregion
