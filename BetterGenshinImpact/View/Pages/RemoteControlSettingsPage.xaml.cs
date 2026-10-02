using BetterGenshinImpact.ViewModel.Pages;
using System.Windows.Controls;

namespace BetterGenshinImpact.View.Pages;

public partial class RemoteControlSettingsPage : Page
{
    public RemoteControlSettingsPageViewModel ViewModel { get; }

    public RemoteControlSettingsPage(RemoteControlSettingsPageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }
}
