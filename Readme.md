[WPF/MVVM] How to deal with fast changing properties

This repository contains 4 sample WPF/MVVM applications that  implement different techniques to deal with fast changing properties in the Model.

1. Project `Problem` keeps the ViewModel synchronized with the Model
2. Project `Solution1` uses Reactive Framework's `Sample()` to reduce the frequency
3. Project `Solution2` uses a `DispatcherTimer` in the ViewModel
4. Project `Solution3` attaches to `CompositionTarget.Rendering`

They all use the same `Model` class from the `Common` project.

See my blog post: [[WPF/MVVM] How to deal with fast changing properties](http://blog.benoitblanchon.fr/wpf-high-speed-mvvm)