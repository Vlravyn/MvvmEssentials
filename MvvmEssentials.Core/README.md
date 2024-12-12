# MvvmEssentials.Core
This package contains the following:
ObservableObject : Implementation of INotifyPropertyChanged and INotifyPropertyChanging.
RelayCommand, RelayCommand(T) : Synchronous implementation of ICommand.
RelayCommandAsync, RelayCommandAsync(T) : Asynchronous implementation of ICommand.
RelayCommandBaseAsync : Base of RelayCommandAsync which can be used for custom implementation.

Contains interfaces for implementing IDialogService and INavigationService.
Implmentation for WPF is available as [MvvmEssentials.Navigation.WPF](https://www.nuget.org/packages/MvvmEssentials.Navigation.WPF/) nuget package.