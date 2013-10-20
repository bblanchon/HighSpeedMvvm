using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Method3
{
    public class CommandTimer : DependencyObject, ICommandSource
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandTimer), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandTimer), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(CommandTimer), new PropertyMetadata(null));

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(TimeSpan), typeof(CommandTimer), new PropertyMetadata(TimeSpan.FromMilliseconds(5), OnIntervalPropertyChanged));
        
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(CommandTimer), new PropertyMetadata(true, OnIsEnabledPropertyChanged));

        [Bindable(true)]
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        [Bindable(true)]
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        [Bindable(true)]
        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }
        
        [Bindable(true)]
        public TimeSpan Interval
        {
            get { return (TimeSpan)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        
        [Bindable(true)]
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        readonly DispatcherTimer timer;

        public CommandTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += OnTimerTick;
            timer.Interval = Interval;
            timer.IsEnabled = IsEnabled;            
        }

        void OnTimerTick(object sender, EventArgs e)
        {
            var command = Command;
            var routedCommand = command as RoutedCommand;
            
            if( routedCommand != null )
            {
                if (routedCommand.CanExecute(CommandParameter, CommandTarget))
                    routedCommand.Execute(CommandParameter, CommandTarget);
            }
            else if( command != null )
            {
                if (command.CanExecute(CommandParameter))
                    command.Execute(CommandParameter);
            }
        }

        static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CommandTimer)d).OnIntervalChanged((TimeSpan)e.NewValue);
        }
        
        void OnIntervalChanged(TimeSpan newInterval)
        {
            timer.Interval = newInterval;
        }

        static void OnIsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CommandTimer)d).OnIsEnabledChanged((bool)e.NewValue);
        }

        private void OnIsEnabledChanged(bool newIsEnabled)
        {
            timer.IsEnabled = newIsEnabled;
        }
    }
}
