﻿// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>

using Clipboard.ViewModels;
using Clipboard.ViewModels.SettingsPanels;
using Clipboard.Views;
using Clipboard.Views.SettingsPanels;

namespace Clipboard.ComponentModel.Messages
{
	internal static class MessageIdentifiers
	{
		#region Properties

			/// <summary>
			/// Gets the LinkCloudStorageServicePopupOpened identifier
			/// </summary>
			private static MessageIdentifier _linkcloudstorageservicepopupopened;
			internal static MessageIdentifier LinkCloudStorageServicePopupOpened { get { if (_linkcloudstorageservicepopupopened == null) _linkcloudstorageservicepopupopened = new MessageIdentifier(typeof(SettingsSynchronizationUserControlViewModel), typeof(SettingsSynchronizationUserControl), "InitializeThePopupView"); return _linkcloudstorageservicepopupopened; } }

			/// <summary>
			/// Gets the UnlinkCloudStorageServicePopupOpened identifier
			/// </summary>
			private static MessageIdentifier _unlinkcloudstorageservicepopupopened;
			internal static MessageIdentifier UnlinkCloudStorageServicePopupOpened { get { if (_unlinkcloudstorageservicepopupopened == null) _unlinkcloudstorageservicepopupopened = new MessageIdentifier(typeof(SettingsSynchronizationUserControlViewModel), typeof(SettingsSynchronizationUserControl), "InitializeThePopupView"); return _unlinkcloudstorageservicepopupopened; } }

			/// <summary>
			/// Gets the ShowNotifyIconBalloon identifier
			/// </summary>
			private static MessageIdentifier _shownotifyiconballoon;
			internal static MessageIdentifier ShowNotifyIconBalloon { get { if (_shownotifyiconballoon == null) _shownotifyiconballoon = new MessageIdentifier(typeof(MainWindowViewModel), typeof(MainWindow), "DisplayABalloon"); return _shownotifyiconballoon; } }

			/// <summary>
			/// Gets the ChangeHotKeyPopupOpened identifier
			/// </summary>
			private static MessageIdentifier _changehotkeypopupopened;
			internal static MessageIdentifier ChangeHotKeyPopupOpened { get { if (_changehotkeypopupopened == null) _changehotkeypopupopened = new MessageIdentifier(typeof(SettingsGeneralUserControlViewModel), typeof(SettingsGeneralUserControl), "InitializeThePopupView"); return _changehotkeypopupopened; } }

			/// <summary>
			/// Gets the RestoreDefaultSettingsPopupOpened identifier
			/// </summary>
			private static MessageIdentifier _restoredefaultsettingspopupopened;
			internal static MessageIdentifier RestoreDefaultSettingsPopupOpened { get { if (_restoredefaultsettingspopupopened == null) _restoredefaultsettingspopupopened = new MessageIdentifier(typeof(SettingsGeneralUserControlViewModel), typeof(SettingsGeneralUserControl), "InitializeThePopupView"); return _restoredefaultsettingspopupopened; } }

			/// <summary>
			/// Gets the RaisePropertyChangedOnAllSettingsUserControl identifier
			/// </summary>
			private static MessageIdentifier _raisepropertychangedonallsettingsusercontrol;
			internal static MessageIdentifier RaisePropertyChangedOnAllSettingsUserControl { get { if (_raisepropertychangedonallsettingsusercontrol == null) _raisepropertychangedonallsettingsusercontrol = new MessageIdentifier(typeof(SettingsGeneralUserControlViewModel), typeof(SettingsGeneralUserControlViewModel), "RestoreDefaultSettings"); return _raisepropertychangedonallsettingsusercontrol; } }

			/// <summary>
			/// Gets the HidePasteBarWindow identifier
			/// </summary>
			private static MessageIdentifier _hidepastebarwindow;
			internal static MessageIdentifier HidePasteBarWindow { get { if (_hidepastebarwindow == null) _hidepastebarwindow = new MessageIdentifier(typeof(PasteBarWindowViewModel), typeof(PasteBarWindow), "MouseMovesAway"); return _hidepastebarwindow; } }

			/// <summary>
			/// Gets the PasteData identifier
			/// </summary>
			private static MessageIdentifier _pastedata;
			internal static MessageIdentifier PasteData { get { if (_pastedata == null) _pastedata = new MessageIdentifier(typeof(PasteBarWindowViewModel), typeof(MainWindowViewModel), "ADataMustBePaste"); return _pastedata; } }

			/// <summary>
			/// Gets the ShowPasteBarWindow identifier
			/// </summary>
			private static MessageIdentifier _showpastebarwindow;
			internal static MessageIdentifier ShowPasteBarWindow { get { if (_showpastebarwindow == null) _showpastebarwindow = new MessageIdentifier(typeof(ClipboardManager), typeof(MainWindowViewModel), "ShowDataEntries"); return _showpastebarwindow; } }

		#endregion
	}
}
