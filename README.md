# Feature Ops - C# SDK
Official C# Library for the Feature Ops Web API

## Features
- Provide (and evaluate) feature flags settings
- Capture and store feature flag statistics within the Feature Ops application
- Continue to serve feature flags if connectivty to Feature Ops goes down
- Improve performance via feature flag caching

## Support
The Feature Ops C# SDK supports the following framework versions:

* .NET Framework 4.6.1
* .NET Core 2.0
* Mono 5.4
* Xamarin.iOS 10.14
* Xamarin.Mac 3.8
* Xamarin.Android 7.5

## Install


## Quick Start

```cs
var client = new FeatureOps.Client("{ENVIRONMENT AUTH KEY}", new Options { PollingInterval = 5 });
var response = await client.InitAsync();

if (response.Success)
{
    while (true)
    {
        if (await client.EvalFlagAsync("{CODE TOKEN}"))
        {
            // Feature Is On
        }
        else
        {
            // Feature Is Off
        }
    }
}
```

## API
An instance of the Feature Ops client can be obtained by passing your `environmentAuthKey` along with an optional `Options` object containing configuration properties.  You should only create one client during the lifetime of your applicaiton.

`var client = FeatureOps.Client(environmentAuthKey, options);`

`authKey`:  Your private environment key accessed, from within the Feature Ops application, by selecting the environment for which you choose to target.

`options`: An optional object of type `FeatureOps.Options` with properties that are used to configure the Feature Ops client.

|Key|Type|Value|
|---|---|---|
|PollingInterval|int|The amount of time, in minutes, that the Feature Ops client should check for changes to its feature flags cache|
|CancellationToken|CancellationToken|Sends notification that feature flags cache refresh polling should be canceled.|

### Client Objects & Methods

`InitResponse`

Object returned with call to `Client.InitAsync()`.

`InitResponse.Success`:  `bool` indicating success or failure of the operation.   
`InitResponse.Message`:  `string` Message indicating reason for failure of the operation.

***

`Client.InitAsync()`

Returns a `InitResponse` after the call is complete.  This method call should be made on application load as it will fetch and locally cache feature flag settings for the environment that you are targeting.  Upon success, you are free to make calls to `EvalFlagAsync`, as needed, to evaluate whether a feature is 'on' or 'off'.

```cs
var client = new FeatureOps.Client("{ENVIRONMENT AUTH KEY}", new Options { PollingInterval = 5 });
var response = await client.InitAsync();

if (response.Success)
{
    // Ready to go!
}
else {
    /* Take Error/Fallback Action */
}
```

***

`Client.EvalFlagAsync(codeToken)`
`Client.EvalFlagAsync(codeToken, targets)`

Returns a `bool` after the call is complete.  This method call should be made when you need to determine if a feature, for a given user, is 'on' or 'off'.

`codeToken`:  A string that was defined when adding the feature to the Feature Ops application.  This is the unique code identifier when accessing its feature flag settings.

`targets`:  A list of `IEnumerable<string>`, which pertain to the end user, that will be used to evaluate whether or not a feature flag should be 'on' or 'off'.  These will only impact the feature flag evaluation if the feature flag setting, for the environment that you are targeting, is set to 'Targets On' otherwise the `targets` will simply be ignored.

```cs
if (await client.EvalFlagAsync("{CODE TOKEN}"))
{
    // Feature Is On
}
else
{
    // Feature Is Off
}
```

***

## License

[MIT License](https://github.com/featureops/featureops-csharp/blob/master/LICENSE)