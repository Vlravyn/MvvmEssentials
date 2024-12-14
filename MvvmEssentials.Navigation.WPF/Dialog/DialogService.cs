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
        /// Creates an instance of <see cref="DialogService"/>
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

            ActiveViews.Add(instance);
            instance.Show();

            var instanceViewModel = instance.DataContext as IViewAware;

            if (instanceViewModel is not null)
            {
                instanceViewModel.OnOpened(parameters);

                Action closefromViewModelAction = null;
                closefromViewModelAction = () =>
                {
                    instanceViewModel.OnClosing();
                    ActiveViews.Remove(instance);

                    //hiding the closed views since they can't be opened again if closed when registered.
                    instance.Visibility = Visibility.Collapsed;
                    instanceViewModel.Close -= closefromViewModelAction;
                };

                instanceViewModel.Close += closefromViewModelAction;
            }

            //setting the visibility to collapsed when window is closing instead of actually closing the window.
            CancelEventHandler closingEvent;
            closingEvent = (_, e) =>
            {
                if (instanceViewModel is not null)
                    e.Cancel = !instanceViewModel.CanClose();

                //this will close the application if the last instance is closed.
                if (e.Cancel is false && ActiveViews.All(window => window.Visibility == Visibility.Collapsed || window.Visibility == Visibility.Hidden))
                    ActiveViews.ForEach(window => window.Close());
                else if (e.Cancel is false)
                {
                    //hiding the closed views since they can't be opened again if closed when registered.
                    instance.Visibility = Visibility.Collapsed;
                }
            };
            instance.Closing += closingEvent;

            //Using closed event to unsubscribe from the closing events.
            EventHandler closedHandler = null;
            closedHandler = (_, e) =>
            {
                instance.Closed -= closedHandler;
                instance.Closing -= closingEvent;
            };

            instance.Closed += closedHandler;
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
                throw new Exception($"{dialogContentType.ToString()} does not have {nameof(NavigateToAttribute)} attached to it");

            var content = serviceProvider.GetService(attribute.DestinationType);

            object? title = null;

            //get the title from the parameters
            if (parameters != null && parameters.Count != 0)
                title = parameters.FirstOrDefault(t => string.Equals(t.Key, "title", StringComparison.OrdinalIgnoreCase)).Value;

            //show persistent dialog window
            var DefaultDialogHost = new DefaultDialogHostWindow(title, content);
            DefaultDialogHost.ShowDialog();

            //getting the result from the dialog box
            DialogResult result = DialogResult.None;

            //Calling the callback method when the dialog closes
            if (DefaultDialogHost.DataContext is IDialogAware vm)
            {
                DefaultDialogHost.Closing += (_, _) =>
                {
                    vm.OnClosing();
                    result = vm.DialogResult;
                };
                callbackMethod.Invoke(vm.ResultParameters());
            }

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

            ActiveViews.Add(instance);
            instance.ShowDialog();

            //getting the result from the dialog box
            DialogResult result = DialogResult.None;

            var instanceViewModel = instance.DataContext as IDialogAware;

            if (instanceViewModel is not null)
            {
                Action closefromViewModelAction = null;
                closefromViewModelAction = () =>
                {
                    result = instanceViewModel.DialogResult;
                    callbackMethod.Invoke(instanceViewModel.ResultParameters());
                    instanceViewModel.OnClosing();
                    ActiveViews.Remove(instance);

                    //hiding the closed views since they can't be opened again if closed when registered.
                    instance.Visibility = Visibility.Collapsed;
                    instanceViewModel.Close -= closefromViewModelAction;
                };

                instanceViewModel.Close += closefromViewModelAction;
            }

            //setting the visibility to collapsed when window is closing instead of actually closing the window.
            CancelEventHandler closingEvent;
            closingEvent = (_, e) =>
            {
                if (instanceViewModel is not null)
                    e.Cancel = !instanceViewModel.CanClose();

                //this will close the application if the last instance is closed.
                if (e.Cancel is false && ActiveViews.All(window => window.Visibility == Visibility.Collapsed || window.Visibility == Visibility.Hidden))
                    ActiveViews.ForEach(window => window.Close());
                else if (e.Cancel is false)
                {
                    //hiding the closed views since they can't be opened again if closed when registered.
                    instance.Visibility = Visibility.Collapsed;
                }
            };
            instance.Closing += closingEvent;

            //Using closed event to unsubscribe from the closing events.
            EventHandler closedHandler = null;
            closedHandler = (_, e) =>
            {
                instance.Closed -= closedHandler;
                instance.Closing -= closingEvent;
            };

            instance.Closed += closedHandler;
            return result;
        }

        /// <inheritdoc/>
        public bool ShowSimpleDialog(string title, string content, string button1Content, string button2Content)
        {
            IDialogParameters parameters = new DialogParameters()
            {
                {"title", title },
                {"text", content },
                {"button1Content", button1Content },
                {"button2Content", button2Content },
            };

            var dialog = new SimpleDialogWindow(parameters);

            IDialogAware vm = dialog.DataContext as IDialogAware;
            if (vm is not null)
            {
                dialog.Closing += (_, _) =>
                {
                    if (vm.DialogResult == DialogResult.Yes)
                        result = true;
                    vm.OnClosing();
                };
            }
            dialog.ShowDialog();

            return result;
        }

        private bool result;
    }
}