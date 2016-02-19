# Hidden Code Auto Generator

Created by [Icer](http://icerdesign.com)

# Overview

    Helper Generator to generate varies of hidden code which we shouldn't care about.

# TODO

- [ ] Support reference custom generator in code
- [ ] Publish vsix on Visual Studio Gallery

# Installation

Make sure you have VS2015 installed, then run `build.bat`, you will get `HiddenCodeAutoGenerator.vsix` in the root folder.

Open it and install to your VS.

# Feature

* VS2015 supported
* Out-of-Box generator for following
  * RelayCommand
  * DependencyProperty
  * **[Experimental]**Entity
  * ViewModel property
* Easy plug-in(with script) mode

# Getting started

![Set Custom Tool Property](https://github.com/wizicer/HiddenCodeAutoGen/blob/master/doc/propertyset.png "Set Custom Tool Property")

Set `Custom Tool` to `HiddenCodeAutoGenerator`.

# Usage

Currently, four generator available, mainly focused on WPF MVVM development.

For details, you can refer to [Test Cases](https://github.com/wizicer/HiddenCodeAutoGen/tree/master/UnitTests/TestCases)

## Relay Command

Turn this:
```csharp
[AutoGenCommand("NewProject")]
[AutoGenCommand("OpenProject", "this.NewProject")]
[AutoGenCommand("SaveProject", "this.SaveProject", "this.CanSaveProject")]
```
To this:
```csharp
/// <summary>
/// The NewProject command
/// </summary>
private RelayCommand _newProjectCommand;

/// <summary>
/// Gets the NewProject command.
/// </summary>
/// <value>The NewProject command.</value>
public ICommand NewProjectCommand
{
    get { return this._newProjectCommand ?? (this._newProjectCommand = new RelayCommand(this.ExecuteNewProjectWrap)); }
}

/// <summary>
/// delegate for invoking NewProjectCommand
/// </summary>
private void ExecuteNewProjectWrap(object obj)
{
    ExecuteNewProject(obj);
}

/// <summary>
/// Invoked when NewProjectCommand invokes
/// </summary>
partial void ExecuteNewProject(object obj);

/// <summary>
/// The OpenProject command
/// </summary>
private RelayCommand _openProjectCommand;

/// <summary>
/// Gets the OpenProject command.
/// </summary>
/// <value>The OpenProject command.</value>
public ICommand OpenProjectCommand
{
    get { return this._openProjectCommand ?? (this._openProjectCommand = new RelayCommand(this.NewProject)); }
}

/// <summary>
/// The SaveProject command
/// </summary>
private RelayCommand _saveProjectCommand;

/// <summary>
/// Gets the SaveProject command.
/// </summary>
/// <value>The SaveProject command.</value>
public ICommand SaveProjectCommand
{
    get { return this._saveProjectCommand ?? (this._saveProjectCommand = new RelayCommand(this.SaveProject, this.CanSaveProject)); }
}
```

## Dependency Property

Turn this:
```csharp
[AutoGenDP("WireInfo", typeof(string), "null")]
[AutoGenDP("WireInfo2", typeof(Wire), "new Wire()")]
```
To this:
```csharp
/// <summary>
/// Gets / sets the WireInfo property value, This is a dependency property
/// </summary>
public string WireInfo
{
    get { return (string)GetValue(WireInfoProperty); }
    set { SetValue(WireInfoProperty, value); }
}

/// <summary>
/// Defines the WireInfo dependnecy property.
/// </summary>
public static readonly System.Windows.DependencyProperty WireInfoProperty =
    System.Windows.DependencyProperty.Register("WireInfo", typeof(string), typeof(WireBaseViewModel),
        new System.Windows.PropertyMetadata("null", new System.Windows.PropertyChangedCallback(OnWireInfoPropertyChanged)));

/// <summary>
/// Invoked when the WireInfo property changes
/// </summary>
partial void OnWireInfoPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs e);

private static void OnWireInfoPropertyChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
{
    WireBaseViewModel control = d as WireBaseViewModel;
    control.OnWireInfoPropertyChanged(e);
}
/// <summary>
/// Gets / sets the WireInfo2 property value, This is a dependency property
/// </summary>
public Wire WireInfo2
{
    get { return (Wire)GetValue(WireInfo2Property); }
    set { SetValue(WireInfo2Property, value); }
}

/// <summary>
/// Defines the WireInfo2 dependnecy property.
/// </summary>
public static readonly System.Windows.DependencyProperty WireInfo2Property =
    System.Windows.DependencyProperty.Register("WireInfo2", typeof(Wire), typeof(WireBaseViewModel),
        new System.Windows.PropertyMetadata(new Wire(), new System.Windows.PropertyChangedCallback(OnWireInfo2PropertyChanged)));

/// <summary>
/// Invoked when the WireInfo2 property changes
/// </summary>
partial void OnWireInfo2PropertyChanged(System.Windows.DependencyPropertyChangedEventArgs e);

private static void OnWireInfo2PropertyChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
{
    WireBaseViewModel control = d as WireBaseViewModel;
    control.OnWireInfo2PropertyChanged(e);
}
```

## Notified Property

Turn this:
```csharp
[AutoGen("WireData", typeof(string), "null")]
[AutoGen("Visibility", typeof(Visibility), "Visibility.Hidden")]
```
To this:
```csharp
/// <summary>
/// Field which backs the WireData property
/// </summary>
private string _wireData = "null";

/// <summary>
/// Gets / sets the WireData value
/// </summary>
public string WireData
{
    get { return _wireData; }
    set
    {
        if (_wireData == value) return;
        _wireData = value;

        OnWireDataChanged(value);
        OnPropertyChanged("WireData");
    }
}

/// <summary>
/// Invoked when the value of WireData changes
/// </summary>
partial void OnWireDataChanged(string value);
/// <summary>
/// Field which backs the Visibility property
/// </summary>
private Visibility _visibility = Visibility.Hidden;

/// <summary>
/// Gets / sets the Visibility value
/// </summary>
public Visibility Visibility
{
    get { return _visibility; }
    set
    {
        if (_visibility == value) return;
        _visibility = value;

        OnVisibilityChanged(value);
        OnPropertyChanged("Visibility");
    }
}

/// <summary>
/// Invoked when the value of Visibility changes
/// </summary>
partial void OnVisibilityChanged(Visibility value);
```

## Entity

Turn this:
```csharp
[AutoGenEntity("WireData", typeof(string), false)]
[AutoGenEntity("Visibility", typeof(Visibility), true, "Visibility.Hidden")]
```
To this:
```csharp
public string WireData { get; set; }
public Visibility Visibility { get; private set; }

public WireBaseViewModel(string wireData, Visibility visibility = Visibility.Hidden)
{
    this.WireData = wireData;
    this.Visibility = visibility;
}
```

# Contributing

Issues and PR are welcome. Please pass unit tests before raise PR.
