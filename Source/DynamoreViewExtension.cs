using System.Windows;
using System.Windows.Controls;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System.IO;
using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.Graph.Workspaces;
using Dynamore.DynamoreStatistics;
using Dynamo.Controls;

namespace Dynamore
{
  /// <summary>
  /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
  /// </summary>
  public class DynamoreViewExtension : IViewExtension
  {
    // Make sure to generate a new guid for your tool
    // e.g. here: https://www.guidgenerator.com
    public string UniqueId => "ee496cc1-9e3a-40b1-aa41-4ecf0b4a04bb";
    public string Name => "Dynamore";

    private MenuItem extensionMenu;
    private ViewLoadedParams viewLoadedParams;
    private DynamoViewModel dynamoViewModel => viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;

    private ReadyParams _readyParams;
    private string _dynamoVersion;


    #region FUNCTIONS_STATISTICS
    public void CurrentWorkspaceModel_NodeChanged(NodeModel obj)
    {
      try
      {
        var workSpaceName = Path.GetFileName(_readyParams.CurrentWorkspaceModel.FileName);

        var nodeUsageCollector = new StatisticsCollector();
        nodeUsageCollector.Start(
          "Dynamo-Dynamore",
          obj.Name,
          obj.Category,
          obj.Category,
          obj.Description,
          "",
          "",
          _dynamoVersion,
          workSpaceName,
          _readyParams.CurrentWorkspaceModel.FileName
          );
        nodeUsageCollector.End();
      }
      catch
      {
        // ignored
      }
    }

    public void CurrentWorkspaceChanged(IWorkspaceModel iWorkspaceModel)
    {
      iWorkspaceModel.NodeAdded += CurrentWorkspaceModel_NodeChanged;
    }
    #endregion FUNCTIONS_STATISTICS


    #region STARTUP
    /// <summary>
    /// Method that is called when Dynamo starts, but is not yet ready to be used.
    /// </summary>
    /// <param name="vsp">Parameters that provide references to Dynamo settings, version and extension manager.</param>
    public void Startup(ViewStartupParams vsp)
    {
      var version = vsp.DynamoVersion;
      _dynamoVersion = version.Major + "." + version.Minor + "." + version.Build;
    }
    #endregion STARTUP

    #region LOADED
    /// <summary>
    /// Method that is called when Dynamo has finished loading and the UI is ready to be interacted with.
    /// </summary>
    /// <param name="vlp">
    /// Parameters that provide references to Dynamo commands, settings, events and
    /// Dynamo UI items like menus or the background preview. This object is supplied by Dynamo itself.
    /// </param>
    public void Loaded(ViewLoadedParams vlp)
    {
      // Hold a reference to the Dynamo params to be used later
      viewLoadedParams = vlp;
      _readyParams = vlp;

      _readyParams.CurrentWorkspaceChanged += CurrentWorkspaceChanged;

      // If this is not here if I create a new model in beggining events dont trigger
      CurrentWorkspaceChanged(_readyParams.CurrentWorkspaceModel);

      // Add custom menu items to Dynamo's UI
      MakeMenuItems();
    }
    #endregion LOADED

    #region MAKE_MENU_ITEMS
    /// <summary>
    /// Adds custom menu items to the Dynamo menu.
    /// </summary>
    public void MakeMenuItems()
    {
      // Create a completely top-level new menu item
      extensionMenu = new MenuItem { Header = "Dynamore" };

      // Create a new sub-menu item for our tool
      var dynamoreMenuItem = new MenuItem { Header = "Dynamore" };
      // Add a tool tip to our menu item
      dynamoreMenuItem.ToolTip = new ToolTip { Content = "Hjelpetekst" };
      // Define what happens when our sub-menu item is clicked
      dynamoreMenuItem.Click += (sender, args) =>
      {
        MessageBox.Show("Det er en start, men vi har mye jobb igjen....");
      };
      // Add our sub-menu item to our top-level menu item
      extensionMenu.Items.Add(dynamoreMenuItem);

      // Add our top-level menu to the Dynamo menu
      viewLoadedParams.dynamoMenu.Items.Add(extensionMenu);
    }
    #endregion MAKE_MENU_ITEMS

    #region SHUTDOWN
    /// <summary>
    /// Method that is called when the host Dynamo application is closed.
    /// </summary>
    public void Shutdown()
    {

    }
    #endregion SHUTDOWN

    #region DISPOSE
    /// <summary>
    /// Method that is called for freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      _readyParams.CurrentWorkspaceChanged -= CurrentWorkspaceChanged;

      foreach (var workspaceModel in _readyParams.WorkspaceModels)
      {
        workspaceModel.NodeAdded -= CurrentWorkspaceModel_NodeChanged;
      }
    }
    #endregion DISPOSE
  }
}
