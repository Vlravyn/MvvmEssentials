# MvvmEssentails.Navigation.WPF
This package contains the implementation of IDialogService and INavigationService whose contracts were defined in the [MvvmEssentials.Core](https://www.nuget.org/packages/MvvmEssentials.Core/) nuget package. Also contains some helper methods.

THIS PACKAGE IS DEPENDANT OF THE [IServiceProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iserviceprovider?view=net-5.0) INTERFACE.

## How to use

Inject the INavigationService and IDialogService into the Host.
```csharp
        IHost _host =
           Host
           .CreateDefaultBuilder()
           .ConfigureAppConfiguration(c =>
           {
               var entryAssembly = Assembly.GetEntryAssembly() ?? throw new Exception("Entry assembly is null. Occured while cofigurating the application host.");

               var assemblyPath = Path.GetDirectoryName(entryAssembly.Location);
               if (string.IsNullOrEmpty(assemblyPath))
                   throw new Exception("Directory path of EntryAssembly is null");

               c.SetBasePath(assemblyPath);
           })
           .ConfigureAppConfiguration(d1 =>
           {
           })
           .ConfigureServices((context, services) =>
           {
               services.AddSingleton<IDialogService, DialogService>();
               services.AddSingleton<INavigationService, NavigationService>();

               services.AddSingleton<ApplicationHostService>();
               ...
           })
           .Build();
```
Set the NavigationName of the Frame in xaml as follows:

```
<Window
    xmlns:navigation="clr-namespace:MvvmEssentials.Navigation.WPF.Navigation;assembly=MvvmEssentials.Navigation.WPF">
    <Grid>
        <Frame navigation:NavigationNamesAP.NavigationName="mainRegion"/>
    </Grid>
</Window>
```
Receive the services into the view model and use them as follows.
IsNavigationContentEnum and IsDialogContentEnum attributes must be set on the enums along with NavigateTo attribute on it's properties for the services to work(if you're using enums)
```charp
    [IsNavigationContentEnum]
    public enum NavigationContent
    {
        [NavigateTo(DestinationType = typeof(Page1))]
        NavigationPage1,
        [NavigateTo(DestinationType = typeof(Page2))]
        NavigationPage2
    }
    [IsDialogContentEnum]
    public enum DialogContent
    {
        [NavigateTo(DestinationType = typeof(DialogPage1))]
        DialogPage1
    }
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;
        public MainWindowViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;
        }

        private void OpenSecondWindow()
        {
            dialogService.Show(typeof(SecondWindow), new Parameters());

            //Open a dialog by creating a Window for it. DO NOT REGISTER THIS WINDOW AS SINGLETON, or else the service will throw an error when the application tries to
            //open the dialog a second time.
            DialogResult result = dialogService.ShowDialog(typeof(SecondWindow), new DialogParameters(), DialogCallback);

            //using enum to open a dialog. A default dialog window will host the content so it must not be a Window. Use a Page or UserControl.
            DialogResult result2 = dialogService.ShowDialog(DialogContent.DialogPage1, new  DialogParameters(), DialogCallback);

            //This is a simple dialog, just specify the parameters and it will return true if the button containing text specified in the button 1 was clicked.
            bool result3 = dialogService.ShowSimpleDialog("Simple Dialog", "Are you sure you want to delete this item?", "Yes", "No");
        }

        private void NavigateToNextPage()
        {
            //navigating to the different page. NavigationNamesAP attached property must be assigned to the Frame and used here.
            navigationService.Navigate("mainRegion", typeof(Page2), new NavigationParameters());

            //Navigating using an enum. IsNavigationContentEnum and NavigateTo attributes must be assigned as shown above.
            navigationService.Navigate("mainRegion", NavigationContent.NavigationPage2, new NavigationParameters());

            //attempts to navigate back to the previous page
            navigationService.NavigateBack("mainRegion");

            //navigates to the next page after the frame has already navigated back.
            navigationService.NavigateForward("mainRegion");
        }

        private void DialogCallback(IDialogParameters? parameters)
        {
        }
    }

```