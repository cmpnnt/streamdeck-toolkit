using System.Collections.Generic;
using Cmpnnt.StreamDeckToolkit.Attributes;

namespace Cmpnnt.StreamDeckToolkit.Components.Settings;

/// <summary>
/// Allows the possible values of a setting to be populated from an external data source.
/// See the <see href="https://sdpi-components.dev/docs/helpers/data-source">SDPI component docs.</see>
/// </summary>
public class DataSourceSettings
{
    /// <summary>
    /// The name of the optional remote data source. This will become the name of an event sent to the
    /// plugin. The plugin is responsible for responding to that event using the
    /// sendToPropertyInspector using a standardized payload structure.
    /// </summary>
    [SdpiPropertyName("datasource")]
    public string RemoteDataSource { get; set; } = string.Empty;
    
    /// <summary>
    /// When present, sendToPropertyInspector is actively monitored allowing for the plugin to update the items.
    /// </summary>
    [SdpiPropertyName("hot-reload")]
    public bool HotReload { get; set; }

    /// <summary>
    /// When a datasource is specified, this text is shown whilst the items are loaded.
    /// </summary>
    public string Loading { get; set; } = string.Empty;
    
    /// <summary>
    /// The collection of options for the select component.
    /// </summary>
    public List<OptionSetting> Options { get; set; } = [];
    
    /// <summary>
    /// When present, a refresh button is displayed next to the input. Applies only to <see cref="Select"/>.
    /// </summary>
    [SdpiPropertyName("show-refresh")]
    public bool? ShowRefresh { get; set; } = null;
    
    // Example of the standardized payload structure your plugin is responsible for emitting in response to a call
    // for a remote data source. Suppose the RemoteDataSource property here is set to `getColors`. Your plugin would
    // respond with something similar to this. Your plugin might get this information from an API or simply provide
    // the JSON string itself. Example:
    
    /* {
        action,
        event,
        context,
        payload: {
            event: "getColors",
            items: [{
                label: 'Primary Colors',
                children: [{
                    label: 'Red',
                    value: '#ff0000'
                }, {
                    label: 'Green',
                    value: '#00ff00'
                }, {
                    label: 'Blue',
                    value: '#0000ff'
                }]
            }, {
                label: 'Black',
                value: '#000000'
            }, {
                label: 'White',
                value: '#ffffff'
            }]
        }
    } */
}
