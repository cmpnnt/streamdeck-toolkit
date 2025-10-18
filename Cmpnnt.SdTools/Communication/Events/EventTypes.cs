namespace Cmpnnt.SdTools.Communication.Events;

/// <summary>
/// List of all supported event types
/// </summary>
internal static class EventTypes
{
    public const string KEY_DOWN = "keyDown";
    public const string KEY_UP = "keyUp";
    public const string WILL_APPEAR = "willAppear";
    public const string WILL_DISAPPEAR = "willDisappear";
    public const string TITLE_PARAMETERS_DID_CHANGE = "titleParametersDidChange";
    public const string DEVICE_DID_CONNECT = "deviceDidConnect";
    public const string DEVICE_DID_DISCONNECT = "deviceDidDisconnect";
    public const string APPLICATION_DID_LAUNCH = "applicationDidLaunch";
    public const string APPLICATION_DID_TERMINATE = "applicationDidTerminate";
    public const string SYSTEM_DID_WAKE_UP = "systemDidWakeUp";
    public const string DID_RECEIVE_SETTINGS = "didReceiveSettings";
    public const string DID_RECEIVE_GLOBAL_SETTINGS = "didReceiveGlobalSettings";
    public const string PROPERTY_INSPECTOR_DID_APPEAR = "propertyInspectorDidAppear";
    public const string PROPERTY_INSPECTOR_DID_DISAPPEAR = "propertyInspectorDidDisappear";
    public const string SEND_TO_PLUGIN = "sendToPlugin";
    public const string DIAL_ROTATE = "dialRotate";
    public const string DIAL_DOWN = "dialDown";
    public const string DIAL_UP = "dialUp";
    public const string TOUCHTAP = "touchTap";
}