# Omnigage .NET SDK ![.NET Core](https://github.com/omnigage/omnigage-sdk-dotnet/workflows/.NET%20Core/badge.svg)

Available NuGet: https://www.nuget.org/packages/Omnigage.SDK/

## Example

```csharp
    Client.Init("token-key", "token-secret", "account-key");

    CallResource call = new CallResource();
    call.To = "+11111111111";
    call.Action = CallAction.Dial;

    await call.Create();
```