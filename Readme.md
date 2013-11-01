WPF/MVVM : How to deal with fast changing properties?
=

Here, I'm gonna describe a problem you may have in a WPF application using the MVVM pattern, if your `Model` is updated at very high frequency.

It happened to me while implementing a `Model` handling a lot of values coming from measuring instruments and I wanted to display the current values, even if they were changing very quickly.

A sample project of each method is available in this repository.


The problem with the classic MVVM approach
-

In the classic MVVM approach, the `ViewModel` is attached to the `Model`'s events, so has to be updated on each change.

Usually, the `ViewModel`'s event handler calls `Dispatcher.BeginInvoke()` to ensure the event `PropertyChanged` is raised in the UI thread.

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

However, this approach wont work well if the Model's events are raised very frequently. Because the ViewModel calls `BeginInvoke()` each time a property changes, it will quickly saturate the event queue of the dispatcher. This usually result in an unresponsive GUI.

If you run the sample project [MvvmHighFrequency.Problem](Problem), you'd see that the GUI is completely frozen. If you're lucky you wont have a `OutOfMemoryException` (indeed, each call to `BeginInvoke()` instanciate a message), you'll see that the execution speed of the `Model` is really affected.

Solution 1 : Ignore events that are too closed
-

The first thing that come in mind is: "OK, I get too many event. I'll just slow them down !". Sure, let's try...

    public ViewModel()
    {
        var dispatcher = Dispatcher.CurrentDispatcher;
        var model = new Model();

        Observable.FromEventPattern<double>(model, "ProgressChanged")
                .Sample(TimeSpan.FromMilliseconds(5))
                .ObserveOn(dispatcher)
                .Subscribe(x => Progress = x.EventArgs);
    }
            
Here I used Reactive Framework because it offers the `Sample()` method which limits the rate of the events. You 

It's a perfectly viable solution for anyone that uses Reactive framework, but the two next solutions provide a much simpler approach without extra library.

If you run the sample project [MvvmHighFrequency.Solution1](Solution1), you'd see that the GUI is responsive an the `Model`'s execution speed is much faster.


Solution 2 :  Poll with a DispatcherTimer
-

Another way to look a the problem is to accept to loose the "push" approach and to use "polling" instead. Instead of attaching to the event of the Model, the ViewModel will periodically read the values of the Model.

The most common way to implement polling in MVVM is to instanciate a `DispatcherTimer` in the ViewModel.

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
    
    // ...the remaining is identical to the original ViewModel
    
If you run the sample project [MvvmHighFrequency.Solution2](Solution2), you'd see that the performance is higher that solution 1.
 
Solution 3 : Poll on CompositionTarget.Rendering
-

To make it even simpler, one could simply move the timer from the `ViewModel` to the `View`. Luckily, WPF offers the `CompositionTarget.Rendering` event which is raise each time a frame is rendered. This means you can completely get rid of the `DispatcherTimer`.

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
        
        // ...the remaining is identical to the original ViewModel
    }
    
If you run the sample project [MvvmHighFrequency.Solution3](Solution3), you'd see that the performance is comparable to solution 2.
    
Conclusion
-

I like simple solutions, that why I really prefer the last one.

Whether it respects or not the MVVM pattern is really a matter of opinion.  I really like the idea of the `View` being responsible of the timer and the `ViewModel` being responsible of updating its value.

One thing I really appreciate on the polling approach is that it really decouples the `Model`'s and the `ViewModel`'s execution threads. Aboluely no `BeginInvoke()` is required, which gives a great boost to the performance !

A word about concurrency
-

You may be carefully with the concurrency when using the polling technique.

Since the properties of the `Model` are accessed from concurrent threads, you may need to add `lock` blocks if the type is bigger that a processor word (in my examples I use a `int` so that's OK).

If you have a lot of changing properties in your model, you should group them in a class, like `ModelState`. That way, the `ViewModel` will only have one property to monitor and only this class needs to be thread safe.
