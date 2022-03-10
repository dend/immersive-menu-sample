namespace ContextMenu.Models
{
    [Flags]
	public enum INPUT_MESSAGE_ORIGIN_ID
	{
		/// <summary>The source isn't identified.</summary>
		IMO_UNAVAILABLE = 0x00,

		/// <summary>
		/// The input message is from a hardware device or has been injected into the message queue by an application that has the
		/// UIAccess attribute set to TRUE in its manifest file. For more information about the UIAccess attribute and application
		/// manifests, see UAC References.
		/// </summary>
		IMO_HARDWARE = 0x01,

		/// <summary>
		/// The input message has been injected (through the SendInput function) by an application that doesn't have the UIAccess
		/// attribute set to TRUE in its manifest file.
		/// </summary>
		IMO_INJECTED = 0x02,

		/// <summary>The system has injected the input message.</summary>
		IMO_SYSTEM = 0x04,
	}

}
