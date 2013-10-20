Updating ViewModel with a Model changing very quickly
=

The classic MVVM approach, in which you constantly keep your ViewModel (and thus your View) perfectly synchronized with your Model, soon reaches its limits 
when the properties of the Model changes at a very high frequency.

It happened to me while implementing a model handling a lot of values coming from measuring instruments and i wanted to display the current values, even if they were changing at a very high frequency.

I'll describe this problem in "Method 1" and then suggest 3 simple solutions.

A sample project of each method is available in this repository.


Method 1 : ViewModel is attached to Model's events
-

In the classic MVVM approach, the ViewModel is attached to the Model's events and calls `Dispatcher.BeginInvoke()` to ensure the event `PropertyChanged` is raised in the UI thread.

    public ViewModel()
    {
        dispatcher = Dispatcher.CurrentDispatcher;
        model = new Model();

        model.ProgressChanged += OnModelProgressChanged;
    }
        
    void OnModelProgressChanged(double newValue)
    {
        dispatcher.BeginInvoke((Action)delegate() { Progress = newValue; });
    }

    public double Progress
    {
        get { return progress; }
        set
        {
            if( progress == value ) return;
            progress = value;
            RaisePropertyChanged("Progress");
        }
    }

However, this approach wont work well if the Model's events are raised very frequently. Because the ViewModel calls `BeginInvoke()` each time a property changes, it will quickly saturate the event queue of the dispatcher. This usually result in an unresponsive UI.

Method 2 : ViewModel owns a DispatcherTimer
-

A first solution is to use a `DispatcherTimer` in the ViewModel. This means we get rid of the observer pattern, and go back to a plain old polling technique.

    public ViewModel()
    {
        model = new Model();

        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(5);
        timer.Tick += OnTimerTick;
        timer.Start();
    }

    void OnTimerTick(object sender, EventArgs e)
    {
        Progress = model.Progress;
    }
    
    // ...the remaining is identical to Method 1
 
Method 3 : View owns a DispatcherTimer
-

A second solution is to move the `DispatcherTimer` into the View. On each `Tick`, the View class a `ICommand` of the ViewModel.

In the example, I encapsulated the timer in a `CommandTimer` because I originally wanted to be able to instantiate and bind it from the XAML.
Unfortunately, this would need to add a `x:Key` to the resource and reference it with a `{StaticResource}` somewhere, otherwise the resource wont be instantiated at all.
So finally, i use the `CommandTimer` from the code behind.

View's code behind

    public MainWindow()
    {
        InitializeComponent();

        var vm = new ViewModel();
        timer = new CommandTimer();
        timer.Command = vm.Refresh;

        DataContext = vm;
    }
    
ViewModel

    public ViewModel()
    {
        model = new Model();
        Refresh = new DelegateCommand(ExecuteRefresh);
    }

    public ICommand Refresh
    {
        get;
        private set;
    }

    private void ExecuteRefresh(object obj)
    {
        Progress = model.Progress;
    }
    
    // ...the remaining is identical to Method 1

Method 4 : View is attached to CompositionTarget.Rendering
-

A third solution, it to simply call a `Refresh()` method of the ViewModel from the View. To make it even simpler, we can get rid of the `DispatcherTimer` and instead attach to the `CompositionTarget.Rendering` event.

Of course, this is very similar to Method 3, you may choose to implement a combination of both.

View's code behind:

    public MainWindow()
    {
        InitializeComponent();

        DataContext = new ViewModel();

        CompositionTarget.Rendering += OnRendering;
    }

    void OnRendering(object sender, EventArgs e)
    {
        if (DataContext is IRefresh)
            ((IRefresh)DataContext).Refresh();
    }
    
ViewModel:

    class ViewModel : INotifyPropertyChanged, IRefresh
    {
        public ViewModel()
        {
            model = new Model();
        }

        public void Refresh()
        {
            Progress = model.Progress;
        }
        
        // ...the remaining is identical to Method 1
    }
    
Conclusion
-

The last solution is the one I prefer.

Whether this respect or not the MVVM pattern is really a matter of opinion. I think it doesn't break the separation of concerns, and it's good that the View handles the timing issues. So to me, it's MVVM compliant.

Another advantage of these 3 solutions compared to the observer pattern is that they really decouple the Model and the ViewModel execution. Indeed, the background thread of the Model is never used for something else, like calling `BeginInvoke()`. You can even get rid of the Model's event !

However, since the properties of the Model are accessed from concurrent threads, you may need to add `lock` blocks if the type is bigger that a processor word (in my examples I use a `int` so that's OK).

Moreover, if you have a lot of changing properties in your model, you should group them in a class, like `ModelState`. That way, the ViewModel will only have one property to monitor and only this class really needs to be thread safe.
