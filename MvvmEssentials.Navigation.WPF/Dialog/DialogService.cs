using MvvmEssentials.Core;
using MvvmEssentials.Core.Common;
using MvvmEssentials.Core.Dialog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace MvvmEssentials.Navigation.WPF.Dialog
{
    /// <summary>
    /// Implementation of <see cref="IDialogService"/> for WPF.
    /// Can be used to open <see cref="Window"/> as a new view or a dialog.
    /// </summary>
    public class DialogService : IDialogService
    {
        private readonly IServiceProvider serviceProvider;

        private static List<Window> activeViews;
        /// <summary>
        /// Contains a list of all the views opened by the <see cref="IDialogService.Show(Type, IParameters?)"/>
        /// </summary>
        internal static List<Window> ActiveViews
        {
            get => activeViews;
            private set => activeViews = value;
        }

        /// <summary>
        /// Contains the list of all the views that were opened by this service.
        /// Does not store the default views that are defined in this project.(<see cref="DefaultDialogHostWindow"/> and <see cref="SimpleDialogWindow"/>)
        /// </summary>
        private static List<Window> allOpenedViews = new();

        /// <summary>
        /// Creates an w of <see cref="DialogService"/>
        /// </summary>
        /// <param name="Provider">The service provider for this application</param>
        public DialogService(IServiceProvider Provider)
        {
            serviceProvider = Provider;
            ActiveViews ??= new();
        }

        ///<inheritdoc/>
        /// <exception cref="ArgumentException">thrown when the <paramref name="viewType"/> is not a <see cref="Window"/></exception>
        /// <exception cref="InvalidProgramException">thrown when type <paramref name="viewType"/> is not registered in the <see cref="IServiceProvider"/></exception>
        public void Show(Type viewType, IParameters? parameters = null)
        {
            if (!viewType.IsAssignableTo(typeof(Window)))
                throw new ArgumentException($"Cannot show a view which is not a type of {nameof(Window)}");

            if (serviceProvider.GetService(viewType) is not Window instance)
                throw new InvalidProgramException($"{viewType.Name} is not registered in the {nameof(IServiceProvider)}");

            var instanceViewModel = instance.DataContext as IViewAware;

            if (instanceViewModel is not null)
            {
                instanceViewModel.OnOpened(parameters);

                Action closefromViewModelAction = null;
                closefromViewModelAction = () =>
                {
                    instance.Close();

                    instanceViewModel.Close -= closefromViewModelAction;
                };

                instanceViewModel.Close += closefromViewModelAction;
            }
            HandleCloseEvents(instance, instanceViewModel);

            if (!allOpenedViews.Contains(instance))
                allOpenedViews.Add(instance);
            ActiveViews.Add(instance);
            instance.Show();

        }

        ///<inheritdoc/>
        /// <exception cref="ArgumentException">thrown when the <typeparamref name="T"/> does not have <see cref="IsDialogContentEnumAttribute"/> attached to it.</exception>
        /// <exception cref="Exception">thrown when the <see cref="NavigateToAttribute"/> is not assigned to the enum value.</exception>
        public DialogResult ShowDialog<T>(T dialogContentType, IDialogParameters parameters, Action<IDialogParameters?> callbackMethod)
            where T : Enum
        {
            if (!typeof(T).HasAttribute<IsDialogContentEnumAttribute>())
                throw new ArgumentException($"enum which does not support dialog content navigation was passed in to the {nameof(IDialogService.ShowDialog)}");

            var attribute = dialogContentType.GetAttribute<NavigateToAttribute>();

            if (attribute == null)
                throw new Exception($"{dialogContentType} does not have {nameof(NavigateToAttribute)} attached to it");

            var content = serviceProvider.GetService(attribute.DestinationType);

            //getting the result from the dialog box
            DialogResult result = DialogResult.None;

            //Calling the callback method when the dialog closes
            var fe = content as FrameworkElement;

            var contentViewModel = fe?.DataContext as IDialogAware;

            object? title = contentViewModel?.Title;

            //show persistent dialog w
            var DefaultDialogHost = new DefaultDialogHostWindow(title, content);
            if (contentViewModel is not null)
            {
                contentViewModel.OnOpened(parameters);

                Action closefromViewModelAction = null;
                closefromViewModelAction = () =>
                {
                    result = contentViewModel.DialogResult;
                    DefaultDialogHost.Close();

                    contentViewModel.Close -= closefromViewModelAction;
                };

                contentViewModel.Close += closefromViewModelAction;

                HandleCloseEvents(DefaultDialogHost, contentViewModel, false);
            }
            if (!allOpenedViews.Contains(DefaultDialogHost))
                allOpenedViews.Add(DefaultDialogHost);
            DefaultDialogHost.ShowDialog();

            allOpenedViews.Remove(DefaultDialogHost);
            callbackMethod.Invoke(contentViewModel?.ResultParameters());
            return result;
        }

        ///<inheritdoc/>
        /// <exception cref="ArgumentException">thrown when the <paramref name="customView"/> is not a <see cref="Window"/></exception>
        /// <exception cref="Exception">thrown when the dialog service could not get type <paramref name="customView"/> from <see cref="IServiceProvider"/></exception>
        public DialogResult ShowDialog(Type customView, IDialogParameters parameters, Action<IDialogParameters?> callbackMethod)
        {
            if (!customView.IsAssignableTo(typeof(Window)))
                throw new ArgumentException("the passed in type must be a type of window");

            if (serviceProvider.GetService(customView) is not Window instance)
                throw new Exception($"Could not get the type {customView.Name} from {nameof(IServiceProvider)}");

            //getting the result from the dialog box
            DialogResult result = DialogResult.None;

            var instanceViewModel = instance.DataContext as IDialogAware;
            if (instanceViewModel is not null)
            {
                instanceViewModel.OnOpened(parameters);

                Action closefromViewModelAction = null;
                closefromViewModelAction = () =>
                {
                    result = instanceViewModel.DialogResult;
                    instance.Close();

                    instanceViewModel.Close -= closefromViewModelAction;
                };

                instanceViewModel.Close += closefromViewModelAction;
            }

            HandleCloseEvents(instance, instanceViewModel);

            if (!allOpenedViews.Contains(instance))
                allOpenedViews.Add(instance);
            ActiveViews.Add(instance);
            instance.ShowDialog();

            callbackMethod.Invoke(instanceViewModel?.ResultParameters());
            return result;
        }

        /// <inheritdoc/>
        public bool ShowSimpleDialog(string title, string content, string button1Content, string button2Content)
        {
            //This method is hard coded to work with <see cref="SimpleDialogWindow"/>.
            // Re-implement this method if the dialog window is updated in the future.
            IDialogParameters parameters = new DialogParameters()
            {
                {"title", title },
                {"text", content },
                {"button1Content", button1Content },
                {"button2Content", button2Content },
            };

            var dialog = new SimpleDialogWindow(parameters);
            bool result = false;

            if (dialog.DataContext is IDialogAware vm)
            {
                dialog.Closing += (_, _) =>
                {
                    if (vm.DialogResult == DialogResult.Yes)
                        result = true;
                    vm.OnClosing();
                };
            }

            if (!allOpenedViews.Contains(dialog))
                allOpenedViews.Add(dialog);
            dialog.ShowDialog();

            allOpenedViews.Remove(dialog);
            return result;
        }

        private bool isClosingViews = false;
        /// <summary>
        /// Subscribes to the <see cref="Window.Closing"/> and <see cref="Window.Closed"/> events and uses <see cref="IClosable.CanClose"/> to determine whether the window should be closed.
        /// </summary>
        /// <param name="window">the window whose data context is inheriting from <see cref="IClosable"/></param>
        /// <param name="viewModel">the viewmodel that inherits from the <see cref="IClosable"/></param>
        private void HandleCloseEvents(Window window, IClosable? viewModel, bool isRegistered = true)
        {
            //setting the visibility to collapsed when w is closing instead of actually closing the w.
            CancelEventHandler closingEvent;
            closingEvent = (_, e) =>
            {
                e.Cancel = !viewModel?.CanClose() ?? false;

                //Handling w close event if not cancelled by now.
                if (!e.Cancel)
                {
                    viewModel?.OnClosing();

                    if (isRegistered && isClosingViews is false)
                    {
                        //hiding the closed views since they can't be opened again if closed when registered.
                        window.Visibility = Visibility.Collapsed;
                        ActiveViews.Remove(window);
                        e.Cancel = true;
                    }

                    //closes all the collapsed windows once the last window is closed.
                    if(!ActiveViews.Any() && !isClosingViews && allOpenedViews.All(t => !t.IsVisible))
                    {
                        //remove this window from opened views since the closing event for this window is already fired.
                        //just make sure this close event is not cancelled.
                        allOpenedViews.Remove(window);
                        e.Cancel = false;

                        //closing all the collapsed views
                        isClosingViews = true;
                        allOpenedViews.ForEach(t => t.Close());
                        isClosingViews = false;
                    }
                }
            };
            window.Closing += closingEvent;

            //Using closed event to unsubscribe from the closing events.
            EventHandler closedHandler = null;
            closedHandler = (_, e) =>
            {
                window.Closed -= closedHandler;
                window.Closing -= closingEvent;
            };

            window.Closed += closedHandler;
        }
    }
}