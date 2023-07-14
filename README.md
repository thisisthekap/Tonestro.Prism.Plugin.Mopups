# Tonestro.Prism.Plugin.Mopups

Prism plugin for navigating [Mopups](https://github.com/LuckyDucko/Mopups) with [Prism](https://github.com/PrismLibrary/Prism)

## How To Use

Just register `IContainerRegistry` from prism like this
```csharp
private void RegisterServices(IContainerRegistry containerRegistry)
{
    // register other services
    containerRegistry.RegisterPopupNavigationService();
}
```

Don't forget to also register `Mopups` in MauiProgram like this
```csharp
 public static MauiApp CreateMauiApp()
 {
     var builder = MauiApp.CreateBuilder();
     builder.UseMauiApp<App>()
            .UsePrism(PrismRegistry.Configure)
            .ConfigureMopups();
 }
```

For more info please check the samples.

## NuGet Feeds
