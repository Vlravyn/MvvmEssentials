using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MvvmEssentials.Core
{
    /// <summary>
    /// Simple Implementation of <see cref="INotifyPropertyChanged"/> and <see cref="INotifyPropertyChanging"/> interfaces for all the view models to inherit from.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangingEventHandler? PropertyChanging;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Checks if the property has the same value as the new value.
        /// Sets the property to new value is changed and raises <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">the type of value being changed</typeparam>
        /// <param name="variableReference">reference of the variable that holds the value</param>
        /// <param name="newValue">the new value for this variable</param>
        /// <param name="propertyName">Name of the property to notify the listeners. Optional</param>
        /// <returns>true if the value was set succesfully</returns>
        public bool SetProperty<T>(ref T variableReference, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(variableReference, newValue))
                return false;

            OnPropertyChanging(propertyName);

            variableReference = newValue;

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">The name of the property to notify the listeners. Optional.</param>
        public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="args">The <see cref="PropertyChangedEventArgs"/> with the property name.</param>
        /// <exception cref="ArgumentNullException">thrown when the <paramref name="args"/> is null</exception>
        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if(args == null)
                throw new ArgumentNullException(nameof(args));

            PropertyChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event
        /// </summary>
        /// <param name="propertyName">the name of the property that is changing to notify the listeners.</param>
        public virtual void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        {
            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event
        /// </summary>
        /// <param name="args">The <see cref="PropertyChangingEventArgs"/> with the property name.</param>
        /// <exception cref="ArgumentNullException">thrown when the <paramref name="args"/> is null</exception>
        protected void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if(args == null)
                throw new ArgumentNullException(nameof(args));

            PropertyChanging?.Invoke(this, args);
        }
    }
}